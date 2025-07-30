import '@testing-library/jest-dom';
import { render, screen, fireEvent } from '@testing-library/react';
import SelectedFlight from '../SelectedFlight';
import { Provider } from 'react-redux';
import { configureStore } from '@reduxjs/toolkit';
import flightsReducer from '../flightsSlice';
import { RootState } from '../../../app/store';


//render with store of Redux
const renderWithRedux = (preloadedState: RootState) => {
    const store = configureStore({
        reducer: {
            selectedFlight: flightsReducer,
        },
        preloadedState,
    });
    return {
        store, ...render(
            <Provider store={store}>
                <SelectedFlight />
            </Provider>
        )
    };
};
describe('SelectedFlight component', () => {
    test('dont present anything when there is no selected flight.', () => {
        renderWithRedux({ selectedFlight: { selectedFlight: null } });
        expect(screen.queryByText(/Details:/)).not.toBeInTheDocument();
    });

    test('when choosing flight, present the flight details.', () => {
        const flight = {
            flightNumber: 'AA123',
            destination: 'London',
            departureTime: new Date().toISOString(),
            gate: 'A8',
            status: 'Scheduled',
        };

        renderWithRedux({ selectedFlight: { selectedFlight: flight } });

        expect(screen.getByText(/Details:/)).toBeInTheDocument();
        expect(screen.getByText(/AA123/)).toBeInTheDocument();
        expect(screen.getByText(/London/)).toBeInTheDocument();
        expect(screen.getByText(/A1/)).toBeInTheDocument();
        expect(screen.getByText(/Scheduled/)).toBeInTheDocument();
    });

    test('press on close button, actvate clearSelectedFlight, delete from the state.', () => {
        const flight = {
            flightNumber: 'BB456',
            destination: 'Berlin',
            departureTime: new Date().toISOString(),
            gate: 'B2',
            status: 'Landed',
        };

        const { store } = renderWithRedux({ selectedFlight: { selectedFlight: flight } });

        const button = screen.getByText(/close/i);
        fireEvent.click(button);

        expect(store.getState().selectedFlight.selectedFlight).toBeNull();
    });
});