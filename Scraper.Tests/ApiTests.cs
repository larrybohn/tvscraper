using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scraper.Domain.Contracts;
using Scraper.Domain.Dto;
using Scraper.Domain.Queries;
using ScraperApiService.Controllers;
using ScraperApiService.Mapping;
using ScraperApiService.Models;

namespace Scraper.Tests
{
    [TestClass]
    public class ApiTests
    {
        [TestInitialize]


        [TestMethod]
        public async Task Api_ReturnsBadRequestForNonPositivePage()
        {
            var controller = CreateController();
            var result = await controller.Get(page: 0);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Api_ReturnsResultsPaged()
        {
            var controller = CreateController();
            var result = await controller.Get(page: 1) as OkObjectResult;
            Assert.IsNotNull(result);
            var items = result.Value as IEnumerable<ShowModel>;
            Assert.IsNotNull(items);
            Assert.AreEqual(10, items.Count());
        }

        [TestMethod]
        public async Task Api_ReturnsEmptyResultWhenPageOutOfRange()
        {
            var controller = CreateController();
            var result = await controller.Get(page: 10) as OkObjectResult;
            Assert.IsNotNull(result);
            var items = result.Value as IEnumerable<ShowModel>;
            Assert.IsNotNull(items);
            Assert.AreEqual(0, items.Count());
        }

        private ShowsController CreateController()
        {
            var mockLogger = Mock.Of<ILogger<ShowsController>>();
            var mockRepository = new Mock<IShowQueryRepository>();
            mockRepository
                .Setup(repository => repository.GetShowsAsync(It.Is<ShowQuery>(query => query.Page == 1)))
                .Returns(Task.FromResult(
                    Enumerable.Range(0, 10)
                        .Select(x => new ShowDto
                        {
                            Cast = Enumerable.Empty<ActorDto>()
                        })));

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(ApiProfile)));
            var mapper = new Mapper(configuration);
            return new ShowsController(mockRepository.Object, mockLogger, mapper);
        }
    }
}
