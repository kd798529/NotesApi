using NotesApi.Models;

namespace NotesApi.Repositories;

public interface INoteRepository
{
    Task<List<Note>> GetAll();

    Task<Note?> GetById(int id);

    Task Add(Note note);

    Task Update(Note note);

    Task Delete(Note note);
}