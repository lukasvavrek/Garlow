import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LocationService } from '../_services/location.service';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class LocationDetailResolver implements Resolve<Location> {
    constructor(
        private locationService: LocationService,
        private authService: AuthService,
        private router: Router,
        private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Location> {
        /* tslint:disable:no-string-literal */
        return this.locationService.getLocation(route.params['locationid']).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
        /* tslint:enable:no-string-literal */
    }
}
