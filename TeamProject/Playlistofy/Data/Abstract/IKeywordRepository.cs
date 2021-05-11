using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Data.Abstract
{
    public interface IKeywordRepository: IRepository<Keyword>
    {
        public Keyword FindByKeyword(string word);
        Task AddPlaylistKeywordMap(string pId, int kId);
        Task RemovePlaylistKeywordMap(int keyMapId);
        List<Keyword> GetAllForPlaylist(string pId);
        List<Playlist> SearchForPlaylist(string word);
    }
}
