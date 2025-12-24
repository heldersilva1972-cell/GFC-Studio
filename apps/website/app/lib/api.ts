// [NEW]
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export async function getPageBySlug(slug: string) {
  const res = await fetch(`${API_URL}/api/content/page/${slug}`);
  if (!res.ok) {
    // This will activate the closest `error.js` Error Boundary
    throw new Error('Failed to fetch page data');
  }
  return res.json();
}
