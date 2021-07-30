using System.Threading.Tasks;
using ConsoleMessenger.DTO;
using ConsoleMessenger.DTO.Authorization;
using ConsoleMessenger.DTO.Authorization.Reset;
using ConsoleMessenger.DTO.User;
using MessengerApp.Core.DTO.Authorization;
using MessengerApp.Core.DTO.User;

namespace ConsoleMessenger.Services.Abstraction
{
    public interface IAccountService
    {
        Task CreateUserAndSendEmailTokenAsync(
            RegisterDto register);

        Task<string> ConfirmRegistrationWithTokenAsync(
            string token, string userId);
        
        Task<TokenDto> GetAccessAndRefreshTokensAsync(
            LogInUserDto userInput);

        Task<TokenDto> RefreshAccessToken(
            RefreshTokenDto refreshTokenDto);

        Task<ProfileDto> GetProfileAsync(
            int userId);

        Task<UserDto> EditUserAsync(
            int id, EditUserDto editUserDto);
        
        Task SendEmailResetTokenAsync(
            int userId, ResetEmailDto resetEmailDto);

        Task ResetEmailAsync(
            int userId, string token, string newEmail);

        Task SendPasswordResetTokenAsync(
            ResetPasswordDto resetPasswordDto);

        Task ResetPasswordAsync(
            TokenPasswordDto tokenPasswordDto);

        Task<Pager<UserDto>> GetUsersInChatAsync(
            int userId, int chatId, string? search, int page, int items);
    }
}