import { LoginRoutingGuard } from './login-routing.guard';
import { LoginComponent } from './login/login.component';
import { OrderComponent } from './order/order.component';
import { ShipperComponent } from './shipper/shipper.component';
import { ProdComponent } from './prod/prod.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo:'prod', pathMatch:'full'},
  { path: 'prod', component: ProdComponent},
  { path: 'shipper', component: ShipperComponent},
  { path: 'order', component: OrderComponent, canActivate:[LoginRoutingGuard]},
  { path: 'login', component: LoginComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
