namespace NotesApi.Repositories;

using NotesApi.Models;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> GetById(int Id);
    Task<User?> GetByEmail(string email);

    Task Add(User user);
    Task Update(User user);
    Task Delete(User user);
}