import { configureStore } from "@reduxjs/toolkit";
import flightsReducer from "../store/flightsSlice";

//Redux store - create state managment 
export const store = configureStore({
    reducer: {
        // slice with the name 'flights' from flightsSlice.ts will be managed by flightsReducer.
        // every slice is key in the global state 
        selectedFlight: flightsReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;