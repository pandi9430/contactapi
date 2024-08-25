using ContactDetailsAPI.Models;
using ContactDetailsAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserDetailsAPI.Repository;
using UserDetailsAPI.Service;

namespace ContactDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly IForgotRepository<ForgotPassword, string> _forgetService;

        public ForgotPasswordController(IForgotRepository<ForgotPassword, string> forgetService)
        {
            _forgetService = forgetService;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<User>> GetUserCheck(string email)
        {
            var User = await _forgetService.GetUserCheck(email);

            if (User == null)
            {
                return NotFound();
            }

            return Ok(User);
        }
    }
}
