using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly AppDBContext _context;

        public SQLWalkRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public Task<Walk> GetWalkAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Walk> GetWalkByIdAsync(Guid id)
        {
            var walk = _context.Walks.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(walk);
        }

        public Task<Walk> UpdateWalkAsync(Walk walk)
        {
            throw new NotImplementedException();
        }
    }
}
