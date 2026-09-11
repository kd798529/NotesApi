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

    public async Task<User?> GetById(int Id)
    {
        return await _dbContext.Users.FindAsync(Id);
    }
}