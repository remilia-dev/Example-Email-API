using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mailer.Core.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Sender = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                WasSent = table.Column<bool>(type: "bit", nullable: true),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "EmailRecipient",
            columns: table => new
            {
                EmailMessageId = table.Column<int>(type: "int", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Address = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Type = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmailRecipient", x => new { x.EmailMessageId, x.Id });
                table.ForeignKey(
                    name: "FK_EmailRecipient_Messages_EmailMessageId",
                    column: x => x.EmailMessageId,
                    principalTable: "Messages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmailRecipient_Address",
            table: "EmailRecipient",
            column: "Address");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_CreatedOn",
            table: "Messages",
            column: "CreatedOn");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_Sender",
            table: "Messages",
            column: "Sender");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmailRecipient");

        migrationBuilder.DropTable(
            name: "Messages");
    }
}
