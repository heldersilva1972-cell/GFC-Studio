// [MODIFIED]
import AnimationRenderer from '@/components/AnimationRenderer';
import { getAnimationById, getHallRentalPageContent } from '@/app/lib/api';
import HallRentalClientPage from './client-page';


const HallRentalPage = async () => {
  let animation;
  try {
    animation = await getAnimationById('hall-rental-showcase');
  } catch (error) {
    console.error("Failed to get animation, using fallback", error);
    animation = { "id": "hall-rental-showcase", "name": "Hall Rental Showcase", "keyframes": [] };
  }
  const content = await getHallRentalPageContent();

  return (
    <>
      <AnimationRenderer animation={animation} content={content} />
      <HallRentalClientPage />
    </>
  );
};

export default HallRentalPage;
