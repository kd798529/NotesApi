using NotesApi.Dtos;
using NotesApi.Models;
using NotesApi.Repositories;

namespace NotesApi.Services;

public class NoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IUserRepository _userRepository;

    public NoteService(INoteRepository noteRepository, IUserRepository userRepository)
    {
        _noteRepository = noteRepository;
        _userRepository = userRepository;
    }

    public async Task<List<Note>> GetAll()
    {
        return await _noteRepository.GetAll();
    }



    public async Task<Note?> GetById(int id)
    {
        return await _noteRepository.GetById(id);
    }

    public async Task<bool> Create(Note note)
    {
        if (note.UserId is not null)
        {
            var user = await _userRepository.GetById(note.UserId.Value);

            if (user is null)
            {
                return false;
            }
        }
        await _noteRepository.Add(note);

        return true;
    }

    public async Task<Note?> Update(int id, UpdateNoteDto dto)
    {
        var note = await _noteRepository.GetById(id);

        if (note is null)
        {
            return null;
        }

        note.Title = dto.Title;
        note.Content = dto.Content;

        await _noteRepository.Update(note);

        return note;
    }

    public async Task<bool> Delete(int id)
    {
        var note = await _noteRepository.GetById(id);

        if (note is null)
        {
            return false;
        }

        await _noteRepository.Delete(note);

        return true;
    }
}