using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using SIEG_API.DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class G_ForumReply1Controller : ControllerBase
    {
        private readonly SIEGContext _context;

        public G_ForumReply1Controller(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/G_ForumReply1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ForumReply>>> GetForumReply()
        {
            return await _context.ForumReply.ToListAsync();
        }

        // GET: api/G_ForumReply1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<G_ForumReplyDTO>>> GetForumReply(int id)
        {
            return await _context.ForumReply.Where(c => c.ForumArticleId == id).Join(_context.Member, rp1 => rp1.MemberId, member => member.MemberId,(rp1, member) => new G_ForumReplyDTO
            {
                ForumReplyId = rp1.ForumReplyId,
                ForumArticleId = rp1.ForumArticleId,
                MemberId = rp1.MemberId,
                Floor = rp1.Floor,
                ForumReplyContent = rp1.ForumReplyContent,
                Img = rp1.Img,
                AddTime = rp1.AddTime,
                ValIdity = rp1.ValIdity,
                LikeCount = rp1.LikeCount,
                NickName = member.NickName,
            }).ToListAsync();
        }

        // PUT: api/G_ForumReply1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutForumReply(int id, G_ForumReplyDTO g_ForumReplyDTO)
        {
            if (id != g_ForumReplyDTO.ForumReplyId)
            {
                return "ID不正確";
            }

            ForumReply pos = await _context.ForumReply.FindAsync(id);
            pos.ForumReplyContent = g_ForumReplyDTO.ForumReplyContent;
            pos.Img = g_ForumReplyDTO.Img;
            pos.ValIdity = g_ForumReplyDTO.ValIdity;
            pos.LikeCount = g_ForumReplyDTO.LikeCount;

            _context.Entry(pos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumReplyExists(id))
                {
                    return "找不到欲修改的資料";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        // POST: api/G_ForumReply1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ForumReply>> PostForumReply(G_ForumReplyDTO forumReply)
        {
            ForumReply pos = new ForumReply
            {
                ForumArticleId = forumReply.ForumArticleId,
                MemberId = forumReply.MemberId,
                Floor = forumReply.Floor,
                ForumReplyContent = forumReply.ForumReplyContent,
                Img = forumReply.Img,
            };

            _context.ForumReply.Add(pos);
            await _context.SaveChangesAsync();

            return pos;
        }

        // DELETE: api/G_ForumReply1/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteForumReply(int id)
        {
            var forumReply = await _context.ForumReply.FindAsync(id);
            if (forumReply == null)
            {
                return "找不到留言";
            }
            forumReply.ValIdity = false;
            _context.ForumReply.Update(forumReply);
            await _context.SaveChangesAsync();

            return "刪除成功";
        }

        private bool ForumReplyExists(int id)
        {
            return _context.ForumReply.Any(e => e.ForumReplyId == id);
        }
    }
}
