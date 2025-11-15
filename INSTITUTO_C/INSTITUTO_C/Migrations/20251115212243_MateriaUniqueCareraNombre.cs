using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INSTITUTO_C.Migrations
{
    /// <inheritdoc />
    public partial class MateriaUniqueCareraNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materias_CarreraId",
                table: "Materias");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_CarreraId_Nombre",
                table: "Materias",
                columns: new[] { "CarreraId", "Nombre" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materias_CarreraId_Nombre",
                table: "Materias");

            migrationBuilder.CreateIndex(
                name: "IX_Materias_CarreraId",
                table: "Materias",
                column: "CarreraId");
        }
    }
}
