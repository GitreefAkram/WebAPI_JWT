
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_JWT.DataDB;
using WebAPI_JWT.Model;

namespace WebAPI_JWT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{

		public IConfiguration _configuration;
		private readonly DialZeroNGContext _context;

		public AuthenticationController(IConfiguration config, DialZeroNGContext context)
		{
			_configuration = config;
			_context = context;
		}

		//[HttpPost]
		//public async Task<IActionResult> Post(Login _userData)
		//{
		//	if (_userData != null && _userData.UserName != null && _userData.Password != null)
		//	{
		//		var user = await GetUser(_userData.UserName, _userData.Password);

		//		if (user != null)
		//		{
		//			//create claims details based on the user information
		//			var claims = new[] {
		//				new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
		//				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		//				new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
		//				new Claim("UserId", user.UserId.ToString()),
		//				new Claim("DisplayName", user.DisplayName),
		//				new Claim("UserName", user.UserName),
		//				new Claim("Email", user.Email)
		//			};

		//			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		//			var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		//			var token = new JwtSecurityToken(
		//				_configuration["Jwt:Issuer"],
		//				_configuration["Jwt:Audience"],
		//				claims,
		//				expires: DateTime.UtcNow.AddMinutes(10),
		//				signingCredentials: signIn);

		//			return Ok(new JwtSecurityTokenHandler().WriteToken(token));
		//		}
		//		else
		//		{
		//			return BadRequest("Invalid credentials");
		//		}
		//	}
		//	else
		//	{
		//		return BadRequest();
		//	}
		//}

		private async Task<bool> GetUser(string username, string password)
		{
			var User = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.PasswordHash == password);
			if (User != null)
			{
				return true;
			}
			else
				return false;

		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] Login user)
		{
			if (user is null)
			{
				return BadRequest("Invalid user request!!!");
			}

			if (await GetUser(user.UserName, user.Password))
			{
				var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
				
				var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
				
				var tokeOptions = new JwtSecurityToken(
					issuer: ConfigurationManager.AppSetting["JWT:Issuer"], 
					audience: ConfigurationManager.AppSetting["JWT:Audience"], 
					claims: new List<Claim>(), 
					expires: DateTime.Now.AddMinutes(10), 
					signingCredentials: signinCredentials);
				
				var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
				return Ok(new JWTTokenResponse
				{
					Token = tokenString
				});
			}
			return Unauthorized();
		}
	}
}
