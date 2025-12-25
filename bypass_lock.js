const fs = require('fs');
const path = require('path');

const baseDir = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview');
const oldDir = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview-old');
const newDir = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview-new');

try {
    // 1. Rename existing studio-preview to studio-preview-old
    if (fs.existsSync(baseDir)) {
        console.log('Renaming base dir to old dir...');
        fs.renameSync(baseDir, oldDir);
    } else {
        console.log('Base dir not found, checking if old dir exists...');
        if (!fs.existsSync(oldDir)) {
            throw new Error('Neither base nor old dir found');
        }
    }

    // 2. Create new studio-preview directory
    if (!fs.existsSync(baseDir)) { // It shouldn't exist after rename
        console.log('Creating new base dir...');
        fs.mkdirSync(baseDir);
    }

    // 3. Copy [slug] content
    const sourceSlug = path.join(oldDir, '[slug]');
    const destSlug = path.join(baseDir, '[slug]');

    if (fs.existsSync(sourceSlug)) {
        console.log('Copying [slug]...');
        fs.cpSync(sourceSlug, destSlug, { recursive: true });
    } else {
        console.log('Warning: [slug] not found in old dir');
    }

    console.log('Migration complete. studio-preview-old contains the locked/conflicting file.');

} catch (e) {
    console.error('Error during forceful migration:', e);
}
