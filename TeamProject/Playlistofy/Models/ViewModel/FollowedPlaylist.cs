using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Models.ViewModel
{
    public class FollowedPlaylist
    {
        public PUser User { get; set; }
        public Playlist Playlist { get; set; }
    }
}
