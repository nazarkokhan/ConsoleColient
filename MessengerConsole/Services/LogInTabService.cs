using System;
using MessengerConsole.Extensions;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Services
{
    public class LogInTabService : ILogInTabService
    {
        private readonly IConsoleService _console;

        public LogInTabService(
            IConsoleService console)
        {
            _console = console;
        }
    }
}