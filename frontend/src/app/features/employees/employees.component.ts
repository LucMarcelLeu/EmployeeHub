import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { EmployeeService } from './employee.service';
import { Employee } from '../../shared/models/employee.model';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeFormComponent } from './employee-form.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfirmDeleteDialog } from '../../shared/ConfirmDeleteDialog';
import { NotificationService } from '../../core/services/notification.service';

@Component({
    selector: 'app-employees',
    standalone: true,
    imports: [
        CommonModule,
        MatTableModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule, 
        MatSnackBarModule,
        MatProgressSpinnerModule
    ],
    templateUrl: './employees.component.html',
    styleUrl: './employees.component.css'
})

export class EmployeesComponent implements OnInit {

    private router = inject(Router);
    private dialog = inject(MatDialog);

    private service = inject(EmployeeService);
    private notification = inject(NotificationService);

    employees: Employee[] = [];
    loading = false;
    error = '';

    displayedColumns: string[] = [
        'id',
        'firstName',
        'lastName',
        'email',
        'actions'
    ];

    ngOnInit(): void {
        this.load();
    }

    load() {
        this.loading = true;
        this.error = '';

        this.service.getAll().subscribe({
            next: data => {
                this.employees = data;
                this.loading = false;
            },
            error: err => {
                this.error = 'Fehler beim Laden der Daten';
                this.loading = false;
            }
        });
    }

    openDetail(emp: Employee) {
        this.router.navigate(['/employees', emp.id]);
    }

    openCreate() {
        const ref = this.dialog.open(EmployeeFormComponent, {
            width: '500px'
        });

        ref.afterClosed().subscribe(result => {
            if (result) {
                this.load(); // refresh list
            }
        });
    }

    openEdit(emp: Employee) {
        const ref = this.dialog.open(EmployeeFormComponent, {
            width: '500px',
            data: emp   // 👈 DAS ist der entscheidende Teil
        });

        ref.afterClosed().subscribe(result => {
            if (result) {
                this.load();
            }
        });
    }

    delete(emp: Employee) {
        const ref = this.dialog.open(ConfirmDeleteDialog);
        ref.afterClosed().subscribe(result => {
            if (!result) return;

            this.service.delete(emp.id).subscribe(() => {
                this.notification.success('Employee deleted');
                this.load();
            });

        });

    }
    // delete(emp: Employee) {

    //     const confirmed = confirm(`Delete ${emp.firstName}?`);

    //     if (!confirmed) return;

    //     this.service.delete(emp.id).subscribe(() => {
    //         this.load();
    //     });

    // }

    onSearch(event: Event) {

        const value = (event.target as HTMLInputElement).value;

        this.service.search(value).subscribe(data => {
            this.employees = data;
        });

    }
}