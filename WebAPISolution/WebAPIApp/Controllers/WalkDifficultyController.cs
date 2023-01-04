using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIApp.Repositories;

namespace WebAPIApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulty()
        {
            var walkDifficultyList = await walkDifficultyRepository.GetAllAsync();
            return Ok(walkDifficultyList);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficulty")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficulty([FromRoute] Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficultyDomain.Code
            };

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficulty(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest) 
        {
            //validate the incoming request
           /* if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }*/
            //convert from DTO model to domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            //Add the new WalkDiffcilty to database
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //convert domain model back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficultyDomain.Code
            };

            //Return response
            return CreatedAtAction(nameof(GetWalkDifficulty), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficulty([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //validate the incoming request
           /* if(!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }*/
            //convert from DTO to Domain
            var exisityWalkDifficulty = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            // update t he database
            exisityWalkDifficulty = await walkDifficultyRepository.UpdateAsync(id, exisityWalkDifficulty);

            //Handle null

            if (exisityWalkDifficulty == null)
            {
                return NotFound();
            }

            //Convert from Domain back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = exisityWalkDifficulty.Code
            };

            return Ok(walkDifficultyDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficulty([FromRoute] Guid id) 
        {
            // Call the repository to Delete the WalkDifficulty
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            //Handle the null case
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //Convert from Domain to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // Send the response back 
            return Ok(walkDifficultyDTO);
        }

        #region private methods
        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest) 
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"{nameof(addWalkDifficultyRequest)} is required.");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} is required.");
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"{nameof(updateWalkDifficultyRequest)} is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} is required.");
                return false;
            }

            return true;
        }
        
        #endregion

    }
}
