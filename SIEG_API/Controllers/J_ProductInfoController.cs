using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NuGet.Protocol;
using SIEG_API.DTO;
using SIEG_API.Models;
using SIEG_API.Parameters;

namespace SIEG_API.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/J")]
    //[Route("api/[controller]")]
    [ApiController]
    public class J_ProductInfoController : ControllerBase
    {
        private readonly SIEGContext _context;
        public J_ProductInfoController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet("GetProductInfo/{pID}")]
        public async Task<IEnumerable<J_ProductInfoDTO>> getProductInfo(int pID)
        {
            var pCateID = await _context.Product.Where(p => p.ProductId == pID).Select(p => p.ProductCategoryId).SingleOrDefaultAsync();
            var list = await _context.Product.Include(p => p.ProductCategory).Where(p => p.ProductCategoryId == pCateID).Select(p => new J_ProductInfoDTO
            {
                pID = pID,
                pName = p.ProductCategory.ProductName,
                pBrand = p.ProductCategory.BrandName,
                pCateID = pCateID,
                pCateName = p.ProductCategory.CategoryName,
                pImg = p.ImgFront,
                pSize = p.Size,
                pModel = p.Model
            })
            .ToListAsync();
            return list;
        }

        [HttpGet("GetSizeAndQuote/{pCateID}")]
        public async Task<IEnumerable<J_PriceListDTO>> GetSizeAndQuote(int pCateID)
        {
            var list = await _context.Procedures.GetSizeAndQuoteAsync(pCateID);
            return list.Select(x => new J_PriceListDTO
            {
                pID = x.pID,
                pSize = x.pSize,
                pPrice = x.quotePrice,
                sID = x.sID,
                quoteID = x.quoteID,
            }).OrderBy(x => float.Parse(x.pSize));
        }

        [HttpGet("GetSizeAndBid/{pCateID}")]
        public async Task<IEnumerable<Object>> GetSizeAndBid(int pCateID)
        {
            var list = await _context.Procedures.GetSizeAndBidAsync(pCateID);
            return list.Select(x => new J_PriceListDTO
            {
                pID = x.pID,
                pSize = x.pSize,
                pPrice = x.bidPrice,
                bID = x.bID,
                bidID = x.bidID,
            }).OrderBy(x => float.Parse(x.pSize));
        }

        //[HttpGet("BidPriceList/{pID}")]
        //public async Task<IEnumerable<J_PriceListDTO>> getBidPrice([FromRoute] PriceParameter val)
        //{
        //    var productName = _context.Product.Include(p => p.ProductCategory).Where(p => p.ProductId == val.pID).ToList()[0].ProductCategory.ProductName;
        //    var BidInfo = await _context.BuyerBid
        //        .Include(b => b.Product).Join(_context.Order, b => b.OrderId, o => o.OrderId, (b, o) => new { b = b, o = o })
        //        .Where(b => b.b.ValIdity == true && b.b.Product.ProductCategory.ProductName == productName && b.o.SellerId == null)
        //        .GroupBy(b => new { b.b.Product.Size, b.b.Price, b.b.Product.ProductCategory.ProductName })
        //        .Select(x => new J_PriceListDTO
        //        {
        //            pID = val.pID,
        //            pPrice = x.Key.Price,
        //            pSize = x.Key.Size,
        //            pCount = x.Sum(b => b.b.Count)
        //        })
        //        .OrderBy(x => x.pPrice).ToListAsync();

        //    return BidInfo;
        //    //return new List<J_PriceListDTO>();
        //}

        [HttpGet("BidPriceList/{pID}")]
        public async Task<IEnumerable<J_PriceListDTO>> getBidPrice([FromRoute] PriceParameter val)
        {
            var productName = _context.Product.Include(p => p.ProductCategory).Where(p => p.ProductId == val.pID).ToList()[0].ProductCategory.ProductName;
            var BidInfo = await _context.BuyerBid
                .Include(b => b.Product)
                //.Join(_context.Order, b => b.OrderId, o => o.OrderId, (b, o) => new { b = b, o = o })
                .Where(b => b.ValIdity == true && b.Product.ProductCategory.ProductName == productName)
                .GroupBy(b => new { b.Product.Size, b.Price, b.Product.ProductCategory.ProductName })
                .Select(x => new J_PriceListDTO
                {
                    pID = val.pID,
                    pPrice = x.Key.Price,
                    pSize = x.Key.Size,
                    pCount = x.Sum(b => b.Count)
                })
                .OrderBy(x => x.pPrice).ToListAsync();

            return BidInfo;
            //return new List<J_PriceListDTO>();
        }

        [HttpGet("QuotePriceList/{pID}")]
        public async Task<IEnumerable<J_PriceListDTO>> getQuotePrice(int pID)
        {
            var productName = _context.Product.Include(p => p.ProductCategory).Where(p => p.ProductId == pID).ToList()[0].ProductCategory.ProductName;
            var QuoteInfo = await _context.SellerAddProduct
                .Include(s => s.Product)
                .Where(s => s.ValIdity == true && s.Product.ProductCategory.ProductName == productName && s.OrderId == null) // 待補上 s.SaleDate == null && 
                .GroupBy(s => new { s.Product.Size, s.Price, s.Product.ProductCategory.ProductName })
                .Select(x => new J_PriceListDTO
                {
                    pID = pID,
                    pPrice = x.Key.Price,
                    pSize = x.Key.Size,
                    pCount = x.Sum(b => b.Count)
                })
                .OrderBy(x => x.pPrice).ToListAsync();

            return QuoteInfo;
        }

        //[HttpGet("QuotePriceList/{pID}")]
        //public async Task<IEnumerable<J_PriceListDTO>> getQuotePrice(int pID)
        //{
        //    var productName = _context.Product.Include(p => p.ProductCategory).Where(p => p.ProductId == pID).ToList()[0].ProductCategory.ProductName;
        //    var QuoteInfo = await _context.SellerAddProduct
        //        .Include(s => s.Product)
        //        .Where(s => s.ValIdity == true && s.Product.ProductCategory.ProductName == productName && s.OrderId == null) // 待補上 s.SaleDate == null && 
        //        .GroupBy(s => new { s.Product.Size, s.Price, s.Product.ProductCategory.ProductName })
        //        .Select(x => new J_PriceListDTO
        //        {
        //            pID = pID,
        //            pPrice = x.Key.Price,
        //            pSize = x.Key.Size,
        //            pCount = x.Sum(b => b.Count)
        //        })
        //        .OrderBy(x => x.pPrice).ToListAsync();

        //    return QuoteInfo;
        //}

        [HttpGet("GetOrderList/{pID}")]
        public async Task<IEnumerable<J_OrderListDTO>> GetOrderList([FromRoute] PriceParameter val)
        {
            var productName = _context.Product.Include(p => p.ProductCategory).Where(p => p.ProductId == val.pID).ToList()[0].ProductCategory.ProductName;

            var list = await _context.Order.Include(o => o.Product)
                .Where(o => o.Product.ProductCategory.ProductName == productName && o.State != null && o.UpdateTime != null)//之後補上 && o.State == "已完成"
                .OrderByDescending(o => o.DoneTime)
                .Select(x => new J_OrderListDTO
                {
                    oID = x.Product.ProductId,
                    oPrice = x.BuyerPrice,
                    oTime = x.UpdateTime,
                    oSize = x.Product.Size,
                }).ToListAsync();

            return list;
        }

        [HttpGet("GetLastDealPrice/{pID}")]
        public async Task<int?> GetLastDealPrice(int pID)
        {
            var lastPrice = 0;
            //var price = await _context.Order.Where(o => o.ProductId == pID && o.DoneTime != null)//之後補上 && o.State == "已完成" )
            var price = await _context.Order.Where(o => o.ProductId == pID && o.State != null && o.UpdateTime != null)//之後補上 && o.State == "已完成" )
               .OrderByDescending(o => o.DoneTime).Select(o => o.BuyerPrice)
               .FirstOrDefaultAsync();
            if (price != null)
            {
                lastPrice = (int)price;
            }
            //.MinAsync(o => o.價錢); 找最少的價格
            return lastPrice;
        }

        [HttpGet("GetMemberInfo/{mID}")]
        public async Task<J_MenberInfo> GetMemberInfo(int mID)
        {
            var Member = await _context.Member.Where(o => o.MemberId == mID).FirstOrDefaultAsync();
            var mDTO = new J_MenberInfo
            {
                mID = Member.MemberId,
                mName = Member.Name,
                mAddress = Member.Address,
                mPhone = Member.Phone
            };

            return mDTO;
        }

        [HttpGet("GetMemberBankInfo/{mID}")]
        public async Task<J_MemberBankInfoDTO> GetMemberBankInfo(int mID)
        {
            var Member = await _context.Member
                .Join(_context.Bank, m => m.BankCode, b => b.BankCode, (m, b) => new { member = m, bank = b })
                .Where(m => m.member.MemberId == mID)
                .Select(o => o).SingleOrDefaultAsync();
            if (Member != null)
            {
                var mDTO = new J_MemberBankInfoDTO
                {
                    mID = Member.member.MemberId,
                    mBankName = Member.bank.Name,
                    mBankCode = Member.member.BankCode,
                    mBankAccount = Member.member.BankAccount.ToString(),
                };
                return mDTO;
            }
            else
            {
                var mDTO = new J_MemberBankInfoDTO
                {
                    mID = mID,
                    mBankName = "",
                    mBankCode = "",
                    mBankAccount = ""
                };
                return mDTO;
            }
        }

        [HttpGet("GetMaxMinPrice/{pID}")]
        public async Task<object> GetMaxMinQuote(int pID)
        {
            var maxBid = 0;
            var minQuote = 0;
            var bid = await _context.BuyerBid.Where(b => b.ProductId == pID)
                .Select(s => s.Price).OrderByDescending(s => s).FirstOrDefaultAsync();
            var quote = await _context.SellerAddProduct
                .Where(s => s.ProductId == pID && s.Price != 0 && s.OrderId == null)
                .Select(s => s.Price).OrderBy(s => s).FirstOrDefaultAsync();
            if (bid != null)
            {
                maxBid = bid;
            }
            if (quote != null)
            {
                minQuote = quote;
            }
            var list = new
            {
                maxBid = maxBid,
                minQuote = minQuote
            };
            return list;
        }

        [HttpGet("GetProductNote/{pCateID}")]
        public async Task<IEnumerable<J_KeyValuePair>> GetProductNote(int pCateID)
        {
            var list = await _context.ProductCategory.FindAsync(pCateID);
            string[] noteArray = list.Note.ToString().Split("|");
            string info = list.Info.ToString();

            List<J_KeyValuePair> items = new List<J_KeyValuePair>();

            for (int i = 0; i < noteArray.Length; i += 2)
            {
                items.Add(new J_KeyValuePair
                {
                    key = noteArray[i],
                    value = noteArray[i + 1],
                });
            }
            items[0].info = info;

            return items;
        }

        [HttpGet("GetHistoricalData/{pCateID}")]
        public async Task<IEnumerable<J_HistoricalData>> GetHistoricalData(int pCateID)
        {
            var list = await _context.Product.Include(p => p.ProductCategory).Include(p => p.Order).Where(p => p.ProductCategoryId == pCateID).ToListAsync();

            var minPrice = list.Select(p => p.Order.Min(o => o.BuyerPrice)).Min().ToString();
            var maxPrice = list.Select(p => p.Order.Max(o => o.BuyerPrice)).Max().ToString();
            var dealPrice = list.Select(p => p.Order.Sum(o => o.BuyerPrice)).Sum().ToString();
            var avgPrice = list.Select(p => p.Order.Average(o => o.BuyerPrice)).Average().ToString();

            List<J_HistoricalData> historList = new List<J_HistoricalData>
            {
                new J_HistoricalData { val1 = minPrice, val2 = maxPrice, val3 = "交易區間" },
                new J_HistoricalData { val1 = dealPrice, val2 = "", val3 = "成交量" },
                new J_HistoricalData { val1 = avgPrice, val2 = "", val3 = "平均交易價" },
            };
            return historList;
        }


        [HttpGet("GetRelatedProducts/{pID}")]
        public async Task<IEnumerable<J_ProductInfoDTO>> GetRelatedProducts(int pID)
        {
            var pCateList = await _context.Product.Include(p => p.ProductCategory)
                .Where(p => p.ProductId == pID).Select(p => new { p.ProductCategory.CategoryName, p.ProductCategoryId }).FirstAsync();

            var result = await _context.ProductCategory
                .Join(_context.Product, pc => pc.ProductCategoryId, p => p.ProductCategoryId, (pc, p) => new { pc, p })
                .Where(i => i.pc.CategoryName == pCateList.CategoryName).Select(i => new J_ProductInfoDTO
                {
                    pID = i.p.ProductId,
                    pName = i.pc.ProductName,
                    pImg = i.p.ImgFront,

                }).ToListAsync();

            var HashSet = new HashSet<string>();
            var filteredList = new List<J_ProductInfoDTO>();

            foreach (var item in result)
            {
                if (HashSet.Add(item.pName))
                {
                    filteredList.Add(item);
                }
            }
            var quote = await GetSizeAndQuote(pCateList.ProductCategoryId);
            filteredList.Join(quote, f => f.pID, q => q.pID, (f, q) => new J_ProductInfoDTO
            {
                pID = f.pID,
                pName = f.pName,
                pImg = f.pImg,

            });
            return filteredList;
        }

        [HttpGet("GetMemberQuote/{mID}")]
        public async Task<object> GetMemberQuote(int mID)
        {
            return await _context.SellerAddProduct.Where(s => s.MemberId == mID)
                .Select(s => new J_MemberQuoteBid
                {
                    mID = s.MemberId,
                    quoteID = s.SellerAddProductId,
                    pPrice = s.Price,
                    pID = s.ProductId,

                }).ToListAsync();
        }

        //[HttpGet("CheckoutMemberBid")]
        //public async Task<J_BuyerBidDTO> CheckoutMemberBid(J_CheckoutMmember list)
        //{
        //    return await _context.BuyerBid
        //        .Where(b => b.ProductId == list.pID && b.MemberId == list.mID)
        //        .Select(b => new J_BuyerBidDTO
        //        {
        //            mID = b.MemberId,
        //            pPrice = b.Price,
        //            finalPrice = b.FinalPrice,
        //            bidID = b.BuyerBidId
        //        }).FirstOrDefaultAsync();
        //}
    }
}

