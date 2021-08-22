using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MessengerConsole.ApiServices;
using MessengerConsole.ApiServices.Abstraction;
using MessengerConsole.Constants;
using MessengerConsole.Extensions;
using MessengerConsole.Services;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable All

namespace MessengerConsole
{
    class Program
    {
        private static Client _client;
        
        static Program()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName.AppSettings))
                .Build();

            var provider = new ServiceCollection()
                .AddSingleton<Client>()
                .AddSingleton<IConfiguration>(config)
                .AddOptionConfigurations(config)
                
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IChatService, ChatService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<IMessageService, MessageService>()
                .AddSingleton<ITokenStorage, TokenStorageJsonFile>()
                
                .AddScoped<ILogInTabService, LogInTabService>()
                .AddScoped<IConsoleService, ConsoleService>()
                .AddSingleton<IColorService, ColorService>()
                
                .AddMemoryCache()
                .AddScoped<AuthInterceptor>()
                .AddHttpClient(
                    nameof(AccountService),
                    client => { client.BaseAddress = new Uri(config.GetApiBaseUrl()); }
                )
                .Services
                .AddHttpClient(
                    ClientName.Authorization,
                    client => { client.BaseAddress = new Uri(config.GetApiBaseUrl()); }
                )
                .AddHttpMessageHandler<AuthInterceptor>()
                .Services
                .BuildServiceProvider();

            _client = provider.GetRequiredService<Client>();
        }

        static void Main(string[] args)
        {
            _client.Run();
        }
    }
}