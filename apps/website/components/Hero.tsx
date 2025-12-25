// [NEW]
import React from 'react';

interface HeroProps {
  backgroundImage: string;
  headline: string;
}

const Hero: React.FC<HeroProps> = ({ backgroundImage, headline }) => {
  return (
    <div
      style={{
        backgroundImage: backgroundImage ? `url(${backgroundImage})` : 'none',
        backgroundColor: '#1a2a3a', // Fallback color
        height: '400px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        color: 'white',
        fontSize: '3rem',
        textAlign: 'center',
      }}
    >
      <h1>{headline}</h1>
    </div>
  );
};

export default Hero;
