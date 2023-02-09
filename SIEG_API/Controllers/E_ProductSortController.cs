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
    public class E_ProductSortController : ControllerBase
    {
        private readonly SIEGContext _context;

        public E_ProductSortController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/E_ProductSort
        [HttpGet]
        public async Task<IEnumerable<E_ProductSortDTO>> GetProductCategory()
        {
            //ps = ProductCategory
            return await _context.ProductCategory
                .Where(ps => ps.ValIdity == true)
                .Select(ps => new E_ProductSortDTO
                {

                    productsortName = ps.CategoryName,
                }).Distinct().ToListAsync();
        }

        // Brand: api/E_ProductSort/Brand
        [HttpGet("Brand")]
        public async Task<IEnumerable<E_ProductSortDTO>> IndexBrand()
        {
            //ps = ProductCategory
            return await _context.ProductCategory
                .Where(ps => ps.ValIdity == true)
                .GroupBy(ps => ps.Img)
                .Select(ps => new E_ProductSortDTO
                {
                productsortId = ps.First().ProductCategoryId,
                productsortBrand = ps.First().BrandName,
                productsortName = ps.First().CategoryName,
                productsortImg = ps.First().Img,
            }).ToListAsync();

        }
        

        // GET: api/E_ProductSort/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
            var productCategory = await _context.ProductCategory.FindAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        // PUT: api/E_ProductSort/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/E_ProductSort
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategory.Add(productCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
        }

        // DELETE: api/E_ProductSort/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            var productCategory = await _context.ProductCategory.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _context.ProductCategory.Remove(productCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategory.Any(e => e.ProductCategoryId == id);
        }
    }
}
