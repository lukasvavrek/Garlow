import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '../_models/location';
import { LocationService } from '../_services/location.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-location-detail',
  templateUrl: './location-detail.component.html',
  styleUrls: ['./location-detail.component.css']
})
export class LocationDetailComponent implements OnInit {
  location: Location;

  constructor(
    private route: ActivatedRoute,
    private locationService: LocationService,
    private router: Router,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      /* tslint:disable:no-string-literal */
      this.location = data['location'];
      /* tslint:enable:no-string-literal */
    });
  }

  deleteLocation() {
    if (confirm('Are you sure you want to delete this location permanently?')) {
      this.locationService.deleteLocation(this.location.id).subscribe(next => {
        this.alertify.success('Location deleted');
        this.router.navigate(['/locations']);
      }, error => {
        this.alertify.error(error);
      });
    }
  }
}
