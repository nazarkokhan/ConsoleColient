using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MessengerConsole.DTO.Authorization;
using MessengerConsole.Services.Abstraction;

namespace MessengerConsole.Services
{
    public class JsonFileTokenStorage : ITokenStorage
    {
        private const string FileName = "tokenStorage.json";

        private readonly string _path;
        private TokenDto _tokenDto;

        public JsonFileTokenStorage()
        {
            _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
        }

        public async Task<TokenDto> GetToken()
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

            await SaveToken(new TokenDto(
                "DEFAULT_TOKEN",
                DateTime.UnixEpoch,
                "DEFAULT_REFRESH_TOKEN",
                DateTime.UnixEpoch)
            );

            return _tokenDto;
        }

        public async Task SaveToken(TokenDto tokenDto)
        {
            await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(tokenDto));
            _tokenDto = tokenDto;
        }
    }
}