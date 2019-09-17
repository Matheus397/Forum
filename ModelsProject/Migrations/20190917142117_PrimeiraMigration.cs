using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelsProject.Migrations
{
    public partial class PrimeiraMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    Nota = table.Column<float>(nullable: true),
                    MediaDeVotos = table.Column<float>(nullable: true),
                    AutorComentarioId = table.Column<Guid>(nullable: false),
                    PublicacaoId = table.Column<string>(nullable: false),
                    ComentarioId = table.Column<string>(nullable: false),
                    CitacaoId = table.Column<string>(nullable: false),
                    mensagem = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.ID);
                    table.UniqueConstraint("AK_Comentarios_AutorComentarioId", x => x.AutorComentarioId);
                    table.UniqueConstraint("AK_Comentarios_CitacaoId", x => x.CitacaoId);
                    table.UniqueConstraint("AK_Comentarios_ComentarioId", x => x.ComentarioId);
                    table.UniqueConstraint("AK_Comentarios_PublicacaoId", x => x.PublicacaoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Senha = table.Column<string>(maxLength: 12, nullable: false),
                    ConfirmacaoDaSenha = table.Column<string>(maxLength: 12, nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.ID);
                    table.UniqueConstraint("AK_Usuarios_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    AutorID = table.Column<Guid>(nullable: true),
                    Titulo = table.Column<string>(nullable: true),
                    Texto = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    MediaDeVotos = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Publicacoes_Usuarios_AutorID",
                        column: x => x.AutorID,
                        principalTable: "Usuarios",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publicacoes_AutorID",
                table: "Publicacoes",
                column: "AutorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");
            migrationBuilder.DropTable(
                name: "Publicacoes");
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
