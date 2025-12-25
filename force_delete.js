const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

const targetPath = path.join(__dirname, 'apps', 'website', 'app', 'studio-preview', '[pageId]');

console.log('Target:', targetPath);

try {
    // Try node fs removal
    fs.rmSync(targetPath, { recursive: true, force: true });
    console.log('fs.rmSync success');
} catch (e) {
    console.log('fs.rmSync failed:', e.message);
    try {
        // Try shell removal
        execSync(`rmdir /s /q "${targetPath}"`);
        console.log('shell rmdir success');
    } catch (e2) {
        console.log('shell rmdir failed:', e2.message);
    }
}
