import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Location } from '../_models/location';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getLocationsForuser(userId: number) {
    return this.http.get(this.baseUrl + 'locations/foruser/' + userId);
  }

  getLocation(locationId: number) {
    return this.http.get(this.baseUrl + 'locations/' + locationId);
  }

  deleteLocation(locationId: number) {
    return this.http.delete(this.baseUrl + 'locations/' + locationId);
  }

  createNewLocation(userId: number, location: Location, photo: File) {
    const formData = new FormData();
    formData.append('photo', photo);
    formData.append('name', location.name);
    formData.append('address', location.address);
    return this.http.post(this.baseUrl + 'locations/foruser/' + userId, formData);
  }

  getMovements(locationId: number) {
    return this.http.get(this.baseUrl + 'movements/' + locationId);
  }
}
