using Playlistofy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Data.Abstract
{
    public interface IHashtagRepository: IRepository<Hashtag>
    {
        Hashtag FindByHashtag(string word);
        Task AddPlaylistHashtagMap(string pId, int hId);
        Task RemovePlaylistHashtagMap(int hId);
        List<Hashtag> GetAllForPlaylist(string pId);
        List<Playlist> SearchForPlaylist(string word);
    }
}
