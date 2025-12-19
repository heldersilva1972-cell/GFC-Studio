/**
 * Animation Deletion Utility
 * 
 * This utility handles the deletion of animations from the registry and file system.
 * Note: File system operations require a server-side API endpoint in production.
 */

export interface DeleteAnimationResult {
  success: boolean;
  message: string;
  deletedId?: string;
}

/**
 * Delete an animation from the registry and file system
 * 
 * @param animationId - The ID of the animation to delete
 * @returns Promise with deletion result
 */
export async function deleteAnimation(
  animationId: string
): Promise<DeleteAnimationResult> {
  try {
    // Step 1: Store deleted animation ID in localStorage for client-side filtering
    const deletedIds = getDeletedAnimationIds();
    if (!deletedIds.includes(animationId)) {
      deletedIds.push(animationId);
      localStorage.setItem("deletedAnimationIds", JSON.stringify(deletedIds));
    }

    // Step 2: In a production environment, you would call a server API here:
    // const response = await fetch('/api/animations/delete', {
    //   method: 'POST',
    //   headers: { 'Content-Type': 'application/json' },
    //   body: JSON.stringify({ animationId }),
    // });
    // if (!response.ok) throw new Error('Failed to delete animation');

    return {
      success: true,
      message: `Animation "${animationId}" has been deleted`,
      deletedId: animationId,
    };
  } catch (error) {
    console.error("Error deleting animation:", error);
    return {
      success: false,
      message: error instanceof Error ? error.message : "Failed to delete animation",
    };
  }
}

/**
 * Get list of deleted animation IDs from localStorage
 */
export function getDeletedAnimationIds(): string[] {
  if (typeof window === "undefined") return [];
  try {
    const stored = localStorage.getItem("deletedAnimationIds");
    return stored ? JSON.parse(stored) : [];
  } catch {
    return [];
  }
}

/**
 * Restore a deleted animation (remove from deleted list)
 */
export function restoreAnimation(animationId: string): void {
  if (typeof window === "undefined") return;
  try {
    const deletedIds = getDeletedAnimationIds();
    const filtered = deletedIds.filter((id) => id !== animationId);
    localStorage.setItem("deletedAnimationIds", JSON.stringify(filtered));
  } catch (error) {
    console.error("Error restoring animation:", error);
  }
}

/**
 * Clear all deleted animations (restore all)
 */
export function clearDeletedAnimations(): void {
  if (typeof window === "undefined") return;
  try {
    localStorage.removeItem("deletedAnimationIds");
  } catch (error) {
    console.error("Error clearing deleted animations:", error);
  }
}

/**
 * Check if an animation is deleted
 */
export function isAnimationDeleted(animationId: string): boolean {
  const deletedIds = getDeletedAnimationIds();
  return deletedIds.includes(animationId);
}

