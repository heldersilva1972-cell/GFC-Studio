// [NEW]
function initializeHlsPlayer(videoElementId, hlsUrl) {
    const video = document.getElementById(videoElementId);
    if (Hls.isSupported()) {
        const hls = new Hls();
        hls.loadSource(hlsUrl);
        hls.attachMedia(video);
        hls.on(Hls.Events.MANIFEST_PARSED, function () {
            video.play();
        });
    } else if (video.canPlayType('application/vnd.apple.mpegurl')) {
        video.src = hlsUrl;
        video.addEventListener('loadedmetadata', function () {
            video.play();
        });
    }
}

function setPlaybackSpeed(videoElementId, speed) {
    const video = document.getElementById(videoElementId);
    video.playbackRate = speed;
}

function seekVideo(videoElementId, percentage) {
    const video = document.getElementById(videoElementId);
    video.currentTime = video.duration * (percentage / 100);
}

function captureVideoSnapshot(videoElementId, fileName) {
    const video = document.getElementById(videoElementId);
    if (!video) return;

    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    const dataUrl = canvas.toDataURL('image/jpeg');
    const link = document.createElement('a');
    link.href = dataUrl;
    link.download = fileName || 'snapshot.jpg';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function toggleFullscreen(videoElementId) {
    const video = document.getElementById(videoElementId);
    if (!video) return;

    if (!document.fullscreenElement) {
        if (video.requestFullscreen) {
            video.requestFullscreen();
        } else if (video.webkitRequestFullscreen) { /* Safari */
            video.webkitRequestFullscreen();
        } else if (video.msRequestFullscreen) { /* IE11 */
            video.msRequestFullscreen();
        }
    } else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        }
    }
}
