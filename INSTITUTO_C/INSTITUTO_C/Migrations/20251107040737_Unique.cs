using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INSTITUTO_C.Migrations
{
    /// <inheritdoc />
    public partial class Unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Personas_DNI",
                table: "Personas",
                column: "DNI",
                unique: true,
                filter: "[DNI] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Carreras_Nombre",
                table: "Carreras",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personas_DNI",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Carreras_Nombre",
                table: "Carreras");
        }
    }
}
