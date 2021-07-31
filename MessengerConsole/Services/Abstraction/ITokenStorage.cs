using System.Threading.Tasks;
using MessengerConsole.DTO.Authorization;

namespace MessengerConsole.Services.Abstraction
{
    public interface ITokenStorage
    {
        Task<TokenDto> GetToken();
        
        Task SaveToken(TokenDto tokenDto);
    }
}