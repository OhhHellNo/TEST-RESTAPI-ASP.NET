using System.ComponentModel.DataAnnotations;

public class RegisterRequestDTO
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    public required string Username { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public string[] Roles { get; set; } = [];
}
