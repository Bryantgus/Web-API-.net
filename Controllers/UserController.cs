using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDconSQL.Models;
// using CRUDconSQL.Data;
using CRUDconSQL.Services;
using System.Security.Cryptography;
using System.Text;

namespace CRUDconSQL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public UsersController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Usuario == registerDto.Usuario))
            {
                return BadRequest("El usuario ya existe");
            }

            var contraseñaHash = HashPassword(registerDto.Contraseña);

            var user = new User
            {
                Usuario = registerDto.Usuario,
                Contraseña = contraseñaHash,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado exitosamente" });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Usuario == loginDto.Usuario);

            if (user == null || !VerifyPassword(loginDto.Contraseña, user.Contraseña))
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var token = _jwtService.GenerateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                Usuario = user.Usuario,
                UserId = user.Id,
                Expiry = DateTime.UtcNow.AddHours(1)
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }
    }
}