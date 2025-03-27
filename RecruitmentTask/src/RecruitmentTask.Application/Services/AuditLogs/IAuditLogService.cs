using RecruitmentTask.Domain.AuditLogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentTask.Application.Services.AuditLogs;

public interface IAuditLogService
{
	Task<IEnumerable<AuditLog>> GetAuditLogsAsync(Guid organizationId, int page = 1, int? pageSize = null);

	Task<int> GetTotalCountAsync(Guid organizationId);
}
