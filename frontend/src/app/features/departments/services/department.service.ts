import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { Department } from '../models/department';

@Injectable({
    providedIn: 'root'
})
export class DepartmentService {

    private readonly http = inject(HttpClient);

    getAll(): Observable<Department[]> {
        return this.http.get<Department[]>(
            `${environment.apiUrl}/departments`);
    }

    getById(id: string): Observable<Department> {
        return this.http.get<Department>(`${environment.apiUrl}/departments/${id}`);
    }
    
    create(department: Partial<Department>) {
        return this.http.post<Department>(`${environment.apiUrl}/departments`, department);
    }

    update(id: string, department: Partial<Department>) {
        return this.http.put<Department>(`${environment.apiUrl}/departments/${id}`, department);
    }


}