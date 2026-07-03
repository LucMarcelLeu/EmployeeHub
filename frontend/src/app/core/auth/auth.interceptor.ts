import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

    console.log('Auth interceptor');

    const auth = inject(AuthService);
    const token = auth.getToken();
    console.log('Token:', token);
    if (!token) {
        return next(req);
    }

    const cloned = req.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`
        }
    });

    return next(cloned);
};