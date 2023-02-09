using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using SIEG_API.DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class B_PaymentInformationController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_PaymentInformationController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_PaymentInformation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMember()
        {
            return await _context.Member.ToListAsync();
        }

        // GET: api/B_PaymentInformation/5
        [HttpGet("{Memberid}")]
        public async Task<ActionResult<B_PaymentInformationDTO>> GetMember(int Memberid)
        {
            var member = await _context.Member.FindAsync(Memberid);

            if (member == null)
            {
                return NotFound();
            }
            B_PaymentInformationDTO PaymentInformation = new B_PaymentInformationDTO
            {
                MemberId = member.MemberId,
                CreditCard = member.CreditCard,
                CreditCardDate = member.CreditCardDate,
                CreditCardCCV = member.CreditCardCcv,
                Name = member.Name,
                BillingAddress = member.BillingAddress,
                Phone = member.Phone,
                Shippingaddress= member.Address,
                BankAccount = member.BankAccount,
                BankCode=member.BankCode,
                Bankname=_context.Bank.Where(bn=>bn.BankCode== member.BankCode).Select(bn=>bn.Name).FirstOrDefault(),
            };
            return PaymentInformation;
        }

        [HttpGet("Mailinginformation/{Memberid}")]
        public async Task<ActionResult<B_MailinginformationDTO>> GetMember2(int Memberid)
        {
            var member = await _context.Member.FindAsync(Memberid);

            if (member == null)
            {
                return NotFound();
            }
            B_MailinginformationDTO PaymentInformation = new B_MailinginformationDTO
            {
                MemberId = member.MemberId,             
                Name = member.Name,
                Shippingaddress = member.Address,
                Phone = member.Phone,
            };
            return PaymentInformation;
        }


        [HttpGet("Sellerinformation/{Memberid}")]
        public async Task<ActionResult<B_SellerinformationdDTO>> GetMember3(int Memberid)
        {
            var member = await _context.Member.FindAsync(Memberid);

            if (member == null)
            {
                return NotFound();
            }
            B_SellerinformationdDTO Sellerinformation = new B_SellerinformationdDTO
            {
                MemberId = member.MemberId,
                Name = member.Name,
                BankCode = member.BankCode,
                Phone = member.Phone,
                BankAccount= member.BankAccount,
            };
            return Sellerinformation;
        }


        [HttpGet("Passwordmodification/{Memberid}")]
        public async Task<ActionResult<B_PasswordmodificationDTO>> GetMember4(int Memberid)
        {
            var member = await _context.Member.FindAsync(Memberid);

            if (member == null)
            {
                return NotFound();
            }
            B_PasswordmodificationDTO Passwordmodification = new B_PasswordmodificationDTO
            {
                MemberId = member.MemberId,
                Email = member.Email,
                Password = member.Password,
            };
            return Passwordmodification;
        }


        // PUT: api/B_PaymentInformation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Memberid}")]
        public async Task<string> PutMember(int Memberid, B_PaymentInformationDTO member)
        {
            if (Memberid != member.MemberId)
            {
                return "不正確";
            }
            Member PaymentInformation = await _context.Member.FindAsync(member.MemberId);
            PaymentInformation.BillingAddress=member.BillingAddress;
            PaymentInformation.CreditCard = member.CreditCard;
            PaymentInformation.CreditCardDate = member.CreditCardDate;
            PaymentInformation.CreditCardCcv = member.CreditCardCCV;
            _context.Entry(PaymentInformation).State = EntityState.Modified;

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

        [HttpPut("Mailinginformation/{Memberid}")]
        public async Task<string> PutMember(int Memberid, B_MailinginformationDTO member)
        {
            if (Memberid != member.MemberId)
            {
                return "不正確";
            }
            Member Mailinginformation = await _context.Member.FindAsync(member.MemberId);
            Mailinginformation.Address = member.Shippingaddress;
           
            _context.Entry(Mailinginformation).State = EntityState.Modified;
          

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

        [HttpPut("Sellerinformation/{Memberid}")]
        public async Task<string> PutMember(int Memberid, B_SellerinformationdDTO member)
        {
            if (Memberid != member.MemberId)
            {
                return "不正確";
            }
            Member Sellerinformation = await _context.Member.FindAsync(member.MemberId);
            Sellerinformation.BankCode = member.BankCode;
            Sellerinformation.BankAccount = member.BankAccount;
            _context.Entry(Sellerinformation).State = EntityState.Modified;


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

        [HttpPut("Passwordmodification/{Memberid}")]
        public async Task<string> PutMember(int Memberid, B_PasswordmodificationDTO member)
        {
            if (Memberid != member.MemberId)
            {
                return "不正確";
            }
            Member Passwordmodification = await _context.Member.FindAsync(member.MemberId);
            Passwordmodification.Password = member.Password;
            _context.Entry(Passwordmodification).State = EntityState.Modified;


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

        // POST: api/B_PaymentInformation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            _context.Member.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/B_PaymentInformation/5
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
