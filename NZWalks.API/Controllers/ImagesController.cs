using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
			_imageRepository = imageRepository;
            
        }

        [HttpPost]

		[Route("Upload")]
		public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTO request)
		{
			ValidateFileUpdload(request);	
			if(ModelState.IsValid)
			{

				//Convert DTO To Domain Model 

				var imageDomainModel = new Image
				{
					File = request.File,
					FileExtension = Path.GetExtension(request.File.FileName),
					FileSizeInBytes = request.File.Length,
					FileName = request.FileName,
					FileDescription = request.FileDescription

				};
				// User repository to upload image

				await _imageRepository.Upload(imageDomainModel);

				return Ok(imageDomainModel);

			}
			
			return BadRequest(ModelState);

		}
		

		private void ValidateFileUpdload(ImageUploadRequestDTO request)
		{
			var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
			if(!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
			{
				ModelState.AddModelError("file", "Unsupported file extension");
			}

			if(request.File.Length > 10485760)
			{
				ModelState.AddModelError("File", "File Size more than 10MB, please upload a smaller size file.");
			}
		}
	}
}
