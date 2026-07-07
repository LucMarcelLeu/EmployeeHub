import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input';
import { EmployeeService } from './employee.service';
import { Employee } from '../../shared/models/employee.model';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { NotificationService } from '../../core/services/notification.service';

@Component({
    selector: 'app-employee-form',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule],
    templateUrl: './employee-form.component.html',
    styleUrl: './employee-form.component.css'
})
export class EmployeeFormComponent implements OnInit {

    private fb = inject(FormBuilder);
    private service = inject(EmployeeService);
    private notification = inject(NotificationService);
    private dialogRef = inject(MatDialogRef<EmployeeFormComponent>);

    readonly employee = inject(MAT_DIALOG_DATA) as Employee | null;

    isEditMode = false;

    readonly form = this.fb.nonNullable.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]]
    });

    ngOnInit(): void {
        if (!this.employee) {
            return;
        }

        this.isEditMode = true;
        this.form.patchValue({
            firstName: this.employee.firstName,
            lastName: this.employee.lastName,
            email: this.employee.email
        });
    }

    save() {
        if (this.form.invalid) {
            return;
        }

        const dto = this.form.getRawValue();

        if (this.isEditMode) {
            this.service.update(this.employee!.id, dto)
                .subscribe(() => this.dialogRef.close(true));

            return;
        }

        this.service.create(dto).subscribe({
            next: () => {
                this.notification.success('Employee created');
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