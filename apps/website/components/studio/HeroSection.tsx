// [NEW]
'use client';
import React from 'react';

interface HeroSectionProps {
    ClientId?: string;
    Headline?: string;
    Subtitle?: string;
    ButtonText?: string;
    ButtonLink?: string;
    // Add other properties from the component library document as needed
}

const HeroSection: React.FC<HeroSectionProps> = ({
    ClientId,
    Headline = "Default Headline",
    Subtitle = "Default subtitle text.",
    ButtonText = "Click Me",
    ButtonLink = "#"
}) => {
    return (
        <div data-component-id={ClientId} style={{
            width: '100%',
            padding: '100px 20px',
            backgroundColor: '#f0f0f0',
            textAlign: 'center',
            borderBottom: '1px solid #ddd'
        }}>
            <h1>{Headline}</h1>
            <p>{Subtitle}</p>
            {ButtonText && ButtonLink && (
                <a href={ButtonLink} style={{
                    display: 'inline-block',
                    padding: '10px 20px',
                    backgroundColor: '#007bff',
                    color: 'white',
                    textDecoration: 'none',
                    borderRadius: '5px',
                    marginTop: '20px'
                }}>
                    {ButtonText}
                </a>
            )}
        </div>
    );
};

export default HeroSection;
