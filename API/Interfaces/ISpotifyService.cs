namespace API.Interfaces
{
    public interface ISpotifyService
    {
        Task<string> GetSportifyAccessToken(string code);
        Task<string> GetSpotifyPlaylist(string playlistId, string accessToken);
    }
}
