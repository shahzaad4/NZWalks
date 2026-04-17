using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly AppDBContext context;

        public RegionsController(AppDBContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = context.Regions.ToList();
            var regionsDTO = new List<RegionDto>();
            foreach (var region in regions)
            {
                var regionDTO = new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                };
                regionsDTO.Add(regionDTO);
            }
            return Ok(regionsDTO );
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            var region = context.Regions.FirstOrDefault(x => x.Id == id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = new RegionDto()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDTO);
            //hello
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionDTO addRegionDTO)
        {
            if (ModelState.IsValid != true)
            {
                return NotFound();
            }
            var regionDomainModel = new Region
            {
                
                Code = addRegionDTO.Code,
                Name = addRegionDTO.Name,
                RegionImageUrl = addRegionDTO.RegionImageUrl
            };

            context.Regions.Add(regionDomainModel);
            context.SaveChanges();

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id },regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id,[FromBody] UpdateRegionDTO updateRegionDTO)
        {
            var regionDomainModel = context.Regions.FirstOrDefault(x => x.Id == id);

            if (ModelState.IsValid == false)
            {
                return NotFound();
            }
            regionDomainModel.Code = updateRegionDTO.Code;
            regionDomainModel.Name = updateRegionDTO.Name;
            regionDomainModel.RegionImageUrl = updateRegionDTO.RegionImageUrl;

            context.SaveChanges();

            var regionDto = new Region
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = context.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            context.Regions.Remove(regionDomainModel);
            context.SaveChanges();

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
