import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuditLogResponse } from '../models/audit-log.model';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  private apiUrl = 'https://localhost:5051/api/auditlogs';

  constructor(private http: HttpClient) { }

  getAuditLogs(organizationId: string, page: number = 1, pageSize: number = 10): Observable<AuditLogResponse> {
    const params = new HttpParams()
      .set('organizationId', organizationId)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<AuditLogResponse>(this.apiUrl, { params }).pipe(
      map((response) => ({
        totalCount: response.totalCount,
        auditLogs: response.auditLogs
      }))
    );
  }
}