using System;
using ConsoleMessenger.DTO.Authorization;

namespace ConsoleMessenger.Extensions
{
    public static class Extension
    {
        public static bool IsExpired(this TokenDto tokenDto)
        {
            return DateTime.UtcNow >= tokenDto.TokenExpTime;
        }
    }
}