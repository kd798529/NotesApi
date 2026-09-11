using System.ComponentModel.DataAnnotations;

namespace NotesApi.Dtos;

public class CreateUserDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}