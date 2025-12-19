/**
 * Realistic flag patterns for US and Portuguese flags
 * These patterns are used across all flag animation components
 */

export const USFlagPattern = () => (
  <>
    <defs>
      {/* US Flag - 13 alternating red and white stripes */}
      <pattern id="usFlagStripes" x="0" y="0" width="100%" height="100%" patternUnits="userSpaceOnUse">
        {/* Red stripes */}
        <rect width="100%" height="7.69%" y="0%" fill="#B22234" />
        <rect width="100%" height="7.69%" y="15.38%" fill="#B22234" />
        <rect width="100%" height="7.69%" y="30.77%" fill="#B22234" />
        <rect width="100%" height="7.69%" y="46.15%" fill="#B22234" />
        <rect width="100%" height="7.69%" y="61.54%" fill="#B22234" />
        <rect width="100%" height="7.69%" y="76.92%" fill="#B22234" />
        <rect width="100%" height="7.69%" y="92.31%" fill="#B22234" />
        {/* White stripes (background is white by default) */}
      </pattern>
      
      {/* US Flag - Blue canton with stars */}
      <pattern id="usFlagCanton" x="0" y="0" width="38.46%" height="53.85%" patternUnits="userSpaceOnUse">
        <rect width="100%" height="100%" fill="#3C3B6E" />
        {/* 50 stars arranged in 9 rows: 6-5-6-5-6-5-6-5-6 */}
        {/* Row 1: 6 stars */}
        <circle cx="8.33%" cy="11.11%" r="2.5%" fill="#FFFFFF" />
        <circle cx="25%" cy="11.11%" r="2.5%" fill="#FFFFFF" />
        <circle cx="41.67%" cy="11.11%" r="2.5%" fill="#FFFFFF" />
        <circle cx="58.33%" cy="11.11%" r="2.5%" fill="#FFFFFF" />
        <circle cx="75%" cy="11.11%" r="2.5%" fill="#FFFFFF" />
        <circle cx="91.67%" cy="11.11%" r="2.5%" fill="#FFFFFF" />
        {/* Row 2: 5 stars */}
        <circle cx="16.67%" cy="22.22%" r="2.5%" fill="#FFFFFF" />
        <circle cx="33.33%" cy="22.22%" r="2.5%" fill="#FFFFFF" />
        <circle cx="50%" cy="22.22%" r="2.5%" fill="#FFFFFF" />
        <circle cx="66.67%" cy="22.22%" r="2.5%" fill="#FFFFFF" />
        <circle cx="83.33%" cy="22.22%" r="2.5%" fill="#FFFFFF" />
        {/* Row 3: 6 stars */}
        <circle cx="8.33%" cy="33.33%" r="2.5%" fill="#FFFFFF" />
        <circle cx="25%" cy="33.33%" r="2.5%" fill="#FFFFFF" />
        <circle cx="41.67%" cy="33.33%" r="2.5%" fill="#FFFFFF" />
        <circle cx="58.33%" cy="33.33%" r="2.5%" fill="#FFFFFF" />
        <circle cx="75%" cy="33.33%" r="2.5%" fill="#FFFFFF" />
        <circle cx="91.67%" cy="33.33%" r="2.5%" fill="#FFFFFF" />
        {/* Row 4: 5 stars */}
        <circle cx="16.67%" cy="44.44%" r="2.5%" fill="#FFFFFF" />
        <circle cx="33.33%" cy="44.44%" r="2.5%" fill="#FFFFFF" />
        <circle cx="50%" cy="44.44%" r="2.5%" fill="#FFFFFF" />
        <circle cx="66.67%" cy="44.44%" r="2.5%" fill="#FFFFFF" />
        <circle cx="83.33%" cy="44.44%" r="2.5%" fill="#FFFFFF" />
        {/* Row 5: 6 stars */}
        <circle cx="8.33%" cy="55.56%" r="2.5%" fill="#FFFFFF" />
        <circle cx="25%" cy="55.56%" r="2.5%" fill="#FFFFFF" />
        <circle cx="41.67%" cy="55.56%" r="2.5%" fill="#FFFFFF" />
        <circle cx="58.33%" cy="55.56%" r="2.5%" fill="#FFFFFF" />
        <circle cx="75%" cy="55.56%" r="2.5%" fill="#FFFFFF" />
        <circle cx="91.67%" cy="55.56%" r="2.5%" fill="#FFFFFF" />
        {/* Row 6: 5 stars */}
        <circle cx="16.67%" cy="66.67%" r="2.5%" fill="#FFFFFF" />
        <circle cx="33.33%" cy="66.67%" r="2.5%" fill="#FFFFFF" />
        <circle cx="50%" cy="66.67%" r="2.5%" fill="#FFFFFF" />
        <circle cx="66.67%" cy="66.67%" r="2.5%" fill="#FFFFFF" />
        <circle cx="83.33%" cy="66.67%" r="2.5%" fill="#FFFFFF" />
        {/* Row 7: 6 stars */}
        <circle cx="8.33%" cy="77.78%" r="2.5%" fill="#FFFFFF" />
        <circle cx="25%" cy="77.78%" r="2.5%" fill="#FFFFFF" />
        <circle cx="41.67%" cy="77.78%" r="2.5%" fill="#FFFFFF" />
        <circle cx="58.33%" cy="77.78%" r="2.5%" fill="#FFFFFF" />
        <circle cx="75%" cy="77.78%" r="2.5%" fill="#FFFFFF" />
        <circle cx="91.67%" cy="77.78%" r="2.5%" fill="#FFFFFF" />
        {/* Row 8: 5 stars */}
        <circle cx="16.67%" cy="88.89%" r="2.5%" fill="#FFFFFF" />
        <circle cx="33.33%" cy="88.89%" r="2.5%" fill="#FFFFFF" />
        <circle cx="50%" cy="88.89%" r="2.5%" fill="#FFFFFF" />
        <circle cx="66.67%" cy="88.89%" r="2.5%" fill="#FFFFFF" />
        <circle cx="83.33%" cy="88.89%" r="2.5%" fill="#FFFFFF" />
        {/* Row 9: 6 stars */}
        <circle cx="8.33%" cy="100%" r="2.5%" fill="#FFFFFF" />
        <circle cx="25%" cy="100%" r="2.5%" fill="#FFFFFF" />
        <circle cx="41.67%" cy="100%" r="2.5%" fill="#FFFFFF" />
        <circle cx="58.33%" cy="100%" r="2.5%" fill="#FFFFFF" />
        <circle cx="75%" cy="100%" r="2.5%" fill="#FFFFFF" />
        <circle cx="91.67%" cy="100%" r="2.5%" fill="#FFFFFF" />
      </pattern>
    </defs>
  </>
);

