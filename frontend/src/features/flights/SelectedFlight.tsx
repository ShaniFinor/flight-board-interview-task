import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { RootState } from '../../app/store';
import { clearSelectedFlight } from './flightsSlice';
import './SelectedFlight.css';

const SelectedFlight = () => {
    const dispatch = useDispatch();
    //get state from Redux store. to get the value of the selected flight.
    const selectedFlight = useSelector(
        (state: RootState) => state.selectedFlight.selectedFlight
    );

    if (!selectedFlight) return null;

    return (
        <div className="modal-overlay" onClick={() => dispatch(clearSelectedFlight())}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <h3>Details:</h3>
                <p><span className="label">Flight Number: </span> {selectedFlight.flightNumber}</p>
                <p><span className="label">Destination: </span> {selectedFlight.destination}</p>
                <p><span className="label">Departure Time: </span> {new Date(selectedFlight.departureTime).toLocaleString('he-IL')}</p>
                <p><span className="label">Gate: </span> {selectedFlight.gate}</p>
                <p><span className="label">Status: </span> {selectedFlight.status}</p>
                <button className="clear-button" onClick={() => dispatch(clearSelectedFlight())}>
                    close
                </button>
            </div>
        </div>
    );
};

export default SelectedFlight;