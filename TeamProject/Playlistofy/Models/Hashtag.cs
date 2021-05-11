using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Playlistofy.Models
{
    [Table("Hashtag")]
    public partial class Hashtag
    {
        public Hashtag()
        {

        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Column("Hashtag")]
        [StringLength(450)]
        [MinLength(2)]
        [RegularExpression(@"^#.*$")]
        public string HashTag1 { get; set; }

        [InverseProperty(nameof(PlaylistHashtagMap.Hashtag))]
        public virtual ICollection<PlaylistHashtagMap> PlaylistHashtagMaps { get; set; }
    }
}
