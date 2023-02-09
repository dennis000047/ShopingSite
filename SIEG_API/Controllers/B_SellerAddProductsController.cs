using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public class B_SellerAddProductsController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_SellerAddProductsController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_SellerAddProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SellerAddProduct>>> GetSellerAddProduct()
        {
            return await _context.SellerAddProduct.ToListAsync();
        }

        // GET: api/B_SellerAddProducts/5
        [HttpGet("{MemberId}")]
        public async Task<IEnumerable<B_SellerAddProductsDTO>> GetSellerAddProduct(int MemberId)
        {
            var sellproducts = _context.SellerAddProduct.Where(bb => bb.MemberId == MemberId && bb.ValIdity == true && bb.OrderId == null).Select(SdId => SdId.SellerAddProductId).ToArray();
            var allmessageslist = new List<B_SellerAddProductsDTO>();
            foreach (var SellerAddId in sellproducts)
            {
                var ProductId = _context.SellerAddProduct.Where(bb => bb.MemberId == MemberId && bb.SellerAddProductId == SellerAddId).Select(pdId => pdId.ProductId).First();
                //var ID = _context.SellerAddProduct.Where(bb => bb.MemberId == MemberId && bb.ProductId == ProductId).Select(pdId => pdId.ProductId).First();
                var datetime = _context.SellerAddProduct.Where(bb => bb.MemberId == MemberId && bb.ProductId == ProductId && bb.OrderId == null).Select(pdId => pdId.AddTime).First();
                var BuylowPrice = await _context.BuyerBid.Where(pdId => pdId.ProductId == ProductId && pdId.ValIdity == true && pdId.SaleTime == null).OrderBy(lp => lp.Price).Select(lp => lp.Price).FirstOrDefaultAsync();
                var BuyhighPrice = await _context.BuyerBid.Where(pdId => pdId.ProductId == ProductId && pdId.ValIdity == true && pdId.SaleTime == null).OrderBy(lp => lp.Price).Select(lp => lp.Price).LastOrDefaultAsync();
                var BuyerBidID = await _context.BuyerBid.Where(pdId => pdId.ProductId == ProductId && pdId.ValIdity == true && pdId.Price == BuyhighPrice && pdId.SaleTime==null).OrderBy(lp => lp.BidTime).Select(BuyerBidID => BuyerBidID.BuyerBidId).FirstOrDefaultAsync();
                var sellPrice = _context.SellerAddProduct.Where(bb => bb.MemberId == MemberId && bb.ProductId == ProductId && bb.OrderId == null).Select(pdId => pdId.Price).First();
                var allmessages = _context.Product.Where(pn => pn.ProductId == ProductId).Select(y => new B_SellerAddProductsDTO
                {
                    SellerAddProductID = SellerAddId,
                    MemberId = MemberId,
                    ProductId = ProductId,
                    ProductName = y.ProductCategory.ProductName,
                    ImgFront = y.ImgFront,
                    Price = (int)sellPrice,
                    lowPrice = BuylowPrice,
                    hightPrice = BuyhighPrice,
                    Size = y.Size,
                    Shelfdate = datetime,
                    Model = y.Model,
                    BuyerBidID = BuyerBidID,
                }).First();
                allmessageslist.Add(allmessages);
            }
            return allmessageslist.OrderByDescending(a => a.Shelfdate);
        }

        // PUT: api/B_SellerAddProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{SellerAddProductid}")]
        public async Task<string> PutSellerAddProduct(int SellerAddProductid, SellerAddProduct sellerAddProduct)
        {
            if (SellerAddProductid != sellerAddProduct.SellerAddProductId)
            {
                return "不正確";
            }
            SellerAddProduct pricemodification = await _context.SellerAddProduct.FindAsync(sellerAddProduct.SellerAddProductId);
            pricemodification.Price = sellerAddProduct.Price;
            pricemodification.FinalPrice = sellerAddProduct.FinalPrice;
            pricemodification.AddTime = DateTime.Now;
            _context.Entry(pricemodification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellerAddProductExists(SellerAddProductid))
                {
                    return "找不到欲修改紀錄";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        // POST: api/B_SellerAddProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SellerAddProduct>> PostSellerAddProduct(SellerAddProduct sellerAddProduct)
        {
            _context.SellerAddProduct.Add(sellerAddProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSellerAddProduct", new { id = sellerAddProduct.SellerAddProductId }, sellerAddProduct);
        }

        // DELETE: api/B_SellerAddProducts/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteSellerAddProduct(int id)
        {
            var sellerAddProduct = await _context.SellerAddProduct.FindAsync(id);
            if (sellerAddProduct == null)
            {
                return "找不到欲刪除的記錄!";
            }

            _context.SellerAddProduct.Remove(sellerAddProduct);
            await _context.SaveChangesAsync();

            return "刪除成功!";
        }

        private bool SellerAddProductExists(int id)
        {
            return _context.SellerAddProduct.Any(e => e.SellerAddProductId == id);
        }
    }
}
