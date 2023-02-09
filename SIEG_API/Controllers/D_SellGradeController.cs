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
    public class D_SellGradeController : ControllerBase
    {
        private readonly SIEGContext _context;

        public D_SellGradeController(SIEGContext context)
        {
            _context = context;
        }

        //// GET: api/D_SellGrade
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Member>>> GetMember()
        //{
        //    return await _context.Member.ToListAsync();
        //}

        // GET: api/D_SellGrade/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<D_GradeSellDTO>> GetMember(int id)
        {
            //var productBAV = _context.Order.Where(pdo => pdo.BuyerId == id && pdo.State == "已完成").Select(a => a.Price).Sum();
            var productSAV = _context.Order.Where(pdo => pdo.SellerId == id && pdo.State == "已完成").Select(b => b.SellerPrice).Sum();
            var buyerG = _context.Member.Where(p => p.MemberId == id).Select(b => b.BuyerGrade).First();
            var sellerG = _context.Member.Where(o => o.MemberId == id).Select(c => c.SellerGrade).First();

            // 會員id是要寫進去的
            var coupon = _context.MemberCoupon.Where(ticket => ticket.MemberId == 200).Select(c => c.Count).Sum();

            var ccc = _context.Member.Where(x => x.MemberId == id).Select(y => new D_GradeSellDTO
            {
                //ID會get session的值來用
                BuyerId = id,
                SellerId = id,
                Price = productSAV,
                BuyerGrade = buyerG,
                SellerGrade = sellerG,
                Count = coupon,
            });
            return ccc;
        }

        // PUT: api/D_SellGrade/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutMemberSell(int id, D_GradeSellDTO memDTO)
        {
            if (id != memDTO.SellerId)
            {
                return "不正確";
            }

            //Member mgrd = await _context.Member.FindAsync(memDTO.BuyerId);//會員.買家Id
            Member sgrd = await _context.Member.FindAsync(memDTO.SellerId);

            //mgrd.BuyerGrade = memDTO.BuyerGrade;
            sgrd.SellerGrade = memDTO.SellerGrade;

            _context.Entry(sgrd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return "exit";
                }
                else
                {
                    throw;
                }
            }
            return "ok";
        }

        // POST: api/D_SellGrade
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            _context.Member.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/D_SellGrade/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Member.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.MemberId == id);
        }
    }
}
