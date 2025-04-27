
import React from "react";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

const GallerySlider = () => {
  const settings = {
    dots: false, /* Disable manual dots for automatic-only slider */
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 3000,
  };

  const images = [
    "img/banner/1.jpg",
    "img/banner/2.jpg",
    "img/banner/3.jpg",
    "img/banner/4.jpg",
    "img/banner/5.jpg",
  ];

  return (
    <div className="gallery-slider">
      <Slider {...settings}>
        {images.map((image, index) => (
          <div key={index} className="slider-image-container">
            <img
              src={image}
              alt={`Slide ${index + 1}`}
              className="slider-image"
            />
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default GallerySlider;
