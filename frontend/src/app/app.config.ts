import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/auth/auth.interceptor';

import { routes } from './app.routes';

import {
  provideKeycloak,
  withAutoRefreshToken,
  AutoRefreshTokenService,
  UserActivityService
} from 'keycloak-angular';

export const appConfig: ApplicationConfig = {
  providers: [

    provideRouter(routes),
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),

    provideKeycloak({
      config: {
        url: 'http://localhost:8080',
        realm: 'employeehub',
        clientId: 'employeehub-api'
      },

      initOptions: {
        onLoad: 'login-required',
        checkLoginIframe: false,
        pkceMethod: 'S256'
      },

      features: [
        withAutoRefreshToken({
          onInactivityTimeout: 'logout',
          sessionTimeout: 30 * 60 * 1000
        })
      ]
    }),

    AutoRefreshTokenService,
    UserActivityService
  ]
};