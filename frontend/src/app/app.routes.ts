import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { ShellComponent } from './layout/shell/shell.component';

export const routes: Routes = [
    {
        path: '',
        component: ShellComponent,
        canActivate: [authGuard],
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            },
            {
                path: 'dashboard',
                loadComponent: () =>
                    import('./features/dashboard/dashboard')
                        .then(m => m.DashboardComponent)
            },
            {
                path: 'employees',
                loadComponent: () =>
                    import('./features/employees/employees.component')
                        .then(m => m.EmployeesComponent)
            },
            {
                path: 'employees/:id',
                loadComponent: () =>
                    import('./features/employees/employee-detail.component')
                        .then(m => m.EmployeeDetailComponent)
            }
        ]
    }
];