import * as signalR from '@microsoft/signalr';
const apiUrl = process.env.REACT_APP_API_BASE_URL!;;
//Hub connecion.
let connection: signalR.HubConnection;

if (process.env.NODE_ENV !== 'test') {
  connection = new signalR.HubConnectionBuilder()
    .withUrl(`${apiUrl}/flightsHub`)
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