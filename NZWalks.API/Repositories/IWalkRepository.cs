  using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public interface IWalkRepository 
	{
		Task<List<Walk>> GetAllAsync();

		Task<Walk?> GetByIdAsync(Guid id);

		Task<Walk> CreateAsync(Walk walk);

		Task<Walk?> UpdateAsync(Guid id, Walk wallk);


		Task<Walk> DeleteAsync(Guid id);
	}
}
