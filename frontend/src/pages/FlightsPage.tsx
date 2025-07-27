import { constants } from "node:http2";
import FlightsList from "../features/flights/FlightsList";

const FlightsPage = () => {
    return (
        <div>
            <h1>Flight Board</h1>
            <FlightsList />
        </div>
    );
};

export default FlightsPage;