using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using Playlistofy.Models;
using Microsoft.AspNetCore.Http;
using Playlistofy.Controllers;
using System.Linq;

namespace Playlistofy.Utils
{
    public class searchSpotify
    {
        private readonly UserManager<IdentityUser> _userManager;
        private static string _spotifyClientId;
        private static string _spotifyClientSecret;

        public searchSpotify(UserManager<IdentityUser> userManager, string spotifyClientId, string spotifyClientPassword)
        {
            _userManager = userManager;
            _spotifyClientId = spotifyClientId;
            _spotifyClientSecret = spotifyClientPassword;
        }

        public SpotifyClient makeSpotifyClient(string spotifyClientId, string spotifyClientSecret)
        {
            SpotifyClientConfig config = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new ClientCredentialsAuthenticator(spotifyClientId, spotifyClientSecret));

            SpotifyClient spotify = new SpotifyClient(config);

            return spotify;
        }

        public async Task<List<Track>> SearchTracks(SpotifyClient _spotifyClient, string SearchKeyword, List<Track> tracks)
        {
            var searchTracks = await _spotifyClient.Search.Item(new SearchRequest(
              SearchRequest.Types.Track, SearchKeyword
            ));

            List<Track> TrackList = new List<Track>();

            int count = 0;
            await foreach (var item in _spotifyClient.Paginate(searchTracks.Tracks, (s) => s.Tracks))
            {
                var _track = tracks.FirstOrDefault(i => i.Id == item.Id);
                if (_track == null)
                {
                    if (count >= 10)
                    {
                        break;
                    }
                    var tempTrack = new Track();
                    tempTrack.Name = item.Name;
                    tempTrack.Id = item.Id;
                    tempTrack.IsPlayable = item.IsPlayable;
                    tempTrack.IsLocal = item.IsLocal;
                    tempTrack.Popularity = item.Popularity;
                    tempTrack.Uri = item.Uri;
                    tempTrack.DurationMs = item.DurationMs;
                    foreach (var artists in item.Artists)
                    {
                        tempTrack.Href = tempTrack.Href + " " + artists.Name;
                    }

                    TrackList.Add(tempTrack);
                    ++count;
                }
            }

            return TrackList;
        }

        public async Task<List<Playlist>> SearchPlaylists(SpotifyClient _spotifyClient, string SearchKeyword, List<Playlist> playlists)
        {
            var searchPlaylists = await _spotifyClient.Search.Item(new SearchRequest(
              SearchRequest.Types.Playlist, SearchKeyword
            ));

            List<Playlist> PlaylistList = new List<Playlist>();

            int count = 0;
            await foreach (var item in _spotifyClient.Paginate(searchPlaylists.Playlists, (s) => s.Playlists))
            {
                var _playlist = playlists.FirstOrDefault(i => i.Id == item.Id);
                if (_playlist == null)
                {
                    if (count >= 15)
                    {
                        break;
                    }
                    var tempPlaylist = new Playlist();
                    tempPlaylist.Name = item.Name;
                    tempPlaylist.Id = item.Id;
                    tempPlaylist.Collaborative = item.Collaborative;
                    tempPlaylist.Description = item.Description;
                    tempPlaylist.Href = item.Href;
                    tempPlaylist.Public = item.Public;
                    tempPlaylist.Uri = item.Uri;

                    //foreach(var track in item.Tracks.Items)
                    //{
                    //    ++tempPlaylist.TrackCount;
                    //}

                    PlaylistList.Add(tempPlaylist);
                    ++count;
                }
            }

            return PlaylistList;
        }

        public async Task<Playlist> GetPlaylist(SpotifyClient spotifyClient, string playlistId)
        {
            var newPlaylist = new Playlist();
            FullPlaylist fullplaylist = null;

            fullplaylist = await spotifyClient.Playlists.Get(playlistId);

            newPlaylist.Name = fullplaylist.Name;
            newPlaylist.Id = fullplaylist.Id;
            newPlaylist.Description = fullplaylist.Description;
            newPlaylist.Collaborative = fullplaylist.Collaborative;
            newPlaylist.Public = fullplaylist.Public;
            newPlaylist.Href = fullplaylist.Href;
            newPlaylist.Uri = fullplaylist.Uri;
            newPlaylist.DateCreated = DateTime.Now;

            return newPlaylist;
        }

        public async Task<List<Track>> GetPlaylistTracks(SpotifyClient spotifyClient, string playlistId)
        {
            List<Track> playlistTracks = new List<Track>();
            FullPlaylist fullplaylist = null;

            fullplaylist = await spotifyClient.Playlists.Get(playlistId);
            var j = fullplaylist.Tracks;
            foreach (var k in j.Items)
            {
                FullTrack m = (FullTrack)k.Track;
                playlistTracks.Add(new Track()
                {
                    DiscNumber = m.DiscNumber,
                    DurationMs = m.DurationMs,
                    Duration = AlgorithmicOperations.MsConversion.ConvertMsToMinSec(m.DurationMs),
                    Explicit = m.Explicit,
                    Href = m.Href,
                    Id = m.Id,
                    IsPlayable = m.IsPlayable,
                    Name = m.Name,
                    Popularity = m.Popularity,
                    PreviewUrl = m.PreviewUrl,
                    TrackNumber = m.TrackNumber,
                    Uri = m.Uri,
                    IsLocal = m.IsLocal
                });
            }
            return playlistTracks;
        }
    }
}
