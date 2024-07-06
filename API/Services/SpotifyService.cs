/* using API.Dto;
using API.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace API.Services
{
    public class SpotifyService 
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SpotifyService> _logger;

        public SpotifyService(IConfiguration config, ILogger<SpotifyService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<string> GetSpotifyAccessToken(string code)
        {
            var clientId = _config["Spotify:ClientId"];
            var clientSecret = _config["Spotify:ClientSecret"];
            var redirectUri = _config["Spotify:RedirectUri"];

            var tokenEndpoint = "https://accounts.spotify.com/api/token";

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
                var credentials = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

                var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
            };

                request.Content = new FormUrlEncodedContent(postData);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Token response: {responseBody}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve token: {responseBody}");
                    return null;
                }

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
                return tokenResponse?.access_token;
            }

        }

        public async Task<string> GetSpotifyPlaylist(string playlistId, string accessToken)
        {
            var apiUrl = $"https://api.spotify.com/v1/playlists/{playlistId}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
    }
}
*/