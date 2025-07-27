import { useState } from "react";
import { isImportEqualsDeclaration } from "typescript";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Flight } from "./flight";
import { addFlight } from "./api";


const initialForm = {
    flightNumber: '',
    destination: '',
    departureTime: '',
    gate: '',
};

export default function AddFlightForm() {
    const [formData, setFormData] = useState(initialForm);
    const queryClient = useQueryClient();

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
        mutation.mutate({
            ...formData,
            departureTime: formData.departureTime,
        });
    }

    return (
        <form onSubmit={handleSubmit}>
            <input name="flightNumber" value={formData.flightNumber} onChange={handleChange} placeholder="Flight Number" required />
            <input name="destination" value={formData.destination} onChange={handleChange} placeholder="Destination" required />
            <input name="departureTime" value={formData.departureTime} onChange={handleChange} placeholder="Departure Time" required type="datetime-local" />
            <input name="gate" value={formData.gate} onChange={handleChange} placeholder="Gate" required />
            <button type="submit">Add Flight</button>
        </form>
    );
}

