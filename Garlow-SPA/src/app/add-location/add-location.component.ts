import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { Location } from '../_models/location';
import { NgForm } from '@angular/forms';
import { AlertifyService } from '../_services/alertify.service';
import { LocationService } from '../_services/location.service';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-location',
  templateUrl: './add-location.component.html',
  styleUrls: ['./add-location.component.css']
})
export class AddLocationComponent implements OnInit {
  @ViewChild('createForm') createForm: NgForm;
  location: Location = { id: 0, name: '', address: '', photoUrl: '' };
  selectedImage: File = null;
  imgURL: any;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.createForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private alertify: AlertifyService,
    private locationService: LocationService,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit() {
  }

  createLocation() {
    this.locationService.createNewLocation(
      this.authService.decodedToken.nameid,
      this.location,
      this.selectedImage)
    .subscribe(next => {
      this.alertify.success('New location created!');
      this.router.navigate(['/locations']);
    }, error => {
      this.alertify.error(error);
    });
  }

  preview(files) {
    if (files.length === 0) {
      return;
    }

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.alertify.error('Only images are supported');
      this.selectedImage = null;
      return;
    }

    const reader = new FileReader();
    this.selectedImage = files[0];
    reader.readAsDataURL(files[0]);
    reader.onload = (_) => {
      this.imgURL = reader.result;
    };
  }
}
