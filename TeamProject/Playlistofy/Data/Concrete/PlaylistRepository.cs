﻿using Microsoft.EntityFrameworkCore;
using Playlistofy.Data.Abstract;
using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Data.Concrete
{
    public class PlaylistRepository : Repository<Playlist>, IPlaylistRepository
    {
        private DbSet<PlaylistTrackMap> trackMaps;

        public PlaylistRepository(SpotifyDbContext ctx) : base(ctx)
        {
            trackMaps = _context.Set<PlaylistTrackMap>();
        }

        public List<Playlist> FindPlaylistsBySearch(string searchQuery)
        {
            var t = _dbSet.Where(a => a.Name.Contains(searchQuery)).ToList();
            return t;
        }

        public IQueryable<Playlist> GetAllWithUser()
        {
            var set = _context.Set<Playlist>().Include("User");
            return set;
        }

        public List<Track> GetAllPlaylistTracks(Playlist playlist)
        {
            List<Playlist> playlists = _dbSet.Include("PlaylistTrackMaps").ToList();
            Playlist pl = _dbSet.Find(playlist.Id);
            List<Track> tracks = new List<Track>();
            foreach (var i in pl.PlaylistTrackMaps)
            {
                tracks.Add(_context.Set<Track>().Find(i.TrackId));
            }
            return tracks;
        }

        public List<PlaylistTrackMap> GetPlaylistTrackMaps(string Id)
        {
            List<Playlist> playlists = _dbSet.Include("PlaylistTrackMaps").ToList();
            Playlist pl = _dbSet.Find(Id);
            List<PlaylistTrackMap> maps = pl.PlaylistTrackMaps.ToList();
            return maps;
        }

        public virtual async Task DeleteTrackMapAsync(PlaylistTrackMap trackMap)
        {
            if (trackMap == null)
            {
                throw new Exception("Entity to delete was null");
            }
            else
            {
                
                trackMaps.Remove(trackMap);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public PlaylistTrackMap GetPlaylistTrackMap(string tId, string pId)
        {
            var map = trackMaps.Where(i => i.TrackId == tId && i.PlaylistId == pId).FirstOrDefault();
            return map;
        }

        public async Task<List<Playlist>> GetMostRecentPlaylists_5Async()
        {
            var playlists = new List<Playlist>();
            //var count = await _dbSet.CountAsync();
            //var DBPlaylists = _dbSet.ToList().ElementAtOrDefault(0);
            //int playlistsCount = DBPlaylists.Count();
            var DBplaylist = await _dbSet.ToListAsync();
            //var DBplaylist = DBplaylist.OrderByDescending(x => x.Id);
            var countPlaylist = DBplaylist.Count;

            for (int i = 0; i < 5; i++)
            {
                --countPlaylist;
                var playlist = DBplaylist.ElementAtOrDefault(countPlaylist);
                playlists.Add(playlist);
            }

            return playlists;
        }
    }
}
