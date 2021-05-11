using Microsoft.EntityFrameworkCore;
using Playlistofy.Data.Abstract;
using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Data.Concrete
{
    public class HashtagRepository: Repository<Hashtag>, IHashtagRepository
    {
        public HashtagRepository(SpotifyDbContext ctx) : base(ctx)
        {

        }

        public Hashtag FindByHashtag(string word)
        {
            Hashtag h = _dbSet.Where(i => i.HashTag1.ToLower() == word.ToLower()).FirstOrDefault();
            return h;
        }

        public async Task AddPlaylistHashtagMap(string pId, int hId)
        {
            _context.Add<PlaylistHashtagMap>(new PlaylistHashtagMap()
            {
               PlaylistId = pId,
               HashtagId = hId
            });
            await _context.SaveChangesAsync();
        }

        public async Task RemovePlaylistHashtagMap(int hId)
        {
            PlaylistHashtagMap hashMap = _context.Set<PlaylistHashtagMap>().Find(hId);
            _context.Remove<PlaylistHashtagMap>(hashMap);
            await _context.SaveChangesAsync();
        }

        public List<Hashtag> GetAllForPlaylist(string pId)
        {
            List<Hashtag> hashtags = new List<Hashtag>();
            Playlist playlist = _context.Set<Playlist>().AsQueryable().Include("PlaylistHashtagMaps").Where(i => i.Id == pId).FirstOrDefault();
            foreach(var i in playlist.PlaylistHashtagMaps)
            {
                hashtags.Add(_dbSet.Find(i.HashtagId));
            }
            return hashtags;
        }

        public List<Playlist> SearchForPlaylist(string word)
        {
            Hashtag hashtag = _dbSet.Include("PlaylistHashtagMaps").Where(i => i.HashTag1 == word).FirstOrDefault();
            List<Playlist> playlists = new List<Playlist>();
            foreach(var a in hashtag.PlaylistHashtagMaps)
            {
                playlists.Add(_context.Set<Playlist>().Where(i => i.Id == a.PlaylistId).FirstOrDefault());
            }
            return playlists;
        }
    }
}
