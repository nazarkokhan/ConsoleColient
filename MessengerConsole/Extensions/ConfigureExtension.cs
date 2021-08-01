using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Extensions
{
    public static class ConfigureExtension
    {
        public static string GetApiBaseUrl(this IConfiguration configuration)
        {
            return configuration.GetSection("Api")["BaseUrl"];
        }
        
        public static string GetAuthorizationHttpClient(this IConfiguration configuration)
        {
            return configuration.GetSection("HttpClient")["Authorization"];
        }
        
        public static List<string> GetCommands(this IConfiguration configuration)
        {
            return configuration
                .GetSection("Commands")
                .AsEnumerable()
                .Select(x => x.Value)
                .Reverse()
                .ToList();
        }
    }
}