using System.Text.Json.Serialization;

namespace ServiceDesk.Auth.Model;

public record JwksKeysDto
{
    [JsonPropertyName("keys")]
    public required List<JwksDto> Keys { get; set; }
}

public record JwksDto
{
    [JsonPropertyName("alg")]
    public required string Algorithm { get; set; }

    [JsonPropertyName("kty")]
    public required string KeyType { get; set; }

    [JsonPropertyName("kid")]
    public required string KeyId { get; set; }

    [JsonPropertyName("use")]
    public required string Use { get; set; }

    [JsonPropertyName("e")]
    public required string Exponent { get; set; }

    [JsonPropertyName("n")]
    public required string Modulus { get; set; }
}
