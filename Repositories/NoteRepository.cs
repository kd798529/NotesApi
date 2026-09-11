using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly AppDbContext _dbContext;

    public NoteRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Note>> GetAll()
    {
        return await _dbContext.Notes.ToListAsync();
    }

    public async Task<Note?> GetById(int id)
    {
        return await _dbContext.Notes.FindAsync(id);
    }

    public async Task Add(Note note)
    {
        _dbContext.Notes.Add(note);

        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Note note)
    {
        _dbContext.Notes.Update(note);

        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Note note)
    {
        _dbContext.Notes.Remove(note);

        await _dbContext.SaveChangesAsync();
    }
}