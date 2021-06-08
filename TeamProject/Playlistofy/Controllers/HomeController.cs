using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Playlistofy.Data.Abstract;
using Playlistofy.Data.Concrete;
using Playlistofy.Models;
using Playlistofy.Models.ViewModel;
using Playlistofy.Utils;
using Playlistofy.Utils.AlgorithmicOperations;
using Playlistofy.Utils.LoadUpload_Information;
using SpotifyAPI.Web;

namespace Playlistofy.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly IPlaylistofyUserRepository _pURepo;
        private readonly IPlaylistRepository _pRepo;
        private readonly ITrackRepository _tRepo;
        private readonly IArtistRepository _arRepo;
        private readonly IAlbumRepository _aRepo;
        private readonly IHashtagRepository _hRepo;
        private readonly IKeywordRepository _kRepo;
        private static string _spotifyClientId;
        private static string _spotifyClientSecret;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, UserManager<IdentityUser> userManager, IPlaylistofyUserRepository pURepo, IPlaylistRepository pRepo, ITrackRepository tRepo, IAlbumRepository aRepo, IArtistRepository arRepo, IHashtagRepository hRepo, IKeywordRepository kRepo)
        {
            _userManager = userManager;
            _logger = logger;
            _config = config;
            _pURepo = pURepo;
            _pRepo = pRepo;
            _tRepo = tRepo;
            _aRepo = aRepo;
            _arRepo = arRepo;
            _hRepo = hRepo;
            _kRepo = kRepo;
            _spotifyClientId = config["Spotify:ClientId"];
            _spotifyClientSecret = config["Spotify:ClientSecret"];
        }

        public async Task<IActionResult> IndexAsync(string PlaylistIDAdded)
        {
            if (PlaylistIDAdded != null)
            {
                IdentityUser usr = await GetCurrentUserAsync();
                if (usr == null) { return RedirectToPage("/Account/Login", new { area = "Identity" }); }

                if (await _pRepo.FindByIdAsync(PlaylistIDAdded) != null)
                {
                    string randomId = RandomString.GetRandomString();

                    var NewPlaylist = new Playlist();
                    NewPlaylist.UserId = _userManager.GetUserId(User);
                    
                    var DbPlaylist = await _pRepo.FindByIdAsync(PlaylistIDAdded);
                    var playlistTracks = new List<Track>();
                    if (DbPlaylist != null)
                    {
                        NewPlaylist.Collaborative = getCurrentUserPlaylists.getPublic(DbPlaylist?.Collaborative);
                        NewPlaylist.DateCreated = DateTime.Now;
                        NewPlaylist.Description = DbPlaylist?.Description;
                        NewPlaylist.Href = DbPlaylist?.Href;
                        NewPlaylist.Id = randomId;
                        NewPlaylist.Name = DbPlaylist?.Name;
                        NewPlaylist.Public = getCurrentUserPlaylists.getPublic(DbPlaylist?.Public);
                        NewPlaylist.Uri = DbPlaylist?.Uri;
                        NewPlaylist.PlaylistHashtagMaps = DbPlaylist?.PlaylistHashtagMaps;
                        NewPlaylist.PlaylistKeywordMaps = DbPlaylist?.PlaylistKeywordMaps;
                        NewPlaylist.FollowedPlaylists = DbPlaylist?.FollowedPlaylists;
                        await _pRepo.AddAsync(NewPlaylist);

                        playlistTracks = _pRepo.GetAllPlaylistTracks(DbPlaylist);
                    }

                    foreach (var track in playlistTracks)
                    {
                        if (await _tRepo.FindByIdAsync(track.Id) == null)
                        {
                            await _tRepo.AddAsync(track);
                        }
                        await _tRepo.AddTrackPlaylistMap(track.Id, NewPlaylist.Id);
                    }
                }
            }

            var UserPlaylistPlaylist = new HomePageViewModel();

            UserPlaylistPlaylist.RecentPlaylists = await _pRepo.GetMostRecentPlaylists_5Async();
            var UserNameList = new List<string>();

            foreach (var playlist in UserPlaylistPlaylist.RecentPlaylists)
            {
                if (playlist.UserId != null)
                {
                    string userId = playlist.UserId;
                    var playlistUser = await _userManager.FindByIdAsync(userId);
                    if (playlistUser?.Email != null)
                    {
                        var emailCut = playlistUser.Email.Split('@').First();
                        UserNameList.Add(emailCut);
                    }
                }
                else
                {
                    UserNameList.Add("No user for this playlist");
                }
            }
            UserPlaylistPlaylist.SearchedPlaylistsUsers = UserNameList;

            if (_userManager.GetUserId(User) != null)
            {
                UserPlaylistPlaylist.UserID = _userManager.GetUserId(User);
                //var uD = new UserData(_config, _userManager, _pURepo, _pRepo, _tRepo, _aRepo, _arRepo, _userManager.GetUserAsync(User).Result);

                //await uD.SetUserData();

                var tempUser = _userManager.GetUserId(User);
                var newPlaylist = _pRepo.GetAll().Include("PlaylistTrackMaps").Where(name => name.UserId == tempUser).ToList();

                UserPlaylistPlaylist.UserPlaylists = newPlaylist;
            }

            if (_userManager.GetUserId(User) != null)
            {
                var playlists = _pRepo.GetAllPlaylistTracks(UserPlaylistPlaylist.UserPlaylists.GetEnumerator().Current);
                var albums = new List<Album>();

                foreach (var t in playlists)
                {
                    if (t.TrackAlbumMaps.Count < 1)
                    {
                        SpotifyClient spotty = getSpotifyClient.makeSpotifyClient(_spotifyClientId, _spotifyClientSecret);
                        albums.Add(await _aRepo.GetAlbumFromTrack(t.Id, spotty));
                    }
                }

                var images = albums.Select(a => a.ImageUrl).ToList();

                UserPlaylistPlaylist.PlaylistAlbums = albums;
                UserPlaylistPlaylist.AlbumImageUrls = images;
            }

            return View(UserPlaylistPlaylist);
            //return View(new List<Playlist>());
        }

        public async Task<IActionResult> SpotifyProfile()
        {
            IdentityUser usr = await GetCurrentUserAsync();
            if (usr == null) { return RedirectToPage("/Account/Login", new { area = "Identity" }); }

            var getUserPlaylists = new getCurrentUserPlaylists(_userManager, _spotifyClientId, _spotifyClientSecret);
            string _userSpotifyId = await getUserPlaylists.GetCurrentUserId(usr);
            if (_userSpotifyId == null || _userSpotifyId == "") { return RedirectToPage("/Account/Login", new { area = "Identity" }); }

            var spotifyClient = getCurrentUserPlaylists.makeSpotifyClient(_spotifyClientId, _spotifyClientSecret);
            var spotifyUserInfo = await spotifyClient.UserProfile.Get(_userSpotifyId);

            string spotifyUserId = spotifyUserInfo.Id;

            return Redirect("https://open.spotify.com/user/" + spotifyUserId);
        }

        [NonAction]
        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Credits()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpGet]
        //public async Task<IActionResult> WebPlayer(string id)
        //{
        //    var viewModel = new userPlaylistsTracks();

        //    //Finds current logged in user using identity 
        //    IdentityUser usr = await GetCurrentUserAsync();
        //    if (usr == null) { return RedirectToPage("/Account/Login", new { area = "Identity" }); }

        //    //Instantiates the Model to call it's functions - Finds current logged in user's spotify ID
        //    var getUserPlaylists = new getCurrentUserPlaylists(_userManager, _spotifyClientId, _spotifyClientSecret);
        //    string _userSpotifyId = await getUserPlaylists.GetCurrentUserId(usr);
        //    if (_userSpotifyId == null || _userSpotifyId == "") { return RedirectToPage("/Account/Login", new { area = "Identity" }); }

        //    //Create's client and then finds all playlists for current logged in user
        //    var _spotifyClient = getCurrentUserPlaylists.makeSpotifyClient(_spotifyClientId, _spotifyClientSecret);
        //    viewModel.Playlists = await getUserPlaylists.GetCurrentUserPlaylists(_spotifyClient, _userSpotifyId, usr.Id);

        //    //Get current logged in user's information
        //    var getUserInfo = new getCurrentUserInformation(_userManager, _spotifyClientId, _spotifyClientSecret);
        //    viewModel.User = await getUserInfo.GetCurrentUserInformation(_spotifyClient, _userSpotifyId);

        //    //Get current logged in user's tracks
        //    var getUserTracks = new getCurrentUserTracks(_userManager, _spotifyClientId, _spotifyClientSecret);

        //    if (await _pRepo.FindByIdAsync(id) == null)
        //    {
        //        var newPlaylist = viewModel.Playlists.Where(name => name.Id == id);
        //        foreach (var playlist in newPlaylist)
        //        {
        //            Console.WriteLine(playlist.Id);

        //            if (await _pRepo.FindByIdAsync(id) == null)
        //            {
        //                List<Track> Tracks = await getUserTracks.GetPlaylistTrack(_spotifyClient, _userSpotifyId, playlist.Id);
        //                await _pRepo.AddAsync(playlist);
        //                foreach (Track track in Tracks)
        //                {
        //                    Console.WriteLine(track.Id);
        //                    if (await _tRepo.FindByIdAsync(track.Id) == null)
        //                    {
        //                        await _tRepo.AddAsync(track);
        //                    }
        //                    await _tRepo.AddTrackPlaylistMap(track.Id, playlist.Id);
        //                }
        //            }
        //        }
        //    }

        //    var currentUserID = await _userManager.GetUserIdAsync(usr);
        //    var userPlaylists = _pRepo.GetAllWithUser().Where(i => i.User.Id == currentUserID);

        //    viewModel._PlaylistsDB = await userPlaylists.ToListAsync();
        //    return View("WebPlayer", viewModel);
        //}
        [HttpPost]
        public JsonResult AutoComplete(string prefix, string searchType)
        {
            if (searchType == "Album")
            {
                var albums = _aRepo.GetAll().Where(i => i.Name.StartsWith(prefix));
                var words = new List<ModelforAuto>();
                foreach(var a in albums)
                {
                    words.Add(new ModelforAuto()
                    {
                        Id = a.Id,
                        Label = a.Name
                    });
                }
                return Json(words);
            }
            else if (searchType == "Playlist")
            {
                var playlists = _pRepo.GetAll().Where(i => i.Name.StartsWith(prefix));
                var words = new List<ModelforAuto>();
                foreach (var a in playlists)
                {
                    words.Add(new ModelforAuto()
                    {
                        Id = a.Id,
                        Label = a.Name
                    });
                }
                return Json(words);
            }
            else if (searchType == "Track")
            {
                var tracks = _tRepo.GetAll().Where(i => i.Name.StartsWith(prefix));
                var words = new List<ModelforAuto>();
                foreach (var a in tracks)
                {
                    words.Add(new ModelforAuto()
                    {
                        Id = a.Id,
                        Label = a.Name
                    });
                }
                return Json(words);
            }
            else if (searchType == "Artist")
            {
                var artists = _arRepo.GetAll().Where(i => i.Name.StartsWith(prefix));
                var words = new List<ModelforAuto>();
                foreach (var a in artists)
                {
                    words.Add(new ModelforAuto()
                    {
                        Id = a.Id,
                        Label = a.Name
                    });
                }
                return Json(words);
            }
            else if (searchType == "Tags")
            {
                var hash = _hRepo.GetAll().Where(i => i.HashTag1.StartsWith(prefix));
                var key = _kRepo.GetAll().Where(i => i.Keyword1.StartsWith(prefix));
                var words = new List<TagsModel>();
                foreach (var h in hash)
                {
                    words.Add(new TagsModel()
                    {
                        Id = h.Id,
                        label = h.HashTag1
                    });
                }
                foreach (var k in key)
                {
                    words.Add(new TagsModel()
                    {
                        Id = k.Id,
                        label = k.Keyword1
                    });
                }

                return Json(words);
            }else
            {
                return null;
            }
        }

        public async Task<IActionResult> FollowPlaylist(string Uid, string pId)
        {

            await _pRepo.AddTrackPlaylistMap(Uid, pId);
            return RedirectToAction("DetailsFromSearch", "Playlists", new { id = pId });
        }

        public async Task<IActionResult> UnfollowPlaylist(string Uid, string pId)
        {
            await _pRepo.DeletePlaylistMap(Uid, pId);
            return RedirectToAction("DetailsFromSearch", "Playlists", new { id = pId });
        }

        public async Task<IActionResult> LikePlaylist(string DispName, string pId)
        {
            await _pRepo.LikePlaylist(DispName, pId);
            return RedirectToAction("DetailsFromSearch", "Playlists", new { id = pId });
        }

        public async Task<IActionResult> UnLikePlaylist(string DispName, string pId)
        {
            await _pRepo.UnlikePlaylist(DispName, pId);
            return RedirectToAction("DetailsFromSearch", "Playlists", new { id = pId });
        }
    }
}
