using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Playlistofy.Models
{
    [Table("PlaylistHashtagMap")]
    public partial class PlaylistHashtagMap
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int HashtagId { get; set; }
        [Required]
        [StringLength(450)]
        public string PlaylistId { get; set; }

        [ForeignKey(nameof(HashtagId))]
        [InverseProperty("PlaylistHashtagMaps")]
        public virtual Hashtag Hashtag { get; set; }
        [ForeignKey(nameof(PlaylistId))]
        [InverseProperty("PlaylistHashtagMaps")]
        public virtual Playlist Playlist { get; set; }
    }
}
