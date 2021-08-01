using System;
using MessengerConsole.DTO.Authorization;

namespace MessengerConsole.Extensions
{
    public static class Extension
    {
        public static bool IsExpired(this TokenDto tokenDto)
        {
            return DateTime.UtcNow >= tokenDto.TokenExpTime;
        }
        
        public static ConsoleColor ToConsoleColor(this string enumValue)
        {
            return Enum.Parse<ConsoleColor>(enumValue);
        }
    }
}