using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleMessenger.Constants;
using ConsoleMessenger.DTO;
using ConsoleMessenger.DTO.Message;
using ConsoleMessenger.Services.Abstraction;
using MessengerApp.Core.DTO.Message;

namespace ConsoleMessenger.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Client.AuthClient);
        }

        public async Task<Pager<MessageDto>> GetMessagesInChatPageAsync(
            int userId, int chatId, string search, int page, int items
        )
        {
            var urn = $"api/message/{userId}/{chatId}";

            if (!string.IsNullOrWhiteSpace(search))
                urn += $"?{nameof(search)}={search}";

            urn += $"?{nameof(page)}={page}";

            urn += $"?{nameof(items)}={items}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Pager<MessageDto>>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<MessageDto> CreateMessageAsync(
            int userId, int chatId, CreateMessageDto createMessageDto
        )
        {
            var urn = $"api/message/{userId}/{chatId}";

            var json = JsonSerializer.Serialize(createMessageDto);

            var response = await _httpClient
                .PostAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<MessageDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<MessageDto> EditMessageAsync(
            int userId, EditMessageDto editMessageDto
        )
        {
            var urn = $"api/message/{userId}";

            var json = JsonSerializer.Serialize(editMessageDto);

            var response = await _httpClient
                .PutAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<MessageDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task DeleteMessageAsync(
            int userId, long messageId
        )
        {
            var urn = $"api/message/{userId}/{messageId}";

            var response = await _httpClient
                .DeleteAsync(
                    urn
                );

            response.EnsureSuccessStatusCode();
        }
    }
}