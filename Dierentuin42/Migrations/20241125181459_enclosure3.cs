using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin42.Migrations
{
    /// <inheritdoc />
    public partial class enclosure3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure");

            migrationBuilder.AlterColumn<int>(
                name: "ZooId",
                table: "Enclosure",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure",
                column: "ZooId",
                principalTable: "Zoo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure");

            migrationBuilder.AlterColumn<int>(
                name: "ZooId",
                table: "Enclosure",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Enclosure_Zoo_ZooId",
                table: "Enclosure",
                column: "ZooId",
                principalTable: "Zoo",
                principalColumn: "Id");
        }
    }
}
