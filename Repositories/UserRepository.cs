namespace NotesApi.Repositories;

using Microsoft.EntityFrameworkCore;
using NotesApi.Data;
using NotesApi.Models;

public class UserRepository : IUserRepository
{

    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetAll()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetById(int id)
    {
        return await _dbContext.Users
            .Include(user => user.Notes)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task Update(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}