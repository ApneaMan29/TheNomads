using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Playlistofy.Models
{
    [Table("PlaylistKeywordMap")]
    public partial class PlaylistKeywordMap
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int KeywordId { get; set; }
        [Required]
        [StringLength(450)]
        public string PlaylistId { get; set; }

        [ForeignKey(nameof(KeywordId))]
        [InverseProperty("PlaylistKeywordMaps")]
        public virtual Keyword Keyword { get; set; }
        [ForeignKey(nameof(PlaylistId))]
        [InverseProperty("PlaylistKeywordMaps")]
        public virtual Playlist Playlist { get; set; }
    }
}
