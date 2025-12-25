const fs = require('fs');
const path = require('path');

const targetDir = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview', '[slug]');

try {
    if (fs.existsSync(targetDir)) {
        fs.rmSync(targetDir, { recursive: true, force: true });
        console.log('Successfully deleted:', targetDir);
    } else {
        console.log('Directory not found:', targetDir);
    }
} catch (e) {
    console.error('Error deleting directory:', e);
}
