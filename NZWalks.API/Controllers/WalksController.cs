using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly AppDBContext context;
        private readonly IWalkRepository repo;

        public WalksController(IMapper mapper, AppDBContext context, IWalkRepository repo)
        {
            this.mapper = mapper;
            this.context = context;
            this.repo = repo;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var walkDomainModel = await repo.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> Create([FromBody]AddWalkRequestDTO addWalkRequestDTO)
        {
           
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTO);
                var createdWalk = await repo.CreateAsync(walkDomainModel);
                var walkDTO = mapper.Map<WalkDTO>(createdWalk);
                return CreatedAtAction(nameof(GetById), new { id = walkDTO.Id }, walkDTO);
            
            
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var walksdomainmodel=  await repo.GetAllAsync();

            return Ok(mapper.Map<List<WalkDTO>>(walksdomainmodel));
        }


        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDTO);
                var updatedWalk = await repo.UpdateAsync(id, walkDomainModel);
                if (updatedWalk == null)
                {
                    return NotFound();
                }
                var walkDTO = mapper.Map<WalkDTO>(updatedWalk);
                return Ok(walkDTO);
            

            
        }


        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deletedWalk = await repo.DeleteAsync(id);
            if (deletedWalk == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<WalkDTO>(deletedWalk);
            return Ok(walkDTO);
        }
    }
}
