using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
	public class SQLRegionRepository : IRegionRepository
	{
		private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;   
        }


        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();

        }

		

		public async Task<Region?> GetByIdAsync(Guid id)
		{
			return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Region> CreateAsync(Region region)
		{
			await _dbContext.Regions.AddAsync(region);
			await _dbContext.SaveChangesAsync();

			return region;
		}

		public async Task<Region?> UpdateAsync(Guid id, Region region)
		{
			var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(i => i.Id == id);
			if (regionDomainModel == null)
				return null;



			regionDomainModel.Code = region.Code;
			regionDomainModel.RegionImageUrl = region.RegionImageUrl;
			regionDomainModel.Name = region.Name;

			await _dbContext.SaveChangesAsync();
			return regionDomainModel;
		}

		public async Task<Region> DeleteAsync(Guid id)
		{
			var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(i =>i.Id == id);
			if (regionDomainModel == null)
				return null;

		   _dbContext.Regions.Remove(regionDomainModel);
			await _dbContext.SaveChangesAsync();

			return regionDomainModel;


		}
	}
}
