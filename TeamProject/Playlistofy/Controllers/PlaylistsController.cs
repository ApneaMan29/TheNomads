using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Playlistofy.Models;
using Playlistofy.Models.ViewModel;
using Playlistofy.Utils;
using Playlistofy.Controllers;
using Playlistofy.Data.Abstract;

namespace Playlistofy.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistRepository _pRepo;
        private readonly ITrackRepository _tRepo;
        private readonly IPlaylistofyUserRepository _puRepo;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IConfiguration _config;

        private static string _spotifyClientId;
        private static string _spotifyClientSecret;

        public PlaylistsController(IPlaylistRepository pRepo, ITrackRepository tRepo, IPlaylistofyUserRepository puRepo, IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _pRepo = pRepo;
            _tRepo = tRepo;
            _puRepo = puRepo;
            _userManager = userManager;

            _config = config;

            _spotifyClientId = config["Spotify:ClientId"];
            _spotifyClientSecret = config["Spotify:ClientSecret"];
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Playlists
        public IActionResult Index()
        {
            var spotifyDBContext = _pRepo.GetAllWithUser().ToList();
            return View(spotifyDBContext);
        }

        // GET: Playlists
        public async Task<IActionResult> UserPlaylists(string id)
        {
            var viewModel = new userPlaylistsTracks();

            //Finds current logged in user using identity 
            IdentityUser usr = await GetCurrentUserAsync();
            if (usr == null) { return View("~/Views/Home/Privacy.cshtml"); }

            //Instantiates the Model to call it's functions - Finds current logged in user's spotify ID
            var getUserPlaylists = new getCurrentUserPlaylists(_userManager, _spotifyClientId, _spotifyClientSecret);
            string _userSpotifyId = await getUserPlaylists.GetCurrentUserId(usr);
            if (_userSpotifyId == null || _userSpotifyId == "") { return View("~/Views/Home/Privacy.cshtml"); }

            //Create's client and then finds all playlists for current logged in user
            var _spotifyClient = getCurrentUserPlaylists.makeSpotifyClient(_spotifyClientId, _spotifyClientSecret);
            viewModel.Playlists = await getUserPlaylists.GetCurrentUserPlaylists(_spotifyClient, _userSpotifyId, usr.Id);

            //Get current logged in user's information
            var getUserInfo = new getCurrentUserInformation(_userManager, _spotifyClientId, _spotifyClientSecret);
            viewModel.User = await getUserInfo.GetCurrentUserInformation(_spotifyClient, _userSpotifyId);

            //Get current logged in user's tracks
            var getUserTracks = new getCurrentUserTracks(_userManager, _spotifyClientId, _spotifyClientSecret);

            if (await _pRepo.FindByIdAsync(id) == null)
            {
                var newPlaylist = viewModel.Playlists.Where(name => name.Id == id);
                foreach (var playlist in newPlaylist)
                {
                    Console.WriteLine(playlist.Id);
                    
                    if (await _pRepo.FindByIdAsync(id) == null)
                    {
                        List<Track> Tracks = await getUserTracks.GetPlaylistTrack(_spotifyClient, _userSpotifyId, playlist.Id);
                        await _pRepo.AddAsync(playlist);
                        foreach (Track track in Tracks)
                        {
                            Console.WriteLine(track.Id);
                            if (await _tRepo.FindByIdAsync(track.Id) == null)
                            {
                                await _tRepo.AddAsync(track);
                            }
                            await _tRepo.AddTrackPlaylistMap(track.Id, playlist.Id);
                        }
                    }
                }
            }

            var currentUserID = await _userManager.GetUserIdAsync(usr);
            var userPlaylists = _pRepo.GetAllWithUser().Where(i => i.User.Id == currentUserID);
            
            viewModel._PlaylistsDB = await userPlaylists.ToListAsync();
            return View(viewModel);
        }

        // GET: Playlists/Details/5
        public IActionResult DetailsFromSearch(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usr = GetCurrentUserAsync();
            if (usr == null)
            {
                return NotFound();
            }
            var playlist = _pRepo.GetAllWithUser().Where(i => i.Id == id).FirstOrDefault();
            if (playlist == null)
            {
                return NotFound();
            }
            var Tracks = _pRepo.GetAllPlaylistTracks(playlist);
            foreach(Track t in Tracks)
            {
                if(t.Duration == null)
                {
                    t.Duration = Utils.AlgorithmicOperations.MsConversion.ConvertMsToMinSec(t.DurationMs);
                }
            }
            var TracksForPlaylistModel = new TracksForPlaylist
            {
                Playlist = playlist,
                Tracks = Tracks,
                PUser = _puRepo.GetPUserByID(usr.Result.Id)
            };
            return View(TracksForPlaylistModel);
        }

        // GET: Playlists/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_puRepo.GetAll(), "Id", "Id");
            return View();
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Description,Href,Name,Public,Collaborative,Uri")] Playlist playlist)
        {
            Random ran = new Random();

            string b = "a1b2c3d4e5f6g7h8i9jklmnopqrstuvwxyz";
            int length = 25;
            string randomId = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(35);
                randomId = randomId + b.ElementAt(a);
            }

            Console.WriteLine("The random alphabet generated is: {0}", randomId);

            IdentityUser usr = await GetCurrentUserAsync();
            playlist.UserId = usr.Id;
            playlist.Id = randomId;
            playlist.Href = " ";

            //playlist.UserId =
            if (ModelState.IsValid)
            {
                await _pRepo.AddAsync(playlist);

                string id = playlist.Id;
                return RedirectToAction("SearchTracks", "Tracks", new { id = id });
            }
            ViewData["UserId"] = new SelectList(_puRepo.GetAll(), "Id", "Id", playlist.UserId);
            return View(playlist);
        }

        // GET: Playlists/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _pRepo.FindByIdAsync(id);
            if (playlist == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_puRepo.GetAll(), "Id", "Id", playlist.UserId);
            return View(playlist);
        }

        // POST: Playlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserId,Description,Href,Name,Public,Collaborative,Uri")] Playlist playlist, string UserId, string PlaylistId)
        {
            if (id != playlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //----Added this code for Href. Delete once fixed and href can be null----
                    if (playlist.Href == null || playlist.Href == "")
                    {
                        playlist.Href = "No Href Here";
                    }
                    if(playlist.Id == null)
                    {
                        playlist.Id = id;
                    }
                    if(playlist.UserId == null)
                    {
                        playlist.UserId = UserId;
                    }
                    //--------------------------------------
                    //_context.Update(playlist);
                    //await _context.SaveChangesAsync();
                    await _pRepo.UpdateAsync(playlist);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _pRepo.ExistsAsync(playlist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserPlaylists));
            }
            ViewData["UserId"] = new SelectList(_puRepo.GetAll(), "Id", "Id", playlist.UserId);
            return View(playlist);
        }

        // GET: Playlists/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var playlist = _pRepo.GetAllWithUser().Where(i => i.Id == id);
            var playlist = await _pRepo.FindByIdAsync(id);

            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //Added to remove tracks too
            var playlistmaps = _pRepo.GetPlaylistTrackMaps(id);
            foreach(var map in playlistmaps)
            {
                await _pRepo.DeleteTrackMapAsync(map);
            }
            //----------------------------
            var playlist = await _pRepo.FindByIdAsync(id);
            await _pRepo.DeleteAsync(playlist);

            return RedirectToAction(nameof(UserPlaylists));
        }

    }
}
