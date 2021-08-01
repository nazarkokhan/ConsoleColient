using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessengerConsole.Extensions;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly IConfiguration _config;

        private readonly IColorService _color;

        public ConsoleService(IConfiguration config, IColorService color)
        {
            _config = config;
            _color = color;
        }

        public void SetupConsole()
        {
            _color.SetDefaultConsoleColor();
            Console.Clear();
        }
        
        public void Commands()
        {
            Console.SetCursorPosition(70, 0);

            WriteTab(nameof(Commands));
            
            _color.SetCommandsConsoleColor();

            foreach (var cmd in _config.GetCommands())
            {
                Console.CursorLeft = 70;
                Console.WriteLine(cmd);
            }

            _color.SetDefaultConsoleColor();
            
            Console.SetCursorPosition(0, 0);
            
            Console.SetCursorPosition(57, 24);
            Console.WriteLine("Last input: ");
            
            Console.SetCursorPosition(57, 25);
            Console.WriteLine("Write cmd: ");
            
            ReadCmd();
            ReadCmd();
            ReadCmd();
            ReadCmd();
            ReadCmd();
            ReadCmd();
            ReadCmd();
            ReadCmd();
        }
        

        public void WriteTab(string tab)
        {
            _color.SetTabConsoleColor();
            Console.WriteLine($"[{tab.ToUpper()}]\n"); //TODO put in frameSetDefaultConsoleColor();
        }

        public void ReadCmd()
        {
            Console.SetCursorPosition(70, 25);

            var readLine = Console.ReadLine();


            Console.WriteLine("                         ");
            Console.SetCursorPosition(70, 24);

            Console.WriteLine(readLine);
            Console.CursorLeft = 70;
            
            Console.WriteLine("                         ");
        }
    }
}