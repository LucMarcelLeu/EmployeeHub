import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeService } from './employee.service';
import { Employee } from './employee.model';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';

@Component({
    selector: 'app-employees',
    standalone: true,
    imports: [CommonModule, MatTableModule, MatProgressSpinnerModule],
    templateUrl: './employees.component.html',
    styleUrl: './employees.component.css'
})
export class EmployeesComponent implements OnInit {

    private router = inject(Router);

    private service = inject(EmployeeService);

    employees: Employee[] = [];
    loading = false;
    error = '';

    displayedColumns: string[] = ['id', 'firstName', 'lastName', 'email'];

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
}