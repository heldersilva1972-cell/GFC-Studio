// [NEW]
import { MetadataRoute } from 'next'

export default function sitemap(): MetadataRoute.Sitemap {
  return [
    {
      url: 'https://gfc-website.vercel.app',
      lastModified: new Date(),
      changeFrequency: 'yearly',
      priority: 1,
    },
    {
      url: 'https://gfc-website.vercel.app/hall-rentals',
      lastModified: new Date(),
      changeFrequency: 'monthly',
      priority: 0.8,
    },
    {
      url: 'https://gfc-website.vercel.app/events',
      lastModified: new Date(),
      changeFrequency: 'weekly',
      priority: 0.5,
    },
    {
        url: 'https://gfc-website.vercel.app/membership',
        lastModified: new Date(),
        changeFrequency: 'monthly',
        priority: 0.8,
    },
    {
        url: 'https://gfc-website.vercel.app/gallery',
        lastModified: new Date(),
        changeFrequency: 'monthly',
        priority: 0.8,
    },
    {
        url: 'https://gfc-website.vercel.app/contact',
        lastModified: new Date(),
        changeFrequency: 'yearly',
        priority: 0.5,
    },
    {
        url: 'https://gfc-website.vercel.app/privacy-policy',
        lastModified: new Date(),
        changeFrequency: 'yearly',
        priority: 0.3,
    },
    {
        url: 'https://gfc-website.vercel.app/terms-of-service',
        lastModified: new Date(),
        changeFrequency: 'yearly',
        priority: 0.3,
    },
    {
        url: 'https://gfc-website.vercel.app/hall-rental-rules',
        lastModified: new Date(),
        changeFrequency: 'yearly',
        priority: 0.3,
    },
  ]
}
