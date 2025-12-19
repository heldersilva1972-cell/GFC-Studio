/**
 * Shared utilities for creating realistic flag patterns
 */

export const createUSFlagPatterns = (flagWidth: number, flagHeight: number) => {
  const stripeHeight = flagHeight / 13;
  const cantonWidth = flagWidth * 0.3846;
  const cantonHeight = flagHeight * 0.5385;

  // Generate 50 stars in 9 rows: 6-5-6-5-6-5-6-5-6
  const generateStars = () => {
    const stars: JSX.Element[] = [];
    const starRows = [6, 5, 6, 5, 6, 5, 6, 5, 6];
    starRows.forEach((count, row) => {
      const offset = count === 5 ? cantonWidth / 12 : 0;
      for (let col = 0; col < count; col++) {
        const x = offset + (col * cantonWidth / 6) + (count === 6 ? cantonWidth / 12 : 0);
        const y = (row + 1) * cantonHeight / 10;
        stars.push(
          <circle key={`star-${row}-${col}`} cx={x} cy={y} r={cantonWidth * 0.012} fill="#FFFFFF" />
        );
      }
    });
    return stars;
  };

  return {
    stripeHeight,
    cantonWidth,
    cantonHeight,
    generateStars,
  };
};

export const createPortugueseFlagPatterns = (flagWidth: number, flagHeight: number) => {
  return {
    coatOfArms: (
      <g>
        <path
          d="M 50,25 L 35,35 L 35,65 Q 35,72 50,75 Q 65,72 65,65 L 65,35 Z"
          fill="#FFFFFF"
          stroke="#FFD700"
          strokeWidth="1.5"
        />
        <rect x="40" y="40" width="6" height="8" fill="#006600" />
        <rect x="54" y="40" width="6" height="8" fill="#006600" />
        <rect x="47" y="37" width="6" height="6" fill="#006600" />
        <circle cx="50" cy="55" r="4" fill="none" stroke="#FFD700" strokeWidth="1.5" />
        <line x1="50" y1="51" x2="50" y2="59" stroke="#FFD700" strokeWidth="1" />
        <line x1="46" y1="55" x2="54" y2="55" stroke="#FFD700" strokeWidth="1" />
      </g>
    ),
  };
};

