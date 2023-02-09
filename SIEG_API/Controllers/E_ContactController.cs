using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIEG_API.DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class E_ContactController : ControllerBase
    {
        private readonly SIEGContext _context;

        public E_ContactController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/E_Contact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactCustomerService>>> GetContactCustomerService()
        {
            return await _context.ContactCustomerService.ToListAsync();
        }

        // GET: api/E_Contact/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactCustomerService>> GetContactCustomerService(int id)
        {
            var contactCustomerService = await _context.ContactCustomerService.FindAsync(id);

            if (contactCustomerService == null)
            {
                return NotFound();
            }

            return contactCustomerService;
        }

        // PUT: api/E_Contact/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactCustomerService(int id, ContactCustomerService contactCustomerService)
        {
            if (id != contactCustomerService.ContactId)
            {
                return BadRequest();
            }

            _context.Entry(contactCustomerService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactCustomerServiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/E_Contact
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ContactCustomerService> PostContactCustomerService(E_ContactDTO contactCustomerService)
        {
            ContactCustomerService contact = new ContactCustomerService
            {
                Name = contactCustomerService.contactName,
                Email = contactCustomerService.contactEmail,
                Title = contactCustomerService.contactTitle,
                InnerText = contactCustomerService.contactText,
                State = contactCustomerService.contactState,
            };

            _context.ContactCustomerService.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        // DELETE: api/E_Contact/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactCustomerService(int id)
        {
            var contactCustomerService = await _context.ContactCustomerService.FindAsync(id);
            if (contactCustomerService == null)
            {
                return NotFound();
            }

            _context.ContactCustomerService.Remove(contactCustomerService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactCustomerServiceExists(int id)
        {
            return _context.ContactCustomerService.Any(e => e.ContactId == id);
        }
    }
}
