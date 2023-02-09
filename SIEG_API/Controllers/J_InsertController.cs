using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SIEG_API.DTO;
using SIEG_API.Models;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/J")]
    [ApiController]
    public class J_InsertController : ControllerBase
    {
        private readonly SIEGContext _context;

        public J_InsertController(SIEGContext context)
        {
            _context = context;
        }

        [HttpPost("InsertBuyerBid")]
        public async Task InsertBuyerBid([FromBody] J_AddBidQuote orderInfo)
        {
            // insert Bid
            BuyerBid bid = new BuyerBid
            {
                MemberId = orderInfo.mID,
                ProductId = orderInfo.pID,
                Price = (int)orderInfo.pPrice,
                FinalPrice = orderInfo.finalPrice,
                EffectiveTime = DateTime.Now
            };
            _context.BuyerBid.Add(bid);
            await _context.SaveChangesAsync();
        }

        [HttpPost("InsertSellerQuote")]
        public async Task InsertSellerQuote([FromBody] J_AddBidQuote quote)
        {
            SellerAddProduct sQuote = new SellerAddProduct
            {
                ProductId = quote.pID,
                MemberId = quote.sID,
                Price = quote.pPrice,
                FinalPrice = quote.finalPrice
            };
            _context.SellerAddProduct.Add(sQuote);
            await _context.SaveChangesAsync();
        }

        [HttpPost("InsertProductCategory")]
        public async Task InsertProductCategory(J_InsertProductCategory Insert)
        {
            Insert.Info = "從充滿校園氣息的 College Colors Program 到充滿活力的 Nike CO.JP 系列，Nike Dunk 系列球鞋自 1985 年設計問世以來已經推出了許多配色。但每一種新配色都為 Dunk 的經典撞色設計保留了一席之地。Nike 將其永恆的撞色設計與這款 Nike Dunk Low Retro White Black 球鞋完美搭配。|鞋面由白色皮革搭配黑色皮革覆面和 Swoosh 對勾組成。尼龍材質的鞋舌上飾有經典的 NIKE 品牌標誌，以向傳統的 Dunk 設計元素致敬。加上白色中底和黑色外底，便構成了這款亮眼潮鞋。|這款 Nike Dunk Low Retro White Black 球鞋於 2021 年 1 月發售，零售價為 $100 美元。";
            ProductCategory category = new ProductCategory
            {
                CategoryName = Insert.CateName,
                BrandName = Insert.BrandName,
                ProductName = Insert.Name,
                Img = Insert.Img,
                Note = Insert.Note,
                Info = Insert.Info
            };
            _context.ProductCategory.Add(category);

            if(Insert.ImgFront == null || Insert.ImgFront == "")
            {
                //Insert.pPrice = 5500;
                //Insert.ImgFront = "/images/product/高檔鞋履/Air Jordan/Jordan 1 Retro Low OG SP.jpg";
                //Insert.SizeList = new List<string> { "S", "M", "L", "XL"};
                //Insert.pModel = "Travis Scott Black Phantom";                
            }
            else if (Insert.CateName == "高檔鞋履")
            {
                Insert.SizeList = new List<string> { "9", "9.5", "10", "10.5", "11", "11.5", "12" };
            }else if (Insert.CateName == "潮流服飾")
            {
                Insert.SizeList = new List<string> { "S", "M", "L", "XL" };
            }
            else if (Insert.CateName == "精品腕錶" || Insert.CateName == "時尚包款")
            {
                Insert.SizeList = new List<string> { "0" };
            }


            List<Product> p = new List<Product>();

            for (int i = 0; i < Insert.SizeList.Count; i++)
            {
                p.Add(new Product
                {
                    Price = Insert.pPrice,
                    ImgFront = Insert.ImgFront,
                    Size = Insert.SizeList[i],
                    Model = Insert.pModel,
                    ProductCategory = category
                });
            }
            _context.Product.AddRange(p);
            await _context.SaveChangesAsync();
        }

        [HttpPost("InsertBuyerOrder")]
        public async Task InsertBuyerOrderAsync([FromBody] J_OrderInfo orderInfo)
        {
            try
            {
                if (orderInfo.info == "buy")
                {
                    // insert Order 
                    Order order = new Order
                    {
                        BuyerId = orderInfo.bID,
                        ProductId = orderInfo.pID,
                        BuyerPrice = orderInfo.finalPrice,
                        Pay = orderInfo.pay,
                        Receiver = orderInfo.receiver,
                        ReceivingPhone = orderInfo.receivingPhone,
                        ShippingAddress = orderInfo.shippingAddress,
                        UpdateTime = DateTime.Now,
                        //DoneTime = DateTime.Now, // 正式版要拿掉
                        AddTime = DateTime.Now,
                    };

                    order.State = "待出貨";
                    order.SellerId = orderInfo.sID;
                    order.SellerPrice = orderInfo.sellerPrice;
                    _context.Order.Add(order);
                    await _context.SaveChangesAsync();

                    // insert Bid
                    BuyerBid bid = new BuyerBid
                    {
                        OrderId = order.OrderId,
                        MemberId = orderInfo.bID,
                        ProductId = orderInfo.pID,
                        Price = (int)orderInfo.buyerPrice,
                        FinalPrice = orderInfo.finalPrice,
                        EffectiveTime = DateTime.Now,
                        SaleTime = DateTime.Now
                    };

                    _context.BuyerBid.Add(bid);
                    await _context.SaveChangesAsync();

                    // update Quote
                    var quote = await _context.SellerAddProduct.FirstOrDefaultAsync(x => x.SellerAddProductId == orderInfo.quoteID);
                    quote.OrderId = order.OrderId;
                    _context.SellerAddProduct.Update(quote);
                    await _context.SaveChangesAsync();
                }
                else if (orderInfo.info == "bid")
                {
                    //// insert Order 
                    //order.State = "出價中";
                    //_context.Order.Add(order);
                    //await _context.SaveChangesAsync();

                    // insert Bid
                    BuyerBid bid = new BuyerBid
                    {
                        MemberId = orderInfo.bID,
                        ProductId = orderInfo.pID,
                        Price = (int)orderInfo.buyerPrice,
                        FinalPrice = orderInfo.finalPrice,
                        EffectiveTime = DateTime.Now
                    };
                    _context.BuyerBid.Add(bid);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {     
                Console.WriteLine(ex.Message);
            }
        }


        [HttpPost("InsertSellOrder")]
        public async Task InsertSellOrder([FromBody] J_OrderInfo orderInfo)
        {
            // error check :
            if (orderInfo.bidID == 0 || orderInfo.sID == 0)
                throw new Exception("無法取得訂單資訊");

            var bid = await _context.BuyerBid.FindAsync(orderInfo.bidID);
            if (bid == null)
                throw new Exception("無法取得競標資訊");

            // alter Order: 
            var order = await _context.Order.Where(o => o.OrderId == bid.OrderId).SingleOrDefaultAsync();
            if (order == null)
            {
                throw new Exception("無法取得訂單資訊");
            }
            else
            {
                order.State = "待出貨";
                order.SellerId = orderInfo.sID;
                order.SellerPrice = orderInfo.finalPrice;
                order.UpdateTime = DateTime.Now;

                _context.Order.Update(order);
                await _context.SaveChangesAsync();

                // insert Quote : 填上 OrderID
                SellerAddProduct quote = new SellerAddProduct
                {
                    OrderId = order.OrderId,
                    FinalPrice = orderInfo.finalPrice,
                    MemberId = orderInfo.sID,
                    Price = (int)orderInfo.sellerPrice,
                    ProductId = orderInfo.pID,
                };

                _context.SellerAddProduct.Add(quote);
                await _context.SaveChangesAsync();
            }     
        }


        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.MemberId == id);
        }
    }
}
