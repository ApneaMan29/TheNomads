using Microsoft.EntityFrameworkCore;
using Playlistofy.Data.Abstract;
using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Data.Concrete
{
    public class KeywordRepository: Repository<Keyword>, IKeywordRepository
    {
        public KeywordRepository(SpotifyDbContext ctx) : base(ctx)
        {

        }

        public Keyword FindByKeyword(string word)
        {
            Keyword k = _dbSet.Where(i => i.Keyword1.ToLower() == word.ToLower()).FirstOrDefault();
            return k;
        }

        public async Task AddPlaylistKeywordMap(string pId, int kId)
        {
            _context.Add<PlaylistKeywordMap>(new PlaylistKeywordMap()
            {
                PlaylistId = pId,
                KeywordId = kId
            });
            await _context.SaveChangesAsync();
        }

        public async Task RemovePlaylistKeywordMap(int keyMapId)
        {
            PlaylistKeywordMap keyMap = _context.Set<PlaylistKeywordMap>().Find(keyMapId);
            _context.Remove<PlaylistKeywordMap>(keyMap);
            await _context.SaveChangesAsync();
        }

        public List<Keyword> GetAllForPlaylist(string pId)
        {
            List<Keyword> keywords = new List<Keyword>();
            Playlist playlist = _context.Set<Playlist>().AsQueryable().Include("PlaylistKeywordMaps").Where(i => i.Id == pId).FirstOrDefault();
            foreach (var i in playlist.PlaylistKeywordMaps)
            {
                keywords.Add(_dbSet.Find(i.KeywordId));
            }
            return keywords;
        }

        public List<Playlist> SearchForPlaylist(string word)
        {
            Keyword keyword = _dbSet.Include("PlaylistKeywordMaps").Where(i => i.Keyword1 == word).FirstOrDefault();
            List<Playlist> playlists = new List<Playlist>();
            foreach(var a in keyword.PlaylistKeywordMaps)
            {
                playlists.Add(_context.Set<Playlist>().Where(i => i.Id == a.PlaylistId).FirstOrDefault());
            }
            return playlists;
        }
    }
}
