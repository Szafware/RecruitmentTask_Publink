using RecruitmentTask.Domain.AuditLogs;
using System.Collections.Generic;

namespace RecruitmentTask.Api.Controllers;

public record AuditLogsResponse(int TotalCount, IEnumerable<AuditLog> AuditLogs);
