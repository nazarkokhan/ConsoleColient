using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleMessenger.Constants;
using ConsoleMessenger.DTO;
using ConsoleMessenger.Services.Abstraction;
using MessengerApp.Core.DTO.Contact;

namespace ConsoleMessenger.Services
{
    public class ContactService : IContactService
    {
        private readonly HttpClient _httpClient;

        public ContactService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Client.AuthClient);
        }

        public async Task<Pager<ContactDto>> GetContactsPageAsync(
            int userId, string search, int page, int items
        )
        {
            var urn = $"api/message/{userId}";

            if (!string.IsNullOrWhiteSpace(search))
                urn += $"?{nameof(search)}={search}";

            urn += $"?{nameof(page)}={page}";

            urn += $"?{nameof(items)}={items}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Pager<ContactDto>>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<ContactDto> GetContactAsync(
            int userId, int userContactId
        )
        {
            var urn = $"api/message/{userId}/{userContactId}";

            var response =
                await _httpClient.GetAsync(urn);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ContactDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<ContactDto> CreateContactAsync(
            int userId, CreateContactDto createContactDto
        )
        {
            var urn = $"api/message/{userId}";

            var json = JsonSerializer.Serialize(createContactDto);

            var response = await _httpClient
                .PostAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ContactDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task<ContactDto> EditContactAsync(
            int userId, EditContactDto editContactDto
        )
        {
            var urn = $"api/message/{userId}";

            var json = JsonSerializer.Serialize(editContactDto);

            var response = await _httpClient
                .PutAsync(
                    urn,
                    new StringContent(json, Encoding.UTF8, "application/json")
                );

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ContactDto>(responseString,
                new JsonSerializerOptions {Converters = {new JsonStringEnumConverter()}}
            );
        }

        public async Task DeleteContactAsync(
            int userId, int contactId
        )
        {
            var urn = $"api/message/{userId}/{contactId}";

            var response = await _httpClient
                .DeleteAsync(
                    urn
                );

            response.EnsureSuccessStatusCode();
        }
    }
}