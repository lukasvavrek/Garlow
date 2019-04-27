import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LocationService } from '../_services/location.service';
import { MovementsChart } from '../_models/movements-chart';

@Injectable()
export class LocationDetailMovementsResolver implements Resolve<MovementsChart> {
    constructor(
        private locationService: LocationService,
        private router: Router,
        private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<MovementsChart> {
        /* tslint:disable:no-string-literal */
        return this.locationService.getMovements(route.params['locationid']).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
        /* tslint:enable:no-string-literal */
    }
}
