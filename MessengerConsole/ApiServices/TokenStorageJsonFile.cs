using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MessengerConsole.ApiServices.Abstraction;
using MessengerConsole.Constants;
using MessengerConsole.DTO.Authorization;

namespace MessengerConsole.ApiServices
{
    public class TokenStorageJsonFile : ITokenStorage
    {
        private readonly string _path;

        private TokenDto _tokenDto;

        public TokenStorageJsonFile()
        {
            _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName.TokenStorage);
        }

        public async Task<TokenDto> GetTokenAsync()
        {
            if (_tokenDto is not null)
            {
                return _tokenDto;
            }

            if (File.Exists(_path))
            {
                return _tokenDto = JsonSerializer.Deserialize<TokenDto>(
                    await File.ReadAllTextAsync(_path)
                );
            }

            await SaveTokenAsync(null);

            return _tokenDto;
        }

        public async Task SaveTokenAsync(TokenDto tokenDto)
        {
            tokenDto ??= new TokenDto(
                "NULL",
                DateTime.UnixEpoch,
                "NULL",
                DateTime.UnixEpoch
            );
            
            await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(tokenDto));
            
            _tokenDto = tokenDto;
        }
    }
}