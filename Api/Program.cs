using Application;
using Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Application.Behaviors;
using Microsoft.OpenApi.Models;
using Application.Providers.JwtTokenProvider.Settings;
using Application.Providers.JwtTokenProvider.Interfaces;
using Application.Providers.JwtTokenProvider;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api.Middlewares;
using Application.Providers.HttpContextProvider;
using Application.Providers.HttpContextProvider.Interfaces;

try
{
	var builder = WebApplication.CreateBuilder(args);

	//Add DbContext:
	//Install-Package Microsoft.EntityFrameworkCore
	//Install-Package Microsoft.EntityFrameworkCore.SqlServer
	//using Microsoft.EntityFrameworkCore;
	builder.Services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

	//Add services to the container
	builder.Services.AddControllers();

	//Add Request with Handlers support
	//Add MediatR:
	//Install-Package MediatR
	//Install-Package MediatR.ServiceFactory
	//Install-Package MediatR.Extensions.Microsoft.DependencyInjection
	//using MediatR;
	builder.Services.AddMediatR(typeof(App));

	//Add entities to dto cast support
	//Add AutoMapper:
	//Install-Package AutoMapper
	//Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
	builder.Services.AddAutoMapper(typeof(App));

	//Add FluentValidation:
	//Install-Package FluentValidation
	//Install-Package FluentValidation.DependencyInjectionExtensions
	//using FluentValidation;
	//Add class with general validation logic (ValidationBehavior)
	builder.Services.AddValidatorsFromAssemblyContaining<App>();
	builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

	//Add swagger
	builder.Services.AddEndpointsApiExplorer();

	//Add token support for swagger
	builder.Services.AddSwaggerGen(options =>
	{
		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			In = ParameterLocation.Header,
			Description = "Please insert JWT with Bearer into field",
			Name = "Authorization",
			Type = SecuritySchemeType.ApiKey
		});

		options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
				new string[] { }
			}
		});
	});

	//Add http context support in request handler
	builder.Services.AddHttpContextAccessor();

	//Settings from appsettings.json
	builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtTokenSettings"));

	//Dependency injection
	builder.Services.AddScoped<IAppDbContext, AppDbContext>();
	builder.Services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
	builder.Services.AddScoped<IHttpContextProvider, HttpContextProvider>();

	//Add domain IIS auth (to get jwt) + jwt token auth
	//Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
	JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
	JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

	//Set "iisSettings": {"windowsAuthentication": true in launchSettings.json
	builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);

	var jwtTokenSettings = builder.Configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();

	builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
	{
		options.SaveToken = true;
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtTokenSettings.Issuer,
			ValidAudience = jwtTokenSettings.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.SecretKey))
		};
	});

	builder.Services.AddAuthorization(options =>
	{
		options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
		{
			policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
			policy.RequireAuthenticatedUser();
		});

		options.AddPolicy(IISDefaults.AuthenticationScheme, policy =>
		{
			policy.AuthenticationSchemes.Add(IISDefaults.AuthenticationScheme);
			policy.RequireAuthenticatedUser();
		});
	});

	//Configure Cors settings
	builder.Services.AddCors(options =>
	{
		options.AddPolicy("CorsPolicy", policy => policy
			.AllowAnyHeader()
			.WithExposedHeaders("Content-Disposition")
			.AllowAnyMethod()
			.WithOrigins(builder.Configuration.GetSection("CorsOrigins").Get<string[]>())
			.AllowCredentials());
	});

	var app = builder.Build();

	//Add middlewares
	app.UseMiddleware<ExceptionHandlingMiddleware>();

	//Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseCors("CorsPolicy");

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();
	app.Run();
}
catch (Exception ex)
{
	throw;
}