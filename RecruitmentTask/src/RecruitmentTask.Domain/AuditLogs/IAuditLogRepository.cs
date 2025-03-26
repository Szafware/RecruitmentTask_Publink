using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentTask.Domain.AuditLogs;

public interface IAuditLogRepository
{
	Task<IEnumerable<AuditLog>> GetAuditLogsAsync(Guid organizationId, int page, int? pageSize = null);
}
