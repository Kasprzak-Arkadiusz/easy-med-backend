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

            migrationBuilder.CreateIndex(
                name: "IX_OfficeLocations_DoctorId",
                table: "OfficeLocations",
                column: "DoctorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeLocations_Users_DoctorId",
                table: "OfficeLocations",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeLocations_Users_DoctorId",
                table: "OfficeLocations");

            migrationBuilder.DropIndex(
                name: "IX_OfficeLocations_DoctorId",
                table: "OfficeLocations");

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
