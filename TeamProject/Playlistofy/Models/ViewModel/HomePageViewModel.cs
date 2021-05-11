using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Playlistofy.Models.ViewModel
{
    public class HomePageViewModel
    {
        //New Home Page Variables
        public string UserID { get; set; }
        public List<Playlist> SearchedPlaylists { get; set; }
        public List<string> SearchedPlaylistsUsers { get; set; }
        public List<Playlist> RecentPlaylists { get; set; }

        [Required(ErrorMessage = "Unquerable Search - Try Again")]
        [MinLength(2, ErrorMessage = "Search Too Short - Try Again")]
        [MaxLength(50, ErrorMessage = "Search Too Long - Try Again")]
        public string SearchingPlaylistParameter { get; set; }

        //Old Home Page Variables
        public IEnumerable<Playlist> UserPlaylists { get; set; }
    }
}