using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{

		[HttpGet]

		public IActionResult GetAllStudent()
		{
			string[] students = new string[] { "mohamed", "Sameh", "amr" };
			return Ok(students) ; 
		}
	}
}
