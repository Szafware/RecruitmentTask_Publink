using Microsoft.AspNetCore.Mvc;
using RecruitmentTask.Application.Services.AuditLogs;
using System;
using System.Threading.Tasks;

namespace RecruitmentTask.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuditLogsController : ControllerBase
{
	private readonly IAuditLogService _auditLogService;

	public AuditLogsController(IAuditLogService auditLogService)
	{
		_auditLogService = auditLogService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAuditLogs(Guid organizationId, int page = 1, int? pageSize = null)
	{
		var auditLogs = await _auditLogService.GetAuditLogsAsync(organizationId, page, pageSize);

		int auditLogTotalCount = await _auditLogService.GetTotalCountAsync(organizationId);

		var auditLogsResponse = new AuditLogsResponse(auditLogTotalCount, auditLogs);

		return Ok(auditLogsResponse);
	}
}