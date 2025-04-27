import React from "react";
import GallerySlider from "./GallerySlider";
import "./Header.css";

const Header = () => {
  return (
    <header id="header">
      <div className="gallery-background">
        <GallerySlider />
      </div>
      <div className="overlay">
        <div className="container">
          <div className="row">
            <div className="col-md-8 col-md-offset-2 intro-text">
              <h1>DANCE SCORE APP</h1>
              <p>Könnyítsd meg a táncversenyek pontozását modern megoldásainkkal.</p>
              <a href="#features" className="btn btn-custom btn-lg page-scroll">
                Tudj meg többet
              </a>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;
