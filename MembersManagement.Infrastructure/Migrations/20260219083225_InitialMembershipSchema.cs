using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembersManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMembershipSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Branches_BranchId",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "MembershipId",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.MembershipId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_MembershipId",
                table: "Members",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Branches_BranchId",
                table: "Members",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Memberships_MembershipId",
                table: "Members",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Branches_BranchId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Memberships_MembershipId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Members_MembershipId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "Members");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Branches_BranchId",
                table: "Members",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
