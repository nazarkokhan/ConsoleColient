using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MessengerConsole.ConfigurationOptions;
using MessengerConsole.Extensions;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Options;

namespace MessengerConsole.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly Dictionary<string, string> _commands;
        private readonly CursorOptions _cursor;
        private readonly IColorService _colorService;

        public ConsoleService(
            IOptions<CommandOptions> optionsExample, 
            IOptions<CursorOptions> cursor, 
            IColorService colorService)
        {
            _commands = optionsExample.Value.Commands;
            _cursor = cursor.Value;
            _colorService = colorService;
        }

        public void CleanConsole()
        {
            _colorService.SetDefaultConsoleColor();
            Console.Clear();
        }
        
        public void AvailableCommands()
        {
            WriteTab(nameof(AvailableCommands));
            
            _colorService.SetCommandsConsoleColor();
            
            foreach (var (key, value) in _commands)
            {
                Console.CursorLeft = _cursor.TabL;
                Console.Write($"{key}:");
                Console.CursorLeft = _cursor.TabL + 15;
                Console.WriteLine($"{value}");
            }

            _colorService.SetDefaultConsoleColor();
            
            ReadCommand(_cursor.ReadCmdL, _cursor.ReadCmdT);
        }

        public void ReadCommand(int l, int t)
        {
            Console.SetCursorPosition(l - 15, t);
            Console.WriteLine("Last input: ");
            
            Console.SetCursorPosition(l - 15, t + 1);
            Console.WriteLine("Write cmd: ");

            while (true)
            {
                ReadLine(l + 15, t);
            }
        }
        

        public void WriteTab(string tab)
        {
            Console.SetCursorPosition(_cursor.TabL, _cursor.TabT);

            _colorService.SetTabConsoleColor();
            
            Console.WriteLine($"[{tab.ToUpper()}]\n");
        }

        public void ReadLine(int l, int t)
        {
            Console.SetCursorPosition(l, t);

            var readLine = Console.ReadLine();

            Console.WriteLine("                                      ");
            Console.SetCursorPosition(l, t + 1);

            Console.WriteLine(readLine + "                           ");
            Console.CursorLeft = l;
            
            Console.WriteLine("                                      ");
        }
    }
}