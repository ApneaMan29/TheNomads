using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Models.ViewModel
{
    public class PlaylistTagModel
    {
        [BindProperty]
        public Playlist playlist { get; set; }
        public List<string> tags { get; set; }
    }
}
