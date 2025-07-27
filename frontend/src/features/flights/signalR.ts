import * as signalR from '@microsoft/signalr';
//Hub connecion.
export const connection = new signalR.HubConnectionBuilder()
  .withUrl('/flightsHub')
  .withAutomaticReconnect()
  .build();