import Keycloak from 'keycloak-js';

export const keycloak = new Keycloak({
    url: window.location.origin + "/auth",
    realm: 'employeehub',
    clientId: 'employeehub-api'
});