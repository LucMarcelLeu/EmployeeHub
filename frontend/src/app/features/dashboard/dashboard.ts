import { Component, inject } from '@angular/core';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <h1>EmployeeHub Dashboard</h1>

    <p>Login erfolgreich 🎉</p>

    <p>User: {{ username }}</p>

    <button (click)="logout()">Logout</button>
  `
})
export class DashboardComponent {

  private auth = inject(AuthService);

  username = this.auth.getUsername();

  logout() {
    this.auth.logout();
  }
}