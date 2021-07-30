using System.ComponentModel.DataAnnotations;

namespace ConsoleMessenger.DTO.Authorization.Reset
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