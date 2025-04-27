import React from "react";

export const About = ({ data }) => {
  return (
    <div id="about" className="about-section">
      <div className="container">
        <div className="row">
          <div className="col-md-6">
            <img src="img/about.jpg" className="img-responsive" alt="Rólunk" />
          </div>
          <div className="col-md-6">
            <div className="about-text">
              <h2>Rólunk</h2>
              <p>{data ? data.paragraph : "Betöltés..."}</p>
              <h3>Miért válassz minket?</h3>
              <ul>
                {data
                  ? data.Why.map((item, index) => <li key={index}>{item}</li>)
                  : "Betöltés..."}
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
