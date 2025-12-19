import PreviewWindow from "@/components/PreviewWindow";
import AnimationDemoLayout from "../components/AnimationDemoLayout";

export default function AnimationDesignerPage() {
  return (
    <AnimationDemoLayout title="Animation Designer">
      <p className="text-lg text-gray-600 mb-6">
        Create and customize animations with the interactive designer.
      </p>
      <div className="mt-8">
        <PreviewWindow />
      </div>
    </AnimationDemoLayout>
  );
}

