// [MODIFIED]
import AnimationRenderer from '@/components/AnimationRenderer';
import { getAnimationById, getHallRentalPageContent } from '@/app/lib/api';
import HallRentalClientPage from './client-page';


const HallRentalPage = async () => {
  const animation = await getAnimationById('hall-rental-showcase');
  const content = await getHallRentalPageContent();

  return (
    <>
      {/* <AnimationRenderer animation={animation} content={content} /> */}
      <HallRentalClientPage />
    </>
  );
};

export default HallRentalPage;
