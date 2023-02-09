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
    public class E_NewsSortController : ControllerBase
    {
        private readonly SIEGContext _context;

        public E_NewsSortController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/E_NewsSort
        [HttpGet]
        public async Task<IEnumerable<E_NewsSortDTO>> GetNewsCategory()
        {
            return await _context.NewsCategory
                .Where(newssort => newssort.ValIdity == true)
                .Select(newssort => new E_NewsSortDTO
                {
                    newssortId = newssort.NewsCategoryId,
                    newssortName = newssort.CategoryName,
                }).ToListAsync();
        }

        // GET: api/E_NewsSort/Boss
        [HttpGet("Boss")]
        public async Task<IEnumerable<E_NewsSortDTO>> BossGetNewsCategory()
        {
            return await _context.NewsCategory
                .Select(newssort => new E_NewsSortDTO
                {
                    newssortId = newssort.NewsCategoryId,
                    newssortName = newssort.CategoryName,
                    newssortValIdity = newssort.ValIdity,
                }).ToListAsync();
        }

        // GET: api/E_NewsSort/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsCategory>> GetNewsCategory(int id)
        {
            var newsCategory = await _context.NewsCategory.FindAsync(id);

            if (newsCategory == null)
            {
                return NotFound();
            }

            return newsCategory;
        }

        // PUT: api/E_NewsSort/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewsCategory(int id, NewsCategory newsCategory)
        {
            if (id != newsCategory.NewsCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(newsCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsCategoryExists(id))
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

        // POST: api/E_NewsSort
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<NewsCategory> PostNewsCategory(E_NewsSortDTO newsCategory)
        {

            NewsCategory x = new NewsCategory
            {
                CategoryName = newsCategory.newssortName,
                ValIdity = newsCategory.newssortValIdity,
            };

            _context.NewsCategory.Add(x);
            await _context.SaveChangesAsync();

            return x;
        }

        // DELETE: api/E_NewsSort/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsCategory(int id)
        {
            var newsCategory = await _context.NewsCategory.FindAsync(id);
            if (newsCategory == null)
            {
                return NotFound();
            }

            _context.NewsCategory.Remove(newsCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsCategoryExists(int id)
        {
            return _context.NewsCategory.Any(e => e.NewsCategoryId == id);
        }
    }
}
