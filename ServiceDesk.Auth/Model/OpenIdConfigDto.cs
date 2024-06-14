using System.Text.Json.Serialization;

namespace ServiceDesk.Auth.Model;

public record OpenIdConfigDto
{
    [JsonPropertyName("issuer")]
    public required string Issuer { get; set; }
    
    [JsonPropertyName("jwks_uri")]
    public required string JwksUri { get; set; }
}
