using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
    public class B_personalinformationController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_personalinformationController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_personalinformation
        [HttpGet]
        public async Task<IEnumerable<B_MemberkycDTO>> GetMember()
        {
            var Memberkyc = _context.Member
                .Where(m => m.Access == "1" && m.IdCardFront != null && m.IdCardBack != null)
                .Select(x => new B_MemberkycDTO
                {
                    MemberId = x.MemberId,
                    Name = x.Name,
                    Access = x.Access,
                    IdCardFront = x.IdCardFront,
                    IdCardBack = x.IdCardBack,
                    Email = x.Email,
                });
            return Memberkyc;
        }

        // GET: api/B_personalinformation/5
        [HttpGet("{Memberid}")]
        public async Task<ActionResult<B_personalinformationDTO>> GetMember(int Memberid)
        {
            var member = await _context.Member.FindAsync(Memberid);

            if (member == null)
            {
                return NotFound();
            }
            B_personalinformationDTO personal = new B_personalinformationDTO
            {
                MemberId = member.MemberId,
                NickName = member.NickName,
                Email = member.Email,
                Password = member.Password,
                Shippingaddress = member.Address,
                BillingAddress = member.BillingAddress,
                Phone = member.Phone,
                Name = member.Name,
                Access = member.Access,
            };

            return personal;
        }


        [HttpGet("Kyccertified/{Memberid}")]
        public async Task<ActionResult<B_KyccertifiedDTO>> GetMember2(int Memberid)
        {
            var member = await _context.Member.FindAsync(Memberid);

            if (member == null)
            {
                return NotFound();
            }
            B_KyccertifiedDTO Kyccertified = new B_KyccertifiedDTO
            {
                MemberId = member.MemberId,
                IdCardFront = member.IdCardFront,
                IdCardBack = member.IdCardBack,
                Access = member.Access,
            };

            return Kyccertified;
        }

        // PUT: api/B_personalinformation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Memberid}")]
        public async Task<string> PutMember(int Memberid, B_personalinformationDTO member)
        {
            if (Memberid != member.MemberId)
            {
                return "不正確";
            }
            Member memberinformation = await _context.Member.FindAsync(member.MemberId);
            memberinformation.NickName = member.NickName;
            //memberinformation.Phone = member.Phone;
            memberinformation.Address = member.Shippingaddress;
            memberinformation.BillingAddress = member.BillingAddress;
            //memberinformation.Name = member.Name;
            _context.Entry(memberinformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(Memberid))
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


        [HttpPut("Kyccertified/{Memberid}")]
        public async Task<string> PutMember2(int Memberid, B_KyccertifiedDTO member)
        {
            if (Memberid != member.MemberId)
            {
                return "不正確";
            }
            Member Kyccertified = await _context.Member.FindAsync(member.MemberId);
            Kyccertified.MemberId = member.MemberId;
            Kyccertified.IdCardFront = member.IdCardFront;
            Kyccertified.IdCardBack = member.IdCardBack;
            _context.Entry(Kyccertified).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(Memberid))
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

        [HttpPut("MemberkycDTO/{Memberid}")]
        public async Task<string> PutMember(int Memberid, B_MemberkycDTO member)
        {
            if (Memberid != member.MemberId)
            {
                return "不正確";
            }
            Member Memberkyc = await _context.Member.FindAsync(member.MemberId);
            Memberkyc.MemberId = member.MemberId;
            Memberkyc.IdCardFront = member.IdCardFront;
            Memberkyc.IdCardBack = member.IdCardBack;
            Memberkyc.Access = member.Access;
            _context.Entry(Memberkyc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(Memberid))
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

        // POST: api/B_personalinformation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            _context.Member.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/B_personalinformation/5
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
