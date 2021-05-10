CREATE TABLE [PUser]
(
	[Id] NVARCHAR(450) NOT NULL, 
    [UserName] NVARCHAR(256) NULL, 
    [NormalizedUserName] NVARCHAR(256) NULL, 
    [Email] NVARCHAR(256) NULL, 
    [NormalizedEmail] NVARCHAR(256) NULL, 
    [EmailConfirmed] BIT NOT NULL DEFAULT 0, 
    [PasswordHash] NVARCHAR(MAX) NULL, 
    [SecurityStamp] NVARCHAR(MAX) NULL, 
    [ConcurrencyStamp] NVARCHAR(MAX) NULL, 
    [PhoneNumber] NVARCHAR(MAX) NULL, 
    [PhoneNumberConfirmed] BIT NOT NULL DEFAULT 0, 
    [TwoFactorEnabled] BIT NOT NULL DEFAULT 0, 
    [LockoutEnd] DATETIMEOFFSET NULL, 
    [LockoutEnabled] BIT NOT NULL DEFAULT 0, 
    [AccessFailedCount] INT NULL,
    --Extra Added--
    [Followers] INT NOT NULL DEFAULT 0,
    [DisplayName] NVARCHAR(256),
    [ImageUrl] NVARCHAR(256),
    [SpotifyUserId] NVARCHAR(256),
    [Href] NVARCHAR(256)
)
GO

CREATE TABLE [Playlist]
(
    [Id] nvarchar(450) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    [Description] nvarchar(450),
    [Href] nvarchar(max),
    [Name] nvarchar(450),
    [Public] bit DEFAULT 0,
    [Collaborative] bit DEFAULT 0,
    [URI] nvarchar(max),
    [TrackCount] INT
)
GO

CREATE TABLE [Track]
(
    [Id] NVARCHAR(450) NOT NULL,
    [DiscNumber] INT,
    [DurationMs] INT NOT NULL DEFAULT 0,
    [Explicit] BIT NOT NULL DEFAULT 0,
    [Href] NVARCHAR(450),
    [IsPlayable] BIT NOT NULL DEFAULT 0,
    [Name] NVARCHAR(450),
    [Popularity] INT,
    [PreviewUrl] NVARCHAR(450),
    [TrackNumber] INT NOT NULL DEFAULT 0,
    [Uri] NVARCHAR(450),
    [IsLocal] BIT NOT NULL DEFAULT 0,
    [Duration] NVARCHAR(450),
    [Artist] NVARCHAR(450)
)
GO

CREATE TABLE [Album]
(
    [Id] NVARCHAR(450) NOT NULL,
    [AlbumType] NVARCHAR(450),
    [Label] NVARCHAR(450),
    [Name] NVARCHAR(450),
    [Popularity] INT,
    [ReleaseDate] NVARCHAR(450),
    [ReleaseDatePrecision] NVARCHAR(450)
)
GO

CREATE TABLE [Artist]
(
    [Id] NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(450) NOT NULL,
    [Popularity] INT,
    [Uri] NVARCHAR(450)
)
GO

CREATE TABLE [ArtistTrackMap]
(
    [Id] INT NOT NULL IDENTITY (1,1),
    [ArtistId] NVARCHAR(450) NOT NULL,
    [TrackId] NVARCHAR(450) NOT NULL
)
GO

CREATE TABLE [TrackAlbumMap]
(
    [Id] INT NOT NULL IDENTITY (1,1),
    [TrackId] NVARCHAR(450),
    [AlbumId] NVARCHAR(450)
)
GO

CREATE TABLE [PlaylistTrackMap]
(
    [ID] INT NOT NULL IDENTITY (1,1),
    [PlaylistID] NVARCHAR(450) NOT NULL,
    [TrackID] NVARCHAR(450) NOT NULL
)
GO

CREATE TABLE [AlbumArtistMap]
(
    [Id] INT NOT NULL IDENTITY (1,1),
    [ArtistId] NVARCHAR(450),
    [AlbumId] NVARCHAR(450)
)
GO

CREATE TABLE [FollowedPlaylist]
(
    [Id] INT NOT NULL IDENTITY (1,1),
    [PlaylistId] NVARCHAR(450),
    [PUserId] NVARCHAR(450)
)
GO

ALTER TABLE [FollowedPlaylist] ADD CONSTRAINT [PK_FollowedPlaylist] PRIMARY KEY CLUSTERED ([Id]ASC)
GO

ALTER TABLE [FollowedPlaylist] ADD CONSTRAINT [FollowedPlaylist_FK_Playlist] FOREIGN KEY ([PlaylistID]) REFERENCES [Playlist] ([Id])
GO

ALTER TABLE [FollowedPlaylist] ADD CONSTRAINT [FollowedPlaylist_FK_PUser] FOREIGN KEY ([PUserId]) REFERENCES [PUser] ([Id])
GO


ALTER TABLE [PUser] ADD CONSTRAINT [PK_PUser] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [Playlist] ADD CONSTRAINT [PK_Playlist] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [Track] ADD CONSTRAINT [PK_Track] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [Album] ADD CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [TrackAlbumMap] ADD CONSTRAINT [TrackAlbumMap_PK] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [PlaylistTrackMap] ADD CONSTRAINT [PlaylistTrackMap_PK] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [AlbumArtistMap] ADD CONSTRAINT [AlbumArtistMap_PK] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [TrackAlbumMap] ADD CONSTRAINT [TrackAlbumMap_PK] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [PlaylistTrackMap] ADD CONSTRAINT [PlaylistTrackMap_PK] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [AlbumArtistMap] ADD CONSTRAINT [AlbumArtistMap_PK] PRIMARY KEY CLUSTERED ([Id] ASC)
GO

ALTER TABLE [Playlist] ADD CONSTRAINT [Playlist_FK_PUser] FOREIGN KEY ([UserId]) REFERENCES [PUser] ([ID])
GO

ALTER TABLE [PlaylistTrackMap] ADD CONSTRAINT [PlaylistTrackMap_FK_Playlist] FOREIGN KEY ([PlaylistId]) REFERENCES [Playlist] ([ID])
GO

ALTER TABLE [PlaylistTrackMap] ADD CONSTRAINT [PlaylistTrackMap_FK_Track] FOREIGN KEY ([TrackId]) REFERENCES [Track] ([ID])
GO

ALTER TABLE [ArtistTrackMap] ADD CONSTRAINT [ArtistTrackMap_FK_Artist] FOREIGN KEY ([ArtistId]) REFERENCES [Artist] ([ID])
GO

ALTER TABLE [ArtistTrackMap] ADD CONSTRAINT [ArtistTrackMap_FK_Track] FOREIGN KEY ([TrackId]) REFERENCES [Track] ([ID])
GO

ALTER TABLE [TrackAlbumMap] ADD CONSTRAINT [TrackAlbumMap_FK_Album] FOREIGN KEY ([AlbumId]) REFERENCES [Album] ([ID])
GO

ALTER TABLE [TrackAlbumMap] ADD CONSTRAINT [TrackAlbumMap_FK_Track] FOREIGN KEY ([TrackId]) REFERENCES [Track] ([ID])
GO

ALTER TABLE [AlbumArtistMap] ADD CONSTRAINT [AlbumArtistMap_FK_Artist] FOREIGN KEY ([ArtistId]) REFERENCES [Artist] ([ID])
GO

ALTER TABLE [AlbumArtistMap] ADD CONSTRAINT [AlbumArtistMap_FK_Album] FOREIGN KEY ([AlbumId]) REFERENCES [Album] ([ID])
GO
