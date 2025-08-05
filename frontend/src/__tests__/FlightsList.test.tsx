import '@testing-library/jest-dom';
import { render, screen } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import FlightsList from '../components/FlightsList';
import { Provider } from 'react-redux';
import { store } from '../store/store';

//useFlights,api mock
jest.mock('../api/api', () => ({
  useFlights: () => ({
    data: [],
    isLoading: false,
    isError: false,
  }),
  useSearchFlights: () => ({
    data: [],
    isLoading: false,
    isError: false,
  }),
  deleteFlight: jest.fn(),
}));

// SignalR mock
// jest.mock('../signalR', () => ({
//   connection: {
//     start: jest.fn(),
//     stop: jest.fn(),
//     on: jest.fn(),
//     off: jest.fn(),
//     state: 'Disconnected',
//   },
// }));

const queryClient = new QueryClient();

test('displays no flights message when list is empty', () => {
  render(
    <QueryClientProvider client={queryClient}>
      <Provider store={store}>
        <FlightsList />
      </Provider>
    </QueryClientProvider>
  );

  const message = screen.getByText(/no flights/i);
  expect(message).toBeInTheDocument();
});