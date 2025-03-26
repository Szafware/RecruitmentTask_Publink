using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RecruitmentTask.Domain.AuditLogs;
using RecruitmentTask.Infrastructure.Repositories;
using System;
using System.Data;

namespace RecruitmentTask.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{

		string connectionString = configuration.GetConnectionString("RekrutacjaDb")
			?? throw new ArgumentNullException(nameof(configuration));

		services.AddScoped<IDbConnection>(provider => new NpgsqlConnection(connectionString));

		services.AddScoped<IAuditLogRepository, PostgresAuditLogRepository>();

		return services;
	}
}
