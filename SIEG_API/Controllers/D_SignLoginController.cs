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
    public class D_SignLoginController : ControllerBase
    {
        private readonly SIEGContext _context;

        public D_SignLoginController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/D_SignLogin
        [HttpGet]
        public async Task<IEnumerable<D_MemberDTO>> GetMember()
        {
            return _context.Member.Select(member => new D_MemberDTO
            {
                MemberId = member.MemberId,
                Name = member.Name,
                NickName = member.NickName,
                Phone= member.Phone,
                Email = member.Email,
                Password = member.Password,
            });
        }

        // GET: api/D_SignLogin/5
        [HttpGet("{id}")]
        public async Task<ActionResult<D_MemberDTO>> GetMember(int id)
        {
            var member = await _context.Member.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }
            D_MemberDTO MbDTO = new D_MemberDTO
            {
                MemberId = member.MemberId,
                Name = member.Name,
                NickName = member.NickName,
                Phone = member.Phone,
                Email = member.Email,
                Password = member.Password,
            };

            return MbDTO;
        }

        // PUT: api/D_SignLogin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutMember(int id, D_MemberDTO memDTO)
        {
            if (id != memDTO.MemberId)
            {
                return "Id不正確";
            }
            Member member = await _context.Member.FindAsync(memDTO.MemberId);
            member.Password = memDTO.Password;
            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return "找不到";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        // POST: api/D_SignLogin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<string> PostMember(D_MemberDTO mem)
        {
            var msg = "";
            Member member = new Member
            {
                Name = mem.Name,
                NickName = mem.NickName,
                Email = mem.Email,
                Phone = mem.Phone,
                Password = mem.Password,
            };
            bool exit = _context.Member.Any(e => e.Email == mem.Email && e.Password == mem.Password);
            if (exit == true)
            {
                msg = "失敗";
            }
            else
            {
                msg = "成功";
                _context.Member.Add(member);
                await _context.SaveChangesAsync();
            }
            return msg;
        }

        [HttpPost("Login")]
        public async Task<D_LoginDTO> LoginMember([FromBody] D_LoginDTO member)
        {
            var aa = _context.Member;
            var aaa = _context.Member.Where(login => CheckLoginInfo(member, login));
            return _context.Member
                .Where(login => login.Email.Contains(member.Email) && login.Password == member.Password)
                .Select(login => new D_LoginDTO
                {
                    MemberId = login.MemberId,
                    Email = login.Email,
                    Password = login.Password,
                    //權限
                }).First();
        }
        private static bool CheckLoginInfo(D_LoginDTO member, Member login)
        {
            //return login.Email.Contains(member.Email) && login.密碼 == member.密碼;
            bool isEmailCorrect = login.Email.Contains(member.Email);
            bool isPasswordCorrect = login. Password == member.Password;
            return isEmailCorrect && isPasswordCorrect;
        }

        [HttpPost("Send")]
        public async Task<D_LoginDTO> SendMember([FromBody] D_LoginDTO member)
        {
            var aa = _context.Member;
            var aaa = _context.Member.Where(login => CheckSendInfo(member, login));
            return _context.Member
                .Where(login => login.Email.Contains(member.Email))
                .Select(login => new D_LoginDTO
                {
                    MemberId = login.MemberId,
                    Email = login.Email,
                    //權限
                }).First();
        }
        private static bool CheckSendInfo(D_LoginDTO member, Member login)
        {
            //return login.Email.Contains(member.Email) && login.密碼 == member.密碼;
            bool isEmailCorrect = login.Email.Contains(member.Email);
            bool isPasswordCorrect = login.Password == member.Password;
            return isEmailCorrect && isPasswordCorrect;
        }


        // DELETE: api/D_SignLogin/5
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
