using System.ComponentModel.DataAnnotations;

namespace NotesApi.Dtos;

public class UpdateUserDto
{
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
}