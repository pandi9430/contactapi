using ContactDetailsAPI.Models;
using ContactDetailsAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository<Contact, int> _contactService;

        public ContactsController(IContactRepository<Contact, int> contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("GetAllContact")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            var contacts = await _contactService.GetAllContactAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }
        [HttpPost("InsertContact")]
        public async Task<IActionResult> InsertContact([FromBody] Contact contact)
        {
            var response = await _contactService.InsertContactAsync(contact);

            if (response.Status == "ERROR")
            {
                return StatusCode(500, response.Message); // Internal Server Error
            }

            return Ok(new { response.Code, response.Status, response.Message, response.Result });
        }
        [HttpPut("PutContact")]
        public async Task<IActionResult> PutContact([FromBody] Contact contact)
        {
            var response = await _contactService.UpdateContactAsync(contact);

            if (response.Status == "ERROR")
            {
                return StatusCode(500, response.Message); // Internal Server Error
            }
            return Ok(new { response.Code, response.Status, response.Message, Data = response.Result });
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _contactService.DeleteContactAsync(id);

            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
    }
}
