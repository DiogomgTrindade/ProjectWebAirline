using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectWebAirlineMVC.Migrations
{
    public partial class AircraftsModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Aircrafts");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Aircrafts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Aircrafts");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Aircrafts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
