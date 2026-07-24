import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EmployeeService } from './services/employee.service';
import { Employee } from '../../shared/models/employee.model';

@Component({
    selector: 'app-employee-detail',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './employee-detail.component.html',
    styleUrl: './employee-detail.component.css'
})
export class EmployeeDetailComponent implements OnInit {

    private route = inject(ActivatedRoute);
    private service = inject(EmployeeService);

    employee?: Employee;
    loading = false;
    aiPrompt = 'Erstelle eine kompakte HR-Zusammenfassung in 3 bis 5 Sätzen mit Fokus auf Department, Skills und beruflichen Kontext.';
    aiAnswer = '';
    aiLoading = false;
    aiSummary = {
        name: '',
        department: '',
        skills: '',
        email: '',
        summary: ''
    };

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

    askAiSummary() {
        if (!this.employee) {
            return;
        }

        this.aiLoading = true;
        this.aiAnswer = '';
        this.aiSummary = {
            name: '',
            department: '',
            skills: '',
            email: '',
            summary: ''
        };

        this.service.askAiSummary(this.employee.id, this.aiPrompt).subscribe({
            next: data => {
                this.aiAnswer = data.answer;
                this.aiSummary = {
                    name: data.name,
                    department: data.department,
                    skills: data.skills,
                    email: data.email,
                    summary: data.summary
                };
                this.aiLoading = false;
            },
            error: err => {
                console.error(err);
                this.aiAnswer = 'Die KI-Anfrage konnte nicht verarbeitet werden.';
                this.aiLoading = false;
            }
        });
    }

    getSkillSummary(): string {
        if (!this.employee?.skills?.length) {
            return 'Keine Skills hinterlegt';
        }

        return this.employee.skills
            .map(skill => skill.skillName ?? 'Unbekannt')
            .join(', ');
    }
}