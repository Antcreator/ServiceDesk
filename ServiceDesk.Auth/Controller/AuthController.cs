using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceDesk.Data.Context;

namespace ServiceDesk.Auth.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(PersistenceContext persistence, IConfiguration configuration) : ControllerBase
{
    [HttpPost("Login")]
    public Results<BadRequest<string>, Ok<string>> Login([FromBody] LoginDto loginDto)
    {
        if (!persistence.Users.Any(e => e.Email == loginDto.Email))
        {
            return TypedResults.BadRequest("Email is incorrect");
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

        return TypedResults.Ok(jws);
    }
}
