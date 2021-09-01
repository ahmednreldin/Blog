using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSchool.src.Models;

namespace OpenSchool.src.Services
{
    public interface IBlogServices
    {
        // CRUD Operations 
        public BlogModel GetArticleById(Guid id);
        public List<BlogModel> GetAllArticles();
        public BlogModel AddArticle(BlogModel Article);
        public void RemoveArticle(Guid id);
        public void UpdateArticle(BlogModel Article);

    }
}
