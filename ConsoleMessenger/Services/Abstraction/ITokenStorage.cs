using System.Threading.Tasks;
using ConsoleMessenger.DTO.Authorization;

namespace ConsoleMessenger.Services.Abstraction
{
    public interface ITokenStorage
    {
        Task<TokenDto> GetToken();
        
        Task SaveToken(TokenDto tokenDto);
    }
}