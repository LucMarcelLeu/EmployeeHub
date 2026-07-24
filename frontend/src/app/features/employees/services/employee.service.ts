import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../../../shared/models/employee.model';
import { Skill } from '../../../shared/models/skill.model';

@Injectable({ providedIn: 'root' })
export class EmployeeService {

    private baseUrl = 'api/employees';
    private skillsUrl = 'api/skills';

    constructor(private http: HttpClient) { }

    getAll(): Observable<Employee[]> {
        return this.http.get<Employee[]>(this.baseUrl);
    }

    getById(id: string): Observable<Employee> {
        return this.http.get<Employee>(`${this.baseUrl}/${id}`);
    }

    create(employee: Partial<Employee>) {
        return this.http.post<Employee>(this.baseUrl, employee);
    }

    update(id: string, employee: Partial<Employee>) {
        return this.http.put<Employee>(`${this.baseUrl}/${id}`, employee);
    }

    delete(id: string) {
        return this.http.delete(`${this.baseUrl}/${id}`);
    }

    search(term: string) {
        return this.http.get<Employee[]>(
            `${this.baseUrl}?search=${term}`
        );
    }

    getSkills(): Observable<Skill[]> {
        return this.http.get<Skill[]>(this.skillsUrl);
    }

    askAiSummary(id: string, prompt: string): Observable<{
        answer: string;
        summary: string;
        name: string;
        department: string;
        skills: string;
        email: string;
    }> {
        return this.http.post<{
            answer: string;
            summary: string;
            name: string;
            department: string;
            skills: string;
            email: string;
        }>(`${this.baseUrl}/${id}/ai-summary`, { prompt });
    }
}