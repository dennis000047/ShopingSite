using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SIEG_API.DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class B_FaviriteProductsController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_FaviriteProductsController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_FaviriteProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaviriteProduct>>> GetFaviriteProduct()
        {
            return await _context.FaviriteProduct.ToListAsync();
        }

        // GET: api/B_FaviriteProducts/5
        [HttpGet("{Memberid}")]
        public async Task<IEnumerable<B_FaviriteProductsDTO>> GetFaviriteProduct(int Memberid)
        {
            var ProductsFavirite = _context.FaviriteProduct.Where(pc => pc.MemberId == Memberid).Select(pc => pc.FaviriteProductId).ToArray();
            //var ProductsFavirite = _context.FaviriteProduct.Where(pc => pc.MemberId == Memberid).Select(pc => pc.ProductId).ToArray();
            var ProductCollection = new List<B_FaviriteProductsDTO>();
            foreach (var FaviriteId in ProductsFavirite)
            {
                var ProductsId = _context.FaviriteProduct.Where(pc => pc.FaviriteProductId == FaviriteId).Select(pc => pc.ProductId).First();
                var averageprice = _context.Order.Where(pc => pc.ProductId == ProductsId).Select(x => x.BuyerPrice).Average();
                var lowprice = await _context.SellerAddProduct.Where(lp => lp.ProductId == ProductsId && lp.OrderId==null).OrderBy(x => x.Price).Select(lowprice => lowprice.Price).FirstOrDefaultAsync();
                var finalprice = await _context.Order.Where(pc => pc.ProductId == ProductsId && pc.DoneTime != null).OrderBy(x => x.DoneTime).Select(x => x.BuyerPrice).LastOrDefaultAsync();
                var ProductDTO = _context.Product.Where(x => x.ProductId == ProductsId).Select(y => new B_FaviriteProductsDTO
                {
                    ProductName = y.ProductCategory.ProductName,
                    ImageFront = y.ImgFront,
                    avecommodityprice = (int?)averageprice,
                    lowprice = lowprice,
                    lastprice = finalprice,
                    MemberId = Memberid,
                    ProductId = y.ProductId,
                    Size = y.Size,
                    FaviriteProductsId = FaviriteId,
                    Model=y.Model,
                }).First();
                ProductCollection.Add(ProductDTO);
            }
            return ProductCollection;
        }

        // PUT: api/B_FaviriteProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFaviriteProduct(int id, FaviriteProduct faviriteProduct)
        {
            if (id != faviriteProduct.FaviriteProductId)
            {
                return BadRequest();
            }

            _context.Entry(faviriteProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaviriteProductExists(id))
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

        // POST: api/B_FaviriteProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FaviriteProduct>> PostFaviriteProduct(FaviriteProduct faviriteProduct)
        {
            _context.FaviriteProduct.Add(faviriteProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFaviriteProduct", new { id = faviriteProduct.FaviriteProductId }, faviriteProduct);
        }

        // DELETE: api/B_FaviriteProducts/5
        [HttpDelete("{FaviriteProductsId}")]
        public async Task<string> DeleteFaviriteProduct(int FaviriteProductsId)
        {
            var faviriteProduct = await _context.FaviriteProduct.FindAsync(FaviriteProductsId);
            if (faviriteProduct == null)
            {
                return "找不到欲刪除的記錄!";
            }

            _context.FaviriteProduct.Remove(faviriteProduct);
            await _context.SaveChangesAsync();

            return "刪除成功!";
        }

        private bool FaviriteProductExists(int id)
        {
            return _context.FaviriteProduct.Any(e => e.FaviriteProductId == id);
        }
    }
}
