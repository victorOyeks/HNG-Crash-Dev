using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/spotify")]
    public class SpotifyController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SpotifyService _spotifyService;

        public SpotifyController(IConfiguration config, SpotifyService spotifyService)
        {
            _config = config;
            _spotifyService = spotifyService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _config["Spotify:ClientId"];
            var redirectUri = _config["Spotify:RedirectUri"];
            var state = "some_random_string"; // A random string for security
            var authorizationEndpoint = "https://accounts.spotify.com/authorize";
            var scope = "playlist-read-private"; // Request the necessary scopes

            var authorizationUrl = $"{authorizationEndpoint}?client_id={clientId}&response_type=code&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={state}&scope={Uri.EscapeDataString(scope)}";

            return Redirect(authorizationUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Invalid authorization code.");
            }

            var accessToken = await _spotifyService.GetSpotifyAccessToken(code);
            return Ok(new { AccessToken = accessToken });
        }

        [HttpGet("playlist/{playlistId}")]
        public async Task<IActionResult> GetPlaylist(string playlistId, [FromQuery] string accessToken)
        {
            var playlist = await _spotifyService.GetSpotifyPlaylist(playlistId, accessToken);
            return Ok(playlist);
        }
    }
}
