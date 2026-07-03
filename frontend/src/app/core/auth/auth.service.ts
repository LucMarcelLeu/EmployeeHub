import { Injectable } from '@angular/core';
import Keycloak from 'keycloak-js';

@Injectable({ providedIn: 'root' })
export class AuthService {

    constructor(private keycloak: Keycloak) { }

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
}