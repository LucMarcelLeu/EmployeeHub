import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Employee } from '../models/employee';
import { environment } from '../../../environments/environment';
import { ApiEndpoints } from '../../core/config/api-endpoints';

@Injectable({
    providedIn: 'root'
})
export class EmployeeService {

    private readonly http = inject(HttpClient);

    private readonly baseUrl =
        `${environment.apiUrl}${ApiEndpoints.employees}`;

    getEmployees(): Observable<Employee[]> {
        return this.http.get<Employee[]>(this.baseUrl);
    }
}