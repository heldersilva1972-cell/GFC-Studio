// [NEW]
'use client';
import React from 'react';

interface TextBlockProps {
    ClientId?: string;
    Content?: string;
}

const TextBlock: React.FC<TextBlockProps> = ({
    ClientId,
    Content = "This is a default text block. Edit the content in the properties panel."
}) => {
    // In a real implementation, this would use a rich text renderer
    // or dangerouslySetInnerHTML if the content is trusted HTML.
    return (
        <div data-component-id={ClientId} style={{
            padding: '20px',
            lineHeight: '1.6'
        }}>
            <p>{Content}</p>
        </div>
    );
};

export default TextBlock;
