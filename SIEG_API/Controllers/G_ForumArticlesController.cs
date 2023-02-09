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
    public class G_ForumArticlesController : ControllerBase
    {
        private readonly SIEGContext _context;

        public G_ForumArticlesController(SIEGContext context)
        {
            _context = context;
        }

        // GET: api/G_ForumArticles
        [HttpGet]
        public async Task<IEnumerable<G_ForumArticlesDTO>> GetForumArticle()
        {
            return await _context.ForumArticle
                .Where(article => article.ValIdity == true)
                .Join(_context.Member, art => art.MemberId,
                member => member.MemberId, (art, member) => new G_ForumArticlesDTO
                {
                    ForumArticleId = art.ForumArticleId,
                    MemberId = art.MemberId,
                    Category = art.Category,
                    ProductCategoryId = art.ProductCategoryId,
                    Title = art.Title,
                    ArticleContent = art.ArticleContent,
                    LikeCount = art.LikeCount,
                    ViewsCount = art.ViewsCount,
                    AddTime = art.AddTime,
                    Img = art.Img,
                    ValIdity = art.ValIdity,
                    ReplyCount = art.ReplyCount,
                    NickName = member.NickName,
                }).ToListAsync();
        }

        // GET: api/G_ForumArticles/5
        [HttpGet("{id}")]
        public async Task<IEnumerable<G_ForumArticlesDTO>> GetForumArticle(int id)
        {

            var Article = await _context.ForumArticle.Where(ArticleId => ArticleId.ForumArticleId == id).Include(ArticleId => ArticleId.ProductCategory).Join(_context.Member, art => art.MemberId, member => member.MemberId, (art, member) => new G_ForumArticlesDTO
            {
                ForumArticleId = art.ForumArticleId,
                MemberId = art.MemberId,
                Category = art.Category,
                ProductCategoryId = art.ProductCategoryId,
                Title = art.Title,
                ArticleContent = art.ArticleContent,
                LikeCount = art.LikeCount,
                //ViewsCount = forumArticle.ViewsCount,
                AddTime = art.AddTime,
                Img = art.Img,
                ReplyCount = art.ReplyCount,
                NickName = member.NickName,
                BrandName = art.ProductCategory.BrandName,

            }).ToArrayAsync();

            return Article;
        }

        // PUT: api/G_ForumArticles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutForumArticle(int id, G_ForumArticlesDTO g_ForumArticlesDTO)
        {
            if (id != g_ForumArticlesDTO.ForumArticleId)
            {
                return "ID不正確";
            }
            ForumArticle pos = await _context.ForumArticle.FindAsync(id);
            pos.ForumArticleId = id;
            pos.MemberId = g_ForumArticlesDTO.MemberId;
            pos.Category = g_ForumArticlesDTO.Category;
            pos.ProductCategoryId = g_ForumArticlesDTO.ProductCategoryId;
            pos.Title = g_ForumArticlesDTO.Title;
            pos.ArticleContent = g_ForumArticlesDTO.ArticleContent;
            pos.LikeCount = g_ForumArticlesDTO.LikeCount;
            pos.Img = g_ForumArticlesDTO.Img;

            _context.Entry(pos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumArticleExists(id))
                {
                    return "找不到欲修改的資料";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        // PUT: api/G_ForumArticles/ReplyCount/id
        [HttpPut("ReplyCount/{id}")]
        public async Task<string> PutForumArticle(int id, G_ArticlesReplyCountDTO g_ArticlesReplyCountDTO)
        {
            if (id != g_ArticlesReplyCountDTO.ForumArticleId)
            {
                return "ID不正確";
            }
            ForumArticle pos = await _context.ForumArticle.FindAsync(id);
            pos.ForumArticleId = id;
            pos.ReplyCount = g_ArticlesReplyCountDTO.ReplyCount;

            _context.Entry(pos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumArticleExists(id))
                {
                    return "找不到欲修改的資料";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        // POST: api/G_ForumArticles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ForumArticle> PostForumArticle(G_ForumArticlesDTO forumArticle)
        {
            ForumArticle pos = new ForumArticle
            {
                MemberId = forumArticle.MemberId,
                Category = forumArticle.Category,
                ProductCategoryId = forumArticle.ProductCategoryId,
                Title = forumArticle.Title,
                ArticleContent = forumArticle.ArticleContent,
                Img = forumArticle.Img,
                ReplyCount = 0,
            };

            _context.ForumArticle.Add(pos);
            await _context.SaveChangesAsync();

            return pos;
        }

        // DELETE: api/G_ForumArticles/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteForumArticle(int id)
        {
            var forumArticle = await _context.ForumArticle.FindAsync(id);
            if (forumArticle == null)
            {
                return "找不到文章";
            }
            forumArticle.ValIdity = false;
            _context.ForumArticle.Update(forumArticle);
            await _context.SaveChangesAsync();

            return "刪除成功";
        }

        // SEARCH api/G_ForumArticles/Filter
        [HttpPost("Filter")]
        public async Task<IEnumerable<G_ForumArticlesDTO>> FilterEmployee([FromBody] G_ForumArticlesDTO ForumArticle)
        {
            return await _context.ForumArticle.Where(art => (art.Title.Contains(ForumArticle.Title) || art.ArticleContent.Contains(ForumArticle.Title)) && art.ValIdity == true).Join(_context.Member, art => art.MemberId, member => member.MemberId, (art, member) => new G_ForumArticlesDTO
            {
                ForumArticleId = art.ForumArticleId,
                MemberId = art.MemberId,
                Category = art.Category,
                ProductCategoryId = art.ProductCategoryId,
                Title = art.Title,
                ArticleContent = art.ArticleContent,
                LikeCount = art.LikeCount,
                AddTime = art.AddTime,
                Img = art.Img,
                ReplyCount = art.ReplyCount,
                NickName = member.NickName,
            }).ToListAsync();
        }

        private bool ForumArticleExists(int id)
        {
            return _context.ForumArticle.Any(e => e.ForumArticleId == id);
        }
    }
}
