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
    public class B_ContactCustomerServicesController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_ContactCustomerServicesController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_ContactCustomerServices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactCustomerService>>> GetContactCustomerService()
        {
            return await _context.ContactCustomerService.Where(a=>a.State=="未處理").ToListAsync();
        }

        // GET: api/B_ContactCustomerServices/5
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

        // PUT: api/B_ContactCustomerServices/5
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

        [HttpPut("contactus/{ContactId}")]
        public async Task<string> PutContactCustomerService1(int ContactId, ContactCustomerService contactCustomerService)
        {
            if (ContactId != contactCustomerService.ContactId)
            {
                return "不正確";
            }
            ContactCustomerService ContactId1 = await _context.ContactCustomerService.FindAsync(contactCustomerService.ContactId);
            ContactId1.ContactId = contactCustomerService.ContactId;
            ContactId1.State = "已完成";
           
            _context.Entry(ContactId1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactCustomerServiceExists(ContactId))
                {
                    return "找不到欲修改紀錄";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功!";
        }
        // POST: api/B_ContactCustomerServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContactCustomerService>> PostContactCustomerService(ContactCustomerService contactCustomerService)
        {
            _context.ContactCustomerService.Add(contactCustomerService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactCustomerService", new { id = contactCustomerService.ContactId }, contactCustomerService);
        }

        // DELETE: api/B_ContactCustomerServices/5
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
