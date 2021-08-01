using System;
using MessengerConsole.Extensions;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Services
{
    public class ColorService : IColorService
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IConfiguration _config;

        private readonly ConsoleColor _defB;
        private readonly ConsoleColor _defF;

        private readonly ConsoleColor _cmdB;
        private readonly ConsoleColor _cmdF;

        private readonly ConsoleColor _tabB;

        public ColorService(IConfiguration config)
        {
            _config = config;

            _defB = _config.GetConsoleColorDefaultBack();
            _defF = _config.GetConsoleColorDefaultFore();

            _cmdB = _config.GetConsoleColorCommandBack();
            _cmdF = _config.GetConsoleColorCommandFore();

            _tabB = _config.GetConsoleColorTabBack();
        }
        
        public void SetDefaultConsoleColor()
        {
            Console.BackgroundColor = _defB;
            Console.ForegroundColor = _defF;
        }

        public void SetCommandsConsoleColor()
        {
            Console.BackgroundColor = _cmdB;
            Console.ForegroundColor = _cmdF;
        }
        
        public void SetTabConsoleColor()
        {
            Console.BackgroundColor = _tabB;
        }
    }
}