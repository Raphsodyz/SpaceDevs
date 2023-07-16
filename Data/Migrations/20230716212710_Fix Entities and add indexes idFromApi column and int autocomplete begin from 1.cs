using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixEntitiesandaddindexesidFromApicolumnandintautocompletebeginfrom1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_LAUNCH_SERVICE_PROVIDER_ID_LAUNCH_SERVICE_PROVIDER",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_MISSION_ID_MISSION",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_PAD_ID_PAD",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_ROCKET_ID_ROCKET",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_STATUS_ID_STATUS",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_MISSION_LAUNCH_DESIGNATOR_ID_LAUNCH_DESIGNATOR",
                table: "MISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_MISSION_ORBIT_ID_ORBIT",
                table: "MISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_PAD_LOCATION_ID_LOCATION",
                table: "PAD");

            migrationBuilder.DropForeignKey(
                name: "FK_ROCKET_CONFIGURATION_ID_CONFIGURATION",
                table: "ROCKET");

            migrationBuilder.AlterColumn<int>(
                name: "ID_CONFIGURATION",
                table: "ROCKET",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_LOCATION",
                table: "PAD",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_ORBIT",
                table: "MISSION",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_LAUNCH_DESIGNATOR",
                table: "MISSION",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "TBD_TIME",
                table: "LAUNCH",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "TBD_DATE",
                table: "LAUNCH",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "INHOLD",
                table: "LAUNCH",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "ID_STATUS",
                table: "LAUNCH",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_ROCKET",
                table: "LAUNCH",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_PAD",
                table: "LAUNCH",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_MISSION",
                table: "LAUNCH",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ID_LAUNCH_SERVICE_PROVIDER",
                table: "LAUNCH",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "UpdateLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransactionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OffSet = table.Column<int>(type: "int", nullable: false),
                    Success = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Error = table.Column<string>(type: "VARCHAR(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityCount = table.Column<int>(type: "int", nullable: false),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateLog", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_LAUNCH_SERVICE_PROVIDER_ID_LAUNCH_SERVICE_PROVIDER",
                table: "LAUNCH",
                column: "ID_LAUNCH_SERVICE_PROVIDER",
                principalTable: "LAUNCH_SERVICE_PROVIDER",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_MISSION_ID_MISSION",
                table: "LAUNCH",
                column: "ID_MISSION",
                principalTable: "MISSION",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_PAD_ID_PAD",
                table: "LAUNCH",
                column: "ID_PAD",
                principalTable: "PAD",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_ROCKET_ID_ROCKET",
                table: "LAUNCH",
                column: "ID_ROCKET",
                principalTable: "ROCKET",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_STATUS_ID_STATUS",
                table: "LAUNCH",
                column: "ID_STATUS",
                principalTable: "STATUS",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MISSION_LAUNCH_DESIGNATOR_ID_LAUNCH_DESIGNATOR",
                table: "MISSION",
                column: "ID_LAUNCH_DESIGNATOR",
                principalTable: "LAUNCH_DESIGNATOR",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MISSION_ORBIT_ID_ORBIT",
                table: "MISSION",
                column: "ID_ORBIT",
                principalTable: "ORBIT",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PAD_LOCATION_ID_LOCATION",
                table: "PAD",
                column: "ID_LOCATION",
                principalTable: "LOCATION",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ROCKET_CONFIGURATION_ID_CONFIGURATION",
                table: "ROCKET",
                column: "ID_CONFIGURATION",
                principalTable: "CONFIGURATION",
                principalColumn: "ID");

            //New Indexes
            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_ID_FROM_API",
                table: "LAUNCH",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_MISSION_ID_FROM_API",
                table: "MISSION",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_PAD_ID_FROM_API",
                table: "PAD",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_ROCKET_ID_FROM_API",
                table: "ROCKET",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_CONFIGURATION_ID_FROM_API",
                table: "CONFIGURATION",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_DESIGNATOR_ID_FROM_API",
                table: "LAUNCH_DESIGNATOR",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_SERVICE_PROVIDER_ID_FROM_API",
                table: "LAUNCH_SERVICE_PROVIDER",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_LOCATION_ID_FROM_API",
                table: "LOCATION",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_ORBIT_ID_FROM_API",
                table: "ORBIT",
                column: "ID_FROM_API");

            migrationBuilder.CreateIndex(
                name: "IX_STATUS_ID_FROM_API",
                table: "STATUS",
                column: "ID_FROM_API");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_LAUNCH_SERVICE_PROVIDER_ID_LAUNCH_SERVICE_PROVIDER",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_MISSION_ID_MISSION",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_PAD_ID_PAD",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_ROCKET_ID_ROCKET",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_LAUNCH_STATUS_ID_STATUS",
                table: "LAUNCH");

            migrationBuilder.DropForeignKey(
                name: "FK_MISSION_LAUNCH_DESIGNATOR_ID_LAUNCH_DESIGNATOR",
                table: "MISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_MISSION_ORBIT_ID_ORBIT",
                table: "MISSION");

            migrationBuilder.DropForeignKey(
                name: "FK_PAD_LOCATION_ID_LOCATION",
                table: "PAD");

            migrationBuilder.DropForeignKey(
                name: "FK_ROCKET_CONFIGURATION_ID_CONFIGURATION",
                table: "ROCKET");

            migrationBuilder.DropTable(
                name: "UpdateLog");

            migrationBuilder.AlterColumn<int>(
                name: "ID_CONFIGURATION",
                table: "ROCKET",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_LOCATION",
                table: "PAD",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_ORBIT",
                table: "MISSION",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_LAUNCH_DESIGNATOR",
                table: "MISSION",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "TBD_TIME",
                table: "LAUNCH",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "TBD_DATE",
                table: "LAUNCH",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "INHOLD",
                table: "LAUNCH",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_STATUS",
                table: "LAUNCH",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_ROCKET",
                table: "LAUNCH",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_PAD",
                table: "LAUNCH",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_MISSION",
                table: "LAUNCH",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ID_LAUNCH_SERVICE_PROVIDER",
                table: "LAUNCH",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_LAUNCH_SERVICE_PROVIDER_ID_LAUNCH_SERVICE_PROVIDER",
                table: "LAUNCH",
                column: "ID_LAUNCH_SERVICE_PROVIDER",
                principalTable: "LAUNCH_SERVICE_PROVIDER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_MISSION_ID_MISSION",
                table: "LAUNCH",
                column: "ID_MISSION",
                principalTable: "MISSION",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_PAD_ID_PAD",
                table: "LAUNCH",
                column: "ID_PAD",
                principalTable: "PAD",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_ROCKET_ID_ROCKET",
                table: "LAUNCH",
                column: "ID_ROCKET",
                principalTable: "ROCKET",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LAUNCH_STATUS_ID_STATUS",
                table: "LAUNCH",
                column: "ID_STATUS",
                principalTable: "STATUS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MISSION_LAUNCH_DESIGNATOR_ID_LAUNCH_DESIGNATOR",
                table: "MISSION",
                column: "ID_LAUNCH_DESIGNATOR",
                principalTable: "LAUNCH_DESIGNATOR",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MISSION_ORBIT_ID_ORBIT",
                table: "MISSION",
                column: "ID_ORBIT",
                principalTable: "ORBIT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PAD_LOCATION_ID_LOCATION",
                table: "PAD",
                column: "ID_LOCATION",
                principalTable: "LOCATION",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ROCKET_CONFIGURATION_ID_CONFIGURATION",
                table: "ROCKET",
                column: "ID_CONFIGURATION",
                principalTable: "CONFIGURATION",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
