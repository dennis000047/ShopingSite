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
    public class E_NewsListController : ControllerBase
    {
        private readonly SIEGContext _context;

        public E_NewsListController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/E_NewsList
        [HttpGet]
        public async Task<IEnumerable<E_NewsListDTO>> GetNews()
        {
            return await _context.News
                .Where(news => news.ValIdity == true)
                .OrderByDescending(news => news.AddTime)
                .Join(_context.NewsCategory, newslist => newslist.NewsCategoryId, newssort => newssort.NewsCategoryId, (newslist, newssort) => new E_NewsListDTO {

                    newslistId = newslist.NewsId,
                    newslistImg = newslist.Img,
                    newslistTitle = newslist.Title,
                    newslistContent = newslist.NewsContent,
                    newslistSort = newssort.CategoryName,
                    newslistTime = newslist.AddTime,
                    newslistviewcount = newslist.ViewsCount,
                }).ToListAsync();
        }

        // GET: api/E_NewsList/Boss
        [HttpGet("Boss")]
        public async Task<IEnumerable<E_NewsListDTO>> BossGetNews()
        {
            return await _context.News
                .OrderByDescending(news => news.AddTime)
                .Join(_context.NewsCategory, newslist => newslist.NewsCategoryId, newssort => newssort.NewsCategoryId, (newslist, newssort) => new E_NewsListDTO
                {
                    newslistId = newslist.NewsId,
                    newslistImg = newslist.Img,
                    newslistTitle = newslist.Title,
                    newslistTime = newslist.AddTime,
                    newsValIdity = newslist.ValIdity,
                    newslistviewcount = newslist.ViewsCount,
                }).ToListAsync();
        }

        // GET: api/E_NewsList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        // PUT: api/E_NewsList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, E_NewsListDTO E_NewsViewDTO)
        {
            if (id != E_NewsViewDTO.newslistId)
            {
                return BadRequest();
            }
            News NewsView = await _context.News.FindAsync(E_NewsViewDTO.newslistId);

            NewsView.ViewsCount = E_NewsViewDTO.newslistviewcount;


            _context.Entry(NewsView).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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

        // POST: api/E_NewsList
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<News>> PostNews(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.NewsId }, news);
        }

        // DELETE: api/E_NewsList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Filter: api/E_NewsList/Filter
        // Contains=包含
        [HttpPost("Filter")]
        public async Task<IEnumerable<E_NewsListDTO>> FilterNews([FromBody] E_NewsListDTO news)
        {
            return await _context.News
                .Where(
                    n => (n.Title.Contains(news.newslistTitle) ||
                            n.NewsContent.Contains(news.newslistContent)) &&
                            n.ValIdity == true
                )
                .OrderByDescending(n => n.AddTime)
                .Join(_context.NewsCategory, newslist => newslist.NewsCategoryId, newssort => newssort.NewsCategoryId, (newslist, newssort) => new E_NewsListDTO 
                {
                    newslistId = newslist.NewsId,
                    newslistImg = newslist.Img,
                    newslistTitle = newslist.Title,
                    newslistContent = newslist.NewsContent,
                    newslistSort = newssort.CategoryName,
                    newslistTime = newslist.AddTime,
                }).ToListAsync();
        }

        // Filter: api/E_NewsList/FilterSort
        [HttpPost("FilterSort")]
        public async Task<IEnumerable<E_NewsListDTO>> FilterNewsSort([FromBody] E_NewsListDTO news)
        {
            //nl = News / ns = NewsCategory / n = nl + ns
            return await _context.News
                .Join(_context.NewsCategory, nl => nl.NewsCategoryId, ns => ns.NewsCategoryId, (nl, ns) => new { nl, ns })
                .Where(
                    n => n.ns.CategoryName.Contains(news.newslistSort) && n.nl.ValIdity == true
                )
                .OrderByDescending(n => n.nl.AddTime).Select(x => new E_NewsListDTO
                {
                    newslistId = x.nl.NewsId,
                    newslistImg = x.nl.Img,
                    newslistTitle = x.nl.Title,
                    newslistContent = x.nl.NewsContent,
                    newslistSort = x.ns.CategoryName,
                    newslistTime = x.nl.AddTime,
                }).ToListAsync();
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsId == id);
        }
    }
}
