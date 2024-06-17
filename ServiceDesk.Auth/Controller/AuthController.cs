using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServiceDesk.Auth.Settings;
using ServiceDesk.Data.Context;
using ServiceDesk.Util.Service;

namespace ServiceDesk.Auth.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(PersistenceContext persistence, PasswordHasherService hasherService, IOptions<JwtSettings> jwtSettings) : ControllerBase
{
    private readonly JwtSettings jwtSettings = jwtSettings.Value;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var user = await persistence.Users
            .FirstOrDefaultAsync(e => e.Email.Equals(loginDto.Email));

        if (user == null || !hasherService.VerifyHash(loginDto.Password, user.Password))
        {
            return BadRequest("Email and/or password is incorrect");
        }

        var keyId = jwtSettings.KeyId;
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, loginDto.Email),
            new Claim("kid", keyId),
        };
        var rsa = RSA.Create();

        rsa.ImportFromPem(jwtSettings.PrivateKey);

        var rsaKey = new RsaSecurityKey(rsa)
        {
            KeyId = keyId,
        };
        var signingCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
        var jwt = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            audience: jwtSettings.Audience,
            issuer: jwtSettings.Issuer);
        var jws = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Ok(jws);
    }
}
