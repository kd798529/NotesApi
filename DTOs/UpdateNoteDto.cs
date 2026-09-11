using System.ComponentModel.DataAnnotations;

namespace NotesApi.Dtos;

public class UpdateNoteDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = "";
}