using Microsoft.AspNetCore.Mvc;
using SIEG.Models;
using System.Diagnostics;

namespace SIEG.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Member_buyerorder()
        {
            return View();
        }

        public IActionResult Member_personal()
        {
            return View();
        }

        public IActionResult Member_Membermodification()
        {
            return View();
        }

        public IActionResult Member_coupon()
        {
            return View();
        }

        public IActionResult Member_ProductCollection()
        {
            return View();
        }
        public IActionResult SellerAddProduct()
        {
            return View();
        }

        public IActionResult FavoritePosts()
        {
            return View();
        }

        public IActionResult SettingCreditcard()
        {
            return View();
        }

        public IActionResult PaymentInformation()
        {
            return View();
        }

        public IActionResult Mailinginformation()
        {
            return View();
        }

        public IActionResult Sellerinformation()
        {
            return View();
        }
        public IActionResult Passwordmodification()
        {
            return View();
        }

        public IActionResult Kyccertified()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost("FileUploads")]
        public async Task<IActionResult> Index2(List<IFormFile> image_uploads)
        {
            long size = image_uploads.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in image_uploads)
            {
                if (formFile.Length > 0)
                {
                    //臨時位置文件的完整路徑
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Benicon/memberidcard", formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            //處理上傳的文件
            //不要依賴或信任未經驗證的 FileName 屬性。
            //return Ok(new { count = image_uploads.Count, size, filePaths });
            //await Task.Delay(2900);
            return View("~/Views/Member/Kyccertified.cshtml");
        }
    }
}
