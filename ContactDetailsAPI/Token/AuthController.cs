using ContactDetailsAPI.Data;
using ContactDetailsAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AuthService _userService;

    public AuthController(IConfiguration configuration, AuthService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var response = await _userService.UserAuth(loginDTO);

        if (response.Code == ResponseCode.Success)
        {
            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, loginDTO.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(23),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {
                Token = tokenString,
                Role_Name = response.Result.Role_Name
            });
        }

         return Unauthorized(response.Message);
        
    }
}

public class LoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Role_Name { get; set; }
}
