using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly UserManager<IdentityUser> _userManager;
		private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager , ITokenRepository tokenRepository)
        {
			_userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
		{
			var identiyUser = new IdentityUser
			{
				UserName = registerRequestDTO.UserName,
				Email = registerRequestDTO.UserName
			};

			var identityResult = await _userManager.CreateAsync(identiyUser , registerRequestDTO.Password);

			if(identityResult.Succeeded)
			{
				if(registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any()){

					identityResult = 	await _userManager.AddToRolesAsync(identiyUser , registerRequestDTO.Roles);

					if(identityResult.Succeeded)
					{
						return Ok("User was registered? Please Login");
					}
				}
			}

			return BadRequest("Something went Wrong");
		}

		[HttpPost]
		[Route("Login")]

		public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
		{
			var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);

			if(user != null)
			{
				var checkPasswordResult  = await _userManager.CheckPasswordAsync(user , loginRequestDTO.Password);

				if (checkPasswordResult)
				{
					//Get Roles for this user
					var roles =  await _userManager.GetRolesAsync(user);
					if(roles != null)
					{
						//Create Token
						var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());
						var response = new LoginResponseDTO
						{
							JwtToken = jwtToken
						};
						return Ok(response);

					}


				}
			}

			return BadRequest("Username or password incorrect");

		}
	}
}
