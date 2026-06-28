import { Routes } from '@angular/router';

export default [
    { path: 'error', loadComponent: () => import('./error/error').then((c) => c.Error) },
    { path: 'error2', loadComponent: () => import('./error2/error2').then((c) => c.Error2) },
    { path: 'access', loadComponent: () => import('./accessdenied/accessdenied').then((c) => c.AccessDenied) },
    { path: 'access2', loadComponent: () => import('./accessdenied2/accessdenied2').then((c) => c.AccessDenied2) },
    { path: 'login', loadComponent: () => import('./login/login').then((c) => c.Login) },
    { path: 'login2', loadComponent: () => import('./login2/login2').then((c) => c.Login2) },
    { path: 'sign-out', loadComponent: () => import('./signout/signout').then((c) => c.SignOut) },
    { path: 'forgotpassword', loadComponent: () => import('./forgotpassword/forgotpassword').then((c) => c.ForgotPassword) },
    { path: 'register', loadComponent: () => import('./register/register').then((c) => c.Register) },
    { path: 'newpassword', loadComponent: () => import('./newpassword/newpassword').then((c) => c.NewPassword) },
    { path: 'verification', loadComponent: () => import('./verification/verification').then((c) => c.Verification) },
    { path: 'lockscreen', loadComponent: () => import('./lockscreen/lockscreen').then((c) => c.LockScreen) },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