//public class Item
//{
//    public string Key { get; set; }
//    public string Value { get; set; }
//}
//[HttpGet("GetProductNote/{pCateID}")]
//public async Task<IEnumerable<Item>> GetProductNote(int pCateID)
//{
//    var list = await _context.ProductCategory.FindAsync(pCateID);
//    string[] noteArray = list.Note.ToString().Split("|");

//    List<Item> items = new List<Item>();
//    for (int i = 0; i < noteArray.Length; i += 2)
//    {
//        items.Add(new Item
//        {
//            Key = noteArray[i],
//            Value = noteArray[i + 1]
//        });
//    }

//    return items;
//}



//var BrandName = "Air Jordan";
//var CategoryName = "高檔鞋履";

//var SaleInfo = await _context.Order.Include(o => o.Product).Include(o => o.Product.SellerAddProduct)
//    .Include(o => o.Product.ProductCategory).Where(x => x.Product.ProductCategory.CategoryName
//    .Contains(CategoryName) && x.Product.ProductCategory.BrandName.Contains(BrandName))
//    .GroupBy(x => new { x.Product.Name, x.Product.ImgFront })
//    .Select(g => new J_SaleCount
//    {
//        SaleCount = g.Count(),
//        Name = g.Key.Name,
//        Img = g.Key.ImgFront,
//        minPrice = g.Min(x => x.Price)
//    }).ToListAsync();

