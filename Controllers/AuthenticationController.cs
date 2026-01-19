using Library.Logic.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryApi.Controllers
{
	public class AuthenticationController : ControllerBase
	{

		private readonly IConfiguration _configuration;

		public AuthenticationController(IConfiguration configuration)
		{
		 _configuration = configuration;
		}
		[HttpPost("login")]
		[AllowAnonymous]
		public IActionResult Login([FromBody] LoginRequestDto request)
		{


			var userName = _configuration["JwtSetting:clientID"];
			var password = _configuration["JwtSetting:clientKey"];


			if (request.Username != userName || request.Password != password)
			{
				return Unauthorized("Invalid credentials");
			}

			var token = GenerateJwtToken();
			return Ok(new { Token = token });
		}


		[HttpPost("Register")]
		[AllowAnonymous]
		public IActionResult Register([FromBody] RegistrationDTO request)
		{

			if (ModelState.IsValid) 
			{ 
			
			}

			var token = GenerateJwtToken();
			return Ok(new { Token = token });
		}
		private string GenerateJwtToken()
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var secret = _configuration["JwtSetting:SecretKey"];
			var expirationtime = _configuration["JwtSetting:ExpirationMinutes"];
			var getClientId = _configuration.GetSection("clientID");

			var claims = new[]
			{
                 new Claim(JwtRegisteredClaimNames.Sub, jwtSettings.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
			  issuer: _configuration["JwtSetting:Issuer"],     // jwtSettings["Issuer"],
				   audience: _configuration["JwtSetting:Audience"],   // jwtSettings["Audience"],
				   claims: claims,
			  expires: DateTime.Now.AddMinutes(Convert.ToDouble(expirationtime)),
			  signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
