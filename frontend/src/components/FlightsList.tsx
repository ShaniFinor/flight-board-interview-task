import React, { useEffect, useState } from "react";
import { useFlights, deleteFlight, useSearchFlights } from "../api/api";
import { Flight } from "../types/flight";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { connection } from '../signalR/signalR';
import FlightSearchForm from "./FlightSearchForm";
import { motion, AnimatePresence } from "framer-motion"; //fade/slide-in animation for rows.
import { useDispatch } from "react-redux";
import { selectFlight } from "../store/flightsSlice";
import SelectedFlight from "./SelectedFlight";

const FlightsList = () => {
    const queryClient = useQueryClient();
    const [filters, setFilters] = useState<{ status?: string; destination?: string }>({});
    const showFiltered = !!(filters.status || filters.destination);
    const flightsQuery = useFlights();
    const filteredQuery = useSearchFlights(filters);
    const dispatch = useDispatch();

    const flights = showFiltered ? filteredQuery.data : flightsQuery.data;
    const isLoading = showFiltered ? filteredQuery.isLoading : flightsQuery.isLoading;
    const isError = showFiltered ? filteredQuery.isError : flightsQuery.isError;
    console.log("API URL: ", process.env.REACT_APP_API_BASE_URL);

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
        onMutate: async (flightNumber) => {
            await queryClient.cancelQueries({ queryKey: ['flights'] });
            //getQueryData - saves the cache in case something crashing and need to restore the cache.
            const previous = queryClient.getQueryData<Flight[]>(['flights']);
            //optimistic update - simulates the deletion on the UI, before it got answer from the server.
            //setQueryData - update the cache.
            queryClient.setQueryData<Flight[]>(['flights'], (old = []) =>
                old.filter(f => f.flightNumber !== flightNumber)
            );

            return { previous };
        },
        onError: (err, flightNumber, context) => {
            if (context?.previous) {
                queryClient.setQueryData(['flights'], context.previous);
            }
        },
        onSettled: () => {
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

    const getStatusColor = (status: string) => {
        switch (status) {
            case "Boarding":
                return "#fff3cd"; // yellow
            case "Departed":
                return "#d1ecf1"; // light blue
            case "Landed":
                return "#d4edda"; //green
            case "Scheduled":
            default:
                return "#f8f9fa"; // gray
        }
    };

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
                    <AnimatePresence>
                        {safeFlights.map((flight: Flight) => (
                            <motion.tr
                                key={flight.flightNumber}
                                initial={{ opacity: 0, y: -10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, x: -20 }}
                                transition={{ duration: 0.3 }}
                            >
                                <td>{flight.flightNumber}</td>
                                <td>{flight.destination}</td>
                                <td> {new Date(flight.departureTime).toLocaleString('he-IL', {
                                    dateStyle: 'short',
                                    timeStyle: 'short'
                                })}</td>
                                <td>{flight.gate}</td>
                                <motion.td
                                    layout
                                    animate={{ backgroundColor: getStatusColor(flight.status) }}
                                    transition={{ duration: 0.5 }}
                                >
                                    {flight.status}
                                </motion.td>
                                <td>
                                    <button onClick={() => handleDelete(flight.flightNumber)} style={{ color: 'red' }}>
                                        Delete
                                    </button>
                                </td>
                                <td>
                                    <button onClick={() => dispatch(selectFlight(flight))}>
                                        Details
                                    </button>
                                </td>
                            </motion.tr>
                        ))}
                    </AnimatePresence>
                </tbody>
            </table>)
        }
        <SelectedFlight />
    </>);
};

export default FlightsList;