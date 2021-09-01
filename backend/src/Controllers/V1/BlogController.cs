using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using OpenSchool.src.Models;
using OpenSchool.src.Services;
using static OpenSchool.src.Contract.V1.ApiRoutes;

namespace OpenSchool.src.Controllers.V1
{
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogServices _service;

        public BlogController(IBlogServices services)
        {
            _service = services;
        }

        [HttpGet(Blog.getAllArticles)]
        public ActionResult<List<BlogModel>> Get()
        {
            var Articles = _service.GetAllArticles();
            return Ok(Articles);
        }
        [HttpGet(Blog.getArticleById)]
        public ActionResult<BlogModel> Get(Guid id)
        {
            var Article = _service.GetArticleById(id);

            if (Article == null)
            {
                return NotFound();
            }
            return Ok(Article);
        }

        [HttpPost(Blog.addArticle)]
        public ActionResult Article([FromBody] BlogModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _service.AddArticle(value);

            return CreatedAtAction("Get", new { id = item.Id }, item);
        }

        [HttpDelete(Blog.removeArticle)]
        public ActionResult Remove(Guid id)
        {
            var existingItem = _service.GetArticleById(id);

            if (existingItem == null)
            {
                return NotFound();
            }
            _service.RemoveArticle(id);
            return Ok();
        }

        [HttpPut(Blog.updateArticle)]
        public ActionResult Update(BlogModel blog)
        {
            var existingItem = _service.GetArticleById(blog.Id);
            if(existingItem == null)
            {
                return NotFound();
            }
            _service.UpdateArticle(blog);
            return Ok();
        }
    }
}
