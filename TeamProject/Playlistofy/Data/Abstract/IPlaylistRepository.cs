using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Data.Abstract
{
    public interface IPlaylistRepository : IRepository<Playlist>
    {
        public List<Playlist> FindPlaylistsBySearch(string searchQuery);
        public IQueryable<Playlist> GetAllWithUser();
        public List<Track> GetAllPlaylistTracks(Playlist playlist);
        public List<PlaylistTrackMap> GetPlaylistTrackMaps(string Id);
        public Task DeleteTrackMapAsync(PlaylistTrackMap trackMap);
        public PlaylistTrackMap GetPlaylistTrackMap(string tId, string pId);
        public Task AddTrackPlaylistMap(string pUId, string pId);
    }

    
}
