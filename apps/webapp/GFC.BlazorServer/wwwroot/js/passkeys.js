window.GFC_Passkeys = {
    // Helper to convert base64url to Uint8Array
    coerceToArrayBuffer: function (data) {
        if (typeof data === 'string') {
            // base64url to base64
            data = data.replace(/-/g, '+').replace(/_/g, '/');
            // base64 to buffer
            var str = window.atob(data);
            var bytes = new Uint8Array(str.length);
            for (var i = 0; i < str.length; i++) {
                bytes[i] = str.charCodeAt(i);
            }
            return bytes.buffer;
        }
        return data;
    },

    // Helper to convert ArrayBuffer to base64url
    coerceToBase64Url: function (buffer) {
        var str = "";
        var bytes = new Uint8Array(buffer);
        for (var i = 0; i < bytes.byteLength; i++) {
            str += String.fromCharCode(bytes[i]);
        }
        return window.btoa(str)
            .replace(/\+/g, '-')
            .replace(/\//g, '_')
            .replace(/=/g, '');
    },

    register: async function (options) {
        // Fix up options from server
        options.user.id = this.coerceToArrayBuffer(options.user.id);
        options.challenge = this.coerceToArrayBuffer(options.challenge);

        if (options.excludeCredentials) {
            for (var i = 0; i < options.excludeCredentials.length; i++) {
                options.excludeCredentials[i].id = this.coerceToArrayBuffer(options.excludeCredentials[i].id);
            }
        }

        try {
            const credential = await navigator.credentials.create({
                publicKey: options
            });

            return {
                id: credential.id,
                rawId: this.coerceToBase64Url(credential.rawId),
                type: credential.type,
                response: {
                    attestationObject: this.coerceToBase64Url(credential.response.attestationObject),
                    clientDataJSON: this.coerceToBase64Url(credential.response.clientDataJSON)
                }
            };
        } catch (e) {
            console.error("Passkey Registration Error:", e);
            throw e;
        }
    },

    login: async function (options) {
        options.challenge = this.coerceToArrayBuffer(options.challenge);

        if (options.allowCredentials) {
            for (var i = 0; i < options.allowCredentials.length; i++) {
                options.allowCredentials[i].id = this.coerceToArrayBuffer(options.allowCredentials[i].id);
            }
        }

        try {
            const assertion = await navigator.credentials.get({
                publicKey: options
            });

            return {
                id: assertion.id,
                rawId: this.coerceToBase64Url(assertion.rawId),
                type: assertion.type,
                response: {
                    authenticatorData: this.coerceToBase64Url(assertion.response.authenticatorData),
                    clientDataJSON: this.coerceToBase64Url(assertion.response.clientDataJSON),
                    signature: this.coerceToBase64Url(assertion.response.signature),
                    userHandle: assertion.response.userHandle ? this.coerceToBase64Url(assertion.response.userHandle) : null
                }
            };
        } catch (e) {
            console.error("Passkey Login Error:", e);
            throw e;
        }
    }
};
