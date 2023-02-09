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
    public class E_AdController : ControllerBase
    {
        private readonly SIEGContext _context;

        public E_AdController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/E_Ad
        [HttpGet]
        public async Task<IEnumerable<E_AdDTO>> GetAd()
        {
            return _context.Ad
                .Where(ad => ad.ValIdity == true)
                .Select(ad => new E_AdDTO
                {
                    AdId = ad.AdId,
                    AdImg = ad.Img,
                    AdLink = ad.Link,
                }).ToList();
        }

        // GET: api/E_Ad/Boss
        [HttpGet("Boss")]
        public async Task<IEnumerable<E_AdDTO>> BossGetAd()
        {
            return _context.Ad
                .Select(ad => new E_AdDTO
                {
                    AdId = ad.AdId,
                    AdImg = ad.Img,
                    AdLink = ad.Link,
                    AdValIdity = ad.ValIdity,
                }).ToList();
        }

        // GET: api/E_Ad/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ad>> GetAd(int id)
        {
            var ad = await _context.Ad.FindAsync(id);

            if (ad == null)
            {
                return NotFound();
            }

            return ad;
        }

        // PUT: api/E_Ad/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAd(int id, Ad ad)
        {
            if (id != ad.AdId)
            {
                return BadRequest();
            }

            _context.Entry(ad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
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

        // POST: api/E_Ad
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Ad> PostAd(E_AdDTO ad)
        {

            Ad x = new Ad
            {
                Img = ad.AdImg,
                Link = ad.AdLink,
                ValIdity = ad.AdValIdity,
            };

            _context.Ad.Add(x);
            await _context.SaveChangesAsync();

            return x;
        }

        // DELETE: api/E_Ad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAd(int id)
        {
            var ad = await _context.Ad.FindAsync(id);
            if (ad == null)
            {
                return NotFound();
            }

            _context.Ad.Remove(ad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdExists(int id)
        {
            return _context.Ad.Any(e => e.AdId == id);
        }
    }
}
