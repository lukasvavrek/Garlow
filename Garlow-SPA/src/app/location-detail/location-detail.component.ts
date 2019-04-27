import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '../_models/location';
import { LocationService } from '../_services/location.service';
import { AlertifyService } from '../_services/alertify.service';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, BaseChartDirective, Label } from 'ng2-charts';
import * as pluginAnnotations from 'chartjs-plugin-annotation';
import { MovementsChart } from '../_models/movements-chart';

@Component({
  selector: 'app-location-detail',
  templateUrl: './location-detail.component.html',
  styleUrls: ['./location-detail.component.css']
})
export class LocationDetailComponent implements OnInit {
  location: Location;

  public lineChartData: ChartDataSets[] = [];
  public lineChartLabels: Label[] = [''];
  public lineChartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    scales: {
      // We use this empty structure as a placeholder for dynamic theming.
      xAxes: [{}],
      yAxes: [
        {
          id: 'y-axis-0',
          position: 'left',
        }
      ]
    },
    annotation: {
      annotations: [],
    },
  };

  public lineChartColors: Color[] = [{ // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
  }];

  @ViewChild(BaseChartDirective) chart: BaseChartDirective;

  constructor(
    private route: ActivatedRoute,
    private locationService: LocationService,
    private router: Router,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      /* tslint:disable:no-string-literal */
      this.location = data['location'];
      const movements = data['movements'] as MovementsChart;

      this.lineChartData.push({ data: movements.counts, label: '' });

      for (const movement of movements.counts) {
      //   this.alertify.message('' + movement);
      //   this.lineChartData[0].data.push(movement);
        this.lineChartLabels.push(['']);
      }
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

  // events
  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }
}
