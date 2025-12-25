const fs = require('fs');
const path = require('path');

const targetPath = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview', '[pageId]');
console.log('Attempting to delete:', targetPath);

try {
    if (fs.existsSync(targetPath)) {
        console.log('Path exists. Deleting...');
        fs.rmSync(targetPath, { recursive: true, force: true });
        // Check if it's still there
        if (fs.existsSync(targetPath)) {
            console.log('Deletion reported success but path still exists!');
        } else {
            console.log('Successfully deleted:', targetPath);
        }
    } else {
        console.log('Path not found:', targetPath);
        // List parent dir to see what's there
        const parentDir = path.dirname(targetPath);
        console.log('Contents of parent dir:', parentDir);
        fs.readdirSync(parentDir).forEach(file => {
            console.log(file);
        });
    }
} catch (e) {
    console.error('Error deleting path:', e);
}
