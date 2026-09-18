import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { Login } from './features/auth/pages/login/login';
import { Dashboard } from './features/user/pages/dashboard/dashboard';
import { Profile } from './features/user/pages/profile/profile';
import { Attendance } from './features/user/pages/attendance/attendance';
import { Results } from './features/user/pages/results/results';
import { Tasks } from './features/user/pages/tasks/tasks';

export const routes:
  Routes = [
    { path: 'login', component: Login, },
    {
      path: 'dashboard', component: Dashboard,
      canActivate: [authGuard,],
    },
    {
      path: 'profile', component: Profile,
      canActivate: [authGuard,],
    },
    {
      path: 'attendance', component: Attendance,
      canActivate: [authGuard,],
    },
    {
      path: 'results', component: Results,
      canActivate: [authGuard,],
    },
    {
      path: 'tasks', component: Tasks,
      canActivate: [authGuard,],
    },
    { path: '', pathMatch: 'full', redirectTo: 'dashboard', },

    { path: '**', redirectTo: 'dashboard', },
];
