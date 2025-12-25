// [MODIFIED]
import React from 'react';

interface ButtonCTAProps {
  link: string;
  text: string;
  padding: string;
  backgroundColor: string;
  variant: 'default' | 'burnished-gold';
}

const ButtonCTA: React.FC<ButtonCTAProps> = ({ link, text, padding, backgroundColor, variant }) => {
  const styles = {
    padding,
    backgroundColor: backgroundColor || (variant === 'burnished-gold' ? '#D4AF37' : '#007bff'),
    color: 'white',
    textDecoration: 'none',
    display: 'inline-block',
    borderRadius: '5px',
  };

  return (
    <a href={link || '#'} style={styles}>
      {text}
    </a>
  );
};

export default ButtonCTA;
