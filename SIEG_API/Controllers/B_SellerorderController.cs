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
    public class B_SellerorderController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_SellerorderController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_Sellerorder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.ToListAsync();
        }

        // GET: api/B_Sellerorder/5
        [HttpGet("{MemberId}")]
        public async Task<IEnumerable<B_SellerorderDTO>> GetOrder(int MemberId)
        {
            var productIDs = _context.Order.Where(pd => pd.SellerId == MemberId);
            var Products = productIDs.OrderByDescending(time => time.AddTime)
                .Join(_context.Product, pd => pd.ProductId, pds => pds.ProductId, (pd, pds) => new B_SellerorderDTO
                {
                    ImgFront = pds.ImgFront,
                    ProductName = pds.ProductCategory.ProductName,
                    OrderId = pd.OrderId,
                    Price = pd.BuyerPrice,
                    Size = pds.Size,
                    State = pd.State,
                    CompleteTime = pd.AddTime,
                    ShippingAddress = pd.ShippingAddress,
                    Receiver = pd.Receiver,
                    Model=pds.Model,

                });
            return Products;
        }

        // PUT: api/B_Sellerorder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{SellerAddProductid}")]
        public async Task<string> PutOrder(int SellerAddProductid, B_SellerorderDTO order)
        {
            if (SellerAddProductid != order.OrderId)
            {
                return "不正確";
            }

            Order Buyer = await _context.Order.FindAsync(order.OrderId);
            Buyer.State = order.State;
            _context.Entry(Buyer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(SellerAddProductid))
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

        [HttpPost("FilterBuyerOrders/{SellerId}")]
        public async Task<IEnumerable<B_SellerorderDTO>> FilterOrder([FromBody] B_SellerorderDTO OrderDTO, int SellerId)
        {
            var Buyerordersearch = _context.Order.Where(
                emp => emp.OrderId.ToString().Contains(OrderDTO.OrderId.ToString()) && emp.State == "已完成" && emp.SellerId == SellerId).Join(_context.Product, pd => pd.ProductId, pds => pds.ProductId, (pd, pds) => new B_SellerorderDTO

                {
                    ProductName = pds.ProductCategory.ProductName,
                    ImgFront = pds.ImgFront,
                    Size = pds.Size,
                    Price = pd.BuyerPrice,
                    CompleteTime = pd.AddTime,
                    ShippingAddress = pd.ShippingAddress,
                    State = pd.State,
                    Receiver = pd.Receiver,
                    OrderId = pd.OrderId,
                    Model = pds.Model,

                }).OrderByDescending(time => time.CompleteTime);


            return Buyerordersearch;
        }

        // POST: api/B_Sellerorder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/B_Sellerorder/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return "找不到欲刪除的記錄!";
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return "刪除成功!";
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
