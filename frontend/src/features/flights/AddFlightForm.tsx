//components/
import { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Flight } from "./flight";
import { addFlight } from "./api";
import "./AddFlightForm.css";

const initialForm = {
    flightNumber: '',
    destination: '',
    departureTime: '',
    gate: '',
};

export default function AddFlightForm() {
    const [formData, setFormData] = useState(initialForm);
    const queryClient = useQueryClient();
    const [formErrors, setFormErrors] = useState<Partial<typeof initialForm>>({});

    const mutation = useMutation({
        mutationFn: addFlight,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['flights'] });
            setFormData(initialForm);
        },
    });

    function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    }

    function handleSubmit(e: React.FormEvent) {
        e.preventDefault();

        const errors = validateForm();
        if (Object.keys(errors).length > 0) {
            setFormErrors(errors);
            return;
        }

        setFormErrors({});

        mutation.mutate({
            ...formData,
            departureTime: formData.departureTime,
        });
    }

    function validateForm() {
        const errors: Partial<typeof initialForm> = {};
        const { flightNumber, destination, departureTime, gate } = formData;

        if (!flightNumber.trim()) errors.flightNumber = "Flight Number is required.";
        if (!destination.trim()) errors.destination = "Destination is required.";
        if (!gate.trim()) errors.gate = "Gate is required.";

        const now = new Date();
        const departure = new Date(departureTime);
        if (!departureTime.trim() || isNaN(departure.getTime())) {
            errors.departureTime = "Invalid date/time.";
        } else if (departure <= now) {
            errors.departureTime = "Departure must be in the future.";
        }

        return errors;
    }

    return (
        <div className="form-add-flight">
            <form onSubmit={handleSubmit}>
                <div className="form-row">
                    <div className="form-group">
                        <input
                            className="form-input"
                            name="flightNumber"
                            value={formData.flightNumber}
                            onChange={handleChange}
                            placeholder="Flight Number"
                        />
                        <div className="error">{formErrors.flightNumber && <span>{formErrors.flightNumber}</span>}</div>
                    </div>

                    <div className="form-group">
                        <input
                            name="destination"
                            value={formData.destination}
                            onChange={handleChange}
                            placeholder="Destination"
                        />
                        <div className="error">{formErrors.destination && <span>{formErrors.destination}</span>}</div>
                    </div>

                    <div className="form-group">
                        <input
                            name="departureTime"
                            value={formData.departureTime}
                            onChange={handleChange}
                            placeholder="Departure Time"
                            type="datetime-local"
                        />
                        <div className="error">{formErrors.departureTime && <span>{formErrors.departureTime}</span>}</div>
                    </div>

                    <div className="form-group">
                        <input
                            name="gate"
                            value={formData.gate}
                            onChange={handleChange}
                            placeholder="Gate"
                        />
                        <div className="error">{formErrors.gate && <span>{formErrors.gate}</span>}</div>
                    </div>

                    <button type="submit">Add Flight</button>
                </div>
            </form>
        </div>
    );
}

