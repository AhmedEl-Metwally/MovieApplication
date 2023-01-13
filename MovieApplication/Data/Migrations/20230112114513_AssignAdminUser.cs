using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApplication.Data.Migrations
{
    public partial class AssignAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [security].[UserRoles] SELECT '23b9c268-a24e-48e0-a8a7-47e733995568' , Id FROM [security].[Roles] ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [security].[UserRoles] WHERE UserId = '23b9c268-a24e-48e0-a8a7-47e733995568'");
        }
    }
}
