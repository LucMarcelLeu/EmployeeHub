import { keycloak } from './keycloak.config';

export class TokenService {

    getToken(): string {
        return keycloak.token || '';
    }

    logout(): void {
        keycloak.logout();
    }
}