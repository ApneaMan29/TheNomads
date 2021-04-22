﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Playlistofy.Models;
using Playlistofy.Models.ViewModel;
using Playlistofy.Utils;

namespace Playlistofy.Controllers
{
    public class TracksController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SpotifyDBContext _context;

        private readonly IConfiguration _config;

        private static string _spotifyClientId;
        private static string _spotifyClientSecret;

        public TracksController(IConfiguration config, UserManager<IdentityUser> userManager, SpotifyDBContext context)

        private readonly SpotifyDbContext _context;

        public TracksController(SpotifyDbContext context)

        {
            _userManager = userManager;
            _context = context;

            _config = config;

            _spotifyClientId = config["Spotify:ClientId"];
            _spotifyClientSecret = config["Spotify:ClientSecret"];
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
            var spotifyDBContext = _context.Tracks;//.Include(t => t.Playlist);
            return View(await spotifyDBContext.ToListAsync());
        }

        // GET: Tracks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.PlaylistTrackMaps)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (track == null)
            {
                return NotFound();
            }
            var Tracks = 
                from playlist in _context.Playlists 
                join PlaylistTrackMap in _context.PlaylistTrackMaps on playlist.Id equals PlaylistTrackMap.PlaylistId
                where (PlaylistTrackMap.TrackId == track.Id) 
                select track;

            
            var InfoForTracksModel = new InfoForTracks
            {
                Track = track,
                PlaylistTrackMaps = track.PlaylistTrackMaps
            };
            return View(InfoForTracksModel);
        }

        // GET: Tracks/Create
        public IActionResult Create()
        {
            //ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id");
            return View();
        }

        // POST: Tracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DiscNumber,DurationMs,Explicit,Href,IsPlayable,Name,Popularity,PreviewUrl,TrackNumber,Uri,IsLocal")] Track track)
        {
            if (ModelState.IsValid)
            {
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id", track.PlaylistId);
            return View(track);
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            //ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id", track.PlaylistId);
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,DiscNumber,DurationMs,Explicit,Href,IsPlayable,Name,Popularity,PreviewUrl,TrackNumber,Uri,IsLocal")] Track track)
        {
            if (id != track.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(track);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackExists(track.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["PlaylistId"] = new SelectList(_context.Playlists, "Id", "Id", track.PlaylistId);
            return View(track);
        }

        // GET: Tracks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                //.Include(t => t.Playlist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var track = await _context.Tracks.FindAsync(id);
            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackExists(string id)
        {
            return _context.Tracks.Any(e => e.Id == id);
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<IActionResult> SearchTracks(string SearchKeywords)
        {
            //Checks if a user is logged in before proceeding, else takes them to login page
            IdentityUser usr = await GetCurrentUserAsync();
            if (usr == null) { return RedirectToPage("/Account/Login", new { area = "Identity" }); }

            //Creates searchSpotify folder with necessary functions to use later
            var SearchSpotify = new searchSpotify(_userManager, _spotifyClientId, _spotifyClientSecret);
            //Creates spotify client
            var _spotifyClient = SearchSpotify.makeSpotifyClient(_spotifyClientId, _spotifyClientSecret);
            //Search and return a list of tracks
            var SearchTracks = await SearchSpotify.SearchTracks(_spotifyClient, SearchKeywords);

            return View(SearchTracks);
        }
    }
}
