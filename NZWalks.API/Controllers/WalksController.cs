using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WalksController : ControllerBase
	{
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper , IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }


        [HttpPost]
        [ValidateModel]

        public async Task<IActionResult> Create([FromBody]AddWalkRequestDTO addWalkRequestDTO)
        {

                //Map DTO To Domain Model


                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);

                await _walkRepository.CreateAsync(walkDomainModel);

                //Map Domain Model To DTO

                return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }


        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] string? filterOn , [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy , [FromQuery] bool? isAscending
            , [FromQuery] int pageNumber = 1, [FromQuery] int pageSize =1000)
        {
            var walksDomainModel = await _walkRepository.GetAllAsync(filterOn , 
                filterQuery ,sortBy , isAscending ??true,
                pageNumber ,
                pageSize);

            //Map domainMOdel To WalkDTO

            return Ok(_mapper.Map<List<WalkDTO>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
                return NotFound();

            //Map Domain To DTO

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }


        [HttpPut]
		[Route("{id:guid}")]
        [ValidateModel]

		public async Task<IActionResult> Update([FromRoute] Guid id , UpdateWalkRequestDTO updateWalkRequestDTO)
        {
           

                 var walkDomainModel =  _mapper.Map<Walk>(updateWalkRequestDTO);

                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                    return NotFound();


                return Ok(_mapper.Map<WalkDTO>(walkDomainModel));

        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(id);
            if (walkDomainModel == null)
                return NotFound();

            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }
    }
}
