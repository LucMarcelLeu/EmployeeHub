export const environment = {
    production: false,
    apiUrl: 'api',
    keycloak: {
        url: window.location.origin + "/auth",
        realm: 'employeehub',
        clientId: 'employeehub-api'
    }
};