export const PortugueseFlagPattern = () => (
  <>
    <defs>
      {/* Portuguese Flag - Green and Red vertical halves */}
      <pattern id="ptFlagBase" x="0" y="0" width="100%" height="100%" patternUnits="userSpaceOnUse">
        <rect width="50%" height="100%" fill="#006600" />
        <rect x="50%" width="50%" height="100%" fill="#FF0000" />
      </pattern>
      
      {/* Portuguese Coat of Arms - Simplified but recognizable */}
      <g id="ptCoatOfArms">
        {/* Shield shape */}
        <path
          d="M 50,20 L 35,30 L 35,60 Q 35,70 50,75 Q 65,70 65,60 L 65,30 Z"
          fill="#FFFFFF"
          stroke="#FFD700"
          strokeWidth="1.5"
        />
        {/* Inner shield divisions */}
        <line x1="50" y1="30" x2="50" y2="60" stroke="#FFD700" strokeWidth="1" />
        <line x1="35" y1="45" x2="65" y2="45" stroke="#FFD700" strokeWidth="1" />
        {/* Simplified castle towers */}
        <rect x="40" y="35" width="6" height="8" fill="#006600" />
        <rect x="54" y="35" width="6" height="8" fill="#006600" />
        <rect x="47" y="32" width="6" height="6" fill="#006600" />
        {/* Simplified armillary sphere */}
        <circle cx="50" cy="55" r="4" fill="none" stroke="#FFD700" strokeWidth="1" />
        <line x1="50" y1="51" x2="50" y2="59" stroke="#FFD700" strokeWidth="0.8" />
        <line x1="46" y1="55" x2="54" y2="55" stroke="#FFD700" strokeWidth="0.8" />
      </g>
    </defs>
  </>
);

