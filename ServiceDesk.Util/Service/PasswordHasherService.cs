using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceDesk.Util.Service;

public class PasswordHasherService(IConfiguration configuration)
{
    private readonly byte[] _salt = Convert.FromBase64String(configuration["Hash:Salt"]);
    private readonly int _iteration = int.Parse(configuration["Hash:Iteration"]);

    public string GetHash(string password)
    {
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: _salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: _iteration,
            numBytesRequested: 256 / 8);

        return Convert.ToBase64String(hash);
    }

    public bool VerifyHash(string password, string hash)
    {
        var match = CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(hash),
            Convert.FromBase64String(GetHash(password)));

        return match;
    }
}

public static class PasswordHasherServiceCollection
{
    public static IServiceCollection AddPasswordHasher(this IServiceCollection services)
    {
        services.AddSingleton<PasswordHasherService>();

        return services;
    }
}
