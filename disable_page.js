const fs = require('fs');
const path = require('path');

const targetDir = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview', '[pageId]');
const oldFile = path.join(targetDir, 'page.tsx');
const newFile = path.join(targetDir, '_page.tsx_disabled');

try {
    if (fs.existsSync(oldFile)) {
        fs.renameSync(oldFile, newFile);
        console.log('Successfully renamed to:', newFile);
    } else {
        console.log('File not found:', oldFile);
    }
} catch (e) {
    console.error('Error renaming:', e);
}
