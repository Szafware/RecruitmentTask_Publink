
export interface AuditLog {
  id: number;
  correlationId: string;
  userEmail: string;
  actionType: number;
  entityType: number;
  contractNumber: string | null;
  actionStartDate: string;
  organizationId: string;
}

export interface AuditLogResponse {
  totalCount: number;
  auditLogs: AuditLog[];
}