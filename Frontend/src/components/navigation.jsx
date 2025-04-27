import React, { useState } from "react";

export const Navigation = ({ onOpenModal, onOpenResultsModal, onOpenRegistrationModal }) => {
  const [menuOpen, setMenuOpen] = useState(false);

  const toggleMenu = () => {
    setMenuOpen(!menuOpen);
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <a href="#page-top" className="navbar-brand">
          DANCE SCORE APP
        </a>
        <button className="navbar-toggle" onClick={toggleMenu}>
          <span className="icon-bar"></span>
          <span className="icon-bar"></span>
          <span className="icon-bar"></span>
        </button>
        <ul className={`navbar-nav ${menuOpen ? "active" : ""}`}>
          <li><a href="#services">Szolgáltatások</a></li>
          <li>
            {/* eslint-disable-next-line jsx-a11y/anchor-is-valid */}
            <a
              className="nevezes-link"
              onClick={onOpenModal}
              style={{ cursor: "pointer" }}
            >
              Nevezés
            </a>
          </li>
          <li>
            {/* eslint-disable-next-line jsx-a11y/anchor-is-valid */}
            <a
              className="eredmenyek-link"
              onClick={onOpenResultsModal}
              style={{ cursor: "pointer" }}
            >
              Eredmények
            </a>
          </li>
          <li>
            {/* eslint-disable-next-line jsx-a11y/anchor-is-valid */}
            <a
              className="regisztracio-link"
              onClick={onOpenRegistrationModal}
              style={{ cursor: "pointer" }}
            >
              Regisztráció
            </a>
          </li>
        </ul>
      </div>
    </nav>
  );
};