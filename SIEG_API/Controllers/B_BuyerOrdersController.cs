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
    public class B_BuyerOrdersController : ControllerBase
    {
        private readonly SIEGContext _context;

        public B_BuyerOrdersController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/B_BuyerOrders
        [HttpGet]
        public async Task<IEnumerable<B_ReturnDTO>> GetOrder()
        {
            var Return = _context.Order
                .Where(p => p.State == "申請退貨")              
                .Include(p => p.Product)
                .Include(p=>p.Product.ProductCategory)
                .Select(x => new B_ReturnDTO
                {
                    OrderId=x.OrderId,
                    BuyerId=x.BuyerId,
                    BuyerPrice=x.BuyerPrice,
                    SellerId=x.SellerId,
                    SellerPrice=x.SellerPrice,
                    DoneTime=x.DoneTime,
                    State = x.State,
                    Image = x.Product.ImgFront,
                    Model=x.Product.Model,
                    SizeId=x.Product.Size,
                    ProductName=x.Product.ProductCategory.ProductName
                });
            return Return.OrderByDescending(time => time.DoneTime);
        }

        // GET: api/B_BuyerOrders/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<B_BuyerOrdersDTO>> GetOrder(int id)
        {
            var productIDs = _context.Order.Where(pd => pd.BuyerId == id );
            var Products = productIDs.OrderByDescending(time => time.DoneTime)
                .Join(_context.Product, pd => pd.ProductId, pds => pds.ProductId, (pd, pds) => new {pd, pds})
                .Join(_context.ProductCategory, x => x.pds.ProductCategoryId, pc => pc.ProductCategoryId, (x, pc) => 
                new B_BuyerOrdersDTO
                {
                    ProductName = pc.ProductName,
                    Image = x.pds.ImgFront,
                    SizeId = x.pds.Size,
                    Price = x.pd.BuyerPrice,
                    CompleteTime = x.pd.DoneTime,
                    ShippingAddress = x.pd.ShippingAddress,
                    State = x.pd.State,
                    Receiver = x.pd.Receiver,
                    OrderId = x.pd.OrderId,
                    Model = x.pds.Model,

                });
            return Products;
        }

        // PUT: api/B_BuyerOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Ordersid}")]
        public async Task<string> PutOrder(int Ordersid, B_BuyerOrdersDTO BuyerOrders)
        {
            if (Ordersid != BuyerOrders.OrderId)
            {
                return "不正確";
            }
            Order Buyer = await _context.Order.FindAsync(BuyerOrders.OrderId);
            Buyer.State= BuyerOrders.State;
            Buyer.DoneTime = DateTime.Now;
            _context.Entry(Buyer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Ordersid))
                {
                    return "找不到欲修改訂單";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        // POST: api/B_BuyerOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("FilterBuyerOrders/{BuyerId}")]
        public async Task<IEnumerable<B_BuyerOrdersDTO>> FilterOrder([FromBody] B_BuyerOrdersDTO OrderDTO, int BuyerId)
        {
            var Buyerordersearch = _context.Order.Where(
                emp => emp.OrderId.ToString().Contains(OrderDTO.OrderId.ToString()) && emp.State == "已完成" && emp.BuyerId == BuyerId).Join(_context.Product, pd => pd.ProductId, pds => pds.ProductId, (pd, pds) => new B_BuyerOrdersDTO

                {
                    ProductName = pds.ProductCategory.ProductName,
                    Image = pds.ImgFront,
                    SizeId = pds.Size,
                    Price = pd.BuyerPrice,
                    CompleteTime = pd.DoneTime,
                    ShippingAddress = pd.ShippingAddress,
                    State = pd.State,
                    Receiver = pd.Receiver,
                    OrderId = pd.OrderId,

                }).OrderByDescending(time => time.CompleteTime);

   
            return Buyerordersearch;
        }

        // DELETE: api/B_BuyerOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
