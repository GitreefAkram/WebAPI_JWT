
using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		

		public AuthenticationController
		(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,

			IConfiguration config, 
			DialZeroNGContext context
			
			)
		{
			_userManager = userManager;
			_roleManager = roleManager;
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
		public async Task<IActionResult> Login([FromBody] Login model)
		{
			var user = await _userManager.FindByNameAsync(model.UserName);
			if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var userRoles = await _userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				var token = GetToken(authClaims);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo
				});
			}
			return Unauthorized();
		}


		private JwtSecurityToken GetToken(List<Claim> authClaims)
		{



			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));

			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var tokeOptions = new JwtSecurityToken(
				issuer: ConfigurationManager.AppSetting["JWT:Issuer"],
				audience: ConfigurationManager.AppSetting["JWT:Audience"],
				claims: authClaims,
				expires: DateTime.Now.AddMinutes(10),
				signingCredentials: signinCredentials);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);


			//var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			//var token = new JwtSecurityToken
			//	(
			//	issuer: _configuration["JWT:ValidIssuer"],
			//	audience: _configuration["JWT:ValidAudience"],
			//	expires: DateTime.Now.AddHours(3),
			//	claims: authClaims,
			//	signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				
			//	);

			return tokeOptions;
		}




	}
}
