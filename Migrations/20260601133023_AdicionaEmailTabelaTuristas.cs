using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceCare.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaEmailTabelaTuristas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EMAIL",
                table: "SC_TURISTAS",
                type: "NVARCHAR2(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SC_TURISTAS_EMAIL",
                table: "SC_TURISTAS",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SC_TURISTAS_EMAIL",
                table: "SC_TURISTAS");

            migrationBuilder.DropColumn(
                name: "EMAIL",
                table: "SC_TURISTAS");
        }
    }
}
