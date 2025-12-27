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
  const response = await fetch(`${API_URL}/api/WebsiteData/EventPromotions`);
  if (!response.ok) {
    console.error("Failed to fetch events");
    return [];
  }
  const data = await response.json();
  return data;
}

export async function getUnavailableDates(): Promise<{ date: Date; status: string; eventType?: string; eventTime?: string }[]> {
  const response = await fetch(`${API_URL}/api/Availability`);
  const data = await response.json();

  // API returns objects but C# might capitalize properties: { Date, Status } or { date, status }
  return data.map((item: any) => {
    const dateValue = item.Date || item.date;
    const statusValue = item.Status || item.status;
    const eventTypeValue = item.EventType || item.eventType;
    const eventTimeValue = item.EventTime || item.eventTime;
    return {
      date: new Date(dateValue),
      status: statusValue,
      eventType: eventTypeValue,
      eventTime: eventTimeValue
    };
  });
}

export async function getAnimationById(id: string) {
    const res = await fetch(`${API_URL}/api/animations/${id}`, { next: { revalidate: 60 } }); // Revalidate every 60 seconds
    if (!res.ok) {
        throw new Error(`Failed to fetch animation: ${res.statusText}`);
    }
    return res.json();
}

export async function getHomePageContent() {
    // In a real application, this would fetch data from a CMS.
    // For now, we'll return the same hardcoded content that was in the AnimationRenderer.
    return Promise.resolve({
        title: "Building Community, Friendship, and Tradition",
        subtitle: "Welcome to the Gloucester Fraternity Club — a place where friendship, family, and community come together. Since our founding in the early 1920s, we've proudly served the Cape Ann area through fellowship, fun, and service.",
        primaryCtaText: "Rent Our Hall",
        primaryCtaLink: "/hall-rentals",
        secondaryCtaText: "View Events",
        secondaryCtaLink: "/events",
        backgroundImage: "/images/hero-bg.jpg",
        stats: [
            { label: "Years of Service", value: "100+" },
            { label: "Active Members", value: "500+" },
            { label: "Events Yearly", value: "50+" }
        ]
    });
}

export async function getHallRentalPageContent() {
    return Promise.resolve({
        title: "Host Your Next Event With Us",
        subtitle: "Our spacious and versatile hall is the perfect venue for weddings, parties, and corporate events. With a capacity of 180 guests, a full-service bar, and a stage for live entertainment, we have everything you need to make your event a success.",
        backgroundImage: "/images/hall-rental-hero.jpg",
    });
}
