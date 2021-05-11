using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Playlistofy.Models
{
    [Table("Artist")]
    public partial class Artist
    {
        public Artist()
        {
            AlbumArtistMaps = new HashSet<AlbumArtistMap>();
            ArtistTrackMaps = new HashSet<ArtistTrackMap>();
        }

        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(450)]
        public string Name { get; set; }
        public int? Popularity { get; set; }
        [StringLength(450)]
        public string Uri { get; set; }

        [InverseProperty(nameof(AlbumArtistMap.Artist))]
        public virtual ICollection<AlbumArtistMap> AlbumArtistMaps { get; set; }
        [InverseProperty(nameof(ArtistTrackMap.Artist))]
        public virtual ICollection<ArtistTrackMap> ArtistTrackMaps { get; set; }
    }
}
