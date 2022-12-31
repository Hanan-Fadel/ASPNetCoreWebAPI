using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIApp.Models.Domain;
using WebAPIApp.Repositories;

namespace WebAPIApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        //Inject the IRegionRepository in the constructor
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            //use the private field to get all the regions from the database
            var regions = await regionRepository.GetAllRegionsAsync();

            //Return DTO regions using automapper to map from domain to DTO so that the controller cannot access the database directly
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);

           /* //Return DTO regions without using automapper
            var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,
                };
                regionsDTO.Add(regionDTO);
            });
            return Ok(regionsDTO);*/
        }
    }
}
