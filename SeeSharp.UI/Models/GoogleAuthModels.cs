using System.Text.Json.Serialization;

namespace SeeSharp.UI.Models
{
    /// <summary>
    /// Response từ Google Authentication
    /// </summary>
    public class GoogleAuthResponse
    {
        /// <summary>
        /// ID token từ Google
        /// </summary>
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; } = string.Empty;

        /// <summary>
        /// Email người dùng
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// URL hình ảnh
        /// </summary>
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request gửi lên API backend để đăng nhập
    /// </summary>
    public class GoogleSignInRequest
    {
        /// <summary>
        /// ID token từ Google
        /// </summary>
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// API response từ backend khi đăng nhập thành công
    /// </summary>
    public class GoogleSignInResponse
    {
        /// <summary>
        /// Access token của ứng dụng
        /// </summary>
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh token của ứng dụng
        /// </summary>
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
    }
} 