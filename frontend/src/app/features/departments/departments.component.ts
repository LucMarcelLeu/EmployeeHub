import { Component, inject, OnInit } from '@angular/core';
import { AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { DepartmentService } from './services/department.service';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DepartmentsFormComponent } from './department-form.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfirmDeleteDialog } from '../../shared/ConfirmDeleteDialog';
import { NotificationService } from '../../core/services/notification.service';
import { AuthService } from '../../core/auth/auth.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { ViewChild } from '@angular/core';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { Department } from './models/department';

@Component({
    selector: 'app-departments',
    standalone: true,
    imports: [
        CommonModule,
        MatTableModule,
        MatSortModule,
        MatPaginatorModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatSnackBarModule,
        MatProgressSpinnerModule,
        MatIconModule
    ],
    templateUrl: './departments.component.html',
    styleUrl: './departments.component.css'
})

export class DepartmentsComponent implements OnInit, AfterViewInit {

    private router = inject(Router);
    private dialog = inject(MatDialog);

    private service = inject(DepartmentService);
    private notification = inject(NotificationService);

    private auth = inject(AuthService);
    isAdmin = this.auth.isAdmin;

    dataSource = new MatTableDataSource<Department>();
    loading = false;
    error = '';

    displayedColumns: string[] = [
        'id',
        'name',
        'actions'
    ];

    @ViewChild(MatSort)
    sort!: MatSort;

    @ViewChild(MatPaginator)
    paginator!: MatPaginator;

    ngOnInit(): void {
        this.load();
    }

    load() {
        this.loading = true;
        this.error = '';

        this.service.getAll().subscribe({
            next: (data) => {
                this.dataSource.data = data;
                this.loading = false;
            },
            error: (err) => {
                this.error = 'Fehler beim Laden der Daten';
                this.loading = false;
                this.notification.error(this.error);
            }
        });
    }

    ngAfterViewInit(): void {

        this.dataSource.sort = this.sort;

        this.dataSource.paginator = this.paginator;

    }

    openDetail(emp: Department) {
        this.router.navigate(['/departments', emp.id]);
    }

    openCreate() {
        const ref = this.dialog.open(DepartmentsFormComponent, {
            width: '500px',
            maxWidth: '90vw',
            autoFocus: false
        });

        ref.afterClosed().subscribe(result => {
            if (result) {
                this.load(); // refresh list
            }
        });
    }

    openEdit(emp: Department) {
        const ref = this.dialog.open(DepartmentsFormComponent, {
            width: '500px',
            data: emp   // 👈 DAS ist der entscheidende Teil
        });

        ref.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });
    }

    // delete(emp: Employee) {
    //     const ref = this.dialog.open(ConfirmDeleteDialog);
    //     ref.afterClosed().subscribe(result => {
    //         if (!result) return;

    //         this.service.delete(emp.id).subscribe(() => {
    //             this.notification.success('Employee deleted');
    //             this.load();
    //         });

    //     });
    // }

    onSearch(event: Event) {
        const value =
            (event.target as HTMLInputElement).value;

        this.dataSource.filter =
            value.trim().toLowerCase();
    }
}