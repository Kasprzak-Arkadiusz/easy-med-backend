using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMed.Infrastructure.Persistence.Migrations
{
    public partial class RestoreDoctorOfficeLocationRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeLocations_Users_DoctorId",
                table: "OfficeLocations");

            migrationBuilder.DropIndex(
                name: "IX_OfficeLocations_DoctorId",
                table: "OfficeLocations");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "OfficeLocations");

            migrationBuilder.AddColumn<int>(
                name: "OfficeLocationId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OfficeLocationId",
                table: "Users",
                column: "OfficeLocationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OfficeLocations_OfficeLocationId",
                table: "Users",
                column: "OfficeLocationId",
                principalTable: "OfficeLocations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_OfficeLocations_OfficeLocationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OfficeLocationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OfficeLocationId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "OfficeLocations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLocations_DoctorId",
                table: "OfficeLocations",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeLocations_Users_DoctorId",
                table: "OfficeLocations",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
