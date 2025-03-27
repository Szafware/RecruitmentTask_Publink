using RecruitmentTask.Domain.AuditLogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentTask.Application.Services.AuditLogs;

public sealed class AuditLogService : IAuditLogService
{
	private readonly IAuditLogRepository _auditLogRepository;

	public AuditLogService(IAuditLogRepository auditLogRepository)
	{
		_auditLogRepository = auditLogRepository;
	}

	public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(Guid organizationId, int page = 1, int? pageSize = null)
	{
		if (organizationId == Guid.Empty)
		{
			throw new ArgumentException("Organization id must not be empty.");
		}

		if (page <= 0)
		{
			throw new ArgumentException("Page must be greater than zero.");
		}

		if (pageSize is not null && pageSize <= 0)
		{
			throw new ArgumentException("Page size must be greater than zero.");
		}

		var auditLogEntries = await _auditLogRepository.GetAuditLogsAsync(organizationId, page, pageSize);

		return auditLogEntries;
	}

	public async Task<int> GetTotalCountAsync(Guid organizationId)
	{
		if (organizationId == Guid.Empty)
		{
			throw new ArgumentException("Organization id must not be empty.");
		}

		int auditLogTotalCount = await _auditLogRepository.GetAuditLogTotalCountAsync(organizationId);

		return auditLogTotalCount;
	}
}
