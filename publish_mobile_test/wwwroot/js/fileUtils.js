// [NEW]
function downloadFileFromStream(fileName, contentStreamReference) {
    const a = document.createElement('a');
    document.body.appendChild(a);
    a.style = 'display: none';

    return contentStreamReference.arrayBuffer().then(buffer => {
        const blob = new Blob([buffer]);
        const url = window.URL.createObjectURL(blob);

        a.href = url;
        a.download = fileName;
        a.click();

        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
    });
}
