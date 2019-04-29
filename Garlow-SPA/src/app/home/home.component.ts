import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  values: any;
  anonymousMode = true;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.currentUserOBS.subscribe(currentUser => {
      if (currentUser) {
        this.anonymousMode = false;
      } else {
        this.anonymousMode = true;
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
