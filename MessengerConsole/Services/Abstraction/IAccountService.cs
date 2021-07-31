using System.Threading.Tasks;
using MessengerApp.Core.DTO.Authorization;
using MessengerApp.Core.DTO.User;
using MessengerConsole.DTO;
using MessengerConsole.DTO.Authorization;
using MessengerConsole.DTO.Authorization.Reset;
using MessengerConsole.DTO.User;
using MessengerConsole.ResultModel;

namespace MessengerConsole.Services.Abstraction
{
    public interface IAccountService
    {
        Task<Result> CreateUserAndSendEmailTokenAsync(RegisterDto register);

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