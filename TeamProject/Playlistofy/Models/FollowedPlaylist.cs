using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playlistofy.Models
{
    [Table("FollowedPlaylist")]
    public partial class FollowedPlaylist
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string PlaylistId { get; set; }

        [Required]
        [StringLength(450)]
        public string PUserId { get; set; }

        [ForeignKey(nameof(PlaylistId))]
        [InverseProperty("FollowedPlaylists")]
        public virtual Playlist playlist { get; set; }

        [ForeignKey(nameof(PUserId))]
        [InverseProperty("FollowedPlaylists")]
        public virtual PUser pUser { get; set; }
    }
}
