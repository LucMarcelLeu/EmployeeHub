import { inject, Injectable } from '@angular/core';
import Keycloak from 'keycloak-js';

@Injectable({ providedIn: 'root' })
export class AuthService {

    private keycloak = inject(Keycloak);

    isLoggedIn(): boolean {
        return this.keycloak.authenticated ?? false;
    }

    getUsername(): string {
        return this.keycloak.tokenParsed?.['preferred_username'] ?? '';
    }

    getToken(): string {
        return this.keycloak.token ?? '';
    }

    logout(): void {
        this.keycloak.logout({
            redirectUri: window.location.origin
        });
    }

    hasRole(role: string): boolean {
        return this.keycloak.hasRealmRole(role);
    }

    get isAdmin(): boolean {
        return this.hasRole('admin');
    }
}