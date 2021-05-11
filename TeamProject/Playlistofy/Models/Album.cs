using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Playlistofy.Models
{
    [Table("Album")]
    public partial class Album
    {
        public Album()
        {
            AlbumArtistMaps = new HashSet<AlbumArtistMap>();
            TrackAlbumMaps = new HashSet<TrackAlbumMap>();
        }

        [Key]
        [Required]
        public string Id { get; set; }
        [StringLength(450)]
        [RegularExpression(@"album|single|compilation")]
        public string AlbumType { get; set; }
        [StringLength(450)]
        public string Label { get; set; }
        [StringLength(450)]
        public string Name { get; set; }
        [Range(1, 100)]
        public int? Popularity { get; set; }
        [StringLength(450)]
        public string ReleaseDate { get; set; }
        [StringLength(450)]
        public string ReleaseDatePrecision { get; set; }

        [InverseProperty(nameof(AlbumArtistMap.Album))]
        public virtual ICollection<AlbumArtistMap> AlbumArtistMaps { get; set; }
        [InverseProperty(nameof(TrackAlbumMap.Album))]
        public virtual ICollection<TrackAlbumMap> TrackAlbumMaps { get; set; }
    }
}
