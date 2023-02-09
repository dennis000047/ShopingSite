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
    public class E_BannerController : ControllerBase
    {
        private readonly SIEGContext _context;

        public E_BannerController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/E_Banner
        [HttpGet]
        public async Task<IEnumerable<E_BannerDTO>> GetBanner()
        {
            return _context.Banner
                .Where(banner => banner.ValIdity == true)
                .Select(banner => new E_BannerDTO
                {
                    BannerId = banner.BannerId,
                    BannerImg = banner.Img,
                    BannerTitle = banner.Title,
                    BannerLink = banner.Link,
                });
        }

        // GET: api/E_Banner/Boss
        [HttpGet("Boss")]
        public async Task<IEnumerable<E_BannerDTO>> BossGetBanner()
        {
            return _context.Banner
                .Select(banner => new E_BannerDTO
                {
                    BannerId = banner.BannerId,
                    BannerImg = banner.Img,
                    BannerTitle = banner.Title,
                    BannerLink = banner.Link,
                    BannerValIdity = banner.ValIdity,
                });
        }

        // GET: api/E_Banner/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Banner>> GetBanner(int id)
        {
            var banner = await _context.Banner.FindAsync(id);

            if (banner == null)
            {
                return NotFound();
            }

            return banner;
        }

        // PUT: api/E_Banner/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanner(int id, Banner banner)
        {
            if (id != banner.BannerId)
            {
                return BadRequest();
            }

            _context.Entry(banner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BannerExists(id))
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

        // POST: api/E_Banner
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Banner> PostBanner(E_BannerDTO banner)
        {

            Banner bn = new Banner
            {
                Img = banner.BannerImg,
                Title = banner.BannerTitle,
                Link = banner.BannerLink,
                ValIdity = banner.BannerValIdity,
            };

            _context.Banner.Add(bn);
            await _context.SaveChangesAsync();

            return bn;
        }

        // DELETE: api/E_Banner/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var banner = await _context.Banner.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            _context.Banner.Remove(banner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BannerExists(int id)
        {
            return _context.Banner.Any(e => e.BannerId == id);
        }
    }
}
