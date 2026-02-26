using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoEFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Films",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Films",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "Directors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "Directors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "CreatedAt", "ReleasedYear", "Title" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1994, "The Shawshank Redemption" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1972, "The Godfather" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2008, "The Dark Knight" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1994, "Pulp Fiction" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1993, "Schindler's List" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2003, "The Lord of the Rings: The Return of the King" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1994, "Forrest Gump" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2010, "Inception" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1999, "Fight Club" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1990, "Goodfellas" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000011"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1999, "The Matrix" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000012"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2014, "Interstellar" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000013"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1995, "Se7en" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000014"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1991, "The Silence of the Lambs" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000015"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1998, "Saving Private Ryan" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000016"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2000, "Gladiator" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000017"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2006, "The Prestige" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000018"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2014, "Whiplash" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000019"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2019, "Parasite" },
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-000000000020"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2023, "Oppenheimer" }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Email_Format",
                table: "Users",
                sql: "Email LIKE '_%@_%._%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Film_ReleasedYear_After_1500",
                table: "Films",
                sql: "ReleasedYear >= 1500");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Email_Format",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Film_ReleasedYear_After_1500",
                table: "Films");

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000001"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000002"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000003"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000004"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000005"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000006"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000007"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000008"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000009"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000010"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000011"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000012"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000013"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000014"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000015"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000016"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000017"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000018"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000019"));

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-000000000020"));

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Films");

            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Films",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "Directors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "Directors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
