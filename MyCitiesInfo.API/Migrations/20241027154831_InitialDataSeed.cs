using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyCitiesInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MyCities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "The one with that big park.", "New York City" },
                    { 2, "The one with the cathedral that was never really finished.", "Antwerp" },
                    { 3, "The one with that big tower.", "Paris" }
                });

            migrationBuilder.InsertData(
                table: "PointOfInterests",
                columns: new[] { "Id", "Description", "MyCityId", "Name" },
                values: new object[,]
                {
                    { 1, "The most visited urban park in the United States.", 1, "Central Park" },
                    { 2, "A 102-story skyscraper located in Midtown Manhattan.", 1, "Empire State Building" },
                    { 3, "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans.", 2, "Cathedral" },
                    { 4, "The the finest example of railway architecture in Belgium.", 2, "Antwerp Central Station" },
                    { 5, "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel.", 3, "Eiffel Tower" },
                    { 6, "The world's largest museum.", 3, "The Louvre" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MyCities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MyCities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MyCities",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
