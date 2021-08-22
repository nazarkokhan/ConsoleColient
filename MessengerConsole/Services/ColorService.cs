using System;
using MessengerConsole.ConfigurationOptions;
using MessengerConsole.Extensions;
using MessengerConsole.Services.Abstraction;
using Microsoft.Extensions.Options;

namespace MessengerConsole.Services
{
    public class ColorService : IColorService
    {
        private readonly ColorOptions _colorOptions;

        public ColorService(
            IOptions<ColorOptions> colorOptions)
        {
            _colorOptions = colorOptions.Value;
        }
        
        public void SetDefaultConsoleColor()
        {
            Console.BackgroundColor = _colorOptions.DefaultBack.ToConsoleColor();
            Console.ForegroundColor = _colorOptions.DefaultFore.ToConsoleColor();
        }

        public void SetCommandsConsoleColor()
        {
            Console.BackgroundColor = _colorOptions.CommandBack.ToConsoleColor();
            Console.ForegroundColor = _colorOptions.CommandFore.ToConsoleColor();
        }
        
        public void SetTabConsoleColor()
        {
            Console.BackgroundColor = _colorOptions.TabBack.ToConsoleColor();
        }
    }
}