// [NEW]
import React from 'react';
import DynamicRenderer from './DynamicRenderer';
import { StudioSection } from '@/app/lib/types';

interface LayoutGridProps {
  columns: 2 | 3;
  children: StudioSection[];
}

const LayoutGrid: React.FC<LayoutGridProps> = ({ columns, children }) => {
  const gridStyle = {
    display: 'grid',
    gridTemplateColumns: `repeat(${columns}, 1fr)`,
    gap: '1rem',
  };

  return (
    <div style={gridStyle}>
      <DynamicRenderer sections={children} />
    </div>
  );
};

export default LayoutGrid;
