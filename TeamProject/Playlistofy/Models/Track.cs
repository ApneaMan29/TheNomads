using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Playlistofy.Models
{
    [Table("Track")]
    public partial class Track
    {
        public Track()
        {
            ArtistTrackMaps = new HashSet<ArtistTrackMap>();
            PlaylistTrackMaps = new HashSet<PlaylistTrackMap>();
            TrackAlbumMaps = new HashSet<TrackAlbumMap>();
        }

        [Key]
        public string Id { get; set; }
        public int? DiscNumber { get; set; }
        public int DurationMs { get; set; }
        public bool Explicit { get; set; }
        [StringLength(450)]
        public string Href { get; set; }
        public bool IsPlayable { get; set; }
        [StringLength(450)]
        public string Name { get; set; }
        public int? Popularity { get; set; }
        [StringLength(450)]
        public string PreviewUrl { get; set; }
        public int TrackNumber { get; set; }
        [StringLength(450)]
        public string Uri { get; set; }
        public bool IsLocal { get; set; }
        [StringLength(450)]
        public string Duration { get; set; }

        [InverseProperty(nameof(ArtistTrackMap.Track))]
        public virtual ICollection<ArtistTrackMap> ArtistTrackMaps { get; set; }
        [InverseProperty(nameof(PlaylistTrackMap.Track))]
        public virtual ICollection<PlaylistTrackMap> PlaylistTrackMaps { get; set; }
        [InverseProperty(nameof(TrackAlbumMap.Track))]
        public virtual ICollection<TrackAlbumMap> TrackAlbumMaps { get; set; }
    }
}
