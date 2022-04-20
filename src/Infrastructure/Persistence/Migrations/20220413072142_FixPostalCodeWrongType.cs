using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyMed.Infrastructure.Persistence.Migrations
{
    public partial class FixPostalCodeWrongType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "OfficeLocations",
                type: "char(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar",
                oldMaxLength: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "OfficeLocations",
                type: "nchar",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(6)",
                oldMaxLength: 6);
        }
    }
}
