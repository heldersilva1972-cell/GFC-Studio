import React from "react";
import StudioShell from "@/components/studio/layout/StudioShell";

export default function StudioLayout({ children }: { children: React.ReactNode }) {
  return <StudioShell>{children}</StudioShell>;
}


