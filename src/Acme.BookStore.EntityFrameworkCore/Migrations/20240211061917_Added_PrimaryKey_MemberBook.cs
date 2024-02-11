using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.BookStore.Migrations
{
    /// <inheritdoc />
    public partial class Added_PrimaryKey_MemberBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppMemberBooks",
                table: "AppMemberBooks");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AppMemberBooks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "AppMemberBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppMemberBooks",
                table: "AppMemberBooks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppMemberBooks_MemberId",
                table: "AppMemberBooks",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppMemberBooks",
                table: "AppMemberBooks");

            migrationBuilder.DropIndex(
                name: "IX_AppMemberBooks_MemberId",
                table: "AppMemberBooks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppMemberBooks");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "AppMemberBooks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppMemberBooks",
                table: "AppMemberBooks",
                columns: new[] { "MemberId", "BookId" });
        }
    }
}
