<!-- [NEW] -->
# Troubleshooting Guide

This guide provides solutions to common issues that may arise with the GFC Video Agent.

## Video Stream Not Playing

-   **Check the Video Agent logs:** The logs in `apps/services/GFC.VideoAgent` will contain information about any errors.
-   **Verify FFmpeg is installed:** Ensure that FFmpeg is installed and accessible in the system's PATH.
-   **Check the NVR credentials:** Make sure the NVR credentials in `appsettings.json` are correct.
-   **Verify the RTSP path:** Ensure that the RTSP path for each camera is correct.
-   **Check the firewall:** The firewall on the server running the Video Agent must allow traffic on the port specified in `appsettings.json`.
-   **Check the browser console:** Look for any errors in the browser's developer console.

## "OFFLINE" Status

-   **RTSP Connection Failure:** This is often caused by incorrect credentials, an invalid RTSP path, or a network issue.
-   **FFmpeg Crash:** The Video Agent will automatically restart the FFmpeg process. Check the logs for more information.
-   **Disk Space Full:** Ensure that there is enough disk space in the `OutputDirectory` for the HLS segments.

## "BUFFERING" Status

-   **Network Interruption:** This can be caused by a poor network connection between the NVR and the Video Agent, or between the Video Agent and the client.
-   **High CPU Usage:** If the server running the Video Agent is under heavy load, it may not be able to process the video stream in real-time.
