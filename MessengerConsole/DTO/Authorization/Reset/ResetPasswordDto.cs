using System.ComponentModel.DataAnnotations;

namespace MessengerConsole.DTO.Authorization.Reset
{
    public class ResetPasswordDto
    {
        public ResetPasswordDto(string email)
        {
            Email = email;
        }

        [DataType(DataType.EmailAddress)] 
        public string Email { get; }
    }
}