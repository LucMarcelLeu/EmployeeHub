import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { EmployeeService } from './services/employee.service';
import { Employee } from '../../shared/models/employee.model';

@Component({
    selector: 'app-employee-detail',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './employee-detail.component.html',
    styleUrl: './employee-detail.component.css'
})
export class EmployeeDetailComponent implements OnInit {

    private route = inject(ActivatedRoute);
    private service = inject(EmployeeService);

    employee?: Employee;
    loading = false;

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');

        if (!id) {
            console.error('No employee id in route');
            return;
        }

        this.load(id);
    }

    load(id: string) {
        this.loading = true;

        this.service.getById(id).subscribe({
            next: data => {
                this.employee = data;
                this.loading = false;
            },
            error: err => {
                console.error(err);
                this.loading = false;
            }
        });
    }
}