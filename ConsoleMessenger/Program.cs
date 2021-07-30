using System;
using System.IO;
using System.Threading.Tasks;
using ConsoleMessenger.Constants;
using ConsoleMessenger.Extensions;
using ConsoleMessenger.Services;
using ConsoleMessenger.Services.Abstraction;
using MessengerApp.Core.DTO.Message;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable All

namespace ConsoleMessenger
{
    class Program
    {
        static HubConnection HubConnection { get; set; }

        private static IAccountService _accountService;
        private static IChatService _chatService;
        private static IContactService _contactService;
        private static IMessageService _messageService;
        
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName.AppSettings))
                .Build();

            var provider = new ServiceCollection()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IChatService, ChatService>()
                .AddScoped<IContactService, ContactService>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<ITokenStorage, JsonFileTokenStorage>()
                .AddScoped<AuthInterceptor>()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IConfiguration>(configuration)
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

            var token = await _accountService.GetAccessAndRefreshTokensAsync(Dto.LogInUserData());
            
            HubConnection = new HubConnectionBuilder()
                .WithUrl(Api.BaseUrl + "/chat",
                    options => { options.AccessTokenProvider = () => Task.FromResult(token.Token); })
                .Build();

            HubConnection.On<MessageDto>("Send",
                (messageDto) =>
                {
                    Console.WriteLine($"UserId: {messageDto.UserId}, Message: {messageDto.Body} ({messageDto.DateTime})");
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

        static async Task Do()
        {
            
        }
    }
}