import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  // 1. Wichtig: MatCardModule und RouterLink für die Navigation laden
  imports: [MatCardModule, MatButtonModule, MatIconModule, RouterLink],
  template: `
    <div class="dashboard-container">
      
      <!-- WILLKOMMENS-HEADER -->
      <header class="welcome-header">
        <h1>Welcome back, {{ username }}! 👋</h1>
        <p class="subtitle">Here is what's happening at EmployeeHub today.</p>
      </header>

      <!-- DASHBOARD CARDS GRID -->
      <div class="card-grid">
        
        <!-- CARD 1: EMPLOYEES SCHNELLZUGRIFF -->
        <mat-card class="dashboard-card" appearance="outlined">
          <mat-card-header>
            <div mat-card-avatar class="card-icon-avatar primary-avatar">
              <mat-icon>people</mat-icon>
            </div>
            <mat-card-title>Employees</mat-card-title>
            <mat-card-subtitle>Manage staff members</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <p>View corporate directory, add new team members, or update profiles and roles.</p>
          </mat-card-content>
          <mat-card-actions align="end">
            <button mat-button routerLink="/employees">
              Open Directory <mat-icon>arrow_forward</mat-icon>
            </button>
          </mat-card-actions>
        </mat-card>

        <!-- CARD 2: DEPARTMENTS SCHNELLZUGRIFF -->
        <mat-card class="dashboard-card" appearance="outlined">
          <mat-card-header>
            <div mat-card-avatar class="card-icon-avatar secondary-avatar">
              <mat-icon>business</mat-icon>
            </div>
            <mat-card-title>Departments</mat-card-title>
            <mat-card-subtitle>Organizational units</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <p>Structure your business, manage company departments, and oversee assignment counts.</p>
          </mat-card-content>
          <mat-card-actions align="end">
            <button mat-button routerLink="/departments">
              View Units <mat-icon>arrow_forward</mat-icon>
            </button>
          </mat-card-actions>
        </mat-card>

      </div>
    </div>
  `,
  // 2. M3 Inline-Styles für das moderne Dashboard-Layout
  styles: `
    .dashboard-container {
      display: flex;
      flex-direction: column;
      gap: 32px;
    }

    .welcome-header h1 {
      margin: 0 0 8px 0;
      font: var(--mat-sys-headline-large);
      font-weight: 700;
      color: var(--mat-sys-on-surface);
    }

    .welcome-header .subtitle {
      margin: 0;
      font: var(--mat-sys-body-large);
      color: var(--mat-sys-on-surface-variant);
    }

    /* Responsives Grid: Zeigt Karten nebeneinander an, falls Platz da ist */
    .card-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
      gap: 24px;
    }

    /* Material 3 Card Customization */
    .dashboard-card {
      background-color: var(--mat-sys-surface-container-low) !important;
      border: 1px solid var(--mat-sys-outline-variant) !important;
      border-radius: 16px !important; /* M3 typische grosse Ecken */
      padding: 16px 8px 8px 8px;
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    /* Leichter Hebe-Effekt beim Drüberfahren */
    .dashboard-card:hover {
      transform: translateY(-4px);
      box-shadow: var(--mat-sys-level1);
      background-color: var(--mat-sys-surface-container-high) !important;
    }

    /* Schicke, runde Icon-Hintergründe für die Avatare */
    .card-icon-avatar {
      display: flex;
      align-items: center;
      justify-content: center;
      border-radius: 12px;
      width: 40px;
      height: 40px;
    }

    .primary-avatar {
      background-color: var(--mat-sys-primary-container);
      color: var(--mat-sys-on-primary-container);
    }

    .secondary-avatar {
      background-color: var(--mat-sys-secondary-container);
      color: var(--mat-sys-on-secondary-container);
    }

    mat-card-title {
      font: var(--mat-sys-title-large) !important;
      font-weight: 600 !important;
    }

    mat-card-content {
      margin-top: 16px;
      color: var(--mat-sys-on-surface-variant);
      font: var(--mat-sys-body-medium);
      line-height: 1.6;
    }

    mat-card-actions button {
      font-weight: 600;
    }

    mat-card-actions mat-icon {
      margin-left: 4px;
      --mdc-icon-size: 18px;
    }
  `
})
export class DashboardComponent {
  private auth = inject(AuthService);
  username = this.auth.getUsername();
}
