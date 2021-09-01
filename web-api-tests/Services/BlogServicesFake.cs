using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSchool.src.Models;
using OpenSchool.src.Services;

namespace web_api_tests.Services
{

    public class BlogServicesFake : IBlogServices
    {
        private readonly List<BlogModel> _Articles;
        public BlogServicesFake()
        {
            _Articles = new List<BlogModel>()
                {
                    new BlogModel {Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),Title="Sport is my Life 1",Body="sport is for all"},
                    new BlogModel {Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),Title="Sport is my Life 2",Body="sport is for all"},
                    new BlogModel {Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),Title="Sport is my Life 3",Body="sport is for all"},
                };
        }
        public BlogModel AddArticle(BlogModel Article)
        {
            Article.Id = Guid.NewGuid();
            _Articles.Add(Article);
            return Article;
        }

        public List<BlogModel> GetAllArticles()
        {
            return _Articles;
        }

        public BlogModel GetArticleById(Guid id)
        {

            return _Articles.Where(p => p.Id == id).FirstOrDefault();
        }

         public void RemoveArticle(Guid id)
        {
            var existing = _Articles.First(p => p.Id == id);
            _Articles.Remove(existing);
        }

        public void UpdateArticle(BlogModel Article)
        {
            throw new NotImplementedException();
        }
    }
}
