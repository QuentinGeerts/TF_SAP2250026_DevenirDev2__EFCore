using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoEFCore.Migrations
{
    /// <inheritdoc />
    public partial class FilmOneToManyDirector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DirectorId",
                table: "Films",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "Firstname", "Lastname" },
                values: new object[,]
                {
                    { new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"), "James", "Cameron" },
                    { new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"), "Quentin", "Geerts" }
                });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000001"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000002"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000003"),
                column: "DirectorId",
                value: new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000004"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000005"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000006"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000007"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000008"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000009"),
                column: "DirectorId",
                value: new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000010"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000011"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000012"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000013"),
                column: "DirectorId",
                value: new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000014"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000015"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000016"),
                column: "DirectorId",
                value: new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000017"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000018"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000019"),
                column: "DirectorId",
                value: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000020"),
                column: "DirectorId",
                value: new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"));

            migrationBuilder.CreateIndex(
                name: "IX_Films_DirectorId",
                table: "Films",
                column: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_Directors_DirectorId",
                table: "Films",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_Directors_DirectorId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_DirectorId",
                table: "Films");

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: new Guid("5e41c1c9-c2a0-4941-a6e4-c5642c1502cd"));

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: new Guid("ea269f2e-7c8c-48ad-944c-c9d754303758"));

            migrationBuilder.DropColumn(
                name: "DirectorId",
                table: "Films");
        }
    }
}
