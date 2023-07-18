using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class stored_procedure_published : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createStoredProcedurePublished = @"
                CREATE PROCEDURE sp_status_published_routine()
                BEGIN

                DECLARE now_time TIMESTAMP;
                SET now_time = CURRENT_TIMESTAMP();

                UPDATE `launch`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `configuration`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `launch_designator`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `launch_service_provider`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `location`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `mission`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `orbit`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `pad`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `rocket`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                UPDATE `status`
                SET `STATUS` = 'PUBLISHED'
                WHERE `IMPORTED_T` BETWEEN now_time - INTERVAL 30 MINUTE AND now_time;

                END;";
            migrationBuilder.Sql(createStoredProcedurePublished);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
