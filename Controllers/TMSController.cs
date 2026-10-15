using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;
using static PetsApi.Common.Enums;

public class TMSController : ControllerBase
{
    private readonly SuppliersContext _context;
    public User _user;
    public Client _client;
    public User CurrentUser
    {
      get
      {
        if (this._user == null) {
          var idClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
          if (idClaim == null) return null;
          int userId = int.Parse(idClaim.Value);
          this._user = _context.Users.FirstOrDefault(user => user.Id == userId);
        }
        return this._user;
      }
    }
    public Client CurrentClient
    {
      get
      {
        if (this._client == null) {
          var idClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
          if (idClaim == null) return null;
          int userId = int.Parse(idClaim.Value);
          this._client = _context.Clients.FirstOrDefault(client => client.UserId == userId);
        }
        return this._client;
      }
    }
    
    public UserRole? CurrentRole
    {
      get
      {
        if (this.CurrentUser == null) return null;
        return (UserRole)this.CurrentUser.RoleId;
      }
    }
    public bool IsAdmin
    {
      get
      {
        return this.CurrentRole == UserRole.Admin;
      }
    }
    public bool IsSupplier
    {
      get
      {
        return this.CurrentRole == UserRole.Supplier;
      }
    }
    public bool IsAuthorizer
    {
      get
      {
        return this.CurrentRole == UserRole.Authorizer;
      }
    }
    public bool IsClient
    {
      get
      {
        return this.CurrentRole == UserRole.Client;
      }
    }

    public TMSController(SuppliersContext context) {
      _context = context;
    }
}
