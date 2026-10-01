namespace NotesApi.Services;

using NotesApi.Repositories;
using NotesApi.Models;
using NotesApi.Dtos;

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

    public async Task<User?> Update(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            return null;
        }

        user.Name = dto.Name;
        user.Email = dto.Email;

        await _userRepository.Update(user);

        return user;
    }

    public async Task<bool> Delete(int id)
    {
        var user = await _userRepository.GetById(id);

        if (user is null)
        {
            return false;
        }

        await _userRepository.Delete(user);
        return true;
    }
}