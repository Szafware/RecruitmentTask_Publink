using Microsoft.Extensions.DependencyInjection;
using RecruitmentTask.Application.Services.AuditLogs;

namespace RecruitmentTask.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IAuditLogService, AuditLogService>();

		return services;
	}
}
