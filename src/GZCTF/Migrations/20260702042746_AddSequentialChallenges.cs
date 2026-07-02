using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GZCTF.Migrations
{
    /// <inheritdoc />
    public partial class AddSequentialChallenges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableSequentialChallenges",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "GameChallenges",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE ""GameChallenges"" 
                SET ""Order"" = sub.rn
                FROM (
                    SELECT ""Id"", ROW_NUMBER() OVER (PARTITION BY ""GameId"" ORDER BY ""Id"") as rn
                    FROM ""GameChallenges""
                ) sub
                WHERE ""GameChallenges"".""Id"" = sub.""Id""
                AND EXISTS (
                    SELECT 1 FROM ""Games"" 
                    WHERE ""Games"".""Id"" = ""GameChallenges"".""GameId"" 
                    AND ""Games"".""EnableSequentialChallenges"" = true
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableSequentialChallenges",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "GameChallenges");
        }
    }
}
