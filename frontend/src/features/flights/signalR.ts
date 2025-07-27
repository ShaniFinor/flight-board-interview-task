import * as signalR from '@microsoft/signalr';
//Hub connecion.
let connection: signalR.HubConnection;

if (process.env.NODE_ENV !== 'test') {
  connection = new signalR.HubConnectionBuilder()
    .withUrl('/flightsHub')
    .withAutomaticReconnect()
    .build();
} else {
  //to prevent errors dummy connection.
  connection = {
    start: () => Promise.resolve(),
    stop: () => Promise.resolve(),
    on: () => {},
    off: () => {},
    state: 'Disconnected',
  } as unknown as signalR.HubConnection;
}

export { connection };