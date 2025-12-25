const fs = require('fs');
const path = require('path');

const targetPath = path.join(__dirname, 'apps/website/app/studio-preview/[pageId]');

try {
    if (fs.existsSync(targetPath)) {
        fs.rmSync(targetPath, { recursive: true, force: true });
        console.log('Successfully deleted:', targetPath);
    } else {
        console.log('Path not found:', targetPath);
    }
} catch (e) {
    console.error('Error deleting path:', e);
}
