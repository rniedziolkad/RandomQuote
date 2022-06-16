using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomQuote.Migrations
{
    public partial class AddLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Quotes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "QuoteModelUser",
                columns: table => new
                {
                    LikedQuotesQuoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserLikesId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteModelUser", x => new { x.LikedQuotesQuoteId, x.UserLikesId });
                    table.ForeignKey(
                        name: "FK_QuoteModelUser_AspNetUsers_UserLikesId",
                        column: x => x.UserLikesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuoteModelUser_Quotes_LikedQuotesQuoteId",
                        column: x => x.LikedQuotesQuoteId,
                        principalTable: "Quotes",
                        principalColumn: "QuoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuoteModelUser_UserLikesId",
                table: "QuoteModelUser",
                column: "UserLikesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes");

            migrationBuilder.DropTable(
                name: "QuoteModelUser");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Quotes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_AspNetUsers_UserId",
                table: "Quotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
