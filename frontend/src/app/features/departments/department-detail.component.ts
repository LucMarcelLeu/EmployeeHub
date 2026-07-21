import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { DepartmentService } from './services/department.service';
import { Department } from './models/department';

@Component({
    selector: 'app-department-detail',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './department-detail.component.html',
    styleUrl: './department-detail.component.css'
})
export class DepartmentDetailComponent implements OnInit {

    private route = inject(ActivatedRoute);
    private service = inject(DepartmentService);

    department?: Department;
    loading = false;

    ngOnInit(): void {
        const id = this.route.snapshot.paramMap.get('id');

        if (!id) {
            console.error('No Department id in route');
            return;
        }

        this.load(id);
    }

    load(id: string) {
        this.loading = true;

        this.service.getById(id).subscribe({
            next: data => {
                this.department = data;
                this.loading = false;
            },
            error: err => {
                console.error(err);
                this.loading = false;
            }
        });
    }
}