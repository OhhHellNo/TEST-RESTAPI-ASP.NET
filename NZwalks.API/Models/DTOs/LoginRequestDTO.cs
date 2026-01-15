using System.ComponentModel.DataAnnotations;

namespace NZwalks.API.Models.DTOs
{
    public class LoginRequestDTO
    {
        [DataType(DataType.EmailAddress)]
        public required string UserName { get; set; }


        [DataType(DataType.Password)]

        public required string Password { get; set; }


    }
}
