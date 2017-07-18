import { Component, OnInit } from '@angular/core';
import { LoginService } from "app/login.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private svcLogin:LoginService) { }

  ngOnInit() {
  }

}
