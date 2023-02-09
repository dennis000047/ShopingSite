using System;
using System.Collections.Generic;
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
    public class B_FaviriteNewsController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_FaviriteNewsController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_FaviriteNews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaviriteNews>>> GetFaviriteNews()
        {
            return await _context.FaviriteNews.ToListAsync();
        }

        // GET: api/B_FaviriteNews/5
        [HttpGet("{MemberId}")]
        public async Task<IEnumerable<B_FaviriteNewsDTO>> GetFaviriteNews(int MemberId)
        {
            var FaviriteAritcleAll = await _context.FaviriteArticle
                  .Where(f => f.MemberId == MemberId)
                .Include(f => f.ForumArticle)
                .Select(x => new B_FaviriteNewsDTO
                {
                    FaviriteArticleID = x.FaviriteAritcleId,
                    ArticleID = x.ForumArticleId,
                    MemberId = MemberId,
                    Title = x.ForumArticle.Title,
                    AddTime = x.ForumArticle.AddTime,
                    Img = x.ForumArticle.Img

                }).ToListAsync();
            var FaviriteNewsAll2 = await _context.FaviriteNews
                .Where(f => f.MemberId == MemberId)
              .Include(f => f.News)
              .Select(x => new B_FaviriteNewsDTO
              {
                  FaviriteNewID = x.FaviriteNewsId,
                  NewID = x.NewsId,
                  MemberId = MemberId,
                  Title = x.News.Title,
                  AddTime = x.News.AddTime,
                  Img = x.News.Img,
              }).ToListAsync(); 
            var PostFavorites = new List<B_FaviriteNewsDTO>() { };         
            foreach (var item in FaviriteAritcleAll)
            {
                PostFavorites.Add(item);
            }

            foreach (var item in FaviriteNewsAll2)
            {
                PostFavorites.Add(item);
            }

            return PostFavorites.OrderByDescending(time => time.AddTime);

        }

        // PUT: api/B_FaviriteNews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFaviriteNews(int id, FaviriteNews faviriteNews)
        {
            if (id != faviriteNews.FaviriteNewsId)
            {
                return BadRequest();
            }

            _context.Entry(faviriteNews).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaviriteNewsExists(id))
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

        // POST: api/B_FaviriteNews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FaviriteNews>> PostFaviriteNews(FaviriteNews faviriteNews)
        {
            _context.FaviriteNews.Add(faviriteNews);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFaviriteNews", new { id = faviriteNews.FaviriteNewsId }, faviriteNews);
        }

        // DELETE: api/B_FaviriteNews/5
        [HttpDelete("{FaviriteNewsid}")]
        public async Task<string> DeleteFaviriteNews(int FaviriteNewsid)
        {
            var faviriteNews = await _context.FaviriteNews.FindAsync(FaviriteNewsid);
            if (faviriteNews == null)
            {
                return "找不到欲刪除的記錄!";
            }

            _context.FaviriteNews.Remove(faviriteNews);
            await _context.SaveChangesAsync();

            return "刪除成功!";
        }

        [HttpDelete("DeleteArticle/{FaviriteArticleid}")]
        public async Task<string> DeleteFaviriteArticle(int FaviriteArticleid)
        {
            var FaviriteArticle = await _context.FaviriteArticle.FindAsync(FaviriteArticleid);
            if (FaviriteArticle == null)
            {
                return "找不到欲刪除的記錄!";
            }

            _context.FaviriteArticle.Remove(FaviriteArticle);
            await _context.SaveChangesAsync();

            return "刪除成功!";
        }

        private bool FaviriteNewsExists(int id)
        {
            return _context.FaviriteNews.Any(e => e.FaviriteNewsId == id);
        }
    }
}
