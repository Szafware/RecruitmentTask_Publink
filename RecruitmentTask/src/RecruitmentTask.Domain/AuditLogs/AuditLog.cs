using System;

namespace RecruitmentTask.Domain.AuditLogs;

public class AuditLog
{
	public int Id { get; set; }

	public Guid CorrelationId { get; set; }

	public string UserEmail { get; set; }

	public Type ActionType { get; set; }

	public EntityType EntityType { get; set; }

	public string ContractNumber { get; set; }

	public string OldValues { get; set; }
	public string NewValues { get; set; }

	public DateTime ActionStartDate { get; set; }

	//public int EntityCount { get; set; }

	public Guid OrganizationId { get; set; }
}
