import { Component, OnInit, Input } from '@angular/core';
import { Location } from '../_models/location';
import { LocationService } from '../_services/location.service';

@Component({
  selector: 'app-location-card',
  templateUrl: './location-card.component.html',
  styleUrls: ['./location-card.component.css']
})
export class LocationCardComponent implements OnInit {
  @Input() location: Location;

  constructor(private locationService: LocationService) { }

  ngOnInit() {
  }
}
