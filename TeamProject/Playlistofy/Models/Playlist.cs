using System;
using System.Collections.Generic;

#nullable disable

namespace Playlistofy.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistTrackMaps = new HashSet<PlaylistTrackMap>();
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string Name { get; set; }
        public bool? Public { get; set; }
        public bool? Collaborative { get; set; }
        public string Uri { get; set; }

        public virtual PUser User { get; set; }
        public virtual ICollection<PlaylistTrackMap> PlaylistTrackMaps { get; set; }
        public virtual ICollection<FollowedPlaylist> FollowedPlaylists { get; set; }
    }
}
