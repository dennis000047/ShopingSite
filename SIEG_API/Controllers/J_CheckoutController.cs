using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
    public class J_CheckoutController : ControllerBase
    {
        private readonly SIEGContext _context;
        public J_CheckoutController(SIEGContext context)
        {
            _context = context;
        }

        [HttpGet("CheckoutMemberBid/{pID}/{mID}")]
        public async Task<J_BuyerBidDTO> CheckoutMemberBid(int pID, int mID)
        {
            return await _context.BuyerBid
                .Where(b => b.ProductId == pID && b.MemberId == mID)
                .Select(b => new J_BuyerBidDTO
                {
                    mID = b.MemberId,
                    pPrice = b.Price,
                    finalPrice = b.FinalPrice,
                    bidID = b.BuyerBidId
                }).FirstOrDefaultAsync();
        }



    }
}
