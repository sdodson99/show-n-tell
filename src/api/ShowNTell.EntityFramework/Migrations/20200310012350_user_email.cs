using Microsoft.EntityFrameworkCore.Migrations;

namespace ShowNTell.EntityFramework.Migrations
{
    public partial class user_email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagePosts_User_UserId",
                table: "ImagePosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_ImagePosts_UserId",
                table: "ImagePosts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ImagePosts");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "ImagePosts",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePosts_UserEmail",
                table: "ImagePosts",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagePosts_User_UserEmail",
                table: "ImagePosts",
                column: "UserEmail",
                principalTable: "User",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagePosts_User_UserEmail",
                table: "ImagePosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_ImagePosts_UserEmail",
                table: "ImagePosts");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "ImagePosts");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ImagePosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePosts_UserId",
                table: "ImagePosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImagePosts_User_UserId",
                table: "ImagePosts",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
