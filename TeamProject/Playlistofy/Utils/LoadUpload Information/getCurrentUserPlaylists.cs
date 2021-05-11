using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using Playlistofy.Models; 

namespace Playlistofy.Utils
{
    public class getCurrentUserPlaylists
    {
        private readonly UserManager<IdentityUser> _userManager;
        private static string _spotifyClientId;
        private static string _spotifyClientSecret;

        public getCurrentUserPlaylists(UserManager<IdentityUser> userManager, string spotifyClientId, string spotifyClientPassword)
        {
            _userManager = userManager;
            _spotifyClientId = spotifyClientId;
            _spotifyClientSecret = spotifyClientPassword;
        }

        [HttpGet]
        public async Task<string> GetCurrentUserId(IdentityUser _user)
        {
            var personalData = new Dictionary<string, string>();
            var logins = await _userManager.GetLoginsAsync(_user);

            string key = "";
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
                key = l.ProviderKey;
            }
            return key;
        }

        public static SpotifyClient makeSpotifyClient(string spotifyClientId, string spotifyClientSecret)
        {
            SpotifyClientConfig config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(spotifyClientId, spotifyClientSecret));

            SpotifyClient spotify = new SpotifyClient(config);

            return spotify;
        }

        public async Task<List<Playlist>> GetCurrentUserPlaylists(SpotifyClient spotifyClient, string userSpotifyId, string userId)
        {
            //This creates an instance of the model getCurrentUserTracks.cs to later call below in the foreach loop
            var playlistTracks = new getCurrentUserTracks(_userManager, _spotifyClientId, _spotifyClientSecret);

            List<Playlist> spotifyPlaylists = new List<Playlist>();
            var playlists = await spotifyClient.Playlists.GetUsers(userSpotifyId);

            foreach (var playlist in playlists.Items)
            {
                spotifyPlaylists.Add(new Playlist()
                {
                    Name = playlist.Name,
                    Id = playlist.Id,
                    Description = playlist.Description,
                    Public = playlist.Public,
                    Collaborative = playlist.Collaborative,
                    Href = playlist.Href,
                    Uri = playlist.Uri,
                    UserId = userId
                    //DateCreated = DateTime.Now
                    //Tracks = await playlistTracks.GetPlaylistTrack(spotifyClient, userSpotifyId, playlist.Id)
                });
            }
            return spotifyPlaylists;
        }
    }
}
