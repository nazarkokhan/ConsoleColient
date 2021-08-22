using System.Collections.Generic;
using System.Linq;
using MessengerConsole.ConfigurationOptions;
using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetApiBaseUrl(this IConfiguration configuration) 
            => configuration.GetSection("Api")["BaseUrl"];

        public static string GetAuthorizationHttpClient(this IConfiguration configuration) 
            => configuration.GetSection("HttpClient")["Authorization"];

        public static IEnumerable<string> GetCommands(this IConfiguration configuration) 
            => configuration
                .GetSection("Commands")
                .AsEnumerable()
                .Select(x => x.Value)
                .Reverse();
    }
}