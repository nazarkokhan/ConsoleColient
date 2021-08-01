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

        static async Task Main(string[] args)
        {
            await _client.RunAsync();
            // while (true)
            // {
            //     var message = Console.ReadLine();
            //
            //     if (message == "end")
            //         break;
            //
            //     await HubConnection.SendAsync("SendMessage", message);
            // }
        }

        // static void Commands()
        // {
        //     Console.Clear();
        //     Console.CursorLeft = 50;
        //     Console.WriteLine(nameof(Commands));
        //     Console.BackgroundColor = ConsoleColor.Black;
        //     Console.ForegroundColor = ConsoleColor.Red;
        //
        //     foreach (var c in _commands)
        //     {
        //         Console.CursorLeft = 50;
        //         Console.WriteLine($"{c}");
        //     }
        //     
        //     Console.SetCursorPosition(0, 0);
        //     ResetConsoleColors();
        // }
        //
        // static string HomePage()
        // {
        //     return "";
        // }
        //
        // static async Task<string> RegistrationPage()
        // {
        //     Commands();
        //
        //     Console.WriteLine($"[{nameof(RegistrationPage)}]");
        //     
        //     Console.BackgroundColor = ConsoleColor.White;
        //     Console.WriteLine("Enter Email:");
        //     ResetConsoleColors();
        //     var email = Console.ReadLine();
        //
        //     Console.BackgroundColor = ConsoleColor.White;
        //     Console.WriteLine("Enter Password:");
        //
        //     Console.ForegroundColor = ConsoleColor.DarkGray;
        //     var password = Console.ReadLine();
        //     
        //     Console.BackgroundColor = ConsoleColor.White;
        //     Console.WriteLine("Enter UserName:");
        //     var userName = Console.ReadLine();
        //
        //     var registerResult = await _accountService.CreateUserAndSendEmailTokenAsync(
        //         new RegisterDto(userName, email, password)
        //     );
        //
        //     if (registerResult.Success)
        //         Console.WriteLine(BaseConst.Successful, ConsoleColor.Red);
        //     else
        //         Console.WriteLine(BaseConst.Failed, ConsoleColor.Red);
        //
        //     Console.WriteLine("Confirm email to use your account");
        //
        //     Console.WriteLine(BaseConst.NextCommand, ConsoleColor.Red);
        //
        //     return Console.ReadLine();
        // }
        //
        // static async Task LogInPage()
        // {
        //     var token = await _accountService.GetAccessAndRefreshTokensAsync(Dto.LogInUserData());
        //
        //     HubConnection = new HubConnectionBuilder()
        //         .WithUrl(Api.BaseUrl + "/chat",
        //             options => { options.AccessTokenProvider = () => Task.FromResult(token.Token); })
        //         .Build();
        //
        //     HubConnection.On<MessageDto>("Send",
        //         (messageDto) =>
        //         {
        //             Console.WriteLine(
        //                 $"UserId: {messageDto.UserId}, Message: {messageDto.Body} ({messageDto.DateTime})");
        //         }
        //     );
        //
        //     await HubConnection.StartAsync();
        // }
        //
        // static void SetConsoleColors(ConsoleColor backgroundColor, ConsoleColor textColor)
        // {
        //     Console.BackgroundColor = backgroundColor;
        //     Console.ForegroundColor = textColor;
        //     
        //     _cache.Set(Cache.BackgroundColor, backgroundColor);
        //     _cache.Set(Cache.TextColor, textColor);
        //     Console.Clear();
        // }
        //
        // static void ResetConsoleColors()
        // {
        //     Console.BackgroundColor = _cache.Get<ConsoleColor>(Cache.Back);
        //     Console.ForegroundColor = _cache.Get<ConsoleColor>(Cache.Fore);
        // }
    }
}