using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
	public class LocalImageRepository : IImageRepository
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly NZWalksDbContext _dbContext;
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment , IHttpContextAccessor httpContextAccessor,
			NZWalksDbContext dbContext)
        {
			_webHostEnvironment = webHostEnvironment;
			_httpContextAccessor = httpContextAccessor;	
			_dbContext = dbContext; ;
        }
        public async Task<Image> Upload(Image image)
		{


			var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath , "Images" ,$"{image.FileName}{image.FileExtension}");

			//Upload Image To Local Path
			using var stream = new FileStream(localFilePath, FileMode.Create);
			await image.File.CopyToAsync(stream);

			var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

			image.FilePath  = urlFilePath;	
		
			//Add Image To Database
			await _dbContext.Images.AddAsync(image);
			await _dbContext.SaveChangesAsync();


			return image;
		}
	}
}
