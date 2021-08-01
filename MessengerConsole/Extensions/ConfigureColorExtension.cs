using System;
using Microsoft.Extensions.Configuration;

namespace MessengerConsole.Extensions
{
    public static class ConfigureColorExtension
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static ConsoleColor GetConsoleColor(this IConfiguration configuration, string of)
        {
            return configuration.GetSection("ConsoleColor")[of].ToConsoleColor();
        }
        
        public static ConsoleColor GetConsoleColorDefaultBack(this IConfiguration configuration)
        {
            return configuration.GetConsoleColor("DefaultBack");
        }
        
        public static ConsoleColor GetConsoleColorDefaultFore(this IConfiguration configuration)
        {
            return configuration.GetConsoleColor("DefaultFore");
        }
        
        public static ConsoleColor GetConsoleColorCommandBack(this IConfiguration configuration)
        {
            return configuration.GetConsoleColor("CommandBack");
        }
        
        public static ConsoleColor GetConsoleColorCommandFore(this IConfiguration configuration)
        {
            return configuration.GetConsoleColor("CommandFore");
        }
        
        public static ConsoleColor GetConsoleColorTabBack(this IConfiguration configuration)
        {
            return configuration.GetConsoleColor("TabBack");
        }
    }
}