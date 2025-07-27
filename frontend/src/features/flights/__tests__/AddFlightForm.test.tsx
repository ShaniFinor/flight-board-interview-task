import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import AddFlightForm from '../AddFlightForm';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import * as api from '../api';
import '@testing-library/jest-dom';

jest.mock('../api'); // create mock to addFligh func

describe('AddFlightForm', () => {
  const mockAddFlight = api.addFlight as jest.Mock;

  beforeEach(() => {
    mockAddFlight.mockResolvedValue({}); // can return empty object
  });

  function renderWithClient(ui: React.ReactElement) {
    const queryClient = new QueryClient();
    return render(<QueryClientProvider client={queryClient}>{ui}</QueryClientProvider>);
  }

  test('submits the form and calls addFlight', async () => {
    renderWithClient(<AddFlightForm />);

    fireEvent.change(screen.getByPlaceholderText(/flight number/i), {
      target: { value: 'AB123' },
    });

    fireEvent.change(screen.getByPlaceholderText(/destination/i), {
      target: { value: 'Rome' },
    });

    fireEvent.change(screen.getByPlaceholderText(/departure time/i), {
      target: { value: '2025-12-01T15:30' },
    });

    fireEvent.change(screen.getByPlaceholderText(/gate/i), {
      target: { value: 'A1' },
    });

    fireEvent.click(screen.getByRole('button', { name: /add flight/i }));

    await waitFor(() => {
      expect(mockAddFlight).toHaveBeenCalledWith({
        flightNumber: 'AB123',
        destination: 'Rome',
        departureTime: '2025-12-01T15:30',
        gate: 'A1',
      });
    });
  });
});