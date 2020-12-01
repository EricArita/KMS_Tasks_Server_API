using Newtonsoft.Json;

namespace Core.Application.Models
{
    public class FacebookAppAccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    public class FacebookUserAccessTokenValidation
    {
        public UserAccessTokenValidationInfo Data { get; set; }
    }

    public class UserAccessTokenValidationInfo
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
        
        public string Application { get; set; }

        [JsonProperty("data_access_expires_at")]
        public int DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at")]
        public int ExpiresAt { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        public string Type { get; set; }
        public object Metadata { get; set; }
        public string[] Scopes { get; set; }
    }

    public class FacebookUserData
    {
        [JsonProperty("id")]
        public string FacebookUserId { get; set; }

        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string  FirstName { get; set; }

        [JsonProperty("middle_name")]
        public string MiddleName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        public string Name { get; set; }

        public string BirthDay { get; set; }
        //public string Location { get; set; } //need refactor 
        public string Gender { get; set; }
        public FacebookUserPicture Picture { get; set; }
    }

    public class FacebookUserPicture
    {
        public FacebookUserPictureInfo Data { get; set; }
    }

    public class FacebookUserPictureInfo
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string Url { get; set; }

        [JsonProperty("is_silhouette")]
        public bool IsSilhouette { get; set; }
    }
}
