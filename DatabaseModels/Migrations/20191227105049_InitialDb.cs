using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseModels.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarRentals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    BookingNumber = table.Column<string>(maxLength: 30, nullable: true),
                    CustomerSocialSecurityNumber = table.Column<string>(maxLength: 12, nullable: true),
                    Rented = table.Column<DateTime>(nullable: true),
                    Returned = table.Column<DateTime>(nullable: true),
                    CarMilageAtRentInKm = table.Column<decimal>(type: "decimal(16, 4)", nullable: true),
                    CarMilageAtReturnInKm = table.Column<decimal>(type: "decimal(16, 4)", nullable: true),
                    CarCategory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRentals", x => x.Id);
                });

            migrationBuilder.CreateIndex(name: "IX_CarRentals_BookingNumber", table: "CarRentals", column: "BookingNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "CarRentals");
        }
    }
}
