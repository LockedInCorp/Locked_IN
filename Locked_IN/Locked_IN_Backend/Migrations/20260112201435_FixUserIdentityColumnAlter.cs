using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIdentityColumnAlter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing sequence if it exists
            migrationBuilder.Sql("DROP SEQUENCE IF EXISTS aspnetusers_id_seq CASCADE;");
            
            // Create the sequence
            migrationBuilder.Sql("CREATE SEQUENCE aspnetusers_id_seq START 1;");
            
            // Set sequence value based on max existing ID (using DO block to handle the SELECT properly)
            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    max_id INTEGER;
                BEGIN
                    SELECT COALESCE(MAX(""Id""), 0) INTO max_id FROM ""AspNetUsers"";
                    PERFORM setval('aspnetusers_id_seq', max_id + 1, false);
                END $$;
            ");
            
            // Set the column default to use the sequence
            migrationBuilder.Sql(@"ALTER TABLE ""AspNetUsers"" ALTER COLUMN ""Id"" SET DEFAULT nextval('aspnetusers_id_seq');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the default and sequence
            migrationBuilder.Sql(@"
                ALTER TABLE ""AspNetUsers"" ALTER COLUMN ""Id"" DROP DEFAULT;
                DROP SEQUENCE IF EXISTS aspnetusers_id_seq;
            ");
        }
    }
}
