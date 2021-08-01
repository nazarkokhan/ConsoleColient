using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MessengerApp.Core.DTO.Chat;
using MessengerConsole.ApiServices.Abstraction;
using MessengerConsole.Constants;
using MessengerConsole.DTO;

namespace MessengerConsole.ApiServices
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(ClientName.Authorization);
        }

        public async Task<Pager<ChatDto>> GetChatsPageAsync(
            string search, int page, int items
        )
        {
            var urn = $"{ApiServiceRoute.Chat}";

            if (!string.IsNullOrWhiteSpace(search))
                urn += $"?{nameof(search)}={search}";

            urn += $"?{nameof(page)}={page}";

            urn += $"?{nameof(items)}={items}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Pager<ChatDto>>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<Pager<ChatDto>> GetUserChatsPageAsync(
            int userId, string search, int page, int items
        )
        {
            var urn = $"{ApiServiceRoute.Chat}/{userId}";

            if (!string.IsNullOrWhiteSpace(search))
                urn += $"?{nameof(search)}={search}";

            urn += $"?{nameof(page)}={page}";

            urn += $"?{nameof(items)}={items}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Pager<ChatDto>>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<ChatDto> AddUserInChatAsync(
            int userId, AddUserInChatDto addUserInChatDto
        )
        {
            var urn = $"{ApiServiceRoute.Chat}/{userId}";

            var json = JsonSerializer.Serialize(addUserInChatDto);

            var response = await _httpClient
                .PostAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ChatDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<ChatDto> GetChatAsync(
            int id
        )
        {
            var urn = $"{ApiServiceRoute.Chat}/{id}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ChatDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<ChatDto> CreateChatAsync(
            int userId, CreateChatDto createChatDto
        )
        {
            var urn = $"{ApiServiceRoute.Chat}/{userId}";

            var json = JsonSerializer.Serialize(createChatDto);

            var response = await _httpClient
                .PostAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ChatDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<ChatDto> EditChatAsync(
            int userId,
            EditChatDto editChatDto
        )
        {
            var urn = $"{ApiServiceRoute.Chat}/{userId}";

            var json = JsonSerializer.Serialize(editChatDto);

            var response = await _httpClient
                .PutAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, FileType.Json)
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ChatDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task DeleteChatAsync(
            int userId,
            int id
        )
        {
            var urn = $"{ApiServiceRoute.Chat}/{userId}/{id}";

            var response = await _httpClient
                .DeleteAsync(
                    urn
                );

            response.EnsureSuccessStatusCode();
        }
    }
}