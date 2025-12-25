const fs = require('fs');
const path = require('path');

const oldPath = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview', '[pageId]');
const newPath = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview', 'pageId_legacy');

try {
    if (fs.existsSync(oldPath)) {
        fs.renameSync(oldPath, newPath);
        console.log('Renamed to:', newPath);
    } else {
        console.log('Old path not found');
    }
} catch (e) {
    console.error('Error renaming:', e);
}
