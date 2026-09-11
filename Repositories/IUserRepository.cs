namespace NotesApi.Repositories;

using NotesApi.Models;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> GetById(int Id);
    Task Add(User user);
}