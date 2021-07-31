using MessengerConsole.DTO.Authorization;

namespace MessengerConsole.Constants
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