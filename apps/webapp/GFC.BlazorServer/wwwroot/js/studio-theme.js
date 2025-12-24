// [MODIFIED]
function postThemeUpdate(tokens) {
    const iframe = document.querySelector('.preview-iframe');
    if (iframe) {
        const message = {
            type: 'THEME_UPDATE',
            payload: tokens
        };
        iframe.contentWindow.postMessage(message, 'http://localhost:3000');
    }
}
