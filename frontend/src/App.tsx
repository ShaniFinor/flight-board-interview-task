import React from 'react';
import './App.css';
import FlightsPage from './pages/FlightsPage';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

function App() {
  return (
    <>
      <FlightsPage />
      <ReactQueryDevtools initialIsOpen={false} />
    </>
  );
}

export default App;
