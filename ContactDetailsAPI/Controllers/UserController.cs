using ContactDetailsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserDetailsAPI.Repository;

namespace UserDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User, int,ResetPasswordFilter,RegisterFilter,UserStatusFilter> _UserService;

        public UserController(IUserRepository<User, int, ResetPasswordFilter, RegisterFilter, UserStatusFilter> UserService)
        {
            _UserService = UserService;
        }

        [HttpGet("GetAllUser")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var Users = await _UserService.GetAllUserAsync();
            return Ok(Users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var User = await _UserService.GetUserByIdAsync(id);

            if (User == null)
            {
                return NotFound();
            }

            return Ok(User);
        }
        [HttpPost("InsertUser")]
        public async Task<IActionResult> InsertUser([FromBody] User User)
        {
            var response = await _UserService.InsertUserAsync(User);

            if (response.Status == "ERROR")
            {
                return StatusCode(500, response.Message); // Internal Server Error
            }
            return Ok(new { response.Code, response.Status, response.Message, response.Result });
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterFilter User)
        {
            var response = await _UserService.RegisterUser(User);

            if (response.Status == "ERROR")
            {
                return StatusCode(500, response.Message); // Internal Server Error
            }
            return Ok(new { response.Code, response.Status, response.Message, response.Result });
        }
        [HttpPut("PutUser")]
        public async Task<IActionResult> PutUser([FromBody] User User)
        {
            var response = await _UserService.UpdateUserAsync(User);

            if (response.Status == "ERROR")
            {
                return StatusCode(500, response.Message); // Internal Server Error
            }
            return Ok(new { response.Code, response.Status, response.Message, Data = response.Result });
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var User = await _UserService.DeleteUserAsync(id);

            if (User == null)
            {
                return NotFound();
            }
            return Ok(User);
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordFilter User)
        {
            var response = await _UserService.ResetPassword(User);

            if (response.Status == "ERROR")
            {
                return StatusCode(500, response.Message); // Internal Server Error
            }
            return Ok(new { response.Code, response.Status, response.Message, Data = response.Result });
        }

        [HttpPut("UpdateUserStatus")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UserStatusFilter User)
        {
            var response = await _UserService.UpdateUserStatus(User);

            if (response.Status == "ERROR")
            {
                return StatusCode(500, response.Message); // Internal Server Error
            }
            return Ok(new { response.Code, response.Status, response.Message, Data = response.Result });
        }
    }
}
