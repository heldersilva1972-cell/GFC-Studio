// [NEW]
// This script sets up a simple Express server that acts as a worker process
// for scraping web pages using Playwright. It's designed to be called by the
// main Blazor application to get the fully rendered HTML of a given URL.

const express = require('express');
const { chromium } = require('playwright');

const app = express();
const port = 3001; // Port for the scraper worker

app.use(express.json());

// The main endpoint for scraping. It expects a JSON body with a 'url' property.
app.post('/scrape', async (req, res) => {
    const { url } = req.body;

    if (!url) {
        return res.status(400).send({ error: 'URL is required' });
    }

    let browser = null;
    try {
        // Launch a new headless Chromium browser instance.
        browser = await chromium.launch();
        const context = await browser.newContext();
        const page = await context.newPage();

        // Navigate to the URL and wait until the network is idle, which indicates
        // that the page has likely finished loading and rendering.
        await page.goto(url, { waitUntil: 'networkidle' });

        // Get the full HTML content of the page.
        const content = await page.content();

        res.send({ html: content });
    } catch (error) {
        console.error('Error scraping URL:', error);
        res.status(500).send({ error: 'Failed to scrape URL' });
    } finally {
        // Ensure the browser is always closed, even if an error occurs.
        if (browser) {
            await browser.close();
        }
    }
});

app.listen(port, () => {
    console.log(`Scraper worker listening at http://localhost:${port}`);
});
