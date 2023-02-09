using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIEG_API.DTO;
using SIEG_API.Models;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class B_MemberCouponsController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_MemberCouponsController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_MemberCoupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberCoupon>>> GetMemberCoupon()
        {
            return await _context.MemberCoupon.ToListAsync();
        }

        // GET: api/B_MemberCoupons/5
        [HttpGet("{MemberId}")]
        public async Task<IEnumerable<B_MemberCouponsDTO>> GetMemberCoupon(int MemberId)
        {
            var CouponId = _context.MemberCoupon.Where(cp => cp.MemberId == MemberId && cp.Count>0).Select(cpid => cpid.CouponId).ToArray();
            var Newcoupon = new List<B_MemberCouponsDTO>();
            foreach (var NewCouponId in CouponId)
            {
                var CouponCount = _context.MemberCoupon.Where(cp => cp.MemberId == MemberId && cp.CouponId == NewCouponId).Select(cp2 => cp2.Count).First();
                var CouponAll = _context.Coupon.Where(cp => cp.CouponId == NewCouponId).Select(newcp => new B_MemberCouponsDTO
                {
                    MemberId = MemberId,
                    CouponName =newcp.Name,
                    CouponId = NewCouponId,
                    count = CouponCount,
                    SerialNumber = newcp.Sn,
                    DiscountPrice = newcp.DiscountPrice,


                }).First();
                Newcoupon.Add(CouponAll);
            }
            return Newcoupon;
        }

        // PUT: api/B_MemberCoupons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberCoupon(int id, MemberCoupon memberCoupon)
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

        // POST: api/B_MemberCoupons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberCoupon>> PostMemberCoupon(MemberCoupon memberCoupon)
        {
            _context.MemberCoupon.Add(memberCoupon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMemberCoupon", new { id = memberCoupon.MemberCouponId }, memberCoupon);
        }

        // DELETE: api/B_MemberCoupons/5
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
