using System.Text.Json.Serialization;

namespace SeeSharp.UI.Models
{
    /// <summary>
    /// Request model for Google Sign-in/Sign-up
    /// </summary>
    public class GoogleAuthRequest
    {
        /// <summary>
        /// The ID token returned from Google Authentication
        /// </summary>
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; } = string.Empty;
    }

    /// <summary>
    /// Google JWT payload structure
    /// </summary>
    public class GoogleJwtPayload
    {
        /// <summary>
        /// Subject Identifier
        /// </summary>
        [JsonPropertyName("sub")]
        public string Sub { get; set; } = string.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Email verification status
        /// </summary>
        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Given name (first name)
        /// </summary>
        [JsonPropertyName("given_name")]
        public string GivenName { get; set; } = string.Empty;

        /// <summary>
        /// Family name (last name)
        /// </summary>
        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>
        /// Profile picture URL
        /// </summary>
        [JsonPropertyName("picture")]
        public string Picture { get; set; } = string.Empty;

        /// <summary>
        /// Locale information
        /// </summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = string.Empty;
    }
} 