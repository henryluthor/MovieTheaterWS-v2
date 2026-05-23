using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTheaterWS_v2.Migrations
{
    /// <inheritdoc />
    public partial class NewEntitiesScreenAndSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Screen_idComplex",
                table: "Screen");

            migrationBuilder.AlterColumn<int>(
                name: "Runtime",
                table: "Movie",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Runtime in minutes.",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Runtime in minutes.");

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    IdSeat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Row = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    IdScreen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.IdSeat);
                    table.ForeignKey(
                        name: "FK_Seat_Screen_IdScreen",
                        column: x => x.IdScreen,
                        principalTable: "Screen",
                        principalColumn: "idScreen",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screen_idComplex_name",
                table: "Screen",
                columns: new[] { "idComplex", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seat_IdScreen",
                table: "Seat",
                column: "IdScreen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropIndex(
                name: "IX_Screen_idComplex_name",
                table: "Screen");

            migrationBuilder.AlterColumn<int>(
                name: "Runtime",
                table: "Movie",
                type: "int",
                nullable: true,
                comment: "Runtime in minutes.",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Runtime in minutes.");

            migrationBuilder.CreateIndex(
                name: "IX_Screen_idComplex",
                table: "Screen",
                column: "idComplex");
        }
    }
}
