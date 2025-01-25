using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vega.Migrations
{
    /// <inheritdoc />
    public partial class SeedModelsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES ('Model 1', " +
                                 "(SELECT TOP (1) Id FROM Makes ORDER BY Id ASC)" +
                                 ")");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES ('Model 2', " +
                                 "(SELECT TOP (1) Id FROM Makes ORDER BY Id ASC)" +
                                 ")");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES ('Model 3', " +
                                 "(SELECT TOP (1) Id FROM (SELECT TOP (2) Id FROM Makes ORDER BY Id ASC) T" +
                                 " ORDER BY Id DESC" +
                                 "))");
            migrationBuilder.Sql("INSERT INTO Models (Name, MakeId) VALUES ('Model 4', " +
                                 "(SELECT TOP (1) Id FROM (SELECT TOP (2) Id FROM Makes ORDER BY Id ASC) T" +
                                 " ORDER BY Id DESC" +
                                 "))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
