// src/router/ParkingSettings.tsx
import React, { useState } from "react";

interface Car {
  id: number;
  plate: string;
  type: string;
  color: string;
}

const ParkingSettings: React.FC = () => {
  const [cars, setCars] = useState<Car[]>([]);
  const [form, setForm] = useState({ plate: "", type: "", color: "" });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleAddCar = () => {
    if (!form.plate || !form.type || !form.color) return;
    const newCar: Car = {
      id: Date.now(),
      plate: form.plate,
      type: form.type,
      color: form.color,
    };
    setCars((prev) => [...prev, newCar]);
    setForm({ plate: "", type: "", color: "" });
  };

  return (
    <div className="bg-black text-white min-vh-100 p-4">
      <h4 className="text-info mb-4">🚗 Parking Settings</h4>

      <div className="card bg-dark text-white p-3 mb-4 border border-secondary">
        <h5 className="mb-3">Add New Car</h5>
        <div className="mb-3">
          <label className="form-label">License Plate</label>
          <input
            type="text"
            name="plate"
            className="form-control bg-secondary text-white"
            value={form.plate}
            onChange={handleChange}
            placeholder="ZG1234AB"
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Car Type</label>
          <input
            type="text"
            name="type"
            className="form-control bg-secondary text-white"
            value={form.type}
            onChange={handleChange}
            placeholder="BMW, Porsche, etc."
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Color</label>
          <input
            type="text"
            name="color"
            className="form-control bg-secondary text-white"
            value={form.color}
            onChange={handleChange}
            placeholder="Red, Blue, Black..."
          />
        </div>
        <button className="btn btn-info w-100" onClick={handleAddCar}>
          ➕ Add Car
        </button>
      </div>

      {cars.length > 0 && (
        <div>
          <h5 className="mb-3">Your Registered Cars</h5>
          {cars.map((car) => (
            <div
              key={car.id}
              className="bg-dark p-3 mb-2 border border-secondary rounded"
            >
              <strong>{car.plate}</strong>
              <div className="text-muted small">
                {car.type} — {car.color}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ParkingSettings;