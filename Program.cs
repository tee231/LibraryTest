//using Library.Logic.Repository.Abstraction;
//using Library.Logic.Repository.Implementation;
using Library.Domain.DataBase;
using Library.Logic.Repository.Abstraction;
using Library.Logic.Repository.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
/*builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();*/

// Configure JWT Authentication


builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
	  ValidateIssuer = true,
	  ValidateAudience = true,
	  ValidateLifetime = true,
	  ValidateIssuerSigningKey = true,
	  ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
	  ValidAudience = builder.Configuration["JwtSetting:Audience"],
	  IssuerSigningKey = new SymmetricSecurityKey(
		  Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:SecretKey"]))
  };
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder =>
	{
		builder.AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader();
	});
});

builder.Services.AddDbContext<BookDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IBookService, BookService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "Enter 'Bearer' [space] and then your token"
	});

	c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
		{
			{
				new Microsoft.OpenApi.Models.OpenApiSecurityScheme
				{
					Reference = new Microsoft.OpenApi.Models.OpenApiReference
					{
						Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				new string[] {}
			}
		});
});

var app = builder.Build();


// Enable CORS
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

// CORS must come before Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



/*app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware(typeof(CustomResponseHeaderMiddleware));
app.UseAuthorization();
app.MapControllers();
app.Run();*/
