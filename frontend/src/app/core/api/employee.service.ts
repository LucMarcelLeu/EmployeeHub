import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Employee } from '../models/employee';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class EmployeeService {

    private readonly http = inject(HttpClient);

    getEmployees(): Observable<Employee[]> {
        return this.http.get<Employee[]>(
            `${environment.apiUrl}/employees`
        );
    }
}