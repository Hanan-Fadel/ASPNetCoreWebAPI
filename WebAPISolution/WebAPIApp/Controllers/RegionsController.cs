using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIApp.Models.Domain;
using WebAPIApp.Models.DTO;
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
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            //use the private field to get all the regions from the database
            var regions = await regionRepository.GetAllAsync();

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

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //map from domain model to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //conver DTO request to domain model as we need to add resource to database
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };


            //Pass details to repository (database) 
            region = await regionRepository.AddAsync(region);

            //Convert data back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            // var regionDTO = mapper.Map<Models.DTO.Region>(region); ???


            //Return 201 response to client 
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {

            //Delete region from database (If found)
            var region = await regionRepository.DeleteAsync(id);

            //If null, retutn NotFound response
            if (region == null)
            {
                return NotFound();
            }

            //If exist, convert response back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            //return Ok response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Convert the DTO to Domain Model
            var region = new Models.Domain.Region()
            {

                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population,

            };

            //Update region using repository
             region = await regionRepository.UpdateAsync(id, region);

            //if null, return NotFound
            if (region == null)
            {
                return NotFound();
            }

            //convert Domain back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            //return Ok response
            return Ok(regionDTO);
        }
    }
}
