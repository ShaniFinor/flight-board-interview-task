//store/..
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {Flight} from "./flight";
interface FlightsState{
    selectedFlight: Flight |null;
}

const initialState : FlightsState={
    selectedFlight:null,
};

//save selected flight.
const flightsSlice = createSlice({
    name: 'selectedFlight',
    initialState,
    reducers:{
        selectFlight(state, action: PayloadAction<Flight>){
            state.selectedFlight = action.payload;
        },
        clearSelectedFlight(state){
            state.selectedFlight = null;
        },
    },
});

export const {selectFlight, clearSelectedFlight} = flightsSlice.actions;
export default flightsSlice.reducer;
