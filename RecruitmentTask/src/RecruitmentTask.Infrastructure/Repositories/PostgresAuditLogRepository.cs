using Dapper;
using Npgsql;
using RecruitmentTask.Domain.AuditLogs;
using RecruitmentTask.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentTask.Infrastructure.Repositories;

internal sealed class PostgresAuditLogRepository : IAuditLogRepository
{
	private readonly IDbConnection _npgsqlConnection;
	private readonly AppSettings _appSettings;

	public PostgresAuditLogRepository(
		IDbConnection npgsqlConnection,
		AppSettings appSettings)
	{
		_npgsqlConnection = npgsqlConnection;
		_appSettings = appSettings;
	}

	public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(Guid organizationId, int page, int? pageSize = null)
	{
		var npgsqlConnection = _npgsqlConnection as NpgsqlConnection 
			?? throw new InvalidOperationException($"{nameof(PostgresAuditLogRepository)} class only supports {nameof(NpgsqlConnection)} connection type.");

		await npgsqlConnection.OpenAsync();

		int entityTypeDocumentNumberJoining = _appSettings.EntityTypeDocumentNumberJoining;

		string query = $@"
			SELECT 
			    al.id AS {nameof(AuditLog.Id)}, 
			    al.user_email AS {nameof(AuditLog.UserEmail)}, 
			    al.type AS {nameof(AuditLog.ActionType)}, 
			    al.created_date AS {nameof(AuditLog.ActionStartDate)}, 
			    al.organization_id AS {nameof(AuditLog.OrganizationId)}, 
			    al.correlation_id AS {nameof(AuditLog.CorrelationId)}, 
			    al.entity_type AS {nameof(AuditLog.EntityType)},
				al.old_values AS {nameof(AuditLog.OldValues)},
				al.new_values AS {nameof(AuditLog.NewValues)},
				CASE 
				    WHEN al.entity_type = {entityTypeDocumentNumberJoining} THEN dh.number 
				    ELSE NULL 
				END AS {nameof(AuditLog.ContractNumber)}
			FROM audit_log al
			LEFT JOIN document_header dh 
			    ON al.entity_type = {entityTypeDocumentNumberJoining}
			    AND al.new_values::jsonb->>'ContractId' = dh.id::text
			WHERE al.organization_id = @OrganizationId
			ORDER BY al.created_date DESC;
			LIMIT @PageSize OFFSET @Offset";

		int actualPageSize = pageSize ?? _appSettings.DefaultPageSize;

		var auditLogs = await npgsqlConnection.QueryAsync<AuditLog>(query, new
		{
			OrganizationId = organizationId,
			PageSize = actualPageSize,
			Offset = (page - 1) * actualPageSize
		});

		return auditLogs.ToList();
	}
}
