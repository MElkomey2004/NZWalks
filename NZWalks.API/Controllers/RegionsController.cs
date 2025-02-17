using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using Serilog.Core;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegionsController : ControllerBase
	{
        private readonly NZWalksDbContext _dbcontext;

        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;
        public RegionsController(NZWalksDbContext dbcontext , IRegionRepository regionRepository , IMapper mapper,
            ILogger<RegionsController> logger)
        {
            _dbcontext = dbcontext;
            _regionRepository =  regionRepository; 
            _mapper = mapper;   
            _logger = logger;
        }


        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {

            try
            {
				var regions = await _regionRepository.GetAllAsync();

				_logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regions)}");

				return Ok(_mapper.Map<List<RegionDTO>>(regions));
			}
            catch (Exception ex )
            {

                _logger.LogError(ex , ex.Message );
                throw;
            }

		}



		[HttpGet]
        [Route("{id:Guid}")]
		[Authorize(Roles = "Reader")]

		public async Task<IActionResult> GetById([FromRoute]Guid id) 
        {
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null)
                return NotFound();

            return Ok(_mapper.Map<RegionDTO>(region));  
        }



        [HttpPost]
        [ValidateModel]
		[Authorize(Roles = "Writer")]

		public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            
           

                    var regionDomainModel = _mapper.Map<Region>(addRegionRequestDTO);

			        regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

                    var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);
                    return CreatedAtAction(nameof(GetById) , new {id = regionDomainModel.Id} , regionDTO);
            

           
        }
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles ="Writer")]
		public async Task<IActionResult> Update([FromRoute]Guid id,[FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
          
                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDTO);

			    regionDomainModel = await _regionRepository.UpdateAsync(id , regionDomainModel);

                if (regionDomainModel == null)
                    return NotFound();

                return Ok(_mapper.Map<RegionDTO>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles ="Writer,Reader")]
		public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
                return NotFound();     

			return Ok(_mapper.Map<RegionDTO>(regionDomainModel));
        }
    }
}
