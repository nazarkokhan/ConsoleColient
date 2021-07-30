using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleMessenger.Constants;
using ConsoleMessenger.DTO;
using ConsoleMessenger.DTO.Authorization;
using ConsoleMessenger.DTO.Authorization.Reset;
using ConsoleMessenger.DTO.User;
using ConsoleMessenger.Services.Abstraction;
using MessengerApp.Core.DTO.Authorization;
using MessengerApp.Core.DTO.User;

namespace ConsoleMessenger.Services
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

        public async Task CreateUserAndSendEmailTokenAsync(
            RegisterDto register
        )
        {
            var urn = $"api/account";

            var json = JsonSerializer.Serialize(register);

            var response = await _httpClient
                .PostAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

            response.EnsureSuccessStatusCode();
        }

        public async Task<string> ConfirmRegistrationWithTokenAsync(
            string token, string userId)
        {
            var urn = $"api/account/{token}/{userId}";

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
                    "api/account/login",
                    new StringContent(json, Encoding.UTF8, FileName.JsonType)
                );

            response.EnsureSuccessStatusCode();

            var responseToken = JsonSerializer
                .Deserialize<TokenDto>(
                    await response.Content.ReadAsStringAsync()
                );
            
            await _jsonFileTokenStorage.SaveToken(responseToken);

            return responseToken;
        }
        
        public async Task<TokenDto> RefreshAccessToken(
            RefreshTokenDto refreshTokenDto)
        {
            var json = JsonSerializer.Serialize(refreshTokenDto);

            var response = await _httpClient
                .PutAsync(
                    "api/account/refresh-token",
                    new StringContent(json, Encoding.UTF8, FileName.JsonType)
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
            var urn = $"api/account/{userId}";

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
                    $"api/account/{id}",
                    new StringContent(json, Encoding.UTF8, "application/json")
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
                    $"api/account/{userId}",
                    new StringContent(json, Encoding.UTF8, "application/json")
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
            var urn = $"api/account";

            var json = JsonSerializer.Serialize(resetPasswordDto);

            var response = await _httpClient
                .PutAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

            response.EnsureSuccessStatusCode();
        }

        public async Task ResetPasswordAsync(
            TokenPasswordDto tokenPasswordDto)
        {
            var urn = $"api/account";

            var json = JsonSerializer.Serialize(tokenPasswordDto);

            var response = await _httpClient
                .PutAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

            response.EnsureSuccessStatusCode();
        }

        public async Task<Pager<UserDto>> GetUsersInChatAsync(
            int userId, int chatId, string search, int page, int items)
        {
            var urn = $"api/account/{userId}/{chatId}";

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