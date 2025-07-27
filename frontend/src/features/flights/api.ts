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