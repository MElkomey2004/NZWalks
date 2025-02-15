 using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public class SQLWalkRepository : IWalkRepository
	{
		private readonly NZWalksDbContext _dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
			_dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
		{
			if (walk == null)
				return null;

			await _dbContext.Walks.AddAsync(walk);
			await _dbContext.SaveChangesAsync();

			return walk;

		}

		public async Task<Walk> DeleteAsync(Guid id)
		{
			var walkDomainModel = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

			if (walkDomainModel == null)
				return null;

			 _dbContext.Walks.Remove(walkDomainModel);
			await _dbContext.SaveChangesAsync();

			return walkDomainModel;

		}

		public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true
			, int pageNumber = 1 , int pageSize = 1000)
		{
			var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

			//Filtering
			if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
			{
				if(filterOn.Equals("Name" , StringComparison.OrdinalIgnoreCase))
				{
					walks = walks.Where(x => x.Name.Contains(filterQuery));
				}

			}

			//Sorting
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if(sortBy.Equals("Name" , StringComparison.OrdinalIgnoreCase))
				{
					walks = isAscending ? walks.OrderBy(x => x.Name): walks.OrderByDescending(x => x.Name);
				}
				else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
				{
					walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
				}
			}


			//Pagination
			var skipResult = (pageNumber - 1) * pageSize;
			return await walks.Skip(skipResult).Take(pageSize).ToListAsync();


			//return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
		}

		public async Task<Walk?> GetByIdAsync(Guid id)
		{
			return await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
			
		}

		public async Task<Walk?> UpdateAsync(Guid id, Walk wallk)
		{
			var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x =>x.Id == id);
			if (existingWalk == null)
				return null;

			existingWalk.Description = wallk.Description;
			existingWalk.Name = wallk.Name;
			existingWalk.LengthInKm =wallk.LengthInKm;
			existingWalk.WalkImageUrl = wallk.WalkImageUrl;
			existingWalk.DifficultyId = wallk.DifficultyId;
			existingWalk.RegionId = wallk.RegionId;

			await _dbContext.SaveChangesAsync();
			
			return existingWalk;
		}


	}
}
