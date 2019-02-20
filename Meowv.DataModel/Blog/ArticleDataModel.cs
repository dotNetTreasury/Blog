﻿using Meowv.Entity;
using Meowv.Entity.Blog;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meowv.DataModel.Blog
{
    public class ArticleDataModel
    {
        private readonly MeowvDbContext _context;
        public ArticleDataModel(MeowvDbContext context) => _context = context;

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddArticle(ArticleEntity entity)
        {
            await _context.Articles.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteArticle(int articleId)
        {
            var article = await GetArticle(articleId);
            if (article != null)
            {
                article.IsDelete = 1;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateArticle(ArticleEntity entity)
        {
            var article = await GetArticle(entity.ArticleId);
            if (article != null)
            {
                article = entity;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        /// <summary>
        /// 获取文章详情
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public async Task<ArticleEntity> GetArticle(int articleId) => await _context.Articles.FindAsync(articleId);

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleEntity>> GetArticles() => await _context.Articles.Where(x => x.IsDelete == 0).OrderByDescending(x => x.PostTime).ToListAsync();

        /// <summary>
        /// 根据分类ID获取文章列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleEntity>> GetArticlesByCategoryId(int categoryId) => await _context.Articles.Where(a => a.ArticleId == 0).Join(_context.ArticleCategories.Where(c => c.IsDelete == 0), a => a.ArticleId, c => c.ArticleId, (a, c) => new ArticleEntity
        {
            ArticleId = a.ArticleId,
            Title = a.Title,
            Url = a.Url,
            Content = a.Content,
            PostTime = a.PostTime
        }).OrderByDescending(a => a.PostTime).ToListAsync();

        /// <summary>
        /// 根据标签ID获取文章列表
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleEntity>> GetArticlesByTagId(int tagId) => await _context.Articles.Where(a => a.ArticleId == 0).Join(_context.ArticleTags.Where(t => t.IsDelete == 0), a => a.ArticleId, t => t.ArticleId, (a, t) => new ArticleEntity
        {
            ArticleId = a.ArticleId,
            Title = a.Title,
            Url = a.Url,
            Content = a.Content,
            PostTime = a.PostTime
        }).OrderByDescending(a => a.PostTime).ToListAsync();
    }
}