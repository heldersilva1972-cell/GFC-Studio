/**
 * Realistic flag rendering components
 * These render actual flag designs that look like real flags
 */

import React from "react";

interface USFlagProps {
  x: number;
  y: number;
  width: number;
  height: number;
}

export const USFlag: React.FC<USFlagProps> = ({ x, y, width, height }) => {
  const stripeHeight = height / 13;
  const cantonWidth = width * 0.4; // 40% of flag width for canton
  const cantonHeight = stripeHeight * 7; // 7 stripes tall

  // Generate 50 stars in 9 rows: 6-5-6-5-6-5-6-5-6
  const generateStars = () => {
    const stars: JSX.Element[] = [];
    const starRows = [6, 5, 6, 5, 6, 5, 6, 5, 6];
    const starSize = Math.min(cantonWidth, cantonHeight) * 0.025;
    
    starRows.forEach((count, row) => {
      const rowY = y + (row + 0.5) * (cantonHeight / 9);
      const offset = count === 5 ? cantonWidth / 12 : 0;
      
      for (let col = 0; col < count; col++) {
        const colX = x + offset + (col * cantonWidth / 6) + (count === 6 ? cantonWidth / 12 : 0);
        
        // Create proper 5-pointed star using path
        const points: string[] = [];
        for (let i = 0; i < 10; i++) {
          const angle = (i * Math.PI) / 5 - Math.PI / 2;
          const radius = i % 2 === 0 ? starSize : starSize * 0.38;
          points.push(`${colX + Math.cos(angle) * radius},${rowY + Math.sin(angle) * radius}`);
        }
        
        stars.push(
          <polygon
            key={`star-${row}-${col}`}
            points={points.join(" ")}
            fill="#FFFFFF"
            stroke="none"
          />
        );
      }
    });
    return stars;
  };

  return (
    <g>
      {/* Flag border/shadow to make it look like a real flag */}
      <rect
        x={x - 2}
        y={y - 2}
        width={width + 4}
        height={height + 4}
        fill="rgba(0,0,0,0.1)"
        rx="2"
      />
      <rect
        x={x - 1}
        y={y - 1}
        width={width + 2}
        height={height + 2}
        fill="none"
        stroke="rgba(0,0,0,0.2)"
        strokeWidth="1"
        rx="1"
      />
      
      {/* 13 alternating red and white stripes - red on top */}
      {Array.from({ length: 13 }, (_, i) => (
        <rect
          key={`stripe-${i}`}
          x={x}
          y={y + i * stripeHeight}
          width={width}
          height={stripeHeight}
          fill={i % 2 === 0 ? "#B22234" : "#FFFFFF"}
        />
      ))}
      
      {/* Blue canton - covers first 7 stripes */}
      <rect
        x={x}
        y={y}
        width={cantonWidth}
        height={cantonHeight}
        fill="#3C3B6E"
      />
      
      {/* 50 white stars on canton */}
      {generateStars()}
    </g>
  );
};

interface PortugueseFlagProps {
  x: number;
  y: number;
  width: number;
  height: number;
}

export const PortugueseFlag: React.FC<PortugueseFlagProps> = ({ x, y, width, height }) => {
  const greenWidth = width * 0.4; // 40% green on left
  const redWidth = width * 0.6; // 60% red on right
  
  // Coat of arms positioned on the boundary between green and red
  const coatOfArmsCenterX = x + greenWidth;
  const coatOfArmsCenterY = y + height / 2;
  const coatOfArmsSize = Math.min(width, height) * 0.35;

  return (
    <g>
      {/* Flag border/shadow to make it look like a real flag */}
      <rect
        x={x - 2}
        y={y - 2}
        width={width + 4}
        height={height + 4}
        fill="rgba(0,0,0,0.1)"
        rx="2"
      />
      <rect
        x={x - 1}
        y={y - 1}
        width={width + 2}
        height={height + 2}
        fill="none"
        stroke="rgba(0,0,0,0.2)"
        strokeWidth="1"
        rx="1"
      />
      
      {/* Green section (left 40%) */}
      <rect
        x={x}
        y={y}
        width={greenWidth}
        height={height}
        fill="#006600"
      />
      
      {/* Red section (right 60%) */}
      <rect
        x={x + greenWidth}
        y={y}
        width={redWidth}
        height={height}
        fill="#FF0000"
      />
      
      {/* Coat of Arms - positioned on the boundary */}
      <g transform={`translate(${coatOfArmsCenterX}, ${coatOfArmsCenterY})`}>
        {/* Shield shape - more detailed */}
        <path
          d={`M 0,-${coatOfArmsSize * 0.35} 
              L -${coatOfArmsSize * 0.25},-${coatOfArmsSize * 0.15} 
              L -${coatOfArmsSize * 0.25},${coatOfArmsSize * 0.25} 
              Q -${coatOfArmsSize * 0.25},${coatOfArmsSize * 0.35} 0,${coatOfArmsSize * 0.4} 
              Q ${coatOfArmsSize * 0.25},${coatOfArmsSize * 0.35} ${coatOfArmsSize * 0.25},${coatOfArmsSize * 0.25} 
              L ${coatOfArmsSize * 0.25},-${coatOfArmsSize * 0.15} Z`}
          fill="#FFFFFF"
          stroke="#FFD700"
          strokeWidth={coatOfArmsSize * 0.02}
        />
        
        {/* Castle towers (simplified but recognizable) */}
        <rect 
          x={-coatOfArmsSize * 0.15} 
          y={-coatOfArmsSize * 0.05} 
          width={coatOfArmsSize * 0.08} 
          height={coatOfArmsSize * 0.12} 
          fill="#006600" 
        />
        <rect 
          x={coatOfArmsSize * 0.07} 
          y={-coatOfArmsSize * 0.05} 
          width={coatOfArmsSize * 0.08} 
          height={coatOfArmsSize * 0.12} 
          fill="#006600" 
        />
        <rect 
          x={-coatOfArmsSize * 0.03} 
          y={-coatOfArmsSize * 0.08} 
          width={coatOfArmsSize * 0.06} 
          height={coatOfArmsSize * 0.06} 
          fill="#006600" 
        />
        
        {/* Armillary sphere - more detailed */}
        <circle 
          cx={0} 
          cy={coatOfArmsSize * 0.1} 
          r={coatOfArmsSize * 0.06} 
          fill="none" 
          stroke="#FFD700" 
          strokeWidth={coatOfArmsSize * 0.015}
        />
        {/* Vertical meridian */}
        <line 
          x1={0} 
          y1={coatOfArmsSize * 0.04} 
          x2={0} 
          y2={coatOfArmsSize * 0.16} 
          stroke="#FFD700" 
          strokeWidth={coatOfArmsSize * 0.01}
        />
        {/* Horizontal meridian */}
        <line 
          x1={-coatOfArmsSize * 0.06} 
          y1={coatOfArmsSize * 0.1} 
          x2={coatOfArmsSize * 0.06} 
          y2={coatOfArmsSize * 0.1} 
          stroke="#FFD700" 
          strokeWidth={coatOfArmsSize * 0.01}
        />
        {/* Inner circle */}
        <circle 
          cx={0} 
          cy={coatOfArmsSize * 0.1} 
          r={coatOfArmsSize * 0.03} 
          fill="none" 
          stroke="#FFD700" 
          strokeWidth={coatOfArmsSize * 0.008}
        />
      </g>
    </g>
  );
};
