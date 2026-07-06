import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../../shared/models/employee.model';

@Injectable({ providedIn: 'root' })
export class EmployeeService {

    private baseUrl = 'http://localhost:5132/api/employees';

    constructor(private http: HttpClient) { }

    getAll(): Observable<Employee[]> {
        return this.http.get<Employee[]>(this.baseUrl);
    }

    getById(id: string): Observable<Employee> {
        return this.http.get<Employee>(`${this.baseUrl}/${id}`);
    }

    create(emp: Employee): Observable<Employee> {
        return this.http.post<Employee>(this.baseUrl, emp);
    }

    update(id: string, emp: Employee): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, emp);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}