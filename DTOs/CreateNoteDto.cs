using System.ComponentModel.DataAnnotations;

namespace NotesApi.Dtos;

public class CreateNoteDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(500)]
    public string Title { get; set; } = "";

    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = "";

    public int? UserId { get; set; }


}