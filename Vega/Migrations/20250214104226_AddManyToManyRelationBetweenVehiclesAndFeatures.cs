using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vega.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyRelationBetweenVehiclesAndFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleFeatures_VehicleFeatureId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VehicleFeatureId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleFeatureId",
                table: "Vehicles");

            migrationBuilder.CreateTable(
                name: "FeaturesForVehicles",
                columns: table => new
                {
                    VehicleFeatureId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturesForVehicles", x => new { x.VehicleFeatureId, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_FeaturesForVehicles_VehicleFeatures_VehicleFeatureId",
                        column: x => x.VehicleFeatureId,
                        principalTable: "VehicleFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeaturesForVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeaturesForVehicles_VehicleId",
                table: "FeaturesForVehicles",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeaturesForVehicles");

            migrationBuilder.AddColumn<int>(
                name: "VehicleFeatureId",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleFeatureId",
                table: "Vehicles",
                column: "VehicleFeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleFeatures_VehicleFeatureId",
                table: "Vehicles",
                column: "VehicleFeatureId",
                principalTable: "VehicleFeatures",
                principalColumn: "Id");
        }
    }
}
