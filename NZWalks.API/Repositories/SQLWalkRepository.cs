using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly AppDBContext _context;
        private readonly IMapper mapper;

        public SQLWalkRepository(AppDBContext context,IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = _context.Walks.FirstOrDefault(x => x.Id == id);
            if(existingWalk == null)
            {
                return null;
            }

            _context.Walks.Remove(existingWalk);
            _context.SaveChanges();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            var walks = await _context.Walks.Include("Difficulty").Include("Region").ToListAsync();
            return walks;
            
        }

        public Task<Walk> GetByIdAsync(Guid id)
        {
            var walk = _context.Walks.Include("Difficulty").Include("Region").FirstOrDefault(x => x.Id == id);
            return Task.FromResult(walk);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = _context.Walks.FirstOrDefault(x => x.Id == id);
            if(existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;


            await _context.SaveChangesAsync();

            
            return existingWalk;
        }
    }
}
