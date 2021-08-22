using System.Collections.Generic;
using MessengerConsole.ConfigurationOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerConsole.Extensions
{
    public static class ConfigureOptionsExtensions
    {
        public static IServiceCollection AddOptionConfigurations(this IServiceCollection services,
            IConfiguration configuration)
            => services
                .Configure<ColorOptions>
                    (options => configuration.GetSection(nameof(ColorOptions)).Bind(options))
                .Configure<CommandOptions>
                    (options => configuration.GetSection(nameof(CommandOptions)).Bind(options))
                .Configure<CursorOptions>
                    (options => configuration.GetSection(nameof(CursorOptions)).Bind(options));
    }
}