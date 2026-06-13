using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediatorCqrsApi.Infra.Migrations
{
    public partial class UsuarioIdToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new Guid column with a default NEWSEQUENTIALID() so existing rows get values
            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "Usuario",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()"
            );

            // Drop old primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario"
            );

            // Remove old integer Id column (identity)
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Usuario"
            );

            // Rename the new column to Id
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Usuario",
                newName: "Id"
            );

            // Add primary key on the new Id
            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Downgrade is non-trivial (converting GUID back to INT identity) and therefore not supported.
            throw new NotSupportedException("Downgrading this migration is not supported.");
        }
    }
}
