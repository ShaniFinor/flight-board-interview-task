import React from "react";
import { useFlights } from "./api";
import { Flight } from "./flight";

const FlightsList = () => {
    const { data: flights, isLoading, isError } = useFlights();

    if (isLoading) {
        return <p>Loading Flights.</p>
    }
    if (isError) {
        return <p>error - Failed to load flights</p>
    }
    if (!flights || flights.length == 0) {
        return <p>No flights.</p>
    }

    console.log("Flights raw:", flights);
    console.log("Flight Numbers:", flights.map(f => f.flightNumber));
    return (
        <table>
            <thead>
                <tr>
                    <th>Flight Number</th>
                    <th>Destination</th>
                    <th>Departure Time</th>
                    <th>Gate</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                {flights.map((flight: Flight) => (
                    <tr key={flight.flightNumber}>
                        <td>{flight.flightNumber}</td>
                        <td>{flight.destination}</td>
                        <td> {new Date(flight.departureTime).toLocaleString('he-IL', {
                            dateStyle: 'short',
                            timeStyle: 'short'
                        })}</td>
                        <td>{flight.gate}</td>
                        <td>{flight.status}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default FlightsList;