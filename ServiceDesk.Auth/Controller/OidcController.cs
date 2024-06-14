using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Auth.Model;

namespace ServiceDesk.Auth.Controller;

[ApiController]
[Route(".well-known")]
public class OidcController(IConfiguration configuration) : ControllerBase
{
    [HttpGet("openid-configuration")]
    public ActionResult<OpenIdConfigDto> DiscoveryDocument()
    {
        var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Issuer is required");
        var jwksUri = issuer + "/.well-known/jwks.json";
        var openIdConfiguration = new OpenIdConfigDto
        {
            Issuer = issuer,
            JwksUri = jwksUri,
        };

        return Ok(openIdConfiguration);
    }
    
    [HttpGet("jwks.json")]
    public ActionResult<JwksKeysDto> JwksJson()
    {
        var algorithm = configuration["Jwt:Algorithm"] ?? throw new InvalidOperationException("Algorithm is required");
        var keyType = configuration["Jwt:KeyType"] ?? throw new InvalidOperationException("KeyType is required");
        var keyId = configuration["Jwt:KeyId"] ?? throw new InvalidOperationException("KeyId is required");
        var use = configuration["Jwt:Use"] ?? throw new InvalidOperationException("Use is required");
        var exponent = configuration["Jwt:Exponent"] ?? throw new InvalidOperationException("Exponent is required");
        var modulus = configuration["Jwt:Modulus"] ?? throw new InvalidOperationException("Modulus is required");
        var jwks = new JwksKeysDto
        {
            Keys = new List<JwksDto>()
            {
                new JwksDto
                {
                    Algorithm = algorithm,
                    KeyType = keyType,
                    KeyId = keyId,
                    Use = use,
                    Exponent = exponent,
                    Modulus = modulus,
                }
            }
        };

        return Ok(jwks);
    }
}
