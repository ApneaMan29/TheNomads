using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playlistofy.Data.Abstract;
using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Controllers
{
    public class SearchController : Controller
    {
        private readonly IPlaylistRepository _pRepo;
        private readonly ITrackRepository _tRepo;
        private readonly IAlbumRepository _aRepo;
        private readonly IArtistRepository _arRepo;
        private readonly IKeywordRepository _kRepo;
        private readonly IHashtagRepository _hRepo;

        public SearchController(IPlaylistRepository pRepo, ITrackRepository tRepo, IAlbumRepository aRepo, IArtistRepository arRepo, IKeywordRepository kRepo, IHashtagRepository hRepo)
        {
            _pRepo = pRepo;
            _tRepo = tRepo;
            _aRepo = aRepo;
            _arRepo = arRepo;
            _kRepo = kRepo;
            _hRepo = hRepo;
        }

        public ActionResult Search(string searchType, string searchQuery)
        {
            if(searchType == "Album")
            {
                return RedirectToAction("AlbumSearch", new { searchTerm = searchQuery });
            }else if(searchType == "Playlist")
            {
                return RedirectToAction("PlaylistSearch", new { searchTerm = searchQuery });
            }
            else if (searchType == "Track")
            {
                return RedirectToAction("TrackSearch", new { searchTerm = searchQuery });
            }
            else if (searchType == "Artist")
            {
                return RedirectToAction("ArtistSearch", new { searchTerm = searchQuery });
            }else if (searchType == "Tags")
            {
                return RedirectToAction("TagSearch", new { searchTerm = searchQuery });
            }
            return View();
        }
        public ViewResult PlaylistSearch(string searchTerm)
        {
            var a = _pRepo.FindPlaylistsBySearch(searchTerm);
            return View(a);
        }

        public ViewResult TrackSearch(string searchTerm)
        {
            var a = _tRepo.FindTracksBySearch(searchTerm);
            return View(a);
        }

        public ViewResult ArtistSearch(string searchTerm)
        {
            var a = _arRepo.FindArtistsBySearch(searchTerm);
            return View(a);
        }

        public ViewResult AlbumSearch(string searchTerm)
        {
            var a = _aRepo.FindAlbumsBySearch(searchTerm);
            return View(a);
        }

        public ViewResult TagSearch(string searchTerm)
        {
            string[] list = searchTerm.Split(',', ' ');
            List<string> kwList = list.ToList();
            List<Playlist> playlists = new List<Playlist>();
            foreach (var word in kwList)
            {
                if (word.Length > 0)
                {
                    if (word.StartsWith("#"))
                    {
                        if (_hRepo.FindByHashtag(word) != null)
                        {
                            List<Playlist> templist = _hRepo.SearchForPlaylist(word);
                            foreach (Playlist p in templist)
                            {
                                if (!playlists.Contains(p))
                                {
                                    playlists.Add(p);
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        if (_kRepo.FindByKeyword(word) != null)
                        {
                            List<Playlist> templist = _kRepo.SearchForPlaylist(word);
                            foreach (Playlist p in templist)
                            {
                                if (!playlists.Contains(p))
                                {
                                    playlists.Add(p);
                                }
                            }
                        }
                    }
                }
            }
            return View("PlaylistSearch", playlists);
        }
    }
}
