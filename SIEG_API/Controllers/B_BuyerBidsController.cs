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
    public class B_BuyerBidsController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_BuyerBidsController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_BuyerBids
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuyerBid>>> GetBuyerBid()
        {
            return await _context.BuyerBid.ToListAsync();
        }

        // GET: api/B_BuyerBids/5
        [HttpGet("{BuyerBidmID}")]
        public async Task<IEnumerable<B_BuyerBidsDTO>> GetBuyerBid(int BuyerBidmID)
        {
            var buyerbid = _context.BuyerBid.Where(bb => bb.MemberId == BuyerBidmID && bb.ValIdity == true && bb.SaleTime == null).Select(pdId => pdId.BuyerBidId).ToArray();
            Console.WriteLine(buyerbid);
            var ProductCollection = new List<B_BuyerBidsDTO>();
            foreach (var bId in buyerbid)
            {
                var ProductId = _context.BuyerBid.Where(bb => bb.MemberId == BuyerBidmID && bb.BuyerBidId == bId).Select(pdId => pdId.ProductId).First();
                var Buyerdatetime = _context.BuyerBid.Where(bb => bb.MemberId == BuyerBidmID && bb.BuyerBidId == bId).Select(pdId => pdId.BidTime).First();
                var SellerlowPrice = await _context.SellerAddProduct.Where(pdId => pdId.ProductId == ProductId && pdId.ValIdity == true && pdId.OrderId == null).OrderBy(a => a.Price).Select(lp => lp.Price).FirstOrDefaultAsync();
                var SellerAddProductID= await _context.SellerAddProduct.Where(SPID =>SPID.ProductId == ProductId  && SPID.OrderId == null&& SPID.Price== SellerlowPrice).OrderBy(time => time.AddTime).Select(SPID => SPID.SellerAddProductId).FirstOrDefaultAsync();
                //var SellerlowPrice1 = _context.SellerAddProduct.Where(pdId => pdId.ProductId == bId && pdId.ValIdity == true).Select(lp => lp.Price).Min() ?? 0;
                var buyerbid2 = _context.BuyerBid.Where(bb => bb.MemberId == BuyerBidmID && bb.BuyerBidId == bId).Select(pdId => pdId.Price).First();
                var Productname = _context.Product.Where(pn => pn.ProductId == ProductId).Select(y => new B_BuyerBidsDTO
                {
                    BuyerBidId = bId,
                    MemberId = BuyerBidmID,
                    ProductId = ProductId,
                    ProductName = y.ProductCategory.ProductName,
                    ImgFront = y.ImgFront,
                    Price = (int)buyerbid2,
                    lowPrice = (int)SellerlowPrice,
                    Size = y.Size,
                    BidTime = Buyerdatetime,
                    Model = y.Model,
                    SellerAddProductID= SellerAddProductID,
                }).First();
                ProductCollection.Add(Productname);
            }
            return ProductCollection.OrderByDescending(time => time.BidTime);
        }

        // PUT: api/B_BuyerBids/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{BuyerBidid}")]
        public async Task<string> PutBuyerBid(int BuyerBidid, B_BuyerBidsDTO BuyerBidid1)
        {
            if (BuyerBidid != BuyerBidid1.BuyerBidId)
            {
                return "不正確";
            }
            BuyerBid pricemodification = await _context.BuyerBid.FindAsync(BuyerBidid1.BuyerBidId);
            pricemodification.Price = BuyerBidid1.Price;
            pricemodification.FinalPrice = BuyerBidid1.FinalPrice;
            pricemodification.BidTime = DateTime.Now;
            //Order order = await _context.Order.FindAsync(pricemodification.OrderId);
            //order.BuyerPrice= BuyerBidid1.FinalPrice;
            _context.Entry(pricemodification).State = EntityState.Modified;
            //_context.Entry(order).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyerBidExists(BuyerBidid))
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


        [HttpPut("Returns/{BuyerBidid}")]
        public async Task<string> PutBuyerBid2(int BuyerBidid, B_BuyerOrdersDTO BuyerReturns)
        {
            if (BuyerBidid != BuyerReturns.OrderId)
            {
                return "不正確";
            }
            Order Returns = await _context.Order.FindAsync(BuyerReturns.OrderId);
            Returns.State = BuyerReturns.State;
            Returns.DoneTime = DateTime.Now;
            _context.Entry(Returns).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyerBidExists(BuyerBidid))
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

        // POST: api/B_BuyerBids
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BuyerBid>> PostBuyerBid(BuyerBid buyerBid)
        {
            _context.BuyerBid.Add(buyerBid);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuyerBid", new { id = buyerBid.BuyerBidId }, buyerBid);
        }

        // DELETE: api/B_BuyerBids/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteBuyerBid(int id)
        {
            var buyerBid = await _context.BuyerBid.FindAsync(id);
            if (buyerBid == null)
            {
                return "找不到欲刪除的記錄!";
            }

            _context.BuyerBid.Remove(buyerBid);
            await _context.SaveChangesAsync();

            return "刪除成功!";
        }

        private bool BuyerBidExists(int id)
        {
            return _context.BuyerBid.Any(e => e.BuyerBidId == id);
        }
    }
}
