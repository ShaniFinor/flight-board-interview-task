// using TanStack Query (React Query) to fetch flights from the server.
import axios from 'axios';
import { useQuery } from '@tanstack/react-query';
import { Flight } from './flight';

const BASE_URL = 'http://localhost:5264/api/flights';

//Get/api/flights
export const fetchFlights = async (): Promise<Flight[]> => {
  const response = await axios.get<Flight[]>(BASE_URL);
  return response.data;
};

//hook to get the data in components.
export const useFlights = () => {
  return useQuery({
    queryKey: ['flights'],
    queryFn: fetchFlights,
  });
};

export async function addFlight(flight: Omit<Flight, 'status'>): Promise<Flight> {
  const response = await fetch('http://localhost:5264/api/flights', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(flight),
  });

  if (!response.ok) {
    throw new Error('Failed to add flight');
  }

  return response.json();
}

export const deleteFlight = async (flightNumber: string) => {
  await fetch(`/api/flights/${flightNumber}`, {
    method: 'DELETE'
  });
};

export const searchFlights = async (filters: { status?: string; destination?: string }) => {
  const params = new URLSearchParams();
  if (filters.status) params.append("status", filters.status);
  if (filters.destination) params.append("destination", filters.destination);

  const response = await fetch(`/api/flights/search?${params.toString()}`);
  if (!response.ok) throw new Error("Failed to search flights");
  return response.json();
};


export const useSearchFlights = (filters: { status?: string; destination?: string }) => {
  return useQuery<Flight[]>({
    queryKey: ['flights', filters],
    queryFn: () => searchFlights(filters),
    enabled: !!(filters.status || filters.destination), //if have filters
  });
};