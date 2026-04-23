using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using StudentRepo.models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly StudentDbContext _context;

    public AuthController(IConfiguration configuration, StudentDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDTO request)
    {
        if (request.Role != "Admin" && request.Role != "User")
        {
            return BadRequest(new { message = "Role must be Admin or User" });
        }

        var user = _context.Users.SingleOrDefault(u => u.UserName == request.UserName && u.Role == request.Role && u.IsActive);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid user credentials" });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = _configuration["JwtSettings:SecretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey ?? "fallback_secret_key_needs_to_be_long_enough");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { token = tokenString, role = user.Role });
    }
}
