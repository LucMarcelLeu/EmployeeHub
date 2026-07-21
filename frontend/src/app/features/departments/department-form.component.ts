import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { DepartmentService } from './services/department.service';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NotificationService } from '../../core/services/notification.service';
import { Department } from './models/department';

@Component({
    selector: 'app-departments-form',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule],
    templateUrl: './department-form.component.html',
    styleUrl: './department-form.component.css'
})
export class DepartmentsFormComponent implements OnInit {

    private fb = inject(FormBuilder);
    private service = inject(DepartmentService);
    private departmentService = inject(DepartmentService);
    private notification = inject(NotificationService);
    private dialogRef = inject(MatDialogRef<DepartmentsFormComponent>);

    readonly department = inject(MAT_DIALOG_DATA) as Department | null;

    isEditMode = false;
    departments: Department[] = [];

    readonly form = this.fb.nonNullable.group({
        name: ['', Validators.required],
    });

    ngOnInit(): void {

        this.departmentService.getAll().subscribe({
            next: departments => this.departments = departments
        });

        if (!this.department) {
            return;
        }

        this.isEditMode = true;
        this.form.patchValue({
            name: this.department.name,
        });
    }

    save() {
        if (this.form.invalid) {
            return;
        }

        const dto = this.form.getRawValue();

        if (this.isEditMode) {
            this.service.update(this.department!.id, dto)
                .subscribe(() => this.dialogRef.close(true));

            return;
        }

        this.service.create(dto).subscribe({
            next: () => {
                this.notification.success('Department created');
                this.dialogRef.close(true);
            },
            error: () => {
                this.notification.error('Create failed');
            }
        });
    }

    cancel(): void {
        this.dialogRef.close(false);
    }

    close() {
        this.dialogRef.close(false);
    }
}