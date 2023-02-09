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
    public class G_FaviriteArticlesController : ControllerBase
    {
        private readonly SIEGContext _context;

        public G_FaviriteArticlesController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/G_FaviriteArticles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaviriteArticle>>> GetFaviriteArticle()
        {
            return await _context.FaviriteArticle.ToListAsync();
        }

        // GET: api/G_FaviriteArticles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FaviriteArticle>> GetFaviriteArticle(int id)
        {
            var faviriteArticle = await _context.FaviriteArticle.FindAsync(id);

            if (faviriteArticle == null)
            {
                return NotFound();
            }

            return faviriteArticle;
        }

        // PUT: api/G_FaviriteArticles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFaviriteArticle(int id, FaviriteArticle faviriteArticle)
        {
            if (id != faviriteArticle.FaviriteAritcleId)
            {
                return BadRequest();
            }

            _context.Entry(faviriteArticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaviriteArticleExists(id))
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

        // POST: api/G_FaviriteArticles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<FaviriteArticle> PostFaviriteArticle(G_FaviriteArticleDTO faviriteArticle)
        {
            FaviriteArticle pos = new FaviriteArticle
            {
                MemberId = faviriteArticle.MemberId,
                ValIdity = faviriteArticle.ValIdity,
                ForumArticleId = faviriteArticle.ForumArticleId,
            };

            _context.FaviriteArticle.Add(pos);
            await _context.SaveChangesAsync();

            return pos;
        }

        // DELETE: api/G_FaviriteArticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaviriteArticle(int id)
        {
            var faviriteArticle = await _context.FaviriteArticle.FindAsync(id);
            if (faviriteArticle == null)
            {
                return NotFound();
            }

            _context.FaviriteArticle.Remove(faviriteArticle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FaviriteArticleExists(int id)
        {
            return _context.FaviriteArticle.Any(e => e.FaviriteAritcleId == id);
        }
    }
}
