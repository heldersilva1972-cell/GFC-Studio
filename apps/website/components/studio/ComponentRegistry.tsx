// [NEW]
import React from 'react';
import HeroSection from './HeroSection';
import TextBlock from './TextBlock';
import Image from './Image';
import Button from './Button';

const ComponentRegistry: { [key: string]: React.ComponentType<any> } = {
    HeroSection,
    TextBlock,
    Image,
    Button,
};

export default ComponentRegistry;
