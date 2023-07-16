using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CONFIGURATION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LAUNCH_LIBRARY_ID = table.Column<int>(type: "int", nullable: true),
                    URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NAME = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FAMILY = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FULL_NAME = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VARIANT = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONFIGURATION", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LAUNCH_DESIGNATOR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LAUNCH_DESIGNATOR", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LAUNCH_SERVICE_PROVIDER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NAME = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TYPE = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LAUNCH_SERVICE_PROVIDER", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LOCATION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NAME = table.Column<string>(type: "VARCHAR(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    COUNTRY_CODE = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MAP_IMAGE = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TOTAL_LAUNCH_COUNT = table.Column<int>(type: "int", nullable: false),
                    TOTAL_LANDING_COUNT = table.Column<int>(type: "int", nullable: false),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCATION", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ORBIT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ABBREV = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORBIT", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "STATUS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NAME = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATUS", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ROCKET",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ID_CONFIGURATION = table.Column<int>(type: "int", nullable: false),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROCKET", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROCKET_CONFIGURATION_ID_CONFIGURATION",
                        column: x => x.ID_CONFIGURATION,
                        principalTable: "CONFIGURATION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PAD",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AGENCY_ID = table.Column<int>(type: "int", nullable: true),
                    NAME = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INFO_URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WIKI_URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MAP_URL = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LATITUDE = table.Column<double>(type: "double", nullable: false),
                    LONGITUDE = table.Column<double>(type: "double", nullable: false),
                    ID_LOCATION = table.Column<int>(type: "int", nullable: false),
                    MAP_IMAGE = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TOTAL_LAUNCH_COUNT = table.Column<int>(type: "int", nullable: false),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAD", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PAD_LOCATION_ID_LOCATION",
                        column: x => x.ID_LOCATION,
                        principalTable: "LOCATION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MISSION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LAUNCH_LIBRARY_ID = table.Column<int>(type: "int", nullable: true),
                    NAME = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_LAUNCH_DESIGNATOR = table.Column<int>(type: "int", nullable: false),
                    TYPE = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_ORBIT = table.Column<int>(type: "int", nullable: false),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MISSION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MISSION_LAUNCH_DESIGNATOR_ID_LAUNCH_DESIGNATOR",
                        column: x => x.ID_LAUNCH_DESIGNATOR,
                        principalTable: "LAUNCH_DESIGNATOR",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MISSION_ORBIT_ID_ORBIT",
                        column: x => x.ID_ORBIT,
                        principalTable: "ORBIT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LAUNCH",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    API_GUID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    URL = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LAUNCH_LIBRARY_ID = table.Column<int>(type: "int", nullable: true),
                    SLUG = table.Column<string>(type: "VARCHAR(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NAME = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_STATUS = table.Column<int>(type: "int", nullable: false),
                    NET = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WINDOW_END = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WINDOW_START = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    INHOLD = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TBD_TIME = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TBD_DATE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PROBABILITY = table.Column<int>(type: "int", nullable: true),
                    HOLD_REASON = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FAIL_REASON = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HASHTAG = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_LAUNCH_SERVICE_PROVIDER = table.Column<int>(type: "int", nullable: false),
                    ID_ROCKET = table.Column<int>(type: "int", nullable: false),
                    ID_MISSION = table.Column<int>(type: "int", nullable: false),
                    ID_PAD = table.Column<int>(type: "int", nullable: false),
                    WEB_CAST_LIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IMAGE = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INFOGRAPHIC = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PROGRAMS = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_FROM_API = table.Column<int>(type: "int", nullable: true),
                    ATUALIZATION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IMPORTED_T = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    STATUS = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LAUNCH", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LAUNCH_LAUNCH_SERVICE_PROVIDER_ID_LAUNCH_SERVICE_PROVIDER",
                        column: x => x.ID_LAUNCH_SERVICE_PROVIDER,
                        principalTable: "LAUNCH_SERVICE_PROVIDER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LAUNCH_MISSION_ID_MISSION",
                        column: x => x.ID_MISSION,
                        principalTable: "MISSION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LAUNCH_PAD_ID_PAD",
                        column: x => x.ID_PAD,
                        principalTable: "PAD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LAUNCH_ROCKET_ID_ROCKET",
                        column: x => x.ID_ROCKET,
                        principalTable: "ROCKET",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LAUNCH_STATUS_ID_STATUS",
                        column: x => x.ID_STATUS,
                        principalTable: "STATUS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_ID_LAUNCH_SERVICE_PROVIDER",
                table: "LAUNCH",
                column: "ID_LAUNCH_SERVICE_PROVIDER");

            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_ID_MISSION",
                table: "LAUNCH",
                column: "ID_MISSION");

            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_ID_PAD",
                table: "LAUNCH",
                column: "ID_PAD");

            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_ID_ROCKET",
                table: "LAUNCH",
                column: "ID_ROCKET");

            migrationBuilder.CreateIndex(
                name: "IX_LAUNCH_ID_STATUS",
                table: "LAUNCH",
                column: "ID_STATUS");

            migrationBuilder.CreateIndex(
                name: "IX_MISSION_ID_LAUNCH_DESIGNATOR",
                table: "MISSION",
                column: "ID_LAUNCH_DESIGNATOR");

            migrationBuilder.CreateIndex(
                name: "IX_MISSION_ID_ORBIT",
                table: "MISSION",
                column: "ID_ORBIT");

            migrationBuilder.CreateIndex(
                name: "IX_PAD_ID_LOCATION",
                table: "PAD",
                column: "ID_LOCATION");

            migrationBuilder.CreateIndex(
                name: "IX_ROCKET_ID_CONFIGURATION",
                table: "ROCKET",
                column: "ID_CONFIGURATION");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LAUNCH");

            migrationBuilder.DropTable(
                name: "LAUNCH_SERVICE_PROVIDER");

            migrationBuilder.DropTable(
                name: "MISSION");

            migrationBuilder.DropTable(
                name: "PAD");

            migrationBuilder.DropTable(
                name: "ROCKET");

            migrationBuilder.DropTable(
                name: "STATUS");

            migrationBuilder.DropTable(
                name: "LAUNCH_DESIGNATOR");

            migrationBuilder.DropTable(
                name: "ORBIT");

            migrationBuilder.DropTable(
                name: "LOCATION");

            migrationBuilder.DropTable(
                name: "CONFIGURATION");
        }
    }
}
