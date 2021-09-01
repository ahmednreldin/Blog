using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSchool.src.Data;
using OpenSchool.src.Models;

namespace OpenSchool.src.Services
{
    public class BlogServices : IBlogServices
    {
        private ApplicationDbContext _db;

        public BlogServices(ApplicationDbContext db)
        {
            _db = db;
        }
        public BlogModel AddArticle(BlogModel Article)
        {
             _db.Add(Article);
            SaveChanges();
            return Article;
        }

        public List<BlogModel> GetAllArticles()
        {
            return _db.Blogs.ToList();
        }

        public BlogModel GetArticleById(Guid id)
        {
            return _db.Blogs.Where(p => p.Id == id).FirstOrDefault();
        }

        public void RemoveArticle(Guid id)
        {
            var existing = GetArticleById(id);
            _db.Remove(existing);
            SaveChanges();
        }


        public void UpdateArticle(BlogModel Article)
        {
            _db.Blogs.Update(Article);
            SaveChanges();
        }
        private void SaveChanges()
        {
            _db.SaveChanges();
        }

    }
}
