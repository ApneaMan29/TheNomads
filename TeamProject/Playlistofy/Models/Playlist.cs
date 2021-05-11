using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Playlistofy.Models
{
    [Table("Playlist")]
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistHashtagMaps = new HashSet<PlaylistHashtagMap>();
            PlaylistKeywordMaps = new HashSet<PlaylistKeywordMap>();
            PlaylistTrackMaps = new HashSet<PlaylistTrackMap>();
            FollowedPlaylists = new HashSet<FollowedPlaylist>();
        }

        [Key]
        public string Id { get; set; }
        //[Required]
        [StringLength(450)]
        public string UserId { get; set; }
        [StringLength(450)]
        public string Description { get; set; }
        public string Href { get; set; }
        [StringLength(450)]
        public string Name { get; set; }
        public bool? Public { get; set; }
        public bool? Collaborative { get; set; }
        [Column("URI")]
        public string Uri { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(PUser.Playlists))]
        public virtual PUser User { get; set; }
        [InverseProperty(nameof(PlaylistHashtagMap.Playlist))]
        public virtual ICollection<PlaylistHashtagMap> PlaylistHashtagMaps { get; set; }
        [InverseProperty(nameof(PlaylistKeywordMap.Playlist))]
        public virtual ICollection<PlaylistKeywordMap> PlaylistKeywordMaps { get; set; }
        [InverseProperty(nameof(PlaylistTrackMap.Playlist))]
        public virtual ICollection<PlaylistTrackMap> PlaylistTrackMaps { get; set; }
        [InverseProperty(nameof(FollowedPlaylist.playlist))]
        public virtual ICollection<FollowedPlaylist> FollowedPlaylists { get; set; }

        public DateTime? DateCreated { get; set; }
        //public int TrackCount {get; set;}

    }
}
