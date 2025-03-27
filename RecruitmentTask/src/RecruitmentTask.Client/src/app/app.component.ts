import { CommonModule } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuditLog, AuditLogResponse } from './models/audit-log.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatIconModule
  ],
  providers: [],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  private apiUrl = 'https://localhost:5051/api/auditlogs';
  organizationIdControl = new FormControl('');
  auditLogsDataSource = new MatTableDataSource<AuditLog>([]);
  displayedColumns = ['id', 'userEmail', 'actionType', 'entityType', 'contractNumber', 'actionStartDate'];
  totalLogs = 0;
  isLoading = false;
  private refresh$ = new BehaviorSubject<void>(undefined);

  page = 1;
  pageSize = 10;

  constructor(private http: HttpClient, private snackBar: MatSnackBar) {
    this.refresh$
      .pipe(
        switchMap(() => {
          const orgId = this.organizationIdControl.value;
          if (!orgId) {
            this.showToast('Please enter an Organization ID!', 'warning-toast');
            return of({ totalCount: 0, auditLogs: [] });
          }

          this.isLoading = true;
          return this.getAuditLogs(orgId, this.page, this.pageSize).pipe(
            catchError((error) => {
              console.error(error);
              this.showToast('Failed to fetch audit logs!', 'error-toast');
              return of({ totalCount: 0, auditLogs: [] });
            })
          );
        })
      )
      .subscribe((response) => {
        this.isLoading = false;
        this.auditLogsDataSource.data = response.auditLogs;
        this.totalLogs = response.totalCount;
      });
  }

  private getAuditLogs(organizationId: string, page: number, pageSize: number): Observable<AuditLogResponse> {
    const params = new HttpParams()
      .set('organizationId', organizationId)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<AuditLogResponse>(this.apiUrl, { params });
  }

  fetchAuditLogs(): void {
    this.refresh$.next();
  }

  onPageChange(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.fetchAuditLogs();
  }

  private showToast(message: string, panelClass: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top',
      panelClass: [panelClass],
    });
  }
}
