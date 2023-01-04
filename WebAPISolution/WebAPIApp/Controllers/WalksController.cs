using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIApp.Models.DTO;
using WebAPIApp.Repositories;

namespace WebAPIApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        //use constructor injecttion to use the WalksRepository that we have injected inside the Services collection 
        public WalksController(IWalkRepository walkRepository, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - domain walks
            var walksDomain = await walkRepository.GetAllAsync();

            //Convert domain walks to dto walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get Walk Domain object from database
            var walkDomain = await walkRepository.GetAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            //Convert domain object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //Return response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {

            // Validate the request 
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

                //convert DTO to domain Object
                var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // Pass domain object to repository to persist this
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //Convert DTO response back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            // Send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //validate the incming request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest))) {
                return BadRequest(ModelState);
            }

            //convert DTO to Domain Object
            var walkDomain = new Models.Domain.Walk
            {
                Id = id,
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
                RegionId = updateWalkRequest.RegionId,
            };

            // Pass details to Repository - Get Domain object  in response (or null)
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            // Handle Null (not found)
            if (walkDomain == null)
            {
                return NotFound();
            }
            else
            {

                //Convert back Domain to DTO
                var walkDTO = new Models.DTO.Walk
                {
                    Id = id,
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    WalkDifficultyId = walkDomain.WalkDifficultyId,
                    RegionId = walkDomain.RegionId,

                };

                //Return response
                return Ok(walkDTO);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            //Call Respository to delete walk
            var walkDomain = await walkRepository.DeleteAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }
            else
            {
                var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
                return Ok(walkDTO);
            }
        }

        #region private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {

           /* if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), "Add Walk data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be null or empty or whitespace.");
            }     
            
            if (addWalkRequest.Length <=0 )
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} should be greater than Zero.");
            }*/

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invlaid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invlaid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            /*if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), "Update Walk data is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be null or empty or whitespace.");
            }

            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater than Zero.");
            }
*/
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invlaid.");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invlaid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
