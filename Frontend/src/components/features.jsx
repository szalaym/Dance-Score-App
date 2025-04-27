import React from "react";

export const Features = ({ data }) => {
  return (
    <div id="features" className="features-section">
      <div className="container">
        <h2>Funkciók</h2>
        <div className="features-grid">
          {data
            ? data.map((feature, index) => (
                <div key={index} className="feature-item">
                  <i className={feature.icon}></i>
                  <h3>{feature.title}</h3>
                  <p>{feature.text}</p>
                </div>
              ))
            : "Betöltés..."}
        </div>
      </div>
    </div>
  );
};
