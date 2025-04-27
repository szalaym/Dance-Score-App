import React from "react";

export const Contact = () => {
  return (
    <div id="contact" className="contact-section">
      <div className="container">
        <h2>Lépj kapcsolatba velünk!</h2>
        <form className="contact-form">
          <input
            type="text"
            placeholder="Név"
            required
            className="form-control"
          />
          <input
            type="email"
            placeholder="Email"
            required
            className="form-control"
          />
          <textarea
            placeholder="Üzenet"
            required
            className="form-control"
          ></textarea>
          <button type="submit" className="btn-submit">
            Üzenet küldése
          </button>
        </form>
      </div>
    </div>
  );
};
