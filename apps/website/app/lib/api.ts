// [MODIFIED]
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5207';

export async function getPageBySlug(slug: string) {
  // This function is mocked to allow frontend rendering without a live backend
  console.log(`Mocking API call for slug: ${slug}`);
  const mockData = {
    title: 'GFC - Home',
    metaDescription: 'Welcome to the Gloucester Fraternity Club website.',
    ogImageUrl: '/default-og-image.png',
    sections: [
      {
        id: 'clxjy2q4p000108l8g2j3e4f5',
        clientId: 'client-hero-1',
        sectionType: 'Hero',
        properties: {
          headline: 'Gloucester Fraternity Club',
          backgroundImage: 'https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?q=80&w=2000&auto=format&fit=crop',
        },
        animationSettingsJson: JSON.stringify({ Effect: 'SlideUp', Duration: 1, Delay: 0.2 }),
        sortOrder: 1,
      },
      {
        id: 'clxjy3a9b000208l8h4f5k6g7',
        clientId: 'client-feature-1',
        sectionType: 'FeatureGrid',
        properties: {},
        animationSettingsJson: JSON.stringify({ Effect: 'FadeIn', Duration: 1, Delay: 0.4 }),
        sortOrder: 2,
      },
    ],
  };
  return Promise.resolve(mockData);
}

export async function getEvents() {
  console.log('Mocking API call for /api/content/events');
  const mockEvents = [
    { title: 'Community BBQ', eventDate: '2025-07-20T12:00:00Z', description: 'Join us for our annual summer BBQ.' },
    { title: 'Holiday Party', eventDate: '2025-12-15T18:00:00Z', description: 'Celebrate the holidays with the community.' },
  ];
  return Promise.resolve(mockEvents);
}

export async function getUnavailableDates(): Promise<Date[]> {
    const response = await fetch(`${API_URL}/api/Availability`);
    const dates = await response.json();
    return dates.map((dateString: string) => new Date(dateString));
}
