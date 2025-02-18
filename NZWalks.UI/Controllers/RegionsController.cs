using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
	public class RegionsController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

		[HttpGet]

        public async Task<IActionResult> Index()
		{
			List<RegionDTO> response = new List<RegionDTO>();
			try
			{
				//Get All Regions from web api
				var client =  _httpClientFactory.CreateClient();

				var httpResponseMessage = await client.GetAsync("https://localhost:44304/api/regions");
				httpResponseMessage.EnsureSuccessStatusCode();

				response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());	

			}
			catch (Exception ex )
			{
				return Content("There are error that exist here");
				throw;
			}


			return View(response);
		}

		[HttpGet]
		
		public IActionResult Add()
		{
			return View();
		}


		[HttpPost]

		public async Task<IActionResult> Add(AddRegionViewModel model)
		{
			var client = _httpClientFactory.CreateClient();

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:44304/api/regions"),
				Content = new StringContent(JsonSerializer.Serialize(model) ,Encoding.UTF8,"application/json"),

			};

			var httpResponseMessage =  await client.SendAsync(httpRequestMessage);

			httpResponseMessage.EnsureSuccessStatusCode();

			var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

			if(response != null)
			{
				return RedirectToAction("Index" , "Regions");
			}

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var client = _httpClientFactory.CreateClient();

			var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:44304/api/regions/{id}");

			if(response is not null)
			{
				return View(response);
			}

			return View(null);

		}


		[HttpPost]

		public async Task<IActionResult> Edit(RegionDTO request)
		{
			var client = _httpClientFactory.CreateClient();

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:44304/api/regions/{request.Id.ToString()}"),
				Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"),

			};

			var httpResponseMessage =await client.SendAsync(httpRequestMessage);

			httpResponseMessage.EnsureSuccessStatusCode();
			var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

			
			if(response != null)
			{
				return RedirectToAction("Index", "Regions");
			}

			return View();

		}





		[HttpPost]
		public async Task<IActionResult> Delete(RegionDTO request)
		{
			
		
				var client = _httpClientFactory.CreateClient();

				var httpResponseMessage = await client.DeleteAsync($"https://localhost:44304/api/regions/{request.Id}");

				httpResponseMessage.EnsureSuccessStatusCode();

				return RedirectToAction("Index" , "Home");

		
	
	
			

		}
	}
}
