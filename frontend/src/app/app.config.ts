import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/auth/auth.interceptor';

import { routes } from './app.routes';

import { environment } from '../environments/environment';

import {
  provideKeycloak,
  withAutoRefreshToken,
  AutoRefreshTokenService,
  UserActivityService
} from 'keycloak-angular';
import { AppConstants } from './core/config/app.constants';

export const appConfig: ApplicationConfig = {
  providers: [

    provideRouter(routes),
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),

    provideKeycloak({
      config: {
        url: environment.keycloak.url,
        realm: environment.keycloak.realm,
        clientId: environment.keycloak.clientId
      },

      initOptions: {
        onLoad: 'login-required',
        checkLoginIframe: false,
        pkceMethod: 'S256'
      },

      features: [
        withAutoRefreshToken({
          onInactivityTimeout: 'logout',
          sessionTimeout: AppConstants.sessionTimeout
        })
      ]
    }),

    AutoRefreshTokenService,
    UserActivityService
  ]
};