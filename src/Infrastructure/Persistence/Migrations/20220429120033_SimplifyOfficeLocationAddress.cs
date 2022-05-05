using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMed.Infrastructure.Persistence.Migrations
{
    public partial class SimplifyOfficeLocationAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "OfficeLocations");

            migrationBuilder.DropColumn(
                name: "House",
                table: "OfficeLocations");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "OfficeLocations");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "OfficeLocations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "OfficeLocations",
                type: "character varying(109)",
                maxLength: 109,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "OfficeLocations");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "OfficeLocations",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "House",
                table: "OfficeLocations",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "OfficeLocations",
                type: "char(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "OfficeLocations",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
