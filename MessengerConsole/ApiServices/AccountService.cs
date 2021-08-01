using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MessengerApp.Core.DTO.Authorization;
using MessengerApp.Core.DTO.User;
using MessengerConsole.ApiServices.Abstraction;
using MessengerConsole.Constants;
using MessengerConsole.DTO;
using MessengerConsole.DTO.Authorization;
using MessengerConsole.DTO.Authorization.Reset;
using MessengerConsole.DTO.User;
using MessengerConsole.ResultModel;

namespace MessengerConsole.ApiServices
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        private readonly ITokenStorage _jsonFileTokenStorage;

        public AccountService(IHttpClientFactory httpClientFactory, ITokenStorage tokenStorage)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(AccountService));

            _jsonFileTokenStorage = tokenStorage;
        }

        public async Task<Result> CreateUserAndSendEmailTokenAsync(RegisterDto register)
        {
            try
            {
                var urn = $"{ApiServiceRoute.Account}/register/token";

                var json = JsonSerializer.Serialize(register);

                var response = await _httpClient
                    .PostAsync(
                        urn,
                        new StringContent(json, Encoding.UTF8, FileType.Json)
                    );

                response.EnsureSuccessStatusCode();
                
                return Result.CreateSuccess();
            }
            catch (Exception e)
            {
                return Result.CreateFailed("UNEXPECTED", e);
            }
        }

        public async Task<string> ConfirmRegistrationWithTokenAsync(
            string token, string userId)
        {
            var urn = $"{ApiServiceRoute.Account}/{token}/{userId}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<string>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<TokenDto> GetAccessAndRefreshTokensAsync(
            LogInUserDto logInUserDto)
        {
            var json = JsonSerializer.Serialize(logInUserDto);
            
            var response = await _httpClient
                .PostAsync(
                    $"{ApiServiceRoute.Account}/login",
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();

            var responseToken = JsonSerializer
                .Deserialize<TokenDto>(
                    await response.Content.ReadAsStringAsync()
                );
            
            await _jsonFileTokenStorage.SaveTokenAsync(responseToken);

            return responseToken;
        }
        
        public async Task<TokenDto> RefreshAccessToken(
            RefreshTokenDto refreshTokenDto)
        {
            var json = JsonSerializer.Serialize(refreshTokenDto);

            var response = await _httpClient
                .PutAsync(
                    $"{ApiServiceRoute.Account}/refresh-token",
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();

            var responseToken = JsonSerializer
                .Deserialize<TokenDto>(
                    await response.Content.ReadAsStringAsync()
                );

            return responseToken;
        }

        public async Task<ProfileDto> GetProfileAsync(
            int userId)
        {
            var urn = $"{ApiServiceRoute.Account}/{userId}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ProfileDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<UserDto> EditUserAsync(
            int id, EditUserDto editUserDto
        )
        {
            var json = JsonSerializer.Serialize(editUserDto);

            var response = await _httpClient
                .PutAsync(
                    $"{ApiServiceRoute.Account}/{id}",
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<UserDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task SendEmailResetTokenAsync(
            int userId, ResetEmailDto resetEmailDto)
        {
            var json = JsonSerializer.Serialize(resetEmailDto);

            var response = await _httpClient
                .PutAsync(
                    $"{ApiServiceRoute.Account}/{userId}",
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();
        }

        public async Task ResetEmailAsync(
            int userId, string token, string newEmail)
        {
            var urn = $"api/account/{userId}/{token}/{newEmail}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();
        }

        public async Task SendPasswordResetTokenAsync(
            ResetPasswordDto resetPasswordDto)
        {
            var urn = $"{ApiServiceRoute.Account}";

            var json = JsonSerializer.Serialize(resetPasswordDto);

            var response = await _httpClient
                .PutAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();
        }

        public async Task ResetPasswordAsync(
            TokenPasswordDto tokenPasswordDto)
        {
            var urn = $"{ApiServiceRoute.Account}";

            var json = JsonSerializer.Serialize(tokenPasswordDto);

            var response = await _httpClient
                .PutAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();
        }

        public async Task<Pager<UserDto>> GetUsersInChatAsync(
            int userId, int chatId, string search, int page, int items)
        {
            var urn = $"{ApiServiceRoute.Account}/{userId}/{chatId}";

            if (!string.IsNullOrWhiteSpace(search))
                urn += $"?{nameof(search)}={search}";
            
            urn += $"?{nameof(page)}={page}";

            urn += $"?{nameof(items)}={items}";
            
            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Pager<UserDto>>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }
    }
}