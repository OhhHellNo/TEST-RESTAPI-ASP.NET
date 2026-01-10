using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.CustomActionFilters;
using NZwalks.API.Models.Domains;
using NZwalks.API.Models.DTOs;
using NZwalks.API.Repository;

namespace NZwalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalks([FromBody] AddnewalkDto addnewalkDto)
        {


            var reqtoadd = mapper.Map<Walk>(addnewalkDto);
            await walkRepository.CreateWalk(reqtoadd);
            return Ok(mapper.Map<AddnewalkDto>(reqtoadd));
        }


        [HttpGet]
        public async Task<IActionResult> Getwalks()
        {
            var walksDomain = await walkRepository.GetWalks();
            var walksDto = new List<AddnewalkDto>();

            walksDto = mapper.Map<List<AddnewalkDto>>(walksDomain);

            return Ok(walksDto);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var domainmodel = await walkRepository.GetWalkbyid(id);
            if (domainmodel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AddnewalkDto>(domainmodel));
        }
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalksDto updateWalksDto)
        {


            //map the dto to model 
            var domainwalkmodel = mapper.Map<Walk>(updateWalksDto);
            domainwalkmodel = await walkRepository.Updatewalk(id, domainwalkmodel);
            if (domainwalkmodel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<UpdateWalksDto>(domainwalkmodel));
        }


        [HttpDelete]
        [Route("{id:guid}")]
        async Task<IActionResult> Deletewalkbyid([FromRoute] Guid id)
        {
            var Tobedeleted = await walkRepository.Deletewalkbyid(id);
            if (Tobedeleted == null)
            {
                return NotFound();
            }
            //map the domain to dto .

            return Ok(mapper.Map<UpdateWalksDto>(Tobedeleted));



        }
    }

}
