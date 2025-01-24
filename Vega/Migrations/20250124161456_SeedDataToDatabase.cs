using Microsoft.EntityFrameworkCore.Migrations;
using Vega.Domain;

#nullable disable

namespace Vega.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES ('Make 1');");
            migrationBuilder.Sql("INSERT INTO Makes (Name) VALUES ('Make 2');");

            migrationBuilder.Sql("INSERT INTO VehicleFeature (Name) VALUES ('Vehicle Fixture 1')");
            migrationBuilder.Sql("INSERT INTO VehicleFeature (Name) VALUES ('Vehicle Fixture 2')");
            migrationBuilder.Sql("INSERT INTO VehicleFeature (Name) VALUES ('Vehicle Fixture 3')");
            migrationBuilder.Sql("INSERT INTO VehicleFeature (Name) VALUES ('Vehicle Fixture 4')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
