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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,string? sortBy=null,bool isAscending=true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _context.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));

                }
                else if (filterOn.Equals("Description",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                }

                

                //var walks = await _context.Walks.Include("Difficulty").Include("Region").ToListAsync();
                //return walks;

            }
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }

            }

            var skipResult = (pageNumber - 1) * pageSize;

            

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();

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
