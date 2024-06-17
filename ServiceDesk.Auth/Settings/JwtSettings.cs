namespace ServiceDesk.Auth.Settings;

public class JwtSettings
{
    public const string Section = "Jwt";
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Algorithm { get; set; }
    public required string Use { get; set; }
    public required string KeyType { get; set; }
    public required string KeyId { get; set; }
    public required string Exponent { get; set; }
    public required string Modulus { get; set; }
    public required string PrivateKey { get; set; }
}
