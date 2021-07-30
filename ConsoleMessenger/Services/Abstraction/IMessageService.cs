using System.Threading.Tasks;
using ConsoleMessenger.DTO;
using ConsoleMessenger.DTO.Message;
using MessengerApp.Core.DTO.Message;

namespace ConsoleMessenger.Services.Abstraction
{
    public interface IMessageService
    {
        Task<Pager<MessageDto>> GetMessagesInChatPageAsync(
            int userId, int chatId, string search = null, int page = 1, int items = 5);

        Task<MessageDto> CreateMessageAsync(
            int userId, int chatId, CreateMessageDto createMessageDto);

        Task<MessageDto> EditMessageAsync(
            int userId, EditMessageDto editMessageDto);

        Task DeleteMessageAsync(
            int userId, long messageId);
    }
}