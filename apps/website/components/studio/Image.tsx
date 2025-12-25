// [NEW]
'use client';
import React from 'react';

interface ImageProps {
    ClientId?: string;
    src?: string;
    alt?: string;
}

const Image: React.FC<ImageProps> = ({
    ClientId,
    src = "https://via.placeholder.com/1200x400?text=Placeholder+Image",
    alt = "Placeholder image"
}) => {
    return (
        <div data-component-id={ClientId} style={{ padding: '20px' }}>
            <img src={src} alt={alt} style={{ maxWidth: '100%', height: 'auto' }} />
        </div>
    );
};

export default Image;
