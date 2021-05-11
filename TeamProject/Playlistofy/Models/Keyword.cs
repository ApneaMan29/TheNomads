using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Playlistofy.Models
{
    [Table("Keyword")]
    public partial class Keyword
    {
        public Keyword()
        {
            PlaylistKeywordMaps = new HashSet<PlaylistKeywordMap>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column("Keyword")]
        [StringLength(450)]
        [RegularExpression("[^#].*")]
        public string Keyword1 { get; set; }

        [InverseProperty(nameof(PlaylistKeywordMap.Keyword))]
        public virtual ICollection<PlaylistKeywordMap> PlaylistKeywordMaps { get; set; }
    }
}