//return SaleInfo;

//var saleCount = from o in _context.Order
//                join p in _context.Product on o.ProductId equals p.ProductId
//                join s in _context.SellerAddProduct on p.ProductId equals s.ProductId
//                join pc in _context.ProductCategory on p.ProductCategoryId equals pc.ProductCategoryId
//                //where pc.CategoryName.Contains("高檔鞋履")
//                group new { p, s } by p.Name into g
//                select new
//                {
//                    Img = g,
//                    銷售數量 = g.Count(),
//                    g.Key,
//                    最低價 = g.Min(x => x.s.Price)
//                };

//[HttpGet("GetSizeAndQuote/{pCateID}")]
//public async Task<List<J_PriceListDTO>> GetSizeAndQuote(int pCateID)
//{
//    var sizeAndPrice = await _context.Product
//    .Join(_context.SellerAddProduct, product => product.ProductId, s => s.ProductId,
//    (p, s) => new { Product = p, seller = s, sID = s.MemberId })
//    .Where(p => p.seller.Price != 0 && p.Product.ProductCategoryId == pCateID)
//    .GroupBy(p => p.Product.Size)
//    .Select(g => g.OrderBy(g => g.seller.Price)
//    .First()).ToListAsync();

//    List<J_PriceListDTO> priceList = sizeAndPrice.Select(p => new J_PriceListDTO
//    {
//        sID = p.sID,
//        pID = p.Product.ProductId,
//        pPrice = p.seller.Price,
//        pSize = p.Product.Size
//    }).ToList();
//    return priceList;
//}