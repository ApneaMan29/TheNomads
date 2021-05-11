using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Playlistofy.Models.ViewModel
{
    public class TracksForPlaylist
    {
        public Playlist Playlist { get; set; }
        public List<Track> Tracks { get; set; }
        public PUser PUser { get; set; }
        public List<string> Tags { get; set; }
    }
}
