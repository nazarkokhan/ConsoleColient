using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessengerConsole.ApiServices.Abstraction;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Caching.Memory;

namespace MessengerConsole
{
    public class Client
    {
        // static HubConnection HubConnection { get; set; }
        private readonly IMemoryCache _cache;
        private readonly IConsoleService _console;
        private readonly IColorService _color;

        public Client(IMemoryCache cache, IConsoleService console, IColorService color)
        {
            _cache = cache;
            _console = console;
            _color = color;
        }

        public async Task RunAsync()
        {
            _console.SetupConsole();
            _console.Commands();

            while (true)
            {
                ChooseCommand("/login");
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public string ChooseCommand(string command) => command switch
        {
            "/home" => "a",
            "/login" => "b",
            "/register" => "c",
            _ => "x"
        };
    }
}