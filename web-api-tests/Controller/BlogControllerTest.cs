using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenSchool.src.Controllers.V1;
using OpenSchool.src.Models;
using OpenSchool.src.Services;
using web_api_tests.Services;
using Xunit;

namespace web_api_tests.Controller
{
    public class BlogControllerTest
    {
        BlogController _controller;
        IBlogServices _service;

        public BlogControllerTest()
        {
            _service = new BlogServicesFake();
            _controller = new BlogController(_service);
        }

        // Testing the Get Method

        [Fact]
        public void Get_WhenCalled_ReturnOkResult()
        {
            // ACT
            var okResult = _controller.Get();
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        [Fact]
        public void Get_WhenCalled_ReturnAllArticles()
        {
            //ACT
            var okResult = _controller.Get().Result as ObjectResult;

            //Assert
            var items = Assert.IsType<List<BlogModel>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        // Testing the GetById method

        [Fact]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // ACT
            var notFoundResult = _controller.Get(new Guid()); // new Id
            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }
        [Fact]
        public void GetById_ExistingIdPassed_ReturnOkResult()
        {
            // Arrange 
            var testId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // ACT
            var okResult = _controller.Get(testId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        [Fact]
        public void GetById_ExistingIdPassed_ReturnRightItem()
        {
             // Arrange
                var testInt = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            // Act
            var okResult = _controller.Get(testInt).Result as OkObjectResult;
            // Assert
            Assert.IsType<BlogModel>(okResult.Value);
            Assert.Equal(testInt, (okResult.Value as BlogModel).Id);
        }

        // Testing the Add Method
        [Fact]
        public void Add_InvalidObjectPassed_ReturnBadRequest()
        {
            // Arrange
            var bodyMissingItem = new BlogModel()
            {
                Title = "BodyMissing",
            };
            _controller.ModelState.AddModelError("Body", "Required");

            // Act
            var badResponse = _controller.Article(bodyMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }
        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            //Arrange
            var blogModel = new BlogModel 
            { Title = "NEW Article", Body = "BodyForBost" };

            //Act
            var createdResponse = _controller.Article(blogModel);

            //Asset 
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }
        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var blogModel = new BlogModel
            { Title = "NEW Article", Body = "BodyForBost" };

            // Act
            var createdRespone = _controller.Article(blogModel) as CreatedAtActionResult;
            var item = createdRespone.Value as BlogModel;

            //Assest
            Assert.IsType<BlogModel>(item);
            Assert.Equal("NEW Article", item.Title);
        }

        // Remove 
        [Fact]
        public void Remove_NonExisitingGuidPassed_ReturnNotFoundResponse()
        {
            //Arrange
            var notExistingGuid = Guid.NewGuid();

            //Act 
            var badResponse = _controller.Remove(notExistingGuid);

            //Assert
            Assert.IsType<NotFoundResult>(badResponse);
        } 
        [Fact]
        public void Remove_ExistingGuidPassed_ReturnOkResult()
        {
            // Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            // Act
            var okResponse = _controller.Remove(existingGuid);

            //Assert
            Assert.IsType<OkResult>(okResponse);

        }
        [Fact]
        public void Remove_ExisitingGuidPassed_ReomvesOneItem()
        {
            //Arrange
            var existingGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            //Act
            var okResponse = _controller.Remove(existingGuid);

            //Assert
            Assert.Equal(2, _service.GetAllArticles().Count());
        }
    }
}
