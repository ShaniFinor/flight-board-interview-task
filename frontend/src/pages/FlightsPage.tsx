import { constants } from "node:http2";
import FlightsList from "../components/FlightsList";
import AddFlightForm from "../components/AddFlightForm";

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