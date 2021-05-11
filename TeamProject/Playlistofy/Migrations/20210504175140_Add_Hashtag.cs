using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Playlistofy.Migrations
{
    public partial class Add_Hashtag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "Hashtag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hashtag = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashtag", x => x.Id);
                });





           





            migrationBuilder.CreateTable(
                name: "PlaylistHashtagMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HashtagId = table.Column<int>(type: "int", nullable: false),
                    PlaylistId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistHashtagMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaylistHashtagMap_Hashtag_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistHashtagMap_Playlist_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });



            migrationBuilder.CreateIndex(
                name: "IX_PlaylistHashtagMap_HashtagId",
                table: "PlaylistHashtagMap",
                column: "HashtagId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistHashtagMap_PlaylistId",
                table: "PlaylistHashtagMap",
                column: "PlaylistId");
        }

           

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumArtistMap");

            migrationBuilder.DropTable(
                name: "ArtistTrackMap");

            migrationBuilder.DropTable(
                name: "PlaylistHashtagMap");

            migrationBuilder.DropTable(
                name: "PlaylistKeywordMap");

            migrationBuilder.DropTable(
                name: "PlaylistTrackMap");

            migrationBuilder.DropTable(
                name: "TrackAlbumMap");

            migrationBuilder.DropTable(
                name: "Artist");

            migrationBuilder.DropTable(
                name: "Hashtag");

            migrationBuilder.DropTable(
                name: "Keyword");

            migrationBuilder.DropTable(
                name: "Playlist");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "PUser");
        }
    }
}
