<!-- [NEW] -->
# Video Agent Setup Guide

This guide provides instructions for setting up and configuring the GFC Video Agent.

## Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [FFmpeg](https://ffmpeg.org/download.html) (must be in the system's PATH)
-   NVR with RTSP streams enabled

## Installation

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    ```
2.  **Navigate to the Video Agent directory:**
    ```bash
    cd apps/services/GFC.VideoAgent
    ```
3.  **Install dependencies:**
    ```bash
    dotnet restore
    ```

## Configuration

The Video Agent is configured through `appsettings.json`.

-   **`VideoAgent:OutputDirectory`**: The directory where HLS files (`.m3u8` and `.ts`) will be stored.
-   **`VideoAgent:ListenPort`**: The port the Video Agent will listen on.
-   **`VideoAgent:FFmpegPath`**: The path to the FFmpeg executable. Defaults to `ffmpeg`.
-   **`VideoAgent:AllowedOrigins`**: An array of URLs that are allowed to make requests to the service (CORS).
-   **`NvrSettings`**: Configuration for the NVR.
    -   **`IpAddress`**: The IP address of the NVR.
    -   **`RtspPort`**: The RTSP port of the NVR.
    -   **`Username`**: The username for the NVR.
    -   **`Password`**: The password for the NVR.
    -   **`Cameras`**: An array of camera configurations.
        -   **`Id`**: A unique ID for the camera.
        -   **`Name`**: A descriptive name for the camera.
        -   **`RtspPath`**: The RTSP path for the camera stream.
        -   **`Enabled`**: Whether the stream for this camera should be started.

## Running the Service

```bash
dotnet run
```

The service will start and begin streaming from the enabled cameras.

## Web App Configuration

The web app also needs to be configured to connect to the Video Agent. In `apps/webapp/GFC.BlazorServer/appsettings.json`, add the following:

```json
"VideoAgent": {
  "BaseUrl": "https://localhost:5101"
}
```

Replace `https://localhost:5101` with the URL of the Video Agent.
