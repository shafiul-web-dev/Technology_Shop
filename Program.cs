using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Technology_Shop.Configurations;
using Technology_Shop.Data;
using Technology_Shop.Interfaces.EmailInterface;
using Technology_Shop.Models;
using Technology_Shop.Repositories;
using Technology_Shop.Services;

namespace Technology_Shop
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// DbContext
			builder.Services.AddDbContext<ApplicationDbContext>(opts =>
				opts.UseSqlServer(builder.Configuration
					.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IUserRepository , UserRepository>();
			builder.Services.AddScoped<IUserService, UserService>();

			// Settings
			builder.Services.Configure<JwtSettings>(
				builder.Configuration.GetSection("JwtSettings"));

			// Services
			builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
			builder.Services.AddScoped<JwtService>();
			builder.Services.AddSingleton<IAuthorizationHandler, SameUserHandler>();
			builder.Services.AddSingleton<IEmailService, ConsoleEmailService>();

			// Authentication & Authorization
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme =
					JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme =
					JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(opts =>
			{
				var jwt = builder.Configuration
					.GetSection("JwtSettings")
					.Get<JwtSettings>();

				opts.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwt.Issuer,
					ValidAudience = jwt.Audience,
					IssuerSigningKey =
						new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(jwt.Key))
				};
			});

			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("CanUpdateOwnProfile", policy =>
					 policy.Requirements.Add(new SameUserRequirement()));
				options.AddPolicy("CanChangeOwnPassword", policy =>
	                 policy.Requirements.Add(new SameUserRequirement()));
				options.AddPolicy("AdminOnly",
					policy => policy.RequireRole("Admin"));
			});

			// Swagger with JWT support
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter: Bearer {your token}"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
			});

			builder.Services.AddControllers();

			var app = builder.Build();

			// Seed predefined Admin
			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider
					.GetRequiredService<ApplicationDbContext>();
				if (!db.Admins.Any())
				{
					var hasher =
						scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
					var pwdHash = hasher.Hash("Admin@123");
					db.Admins.Add(new Admin
					{
						Email = "admin@example.com",
						PasswordHash = pwdHash
					});
					db.SaveChanges();
				}
			}

			// Middleware
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();
			app.Run();
		}
	}
}