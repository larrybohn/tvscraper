using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Scraper.Domain.Contracts;
using Scraper.Domain.Dto;
using Scraper.Domain.Queries;
using ScraperApiService.Models;

namespace ScraperApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowQueryRepository repository;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public ShowsController(IShowQueryRepository repository, ILogger<ShowsController> logger, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1)
        {

            if (page < 1)
            {
                return BadRequest();
            }

            try
            {
                var shows = await repository.GetShowsAsync(new ShowQuery
                {
                    Page = page
                });

                foreach (var showDto in shows)
                {
                    showDto.Cast = showDto.Cast.OrderByDescending(a => a.Birthday);
                }

                return Ok(shows.Select(mapper.Map<ShowDto, ShowModel>));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error when getting shows");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
