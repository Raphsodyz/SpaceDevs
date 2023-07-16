using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Update_log_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UpdateLog",
                table: "UpdateLog");

            migrationBuilder.RenameTable(
                name: "UpdateLog",
                newName: "UPDATE_LOG");

            migrationBuilder.RenameColumn(
                name: "Success",
                table: "UPDATE_LOG",
                newName: "SUCCESS");

            migrationBuilder.RenameColumn(
                name: "OffSet",
                table: "UPDATE_LOG",
                newName: "OFFSET");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "UPDATE_LOG",
                newName: "TRANSACTION_DATE");

            migrationBuilder.RenameColumn(
                name: "EntityCount",
                table: "UPDATE_LOG",
                newName: "ENTITY_COUNT");

            migrationBuilder.RenameColumn(
                name: "Error",
                table: "UPDATE_LOG",
                newName: "MESSAGE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UPDATE_LOG",
                table: "UPDATE_LOG",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UPDATE_LOG",
                table: "UPDATE_LOG");

            migrationBuilder.RenameTable(
                name: "UPDATE_LOG",
                newName: "UpdateLog");

            migrationBuilder.RenameColumn(
                name: "SUCCESS",
                table: "UpdateLog",
                newName: "Success");

            migrationBuilder.RenameColumn(
                name: "OFFSET",
                table: "UpdateLog",
                newName: "OffSet");

            migrationBuilder.RenameColumn(
                name: "TRANSACTION_DATE",
                table: "UpdateLog",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "ENTITY_COUNT",
                table: "UpdateLog",
                newName: "EntityCount");

            migrationBuilder.RenameColumn(
                name: "MESSAGE",
                table: "UpdateLog",
                newName: "Error");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UpdateLog",
                table: "UpdateLog",
                column: "ID");
        }
    }
}
