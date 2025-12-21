# GFC Video Agent

The GFC Video Agent is a .NET Core background service responsible for streaming live video from the NVR to the GFC web application. It uses FFmpeg to convert RTSP streams to HLS, which can be played in a web browser.

## Features

-   **RTSP to HLS Conversion:** Converts RTSP streams from the NVR to HLS for web playback.
-   **Health Monitoring:** Monitors the health of FFmpeg processes and automatically restarts them if they fail.
-   **Configuration:** All settings are managed through `appsettings.json`.
-   **CORS:** Allows requests from the GFC web application.

## Configuration

The service is configured through `appsettings.json`.

-   **VideoAgent:**
    -   `OutputDirectory`: The directory where HLS files (.m3u8 and .ts) are stored.
    -   `ListenPort`: The port the service listens on.
    -   `FFmpegPath`: The path to the FFmpeg executable.
    -   `AllowedOrigins`: An array of URLs that are allowed to make requests to the service.
-   **NvrSettings:**
    -   `IpAddress`: The IP address of the NVR.
    -   `RtspPort`: The RTSP port of the NVR.
    -   `Username`: The username for the NVR.
    -   `Password`: The password for the NVR.
    -   `Cameras`: An array of camera configurations.
        -   `Id`: A unique ID for the camera.
        -   `Name`: A descriptive name for the camera.
        -   `RtspPath`: The RTSP path for the camera stream.
        -   `Enabled`: Whether the stream for this camera should be started.

## Running the Service

1.  **Install FFmpeg:** Ensure that FFmpeg is installed and accessible in the system's PATH.
2.  **Configure `appsettings.json`:** Update the settings in `appsettings.json` to match your environment.
3.  **Run the service:**
    ```bash
    dotnet run
    ```

The service will start and begin streaming from the enabled cameras.
