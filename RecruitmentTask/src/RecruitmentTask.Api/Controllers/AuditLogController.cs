using Microsoft.AspNetCore.Mvc;
using RecruitmentTask.Application.Services.AuditLogs;
using System;
using System.Threading.Tasks;

namespace RecruitmentTask.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuditLogController : ControllerBase
{
	private readonly IAuditLogService _auditLogService;

	public AuditLogController(IAuditLogService auditLogService)
	{
		_auditLogService = auditLogService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAuditLogs(Guid organizationId, int page = 1, int? pageSize = null)
	{
		var auditLogEntries = await _auditLogService.GetAuditLogsAsync(organizationId, page, pageSize);

		return Ok(auditLogEntries);
	}
}