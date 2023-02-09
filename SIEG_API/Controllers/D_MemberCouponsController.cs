using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIEG_API.D_DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class D_MemberCouponsController : ControllerBase
    {
        private readonly SIEGContext _context;

        public D_MemberCouponsController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/D_MemberCoupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberCoupon>>> GetMemberCoupon()
        {
            return await _context.MemberCoupon.ToListAsync();
        }

        // GET: api/D_MemberCoupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberCoupon>> GetMemberCoupon(int id)
        {
            var memberCoupon = await _context.MemberCoupon.FindAsync(id);

            if (memberCoupon == null)
            {
                return NotFound();
            }

            return memberCoupon;
        }

        // PUT: api/D_MemberCoupons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberCoupon(int id, D_CouponDTO memberCoupon)
        {
            if (id != memberCoupon.MemberCouponId)
            {
                return BadRequest();
            }

            _context.Entry(memberCoupon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberCouponExists(id))
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

        // POST: api/D_MemberCoupons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberCoupon>> PostMemberCoupon(MemberCoupon memberCoupon)
        {
            _context.MemberCoupon.Add(memberCoupon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMemberCoupon", new { id = memberCoupon.MemberCouponId }, memberCoupon);
        }

        // DELETE: api/D_MemberCoupons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberCoupon(int id)
        {
            var memberCoupon = await _context.MemberCoupon.FindAsync(id);
            if (memberCoupon == null)
            {
                return NotFound();
            }

            _context.MemberCoupon.Remove(memberCoupon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberCouponExists(int id)
        {
            return _context.MemberCoupon.Any(e => e.MemberCouponId == id);
        }
    }
}
