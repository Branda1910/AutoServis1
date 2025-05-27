using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServis.Migrations
{
    /// <inheritdoc />
    public partial class DodavanjeVrsteUslugeICijene : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cijena",
                table: "NarudzbeServisa",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "VrstaUsluge",
                table: "NarudzbeServisa",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cijena",
                table: "NarudzbeServisa");

            migrationBuilder.DropColumn(
                name: "VrstaUsluge",
                table: "NarudzbeServisa");
        }
    }
}
