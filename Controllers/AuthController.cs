using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetsApi.Services;
using PetsApi.Models;
using PetsApi.Models.Custom;

namespace PetsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : TMSController
    {
        private readonly IConfiguration _configuration;
        private readonly SuppliersContext _context;
        private readonly IUsersService _usersService;

        public AuthController(IConfiguration configuration, SuppliersContext context, IUsersService usersService): base(context)
        {
            _configuration = configuration;
            _context = context;
            _usersService = usersService;
        }

        // GET: api/Auth/me
        [HttpGet("me", Name = "GetMe")]
        [Authorize]
        public ActionResult<UserDto> Get()
        {
            User user = this.CurrentUser;

            if (user == null)
            {
                return NotFound();
            }

            if (!user.Active) {
                return Unauthorized();
            }

            var newUser = new UserDto() {
                Id = user.Id,
                Name = user.Name,
                // Password = user.Password,
                RoleId = user.RoleId,
                Active = user.Active,
                Email = user.Email
                // UserPlants = user.UserPlants,
            };

            return newUser;
        }

        // POST: api/Auth
        [HttpPost]
        public ActionResult<string> Post([FromBody] User user)
        {
            user = _context.Users.FirstOrDefault(u => 
                u.Email == user.Email 
                && u.Password == user.Password);

            if (user == null)
                throw new Exception("Usuario o clave incorrectos");

            if (!user.Active)
                throw new Exception("Cuenta deshabilitada");
            
            var token = GetToken(_configuration, user);
            return "{\"token\": \"" + token + "\"}";
        }

        public static string GetToken(IConfiguration _configuration, User user) {
            // Leemos el secret_key desde nuestro appseting
            var secretKey = _configuration.GetValue<string>("JWTSecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);

            // Creamos los claims (pertenencias, características) del usuario
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                // Nuestro token va a durar un día
                Expires = DateTime.UtcNow.AddDays(1),
                // Credenciales para generar el token usando nuestro secretykey y el algoritmo hash 256
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(createdToken);
            return token;
        }
    }
}