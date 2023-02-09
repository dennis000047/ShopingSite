using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIEG_API.D_DTO;
using SIEG_API.DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class D_BuyerGradeController : ControllerBase
    {
        private readonly SIEGContext _context;

        public D_BuyerGradeController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/BuyerGrade
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Member>>> GetMember()
        //{
        //    return await _context.Member.ToListAsync();
        //}

        // GET: api/BuyerGrade/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<D_GradeBuyDTO>> GetMember(int id)
        {
            var productBAV = _context.Order.Where(pdo => pdo.BuyerId == id && pdo.State == "已完成").Select(a => a.BuyerPrice).Sum();
            //var productSAV = _context.訂單.Where(pdo => pdo.賣家Id == id && pdo.狀態 == "已完成").Select(b => b.價錢).Sum();
            var buyerG = _context.Member.Where(p => p.MemberId == id).Select(b => b.BuyerGrade).First();
            var sellerG = _context.Member.Where(o => o.MemberId == id).Select(c => c.SellerGrade).First();

            // 會員id是要寫進去的
            var coupon = _context.MemberCoupon.Where(ticket => ticket.MemberId == id).Select(c => c.Count).Sum();

            var bbb = _context.Member.Where(x => x.MemberId == id).Select(y => new D_GradeBuyDTO
            {
                //ID會get session的值來用
                BuyerId = id,
                SellerId = id,
                Price = productBAV,
                BuyerGrade = buyerG,
                SellerGrade = sellerG,
                Count = coupon,
            });
            return bbb;
        }

        // PUT: api/BuyerGrade/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutMemberBuy(int id, D_GradeBuyDTO memDTO)
        {
            if (id != memDTO.BuyerId)
            {
                return "不正確";
            }

            Member mgrd = await _context.Member.FindAsync(memDTO.BuyerId);//會員.買家Id
            //會員 sgrd = await _context.會員.FindAsync(會員DTO.賣家Id);

            mgrd.BuyerGrade = memDTO.BuyerGrade;
            //sgrd.賣家等級 = 會員DTO.賣家等級;

            _context.Entry(mgrd).State = EntityState.Modified;

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

        //優惠券
        [HttpPut("xxx/{id}")]
        public async Task<string> PutMember(int id, D_CouponDTO cpDTO)
        {
            if (id != cpDTO.MemberId && 102!= cpDTO.CouponId)
            {
                return "不正確";
            }
            var zz =  _context.MemberCoupon.Where(A => A.MemberId == id && A.CouponId == 102).Select(A => A.MemberCouponId).FirstOrDefault();
            MemberCoupon mem = await _context.MemberCoupon.FindAsync(zz);

            mem.Count = cpDTO.Count;

            _context.Entry(mem).State = EntityState.Modified;

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

        // POST: api/BuyerGrade
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<string> PostMember(D_CouponDTO cpDTO)
        {
            var msg = "";
            MemberCoupon mcp = new MemberCoupon {
            CouponId = cpDTO.CouponId,
            MemberId = cpDTO.MemberId,
            Count = cpDTO.Count,
            };
            bool repeat = _context.MemberCoupon.Any(e => e.MemberId == cpDTO.MemberId);
            if (repeat == true)
            {
                msg = "已經有了";
            }
            else
            {
                msg = "還沒有";
                _context.MemberCoupon.Add(mcp);
                await _context.SaveChangesAsync();
            }
            return msg;
        }

        // DELETE: api/BuyerGrade/5
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
