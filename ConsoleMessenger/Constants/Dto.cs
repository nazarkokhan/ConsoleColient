using ConsoleMessenger.DTO.Authorization;

namespace ConsoleMessenger.Constants
{
    public static class Dto
    {
        public static LogInUserDto LogInUserData()
        {
            return new(
                LoginData.Email,
                LoginData.Password
            );
        }
    }
}