namespace the_fitness_assistant.Services;

public class DeviceApiKeyService
{
    private readonly IConfiguration _configuration;

    public DeviceApiKeyService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool IsValid(string? apiKey)
    {
        var validKey =
            _configuration["DeviceApi:ApiKey"];

        return apiKey == validKey;
    }
}