using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Playlistofy.Models
{
    [Table("AlbumArtistMap")]
    public partial class AlbumArtistMap
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        public string ArtistId { get; set; }
        [StringLength(450)]
        public string AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        [InverseProperty("AlbumArtistMaps")]
        public virtual Album Album { get; set; }
        [ForeignKey(nameof(ArtistId))]
        [InverseProperty("AlbumArtistMaps")]
        public virtual Artist Artist { get; set; }
    }
}
