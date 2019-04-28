import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { AlertifyService } from './alertify.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  private baseUrl = environment.apiUrl;

  constructor(private alertify: AlertifyService) { }

  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(this.baseUrl + 'signalr').build();

    this.hubConnection
      .start()
      .then(() => this.alertify.success('SignalR connection established!'))
      .catch(error => this.alertify.error(error));
  }

  public listenForMovements(callback: (direction: number) => void) {
    this.hubConnection.on('remote-method', (data) => {
      callback(data as number);
    });
  }
}
