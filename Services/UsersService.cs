
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using static PetsApi.Common.Enums;

namespace PetsApi.Services
{
  public interface IUsersService
  {
    Task<List<User>> GetByRol(UserRole role);
    List<User> MapUsers(List<User> users);
    User MapUser(User user);
  }
  public class UsersService : IUsersService
  {
    private readonly SuppliersContext _context;
    public UsersService(SuppliersContext context)
    {
      _context = context;
    }

    public List<User> MapUsers(List<User> users)
    {
      if (users == null) return null;
      return users.Select(user => MapUser(user)).ToList();
    }

    public User MapUser(User user)
    {
      var newUser = new User() {
        Id = user.Id,
        Name = user.Name,
        RoleId = user.RoleId,
        Active = user.Active,
        Email = user.Email,
        CreatedDate = user.CreatedDate,
        UpdatedDate = user.UpdatedDate,
      };
      return newUser;
    }

    public async Task<List<User>> GetByRol(UserRole role)
    {
      var users = await _context.Users
        .Where(user => user.RoleId == (int)role)
        .ToListAsync();
      return MapUsers(users);
    }
  }
}
