const fs = require('fs');
const path = require('path');

const dirPath = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview', '[pageId]');
const oldFile = path.join(dirPath, 'page.tsx');
const newFile = path.join(dirPath, '_page.tsx');

try {
    if (fs.existsSync(oldFile)) {
        fs.renameSync(oldFile, newFile);
        console.log('Renamed file to:', newFile);
    } else {
        console.log('File not found:', oldFile);
    }
} catch (e) {
    console.error('Error renaming file:', e);
}
