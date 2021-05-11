using Microsoft.EntityFrameworkCore;
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
        private DbSet<FollowedPlaylist> playlistMaps;

        public PlaylistRepository(SpotifyDbContext ctx) : base(ctx)
        {
            trackMaps = _context.Set<PlaylistTrackMap>();
            playlistMaps = _context.Set<FollowedPlaylist>();
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
            Playlist pl = _dbSet.Include("PlaylistTrackMaps").Where(i => i.Id == playlist.Id).FirstOrDefault();
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

            var DBplaylist = await _dbSet.Include("PlaylistTrackMaps").ToListAsync();
            var list = DBplaylist.OrderBy(t => t.DateCreated);
            var countPlaylist = list.Count();

            for (int i = 0; i < 5; i++)
            {
                --countPlaylist;
                var playlist = list.ElementAtOrDefault(countPlaylist);
                playlists.Add(playlist);
            }

            return playlists;
        }

        public Playlist GetPlaylistWithAllMaps(string id)
        {
            Playlist playlist = _dbSet.Include("PlaylistTrackMaps").Include("PlaylistKeywordMaps").Include("PlaylistHashtagMaps").Include("FollowedPlaylists").Where(i => i.Id == id).FirstOrDefault();
            return playlist;
        }

        public async Task RemoveFollowedPlaylist(int Id)
        {
            FollowedPlaylist follow = _context.Set<FollowedPlaylist>().Find(Id);
            _context.Remove<FollowedPlaylist>(follow);
            await _context.SaveChangesAsync();
        }

        public async Task AddTrackPlaylistMap(string pUId, string pId)
        {
            _context.Add(new FollowedPlaylist()
            {
                PUserId = pUId,
                PlaylistId = pId
            });
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeletePlaylistMap(FollowedPlaylist pMap)
        {
            if (pMap == null)
            {
                throw new Exception("Entity to delete was null");
            }

            playlistMaps.Remove(pMap);
            await _context.SaveChangesAsync();
        }

    }
}
