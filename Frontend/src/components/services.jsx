import React from "react";

export const Services = ({ data }) => {
  return (
    <div id="services" className="services-section">
      <div className="container">
        <h2>Szolgáltatásaink</h2>
        <div className="row">
          {data
            ? data.map((service, index) => (
                <div key={index} className="col-md-4">
                  <i className={service.icon}></i>
                  <h3>{service.name}</h3>
                  <p>{service.text}</p>
                </div>
              ))
            : "Betöltés..."}
        </div>
      </div>
    </div>
  );
};