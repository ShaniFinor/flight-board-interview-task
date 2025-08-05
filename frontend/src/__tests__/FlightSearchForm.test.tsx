import '@testing-library/jest-dom';
import { render, screen, fireEvent } from '@testing-library/react';
import FlightSearchForm from '../components/FlightSearchForm';

describe('FlightSearchForm', () => {
  test('calls onSearch with correct values', () => {
    const onSearch = jest.fn();
    const onClear = jest.fn();

    render(<FlightSearchForm onSearch={onSearch} onClear={onClear} />);

    // choose status by role
    fireEvent.change(screen.getByRole('combobox'), {
      target: { value: 'Landed' },
    });

    // write destination - placeholder
    fireEvent.change(screen.getByPlaceholderText(/destination/i), {
      target: { value: 'Paris' },
    });

    fireEvent.click(screen.getByRole('button', { name: /search/i }));

    expect(onSearch).toHaveBeenCalledWith({ status: 'Landed', destination: 'Paris' });
  });

  test('calls onClear and resets fields', () => {
    const onSearch = jest.fn();
    const onClear = jest.fn();

    render(<FlightSearchForm onSearch={onSearch} onClear={onClear} />);

    fireEvent.change(screen.getByRole('combobox'), {
      target: { value: 'Departed' },
    });
    fireEvent.change(screen.getByPlaceholderText(/destination/i), {
      target: { value: 'Rome' },
    });

    fireEvent.click(screen.getByRole('button', { name: /clear/i }));

    expect(onClear).toHaveBeenCalled();
    expect(screen.getByRole('combobox')).toHaveValue('');
    expect(screen.getByPlaceholderText(/destination/i)).toHaveValue('');
  });
});