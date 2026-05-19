using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTheaterWS_v2.Migrations
{
    /// <inheritdoc />
    public partial class FixComplexMovieRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplexMovie");

            migrationBuilder.CreateTable(
                name: "CompleMovie",
                columns: table => new
                {
                    IdComplex = table.Column<int>(type: "int", nullable: false),
                    IdMovie = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleMovie", x => new { x.IdComplex, x.IdMovie });
                    table.ForeignKey(
                        name: "FK_CompleMovie_Complex_IdComplex",
                        column: x => x.IdComplex,
                        principalTable: "Complex",
                        principalColumn: "idComplex",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompleMovie_Movie_IdMovie",
                        column: x => x.IdMovie,
                        principalTable: "Movie",
                        principalColumn: "idMovie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompleMovie_IdMovie",
                table: "CompleMovie",
                column: "IdMovie");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompleMovie");

            migrationBuilder.CreateTable(
                name: "ComplexMovie",
                columns: table => new
                {
                    idMovie = table.Column<int>(type: "int", nullable: false),
                    idComplex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ComplexMovie_Movie",
                        column: x => x.idMovie,
                        principalTable: "Movie",
                        principalColumn: "idMovie");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComplexMovie_idMovie",
                table: "ComplexMovie",
                column: "idMovie");
        }
    }
}
