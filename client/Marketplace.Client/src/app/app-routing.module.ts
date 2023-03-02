import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {Route} from "./Shared/Route/Route";

const routes: Routes = [
      {
        path: '',
        redirectTo: Route.AUTH,
        pathMatch : 'full'
      },
      {
        path: Route.AUTH,
        loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
      },
      {
        path: Route.MAIN,
        loadChildren: () => import('./main/main.module').then(m => m.MainModule)
      }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
