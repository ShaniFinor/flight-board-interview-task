import { constants } from "node:http2";
import FlightsList from "../features/flights/FlightsList";
import AddFlightForm from "../features/flights/AddFlightForm";

const FlightsPage = () => {
    return (
        <div>
            <h1>Flight Board</h1>
            <AddFlightForm />
            <FlightsList />
        </div>
    );
};

export default FlightsPage;