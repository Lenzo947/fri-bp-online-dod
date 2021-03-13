using Microsoft.EntityFrameworkCore.Migrations;

namespace BP_OnlineDOD.Server.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO onlinedod_db.aspnetroles (Id, Name, NormalizedName) VALUES ('5519c0bb-f70d-4c2d-bd56-9e91cbcd6639', 'Admin', 'Admin')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
