import { LoginRoutingGuard } from './login-routing.guard';
import { LoginService } from 'app/login.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from "@angular/forms";   //加回來
import { HttpModule } from "@angular/http";   //加回來

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AlertModule } from 'ngx-bootstrap';
import { ProdComponent } from './prod/prod.component';
import { ShipperComponent } from './shipper/shipper.component';
import { OrderComponent } from './order/order.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    ProdComponent,
    ShipperComponent,
    OrderComponent,
    LoginComponent
  ],
  imports: [
    AlertModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    FormsModule,    //加回來
    HttpModule  //加回來
  ],
  providers: [LoginService,LoginRoutingGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
