using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTheaterWS_v2.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmailRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This assigns a temporal value to any record that by mistake is null before changing the column to NOT NULL
            migrationBuilder.Sql("UPDATE AspNetUsers SET Email = 'without-email@temp.com' WHERE Email IS NULL");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
