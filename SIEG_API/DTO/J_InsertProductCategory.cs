using Microsoft.EntityFrameworkCore;
using SIEG_API.Models;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace SIEG_API.DTO
{
    public class J_InsertProductCategory
    {

        public string CateName { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string Note { get; set; }
        public int pPrice { get; set; }
        public string ImgFront { get; set; }
        public string? Info { get; set; }
        public List<string>? SizeList { get; set; }
        public string pModel { get; set; }
        public virtual ProductCategory? ProductCategory { get; set; }

    }
}
