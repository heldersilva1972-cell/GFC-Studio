// [NEW]
'use client';
import React from 'react';

interface ButtonProps {
    ClientId?: string;
    ButtonText?: string;
    ButtonLink?: string;
}

const Button: React.FC<ButtonProps> = ({
    ClientId,
    ButtonText = "Click Me",
    ButtonLink = "#"
}) => {
    return (
        <div data-component-id={ClientId} style={{ padding: '20px' }}>
            <a href={ButtonLink} style={{
                display: 'inline-block',
                padding: '10px 20px',
                backgroundColor: '#007bff',
                color: 'white',
                textDecoration: 'none',
                borderRadius: '5px'
            }}>
                {ButtonText}
            </a>
        </div>
    );
};

export default Button;
