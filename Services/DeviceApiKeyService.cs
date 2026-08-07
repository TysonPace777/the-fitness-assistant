using System.Security.Cryptography;
using System.Text;

namespace the_fitness_assistant.Services;

public class DeviceApiKeyService
{
    private readonly IConfiguration _configuration;

    public DeviceApiKeyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Checks the key sent by the Raspberry Pi against the key in configuration.
    public bool IsValid(string? apiKey)
    {
        var validKey =
            _configuration["DeviceApi:ApiKey"];

        // If no key is configured, or no key was sent, nobody gets in.
        // Without this check a missing header would match a missing setting.
        if (string.IsNullOrWhiteSpace(validKey) || string.IsNullOrWhiteSpace(apiKey))
        {
            return false;
        }

        // Fixed time comparison so the key cannot be guessed by timing the response.
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(apiKey),
            Encoding.UTF8.GetBytes(validKey));
    }
}