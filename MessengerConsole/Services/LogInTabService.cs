using System;
using MessengerConsole.Extensions;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Services
{
    public class LogInTabService : ILogInTabService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _config;
        private readonly IConsoleService _console;
        private readonly ILogInTabService _logInTab;

        public LogInTabService(IMemoryCache cache, IConfiguration config, IConsoleService console, ILogInTabService logInTab)
        {
            _cache = cache;
            _config = config;
            _console = console;
            _logInTab = logInTab;
        }
    }
}