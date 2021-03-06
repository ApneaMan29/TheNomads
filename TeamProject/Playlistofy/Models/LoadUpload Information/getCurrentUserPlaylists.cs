﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;

namespace Playlistofy.Models
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

        public SpotifyClient makeSpotifyClient(string spotifyClientId, string spotifyClientSecret)
        {
            SpotifyClientConfig config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(spotifyClientId, spotifyClientSecret));

            SpotifyClient spotify = new SpotifyClient(config);

            return spotify;
        }

        public async Task<List<Playlist>> GetCurrentUserPlaylists(SpotifyClient spotifyClient, string userSpotifyId)
        {
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
                    Href = playlist.Href
                });
            }

            return spotifyPlaylists;
        }
    }
}