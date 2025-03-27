using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecruitmentTask.Api.Middleware;
using RecruitmentTask.Application;
using RecruitmentTask.Infrastructure;
using RecruitmentTask.Infrastructure.Configuration;
using System;

namespace RecruitmentTask.Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddCors(options =>
		{
			options.AddPolicy("AllowAllOrigins", policy =>
			{
				policy.AllowAnyOrigin()
					  .AllowAnyHeader()
					  .AllowAnyMethod();
			});
		});


		builder.Services.AddControllers();

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services.AddApplication();
		builder.Services.AddInfrastructure(builder.Configuration);

		var appSettings = builder.Configuration.GetSection("AppSettings") 
			?? throw new ArgumentException("AppSettings section not found");

		builder.Services.Configure<AppSettings>(appSettings);

		var app = builder.Build();

		app.UseMiddleware<ExceptionHandlingMiddleware>();

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseCors("AllowAllOrigins");

		app.UseHttpsRedirection();

		app.MapControllers();

		app.Run();
	}
}
