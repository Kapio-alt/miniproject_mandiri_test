using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Data;
using AuthService.Models;
using AuthService.Models.APIModel.Register;
using AuthService.Models.APIModel.Login;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AuthDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            if (await _context.Users.AnyAsync(u => u.Email == req.Email))
                return BadRequest("User already exists.");

            req.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
            User user = new User { Email = req.Email, Name = req.Name, PasswordHash = req.Password };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.email);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(request.password, existingUser.PasswordHash))
                return Unauthorized("Email or Password is Wrong");

            var token = GenerateJwtToken(existingUser);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {

            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["JwtSettings:Issuer"],
                _config["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
