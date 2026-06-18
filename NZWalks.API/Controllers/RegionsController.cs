using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly AppDBContext context;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(AppDBContext context,IRegionRepository regionRepository,IMapper mapper)
        {
            this.context = context;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
            //var regionsDTO = new List<RegionDto>();
            //foreach (var region in regions)
            //{
            //    var regionDTO = new RegionDto()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl
            //    };
            //    regionsDTO.Add(regionDTO);
            //}
            var regionsDTO= mapper.Map<List<RegionDto>>(regions);
            return Ok(regionsDTO );
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            //var regionDTO = new RegionDto()
            //{
            //    Id = region.Id,
            //    Name = region.Name,
            //    Code = region.Code,
            //    RegionImageUrl = region.RegionImageUrl
            //};

            var regionDTO = mapper.Map<RegionDto>(region);
            return Ok(regionDTO);
            //hello
        }

        [HttpPost]
        [ValidateModel] // Custom action filter to validate the model state
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            
                var regionDomainModel = mapper.Map<Region>(addRegionDTO);

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            
            //var regionDomainModel = new Region
            //{

            //    Code = addRegionDTO.Code,
            //    Name = addRegionDTO.Name,
            //    RegionImageUrl = addRegionDTO.RegionImageUrl
            //};
            
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel] // Custom action filter to validate the model state
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionDTO updateRegionDTO)
        {
            
                //var regionDomainModel = new Region
                //{
                //    Code = updateRegionDTO.Code,
                //    Name = updateRegionDTO.Name,
                //    RegionImageUrl = updateRegionDTO.RegionImageUrl
                //};
                var regionDomainModel = mapper.Map<Region>(updateRegionDTO);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);



                await context.SaveChangesAsync();

                //var regionDto = new Region
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl
                //};
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
            
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }


            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
