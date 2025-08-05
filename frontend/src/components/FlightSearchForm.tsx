import React, { useState } from "react";

type Props = {
  onSearch: (filters: { status?: string; destination?: string }) => void;
  onClear: () => void;
};

const FlightSearchForm = ({ onSearch, onClear }: Props) => {
  const [status, setStatus] = useState("");
  const [destination, setDestination] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
     onSearch({ status, destination });
  };

   const handleClear = () => {
    setStatus('');
    setDestination('');
    onClear();
  };

  return (
    <form onSubmit={handleSubmit}>
      <select value={status} onChange={(e) => setStatus(e.target.value)}>
        <option value="">All Statuses</option>
        <option value="Scheduled">Scheduled</option>
        <option value="Boarding">Boarding</option>
        <option value="Departed">Departed</option>
        <option value="Landed">Landed</option>
      </select>

      <input
        type="text"
        placeholder="Destination"
        value={destination}
        onChange={(e) => setDestination(e.target.value)}
      />

      <button type="submit">Search</button>
      <button type="button" onClick={handleClear}>Clear Filters</button>
    </form>
  );
};

export default FlightSearchForm;