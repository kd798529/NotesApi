namespace NotesApi.Services;

using NotesApi.Repositories;
using NotesApi.Models;
using NotesApi.Dtos;
using Microsoft.AspNetCore.Identity;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User?> GetById(int Id)
    {
        return await _userRepository.GetById(Id);
    }

    public async Task<bool> Create(User user, string password)
    {
        var existingUser = await _userRepository.GetByEmail(user.Email);

        if (existingUser is not null)
        {
            return false;
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        await _userRepository.Add(user);
        return true;
    }

    public async Task<User?> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        if (user is null || user.PasswordHash is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return user;
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