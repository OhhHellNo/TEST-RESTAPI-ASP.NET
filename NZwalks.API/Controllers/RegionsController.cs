using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.Models.Domains;
using NZwalks.API.Models.DTOs;
using NZwalks.API.Repository;

namespace NZwalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();
            var regionDto = new List<RegionDto>();

            regionDto = mapper.Map<List<RegionDto>>(regionsDomain);

            return Ok(regionDto);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetbyId(Guid id)
        {
            var regionDomain = await regionRepository.GetbyIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }
        [HttpPost]
        public async Task<IActionResult> PostRegion([FromBody] AddRegionDto addRegionDto)
        {
            //covert the dto into domain model 
            var region = mapper.Map<Region>(addRegionDto);
            //use domain model to create the region  

            var Region = await regionRepository.CreateAsync(region);
            //send the dto back to the user

            var addedregionDto = mapper.Map<RegionDto>(Region);

            return CreatedAtAction(
                nameof(GetbyId),
                new { id = addedregionDto.Id },
                region);

        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateregiondto)
        {
            // 1. Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateregiondto);
            var updatedDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (updatedDomainModel == null)
            {
                return NotFound(); // Better than BadRequest if the ID wasn't found
            }
            //convert the domainmodlelto dto 
            var regionDto = mapper.Map<RegionDto>(updatedDomainModel);
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {

            var tobedeletedregion = await regionRepository.DeleteByIdAsync(id);
            if (tobedeletedregion == null)
            {
                return NotFound();

            }

            var deletedata = mapper.Map<RegionDto>(tobedeletedregion);
            return Ok(deletedata);
        }

    }

}

