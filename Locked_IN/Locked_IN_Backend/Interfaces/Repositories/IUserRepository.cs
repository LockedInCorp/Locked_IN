using Locked_IN_Backend.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Locked_IN_Backend.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUserById(int id);
    Task AddUser(User user);
    Task UpdateUser(User user);
    Task DeleteUser(User user);
    
    // Identity methods
    Task<IdentityResult> CreateUserAsync(User user, string password);
    Task<User?> FindByIdAsync(string userId);
    Task<User?> FindByNameAsync(string userName);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent, bool lockoutOnFailure);
    Task SignOutAsync();
}