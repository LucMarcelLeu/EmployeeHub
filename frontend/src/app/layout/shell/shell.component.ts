import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
    selector: 'app-shell',
    standalone: true,
    imports: [RouterModule],
    templateUrl: './shell.component.html',
    styleUrl: './shell.component.css'
})
export class ShellComponent {

    private auth = inject(AuthService);

    get username() {
        return this.auth.getUsername();
    }

    logout() {
        this.auth.logout();
    }
}