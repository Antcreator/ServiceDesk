using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ServiceDesk.Util.Service;

public class PasswordHasherService(IOptions<HashSettings> hashSettings)
{
    private readonly byte[] salt = Convert.FromBase64String(hashSettings.Value.Salt);
    private readonly int iteration = hashSettings.Value.Iteration;

    public string GetHash(string password)
    {
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: iteration,
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
        services
            .AddOptions<HashSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection(HashSettings.Section).Bind(settings);
            });

        services.AddSingleton<PasswordHasherService>();

        return services;
    }
}
