// Hàm xác thực Google sử dụng Google Identity API
// Tham khảo: https://developers.google.com/identity/gsi/web/guides/overview

// Tải Google Identity Services API
function loadGoogleIdentityAPI() {
    // Kiểm tra xem script đã được tải chưa
    if (document.querySelector('script[src*="accounts.google.com/gsi/client"]')) {
        return Promise.resolve();
    }

    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = 'https://accounts.google.com/gsi/client';
        script.async = true;
        script.defer = true;
        script.onload = resolve;
        script.onerror = reject;
        document.head.appendChild(script);
    });
}

// Hàm để xác thực với Google
window.googleAuthenticate = async function (clientId) {
    try {
        // Tải Google Identity API
        await loadGoogleIdentityAPI();

        return new Promise((resolve, reject) => {
            // Sử dụng Google Identity API
            google.accounts.id.initialize({
                client_id: clientId,
                callback: (response) => {
                    if (response && response.credential) {
                        // Trả về thông tin cần thiết cho Blazor
                        const result = {
                            idToken: response.credential,
                            // Các thông tin khác sẽ được giải mã từ token ở phía server
                            email: '',
                            name: '',
                            imageUrl: ''
                        };
                        resolve(result);
                    } else {
                        reject('Không thể lấy thông tin xác thực từ Google');
                    }
                },
                auto_select: false
            });

            // Hiển thị popup đăng nhập Google
            google.accounts.id.prompt((notification) => {
                if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
                    // Nếu không hiển thị được popup, chuyển sang phương thức khác
                    fallbackToRedirect(clientId, resolve, reject);
                }
            });
        });
    } catch (error) {
        console.error('Lỗi xác thực Google:', error);
        throw error;
    }
};

// Phương thức dự phòng sử dụng OAuth 2.0 redirect flow
function fallbackToRedirect(clientId, resolve, reject) {
    // Tạo URL redirect để đăng nhập Google
    const redirectUri = window.location.origin + '/login';
    const scope = 'email profile';
    const responseType = 'id_token';
    const authUrl = `https://accounts.google.com/o/oauth2/v2/auth?client_id=${clientId}&redirect_uri=${encodeURIComponent(redirectUri)}&response_type=${responseType}&scope=${encodeURIComponent(scope)}&nonce=${Math.random().toString(36).substring(2, 15)}`;

    // Mở cửa sổ popup cho quá trình đăng nhập
    const authWindow = window.open(authUrl, '_blank', 'width=500,height=600');
    
    // Check nếu cửa sổ popup bị chặn
    if (!authWindow || authWindow.closed || typeof authWindow.closed === 'undefined') {
        reject('Popup bị chặn. Vui lòng cho phép popup để đăng nhập.');
        return;
    }

    // Hàm xử lý callback từ Google
    window.handleGoogleCallback = function (idToken) {
        console.log('ID Token:', idToken);
        if (idToken) {
            const result = {
                idToken: idToken,
                email: '',
                name: '',
                imageUrl: ''
            };
            resolve(result);
            
            // Đóng cửa sổ popup nếu còn mở
            if (authWindow && !authWindow.closed) {
                authWindow.close();
            }
        } else {
            reject('Xác thực Google thất bại');
        }
    };
}

// Hàm để xử lý callback từ Google sau khi redirect
window.processGoogleCallback = function() {
    // Lấy id_token từ URL fragment hoặc query parameters
    const urlParams = new URLSearchParams(window.location.search);
    const fragmentParams = new URLSearchParams(window.location.hash.substring(1));
    
    const idToken = fragmentParams.get('id_token') || urlParams.get('id_token');
    
    if (idToken) {
        // Gọi hàm xử lý trong cửa sổ mở
        if (window.opener && window.opener.handleGoogleCallback) {
            window.opener.handleGoogleCallback(idToken);
            window.close();
        }
    }
}; 