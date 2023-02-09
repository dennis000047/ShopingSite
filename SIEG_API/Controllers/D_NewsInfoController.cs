using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using SIEG_API.D_DTO;
using SIEG_API.DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class D_NewsInfoController : ControllerBase
    {
        private readonly SIEGContext _context;

        public D_NewsInfoController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/D_NewsInfo
        [HttpGet]
        public async Task<IEnumerable<D_NewsInfoDTO>> GetNews()
        {
            return await _context.News
                .Where(news => news.ValIdity == true )
                .Join(_context.NewsCategory, newsinfo => newsinfo.NewsCategoryId, newssort => newssort.NewsCategoryId, (newsinfo, newssort) => new D_NewsInfoDTO
                {
                    NewsID = newsinfo.NewsId,
                    Img = newsinfo.Img,
                    Title = newsinfo.Title,
                    NewsContent = newsinfo.NewsContent,
                    CategoryName = newssort.CategoryName,
                    AddTime = newsinfo.AddTime,
                    ViewsCount = newsinfo.ViewsCount,
                }).ToListAsync();
        }

        // GET: api/D_NewsInfo/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<D_NewsInfoDTO>> GetNewsInfo(int id)
        {
            var news = _context.News.Include(x => x.NewsCategory).Where(p => p.ValIdity == true && p.NewsId == id).Select(x => new D_NewsInfoDTO
            {
                NewsID = x.NewsId,
                Img = x.Img,
                Title = x.Title,
                NewsContent = x.NewsContent,
                CategoryName =x.NewsCategory.CategoryName,
                AddTime = x.AddTime
            });
            return news;
    }

        // PUT: api/D_NewsInfo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, News news)
        {
            if (id != news.NewsId)
            {
                return BadRequest();
            }

            _context.Entry(news).State = EntityState.Modified;

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

        // POST: api/D_NewsInfo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<string> PostNews(D_FavoreiteNewsDTO collectnews)
        //{
        //    var msg = "";
        //    FaviriteNews news = new FaviriteNews
        //    {
        //        NewsId = collectnews.NewsId,
        //        MemberId = collectnews.MemberId,
        //        ValIdity = collectnews.ValIdity,
        //    };
        //    bool exit = _context.FaviriteNews.Any(e => e.NewsId == collectnews.NewsId );
        //    bool exit1 = _context.FaviriteNews.Any(e => e.MemberId == collectnews.MemberId);
        //    if (exit == true && exit1==true)
        //    {
        //        msg = "成功";
        //        _context.FaviriteNews.Add(news);
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        //msg = "成功";
        //        //_context.FaviriteNews.Add(news);
        //        //await _context.SaveChangesAsync();
        //    }
        //    return msg;
        //}
        //新聞發送
        [HttpPost]
        public async Task<FaviriteNews> PostNews(D_FavoreiteNewsDTO collectnews)
        {
            FaviriteNews news = new FaviriteNews
            {
                NewsId = collectnews.NewsId,
                MemberId = collectnews.MemberId,
                ValIdity = collectnews.ValIdity,
            };
         
                _context.FaviriteNews.Add(news);
                await _context.SaveChangesAsync();
        
                //msg = "成功";
                //_context.FaviriteNews.Add(news);
                //await _context.SaveChangesAsync();
            
            return news;
        }

        // DELETE: api/D_NewsInfo/5
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

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsId == id);
        }
    }
}
