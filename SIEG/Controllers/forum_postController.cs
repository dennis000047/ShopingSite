using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class forum_postController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index2(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    //臨時位置文件的完整路徑
                   var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/forum/post", formFile.FileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            //處理上傳的文件
            //不要依賴或信任未經驗證的 FileName 屬性。
            await Task.Delay(2900);
            return View("~/Views/forum_list/Index.cshtml");
        }
    }
}
