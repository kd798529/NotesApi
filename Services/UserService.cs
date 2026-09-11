namespace NotesApi.Services;

using NotesApi.Repositories;
using NotesApi.Models;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User?> GetById(int Id)
    {
        return await _userRepository.GetById(Id);
    }

    public async Task Create(User user)
    {
        await _userRepository.Add(user);
    }
}