using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> GetWalkAsync();
        Task<Walk> GetWalkByIdAsync(Guid id);
        Task<Walk> UpdateWalkAsync(Walk walk);
        Task<Walk> CreateAsync(Walk walk);
    }
}
