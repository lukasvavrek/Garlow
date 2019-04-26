import { Component, OnInit, Input } from '@angular/core';
import { Location } from '../_models/location';

@Component({
  selector: 'app-location-card',
  templateUrl: './location-card.component.html',
  styleUrls: ['./location-card.component.css']
})
export class LocationCardComponent implements OnInit {
  @Input() location: Location;

  constructor() { }

  ngOnInit() {
  }
}
