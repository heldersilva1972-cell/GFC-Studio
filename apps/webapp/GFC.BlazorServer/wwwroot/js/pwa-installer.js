window.GFC = window.GFC || {};

(function () {
    let deferredPrompt;

    window.addEventListener('beforeinstallprompt', (e) => {
        // Prevent the mini-infobar from appearing on mobile
        e.preventDefault();
        // Stash the event so it can be triggered later.
        deferredPrompt = e;
        console.log("PWA: beforeinstallprompt fired and captured.");
        
        // Notify Blazor that install is available (optional, can be done via dotnet interop)
        // window.DotNet.invokeMethodAsync('GFC.BlazorServer', 'OnInstallable');
    });

    window.GFC.triggerInstall = async () => {
        if (!deferredPrompt) {
            console.log("PWA: No install prompt available.");
            return false;
        }

        // Show the install prompt
        deferredPrompt.prompt();

        // Wait for the user to respond to the prompt
        const { outcome } = await deferredPrompt.userChoice;
        console.log(`PWA: User response to install prompt: ${outcome}`);

        // We've used the prompt, and can't use it again, throw it away
        deferredPrompt = null;
        return outcome === 'accepted';
    };

    window.GFC.isPwaInstalled = () => {
        // Check if running in standalone mode (installed)
        const isStandalone = window.matchMedia('(display-mode: standalone)').matches || 
                             window.navigator.standalone === true; 
        return isStandalone;
    };
    
    window.GFC.canInstall = () => {
        return !!deferredPrompt;
    };
    
    // Check for iOS
    window.GFC.isIOS = () => {
        return /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
    };

})();
