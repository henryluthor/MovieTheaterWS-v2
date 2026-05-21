using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTheaterWS_v2.Migrations
{
    /// <inheritdoc />
    public partial class NewEntityScreen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Screen",
                columns: table => new
                {
                    idScreen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    idComplex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screen", x => x.idScreen);
                    table.ForeignKey(
                        name: "FK_Screen_Complex_idComplex",
                        column: x => x.idComplex,
                        principalTable: "Complex",
                        principalColumn: "idComplex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screen_idComplex",
                table: "Screen",
                column: "idComplex");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Screen");
        }
    }
}
