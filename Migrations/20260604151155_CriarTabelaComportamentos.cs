using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceCare.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaComportamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SC_COMPORTAMENTOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_TURISTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DURACAO_SEGUNDOS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    GESTO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ALERTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DT_LEITURA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SC_COMPORTAMENTOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SC_COMPORTAMENTOS_SC_TURISTAS_ID_TURISTA",
                        column: x => x.ID_TURISTA,
                        principalTable: "SC_TURISTAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SC_COMPORTAMENTOS_ID_TURISTA",
                table: "SC_COMPORTAMENTOS",
                column: "ID_TURISTA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SC_COMPORTAMENTOS");
        }
    }
}
