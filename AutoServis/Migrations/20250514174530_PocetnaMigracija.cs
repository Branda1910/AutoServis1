using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServis.Migrations
{
    /// <inheritdoc />
    public partial class PocetnaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ime = table.Column<string>(type: "TEXT", nullable: false),
                    Prezime = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Lozinka = table.Column<string>(type: "TEXT", nullable: false),
                    Aktiviran = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NarudzbeServisa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailKorisnika = table.Column<string>(type: "TEXT", nullable: false),
                    OpisProblema = table.Column<string>(type: "TEXT", nullable: false),
                    PredlozeniDatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PotvrdjeniDatum = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NarudzbeServisa", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "NarudzbeServisa");
        }
    }
}
