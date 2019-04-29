import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  values: any;
  anonymousMode = true;

  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.authService.currentUserOBS.subscribe(currentUser => {
      if (currentUser) {
        this.anonymousMode = false;
      } else {
        this.anonymousMode = true;
        this.alertify.message('fooo');
      }
    });
  }

  registerToggle() {
    this.registerMode = true;
  }

  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
}
