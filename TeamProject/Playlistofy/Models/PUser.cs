using System;
using System.Collections.Generic;

#nullable disable

namespace Playlistofy.Models
{
    public partial class PUser
    {
        public PUser()
        {
            Playlists = new HashSet<Playlist>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public int Followers { get; set; }
        public string DisplayName { get; set; }
        public string ImageUrl { get; set; }
        public string SpotifyUserId { get; set; }
        public string Href { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
        public virtual ICollection<FollowedPlaylist> FollowedPlaylists { get; set; }
    }
}
