using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Extensions
{
    public static class ConfigurationExtension
    {
        public static string GetApiBaseUrl(this IConfiguration configuration)
        {
            return configuration.GetSection("Api")["BaseUrl"];
        }
    }
}