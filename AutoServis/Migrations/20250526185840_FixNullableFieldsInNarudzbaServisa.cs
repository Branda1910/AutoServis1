using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServis.Migrations
{
    /// <inheritdoc />
    public partial class FixNullableFieldsInNarudzbaServisa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PotvrdjeniDatum",
                table: "NarudzbeServisa",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumNarudzbe",
                table: "NarudzbeServisa",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumNarudzbe",
                table: "NarudzbeServisa");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PotvrdjeniDatum",
                table: "NarudzbeServisa",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }
    }
}
