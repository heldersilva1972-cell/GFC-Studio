/**
 * GFC Onboarding Gateway - Setup Script
 * Minimal, secure JavaScript for OS detection and token validation
 */

(function () {
    'use strict';

    // Configuration
    const CONFIG = {
        // API base URL - update this to point to your private GFC app
        apiBaseUrl: window.location.hostname === 'localhost'
            ? 'http://localhost:5000'
            : 'https://gfc.lovanow.com',

        // Rate limiting
        maxRetries: 3,
        retryDelay: 2000,

        // Timeouts
        validationTimeout: 10000,
        downloadTimeout: 15000,
    };

    // State
    let currentStep = 1;
    let token = null;
    let userId = null;
    let platform = null;
    let retryCount = 0;

    // Platform detection
    const PLATFORMS = {
        WINDOWS: {
            name: 'Windows',
            downloadUrl: 'https://download.wireguard.com/windows-client/wireguard-installer.exe',
            certInstructions: 'Open "GFC_Root_CA.cer", click "Install Certificate", select "Local Machine", and place in "Trusted Root Certification Authorities".',
            configInstructions: 'After downloading, open WireGuard and click "Import Tunnel(s) from File". Select the downloaded .conf file.',
            isMobile: false
        },
        MACOS: {
            name: 'macOS',
            downloadUrl: 'https://apps.apple.com/us/app/wireguard/id1451685025',
            certInstructions: 'Open the file, add to "System" keychain in Keychain Access, double-click it, and set "Always Trust" under the Trust section.',
            configInstructions: 'After downloading, open WireGuard and click "Import Tunnel(s) from File". Select the downloaded .conf file.',
            isMobile: false
        },
        IOS: {
            name: 'iOS',
            downloadUrl: 'https://apps.apple.com/us/app/wireguard/id1441195209',
            certInstructions: 'Install profile in Settings -> General -> VPN & Device Management. Then enable "Full Trust" in Settings -> General -> About -> Certificate Trust Settings.',
            configInstructions: 'When prompted, choose "Open with WireGuard" or find the file in your downloads and import it into the WireGuard app.',
            isMobile: true
        },
        ANDROID: {
            name: 'Android',
            downloadUrl: 'https://play.google.com/store/apps/details?id=com.wireguard.android',
            certInstructions: 'Go to Settings -> Security -> Advanced -> Encryption & Credentials -> Install from storage -> CA certificate and select the file.',
            configInstructions: 'When prompted, choose "Open with WireGuard" or find the file in your downloads and import it into the WireGuard app.',
            isMobile: true
        },
        LINUX: {
            name: 'Linux',
            downloadUrl: 'https://www.wireguard.com/install/',
            certInstructions: 'Copy to /usr/local/share/ca-certificates/ and run: sudo update-ca-certificates',
            configInstructions: 'After downloading the config, import it using: wg-quick up /path/to/gfc-access.conf',
            isMobile: false
        },
        UNKNOWN: {
            name: 'Unknown',
            downloadUrl: 'https://www.wireguard.com/install/',
            certInstructions: 'Please refer to your OS documentation for installing a Root CA certificate.',
            configInstructions: 'Please refer to WireGuard documentation for your platform.',
            isMobile: false
        }
    };

    // DOM Elements
    const elements = {
        loading: document.getElementById('loading'),
        errorInvalid: document.getElementById('error-invalid'),
        errorDisabled: document.getElementById('error-disabled'),
        wizard: document.getElementById('wizard'),
        platformName: document.getElementById('platform-name'),
        downloadLink: document.getElementById('download-link'),
        certInstructions: document.getElementById('cert-instructions'),
        platformInstructions: document.getElementById('platform-instructions'),
        nextStep1: document.getElementById('next-step-1'),
        downloadCert: document.getElementById('download-cert'),
        nextStep2: document.getElementById('next-step-2'),
        downloadConfig: document.getElementById('download-config'),
        skipToTest: document.getElementById('skip-to-test'),
        testConnection: document.getElementById('test-connection'),
        testResult: document.getElementById('test-result'),
        successActions: document.getElementById('success-actions'),
        windowsOneClick: document.getElementById('windows-one-click-container'),
        downloadWindowsSetup: document.getElementById('download-windows-setup'),
        appleOneClick: document.getElementById('apple-one-click-container'),
        downloadAppleProfile: document.getElementById('download-apple-profile'),
        year: document.getElementById('year')
    };

    /**
     * Detect user's operating system
     */
    function detectPlatform() {
        const userAgent = window.navigator.userAgent;
        const platform = window.navigator.platform;
        const macosPlatforms = ['Macintosh', 'MacIntel', 'MacPPC', 'Mac68K'];
        const windowsPlatforms = ['Win32', 'Win64', 'Windows', 'WinCE'];
        const iosPlatforms = ['iPhone', 'iPad', 'iPod'];

        if (macosPlatforms.indexOf(platform) !== -1) {
            return PLATFORMS.MACOS;
        } else if (iosPlatforms.indexOf(platform) !== -1) {
            return PLATFORMS.IOS;
        } else if (windowsPlatforms.indexOf(platform) !== -1) {
            return PLATFORMS.WINDOWS;
        } else if (/Android/.test(userAgent)) {
            return PLATFORMS.ANDROID;
        } else if (/Linux/.test(platform)) {
            return PLATFORMS.LINUX;
        }

        return PLATFORMS.UNKNOWN;
    }

    /**
     * Get token from URL query parameters
     */
    function getTokenFromUrl() {
        const params = new URLSearchParams(window.location.search);
        return params.get('token');
    }

    /**
     * Validate token with backend API
     */
    async function validateToken(tokenValue) {
        try {
            const controller = new AbortController();
            const timeoutId = setTimeout(() => controller.abort(), CONFIG.validationTimeout);

            const response = await fetch(`${CONFIG.apiBaseUrl}/api/onboarding/validate?token=${encodeURIComponent(tokenValue)}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                },
                signal: controller.signal
            });

            clearTimeout(timeoutId);

            if (!response.ok) {
                if (response.status === 404 || response.status === 400) {
                    return { valid: false, reason: 'invalid' };
                } else if (response.status === 403) {
                    return { valid: false, reason: 'disabled' };
                } else if (response.status === 429) {
                    return { valid: false, reason: 'rate_limit' };
                }
                throw new Error(`HTTP ${response.status}`);
            }

            const data = await response.json();
            return {
                valid: true,
                userId: data.userId,
                userName: data.userName
            };

        } catch (error) {
            console.error('Token validation error:', error);

            if (error.name === 'AbortError') {
                return { valid: false, reason: 'timeout' };
            }

            // Retry logic
            if (retryCount < CONFIG.maxRetries) {
                retryCount++;
                await new Promise(resolve => setTimeout(resolve, CONFIG.retryDelay));
                return validateToken(tokenValue);
            }

            return { valid: false, reason: 'network_error' };
        }
    }

    /**
     * Download Root CA Certificate
     */
    async function downloadCertificate() {
        try {
            const button = elements.downloadCert;
            button.disabled = true;

            const response = await fetch(`${CONFIG.apiBaseUrl}/api/onboarding/ca-cert`, {
                method: 'GET'
            });

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'GFC_Root_CA.cer';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);

            button.disabled = false;
        } catch (error) {
            console.error('Certificate download error:', error);
            alert('Failed to download certificate. Please try again.');
            elements.downloadCert.disabled = false;
        }
    }

    /**
     * Download Windows One-Click Setup Script
     */
    async function downloadWindowsSetup() {
        try {
            const button = elements.downloadWindowsSetup;
            button.disabled = true;
            const originalText = button.innerHTML;
            button.innerHTML = '<span class="btn-spinner"></span> Downloading...';

            window.location.href = `${CONFIG.apiBaseUrl}/api/onboarding/windows-setup?token=${encodeURIComponent(token)}`;

            setTimeout(() => {
                button.disabled = false;
                button.innerHTML = originalText;
                // Move to connection test step since the script is supposed to handle everything
                goToStep(4);
            }, 3000);

        } catch (error) {
            console.error('Setup script download error:', error);
            alert('Failed to download setup script. Please try the manual steps or contact support.');
            elements.downloadWindowsSetup.disabled = false;
        }
    }

    /**
     * Download Apple Profile (.mobileconfig)
     */
    async function downloadAppleProfile() {
        try {
            const button = elements.downloadAppleProfile;
            button.disabled = true;
            const originalText = button.innerHTML;
            button.innerHTML = '<span class="btn-spinner"></span> Creating...';

            window.location.href = `${CONFIG.apiBaseUrl}/api/onboarding/apple-profile?token=${encodeURIComponent(token)}`;

            setTimeout(() => {
                button.disabled = false;
                button.innerHTML = originalText;
                // Move to test step
                goToStep(4);
            }, 3000);

        } catch (error) {
            console.error('Apple profile download error:', error);
            alert('Failed to download profile. Please follow the manual steps.');
            elements.downloadAppleProfile.disabled = false;
        }
    }

    /**
     * Download WireGuard configuration
     */
    async function downloadConfiguration() {
        try {
            const button = elements.downloadConfig;
            const btnText = button.querySelector('.btn-text') || button;
            const originalText = btnText.textContent;

            button.disabled = true;
            btnText.textContent = 'Generating...';

            const controller = new AbortController();
            const timeoutId = setTimeout(() => controller.abort(), CONFIG.downloadTimeout);

            const response = await fetch(`${CONFIG.apiBaseUrl}/api/onboarding/config?token=${encodeURIComponent(token)}&deviceName=${encodeURIComponent(navigator.platform)}&deviceType=${encodeURIComponent(platform.name)}`, {
                method: 'GET',
                signal: controller.signal
            });

            clearTimeout(timeoutId);

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'gfc-access.conf';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);

            btnText.textContent = 'Downloaded!';
            setTimeout(() => {
                goToStep(4);
            }, 1000);

        } catch (error) {
            console.error('Download error:', error);
            alert('Failed to download configuration. Please try again or contact support.');

            const button = elements.downloadConfig;
            button.disabled = false;
            const btnText = button.querySelector('.btn-text') || button;
            btnText.textContent = 'Download Secure Access Profile';
        }
    }

    /**
     * Test VPN connection
     */
    async function testConnection() {
        const button = elements.testConnection;
        const btnText = button.querySelector('.btn-text');
        const btnSpinner = button.querySelector('.btn-spinner');

        button.disabled = true;
        btnText.classList.add('hidden');
        btnSpinner.classList.remove('hidden');
        elements.testResult.classList.add('hidden');

        try {
            // Simple test: try to reach the private API
            const controller = new AbortController();
            const timeoutId = setTimeout(() => controller.abort(), 5000);

            const response = await fetch(`${CONFIG.apiBaseUrl}/api/health/vpn-check`, {
                method: 'GET',
                signal: controller.signal
            });

            clearTimeout(timeoutId);

            const isConnected = response.ok;
            showTestResult(isConnected);

            if (isConnected) {
                // Mark onboarding as complete in backend
                await completeOnboarding();
            }

        } catch (error) {
            console.error('Connection test error:', error);
            showTestResult(false);
        } finally {
            button.disabled = false;
            btnText.classList.remove('hidden');
            btnSpinner.classList.add('hidden');
        }
    }

    /**
     * Notify backend that onboarding is complete
     */
    async function completeOnboarding() {
        try {
            const deviceInfo = `${navigator.platform} - ${navigator.userAgent.substring(0, 100)}`;
            await fetch(`${CONFIG.apiBaseUrl}/api/onboarding/complete?token=${encodeURIComponent(token)}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    deviceInfo: deviceInfo,
                    platform: platform.name,
                    testPassed: true
                })
            });
        } catch (error) {
            console.error('Error completing onboarding:', error);
        }
    }

    /**
     * Show test result
     */
    function showTestResult(success) {
        const result = elements.testResult;
        result.classList.remove('hidden', 'alert-success', 'alert-error');

        if (success) {
            result.classList.add('alert-success');
            result.innerHTML = `
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                    <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2"/>
                    <path d="M8 12l2 2 4-4" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                </svg>
                <div>
                    <strong>Connected!</strong> Your setup is complete.
                </div>
            `;
            elements.successActions.classList.remove('hidden');
        } else {
            result.classList.add('alert-error');
            result.innerHTML = `
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none">
                    <circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2"/>
                    <path d="M12 8v4m0 4h.01" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                </svg>
                <div>
                    <strong>Not Connected.</strong> Ensure the toggle in WireGuard is active, then try again.
                </div>
            `;
        }
    }

    /**
     * Navigate to a specific step
     */
    function goToStep(step) {
        // Update step indicators
        document.querySelectorAll('.progress-step').forEach((el, index) => {
            const stepNum = index + 1;
            el.classList.remove('active', 'completed');

            if (stepNum === step) {
                el.classList.add('active');
            } else if (stepNum < step) {
                el.classList.add('completed');
                const circle = el.querySelector('.step-circle');
                circle.innerHTML = `
                    <svg width="20" height="20" viewBox="0 0 20 20" fill="currentColor">
                        <path d="M6 10l2 2 4-4"/>
                    </svg>
                `;
            } else {
                const circle = el.querySelector('.step-circle');
                circle.textContent = stepNum;
            }
        });

        // Update step content
        document.querySelectorAll('.step-content').forEach((el, index) => {
            el.classList.remove('active');
            if (index + 1 === step) {
                el.classList.add('active');
            }
        });

        currentStep = step;
    }

    /**
     * Show specific state
     */
    function showState(state) {
        // Hide all states
        elements.loading.classList.add('hidden');
        elements.errorInvalid.classList.add('hidden');
        elements.errorDisabled.classList.add('hidden');
        elements.wizard.classList.add('hidden');

        // Show requested state
        switch (state) {
            case 'loading':
                elements.loading.classList.remove('hidden');
                break;
            case 'invalid':
                elements.errorInvalid.classList.remove('hidden');
                break;
            case 'disabled':
                elements.errorDisabled.classList.remove('hidden');
                break;
            case 'wizard':
                elements.wizard.classList.remove('hidden');
                break;
        }
    }

    /**
     * Initialize the application
     */
    async function init() {
        // Set current year
        elements.year.textContent = new Date().getFullYear();

        // Detect platform
        platform = detectPlatform();
        elements.platformName.textContent = platform.name;
        elements.downloadLink.href = platform.downloadUrl;
        elements.certInstructions.textContent = platform.certInstructions;
        elements.platformInstructions.textContent = platform.configInstructions;

        // Get token from URL
        token = getTokenFromUrl();

        if (!token) {
            showState('invalid');
            return;
        }

        // Validate token
        showState('loading');
        const validation = await validateToken(token);

        if (!validation.valid) {
            if (validation.reason === 'disabled') {
                showState('disabled');
            } else {
                showState('invalid');
            }
            return;
        }

        // Token is valid, show wizard
        userId = validation.userId;
        showState('wizard');
        goToStep(1);

        // Show Windows One-Click if on Windows
        if (platform.name === 'Windows') {
            elements.windowsOneClick.classList.remove('hidden');
        } else if (platform.name === 'macOS' || platform.name === 'iOS') {
            elements.appleOneClick.classList.remove('hidden');
        }

        // Setup event listeners
        setupEventListeners();
    }

    /**
     * Setup event listeners
     */
    function setupEventListeners() {
        elements.nextStep1.addEventListener('click', () => goToStep(2));
        elements.downloadCert.addEventListener('click', downloadCertificate);
        elements.nextStep2.addEventListener('click', () => goToStep(3));
        elements.downloadConfig.addEventListener('click', downloadConfiguration);
        elements.downloadWindowsSetup.addEventListener('click', downloadWindowsSetup);
        elements.downloadAppleProfile.addEventListener('click', downloadAppleProfile);
        elements.skipToTest.addEventListener('click', () => goToStep(4));
        elements.testConnection.addEventListener('click', testConnection);
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();
