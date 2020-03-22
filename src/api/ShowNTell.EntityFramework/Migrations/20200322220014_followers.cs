using Microsoft.EntityFrameworkCore.Migrations;

namespace ShowNTell.EntityFramework.Migrations
{
    public partial class followers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    UserEmail = table.Column<string>(nullable: false),
                    FollowerEmail = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => new { x.UserEmail, x.FollowerEmail });
                    table.ForeignKey(
                        name: "FK_Follows_Users_FollowerEmail",
                        column: x => x.FollowerEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Follows_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerEmail",
                table: "Follows",
                column: "FollowerEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follows");
        }
    }
}
