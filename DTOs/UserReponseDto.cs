namespace NotesApi.Dtos;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public List<NoteResponseDto> Notes { get; set; } = new();
}