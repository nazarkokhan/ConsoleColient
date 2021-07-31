using System;
using System.IO;
using System.Threading.Tasks;
using MessengerConsole.Extensions;
using MessengerApp.Core.DTO.Message;
using MessengerConsole.Constants;
using MessengerConsole.DTO.Authorization;
using MessengerConsole.Services;
using MessengerConsole.Services.Abstraction;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable All

namespace MessengerConsole
{
    class Program
    {
        static HubConnection HubConnection { get; set; }

        private static IAccountService _accountService;
        private static IChatService _chatService;
        private static IContactService _contactService;
        private static IMessageService _messageService;

        private static IMemoryCache _cache;

        static async Task Main(string[] args)
        {
            // var configuration = new ConfigurationBuilder()
            //     .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName.AppSettings))
            //     .Build();

            var provider = new ServiceCollection()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IChatService, ChatService>()
                .AddScoped<IContactService, ContactService>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<ITokenStorage, JsonFileTokenStorage>()
                .AddMemoryCache()
                .AddScoped<AuthInterceptor>()
                // .AddSingleton<IConfiguration>(configuration)
                .AddHttpClient(
                    nameof(AccountService),
                    client => { client.BaseAddress = new Uri(Api.BaseUrl); }
                )
                .Services
                .AddHttpClient(
                    Client.AuthClient,
                    client => { client.BaseAddress = new Uri(Api.BaseUrl); }
                )
                .AddHttpMessageHandler<AuthInterceptor>()
                .Services
                .BuildServiceProvider();

            _accountService = provider.GetRequiredService<IAccountService>();
            _chatService = provider.GetRequiredService<IChatService>();
            _contactService = provider.GetRequiredService<IContactService>();
            _messageService = provider.GetRequiredService<IMessageService>();
            _cache = provider.GetRequiredService<IMemoryCache>();

            RegisterPage();
            
            var token = await _accountService.GetAccessAndRefreshTokensAsync(Dto.LogInUserData());

            HubConnection = new HubConnectionBuilder()
                .WithUrl(Api.BaseUrl + "/chat",
                    options => { options.AccessTokenProvider = () => Task.FromResult(token.Token); })
                .Build();

            HubConnection.On<MessageDto>("Send",
                (messageDto) =>
                {
                    Console.WriteLine(
                        $"UserId: {messageDto.UserId}, Message: {messageDto.Body} ({messageDto.DateTime})");
                }
            );

            await HubConnection.StartAsync();

            while (true)
            {
                var message = Console.ReadLine();

                if (message == "end")
                    break;

                await HubConnection.SendAsync("SendMessage", message);
            }

            Console.ReadLine();
        }

        static string RegisterPage()
        {
            Console.Clear();
            Console.WriteLine($"[{nameof(RegisterPage)}]", ConsoleColor.Red);
            Console.WriteLine("Enter Email:");
            var email = Console.ReadLine();

            Console.WriteLine("Enter Password:");
            Console.ForegroundColor = ConsoleColor.Black;
            var password = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine("*******hiden*******");
            
            Console.WriteLine("Enter UserName:");
            var userName = Console.ReadLine();

            var registerResult =  _accountService.CreateUserAndSendEmailTokenAsync(
                new RegisterDto(userName, email, password)
            ).Result;

            if (registerResult.Success)
                Console.WriteLine(BaseConst.Successful, ConsoleColor.Red);
            else
                Console.WriteLine(BaseConst.Failed, ConsoleColor.Red);

            Console.WriteLine("Confirm email to use your account");
            
            Console.WriteLine(BaseConst.NextCommand, ConsoleColor.Red);
            
            return Console.ReadLine();
        }
    }
}