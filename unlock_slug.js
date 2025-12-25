const fs = require('fs');
const path = require('path');

const file = path.join(__dirname, 'apps/website/app/studio-preview/[slug]/page.tsx');
console.log('Target:', file);

try {
    if (fs.existsSync(file)) {
        fs.unlinkSync(file);
        console.log('Deleted file:', file);
    } else {
        console.log('File not found');
    }
} catch (e) {
    console.log('Error:', e.message);
}
