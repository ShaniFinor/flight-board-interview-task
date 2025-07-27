import React, { useEffect, useState } from "react";
import { useFlights, deleteFlight, useSearchFlights } from "./api";
import { Flight } from "./flight";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { connection } from './signalR';
import FlightSearchForm from "./FlightSearchForm";

const FlightsList = () => {
    const queryClient = useQueryClient();
    const [filters, setFilters] = useState<{ status?: string; destination?: string }>({});
    const showFiltered = !!(filters.status || filters.destination);
    const flightsQuery = useFlights();
    const filteredQuery = useSearchFlights(filters);

    const flights = showFiltered ? filteredQuery.data : flightsQuery.data;
    const isLoading = showFiltered ? filteredQuery.isLoading : flightsQuery.isLoading;
    const isError = showFiltered ? filteredQuery.isError : flightsQuery.isError;
    const handleDelete = (flightNumber: string) => {
        deleteMutation.mutate(flightNumber);
    };

    const safeFlights = flights ?? [];

    const handleSearch = (newFilters: typeof filters) => {
        setFilters(newFilters);
    };

    const handleClear = () => {
        setFilters({});
    };
    const deleteMutation = useMutation({
        mutationFn: deleteFlight,
        onSuccess: () => {
            // refresh the flight list after the delete.
            queryClient.invalidateQueries({ queryKey: ['flights'] });
        }
    });

    // Listen to events that in the server - FlightAdded, FlightDeleted.
    useEffect(() => {
        if (connection.state === 'Disconnected') {
            connection.start().catch(err => console.error('SignalR Connection Error: ', err));
        }

        connection.on('FlightAdded', () => {
            queryClient.invalidateQueries({ queryKey: ['flights'] });
        });

        connection.on('FlightDeleted', () => {
            queryClient.invalidateQueries({ queryKey: ['flights'] });
        });

        return () => {
            connection.off('FlightAdded');
            connection.off('FlightDeleted');
        };
    }, []);


    if (isLoading) {
        return <p>Loading Flights.</p>
    }
    if (isError) {
        return <p>error - Failed to load flights</p>
    }


    return (<>
        <FlightSearchForm onSearch={handleSearch} onClear={handleClear} />
        {isLoading && <p>Loading Flights...</p>}
        {isError && <p>Failed to load flights.</p>}

        {!isLoading && !isError && flights?.length === 0 && (
            <p>No flights match your criteria.</p>
        )}

        {!isLoading && !isError && safeFlights?.length > 0 && (
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
                    {safeFlights.map((flight: Flight) => (
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
            </table>)}
    </>);
};

export default FlightsList;