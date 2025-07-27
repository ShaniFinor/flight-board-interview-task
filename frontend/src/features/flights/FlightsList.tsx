import React from "react";
import { useFlights } from "./api";
import { Flight } from "./flight";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteFlight } from "./api";

const FlightsList = () => {
    const { data: flights, isLoading, isError } = useFlights();
    const queryClient = useQueryClient();

    const deleteMutation = useMutation({
        mutationFn: deleteFlight,
        onSuccess: () => {
            // refresh the flight list after the delete.
            queryClient.invalidateQueries({ queryKey: ['flights'] });
        }
    });
    
    if (isLoading) {
        return <p>Loading Flights.</p>
    }
    if (isError) {
        return <p>error - Failed to load flights</p>
    }
    if (!flights || flights.length == 0) {
        return <p>No flights.</p>
    }

    const handleDelete = (flightNumber: string) => {
        deleteMutation.mutate(flightNumber);
    };

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
                        <td>
                            <button onClick={() => handleDelete(flight.flightNumber)} style={{ color: 'red' }}>
                                Delete
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default FlightsList;