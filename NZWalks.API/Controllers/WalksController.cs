using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult> GetById(Guid id)
        {
            var walkDomainModel = await repo.GetWalkByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody]AddWalkRequestDTO addWalkRequestDTO)
        {
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTO);
            var createdWalk = await repo.CreateAsync(walkDomainModel);
            var walkDTO = mapper.Map<WalkDTO>(createdWalk);
            return CreatedAtAction(nameof(GetById), new { id = walkDTO.Id }, walkDTO);
        }
    }
}
