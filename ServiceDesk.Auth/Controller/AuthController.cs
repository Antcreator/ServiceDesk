using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceDesk.Data.Context;
using ServiceDesk.Util.Service;

namespace ServiceDesk.Auth.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(PersistenceContext persistence, IConfiguration configuration, PasswordHasherService hasherService) : ControllerBase
{
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

        var keyId = configuration["Jwt:KeyId"];
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, loginDto.Email),
            new Claim("kid", keyId),
        };
        var rsa = RSA.Create();

        rsa.ImportFromPem(configuration["Jwt:PrivateKey"]);

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
            audience: configuration["Jwt:Audience"],
            issuer: configuration["Jwt:Issuer"]);
        var jws = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Ok(jws);
    }
}
