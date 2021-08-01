using System.Threading.Tasks;
using MessengerConsole.DTO.Authorization;

namespace MessengerConsole.ApiServices.Abstraction
{
    public interface ITokenStorage
    {
        Task<TokenDto> GetTokenAsync();
        
        Task SaveTokenAsync(TokenDto tokenDto);
    }
}