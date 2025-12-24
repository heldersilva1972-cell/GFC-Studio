// [MODIFIED]
const fs = require('fs');
const path = require('path');

async function generateSitemap() {
  const GFC_URL = "https://gfc-website-preview.vercel.app";

  // Hardcoded pages for now, as the API is not available at build time.
  const pages = [
    { slug: 'events', updatedAt: new Date().toISOString() },
    { slug: 'hall-rentals', updatedAt: new Date().toISOString() },
    { slug: 'membership', updatedAt: new Date().toISOString() },
    { slug: 'gallery', updatedAt: new Date().toISOString() },
    { slug: 'contact', updatedAt: new Date().toISOString() },
  ];

  const sitemap = `
    <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
      <url>
        <loc>${GFC_URL}</loc>
        <lastmod>${new Date().toISOString()}</lastmod>
        <changefreq>monthly</changefreq>
        <priority>1.0</priority>
      </url>
      ${pages
        .map((page) => {
          return `
            <url>
              <loc>${GFC_URL}/${page.slug}</loc>
              <lastmod>${new Date(page.updatedAt).toISOString()}</lastmod>
              <changefreq>weekly</changefreq>
              <priority>0.8</priority>
            </url>
          `;
        })
        .join('')}
    </urlset>
  `;

  const sitemapPath = path.join(__dirname, '../public/sitemap.xml');
  fs.writeFileSync(sitemapPath, sitemap.trim());
  console.log('Sitemap generated successfully!');
}

generateSitemap();
