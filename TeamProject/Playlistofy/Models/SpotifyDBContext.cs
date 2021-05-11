using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Playlistofy.Models
{
    public partial class SpotifyDbContext : DbContext
    {
        public SpotifyDbContext()
        {
        }

        public SpotifyDbContext(DbContextOptions<SpotifyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<AlbumArtistMap> AlbumArtistMaps { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<ArtistTrackMap> ArtistTrackMaps { get; set; }
        public virtual DbSet<Hashtag> Hashtags { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<PlaylistHashtagMap> PlaylistHashtagMaps { get; set; }
        public virtual DbSet<PlaylistKeywordMap> PlaylistKeywordMaps { get; set; }
        public virtual DbSet<PlaylistTrackMap> PlaylistTrackMaps { get; set; }
        public virtual DbSet<PUser> Pusers { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<TrackAlbumMap> TrackAlbumMaps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=AzureSpotifyDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AlbumArtistMap>(entity =>
            {
                entity.HasOne(d => d.Album)
                    .WithMany(p => p.AlbumArtistMaps)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("AlbumArtistMap_FK_Album");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.AlbumArtistMaps)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("AlbumArtistMap_FK_Artist");
            });

            modelBuilder.Entity<ArtistTrackMap>(entity =>
            {
                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistTrackMaps)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ArtistTrackMap_FK_Artist");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.ArtistTrackMaps)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ArtistTrackMap_FK_Track");
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.Property(e => e.Collaborative).HasDefaultValueSql("((0))");

                entity.Property(e => e.Public).HasDefaultValueSql("((0))");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("2021-1-01 01:01:01");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Playlists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Playlist_FK_PUser");
            });

            modelBuilder.Entity<PlaylistKeywordMap>(entity =>
            {
                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.PlaylistKeywordMaps)
                    .HasForeignKey(d => d.KeywordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PlaylistKeyMap_FK_Keyword");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistKeywordMaps)
                    .HasForeignKey(d => d.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PlaylistKeyMap_FK_Playlist");
            });

            modelBuilder.Entity<PlaylistTrackMap>(entity =>
            {
                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.PlaylistTrackMaps)
                    .HasForeignKey(d => d.PlaylistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PlaylistTrackMap_FK_Playlist");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.PlaylistTrackMaps)
                    .HasForeignKey(d => d.TrackId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PlaylistTrackMap_FK_Track");
            });

            modelBuilder.Entity<TrackAlbumMap>(entity =>
            {
                entity.HasOne(d => d.Album)
                    .WithMany(p => p.TrackAlbumMaps)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("TrackAlbumMap_FK_Album");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.TrackAlbumMaps)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("TrackAlbumMap_FK_Track");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
