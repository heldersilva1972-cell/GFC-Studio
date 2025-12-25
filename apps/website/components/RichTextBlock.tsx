// [NEW]
import React from 'react';

interface RichTextBlockProps {
  content: string;
}

const RichTextBlock: React.FC<RichTextBlockProps> = ({ content }) => {
  return (
    <div dangerouslySetInnerHTML={{ __html: content }} />
  );
};

export default RichTextBlock;
