import { Component, inject } from '@angular/core';
import { EmployeeService } from './core/api/employee.service';

@Component({
    selector: 'app-root',
    template: `
    <h1>EmployeeHub</h1>

    <button (click)="load()">Load Employees</button>

    <ul>
      @for (employee of employees; track employee.id) {
        {{employee.firstName}} {{employee.lastName}} - {{employee.email}}
      }
    </ul>
  `
})
export class AppComponent {

    private service = inject(EmployeeService);

    employees: any[] = [];

    load() {
        this.service.getEmployees().subscribe(data => {
            this.employees = data;
        });
    }
}