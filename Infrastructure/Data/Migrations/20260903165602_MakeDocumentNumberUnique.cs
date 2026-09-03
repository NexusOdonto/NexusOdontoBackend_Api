using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeDocumentNumberUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Unique index on document_number fails if duplicates already exist
            // (same number with different document_type_id was previously allowed).
            // List them first:
            //   SELECT "document_number", COUNT(*) FROM "persons"
            //   GROUP BY "document_number" HAVING COUNT(*) > 1;
            migrationBuilder.Sql("""
                DECLARE
                  v_count NUMBER;
                  v_list VARCHAR2(4000);
                BEGIN
                  SELECT COUNT(*) INTO v_count
                  FROM (
                    SELECT "document_number"
                    FROM "persons"
                    GROUP BY "document_number"
                    HAVING COUNT(*) > 1
                  );

                  IF v_count > 0 THEN
                    SELECT LISTAGG("document_number", ', ') WITHIN GROUP (ORDER BY "document_number")
                    INTO v_list
                    FROM (
                      SELECT "document_number"
                      FROM "persons"
                      GROUP BY "document_number"
                      HAVING COUNT(*) > 1
                    )
                    WHERE ROWNUM <= 20;

                    RAISE_APPLICATION_ERROR(
                      -20001,
                      'Cannot create unique index IX_persons_document_number: duplicate document_number values exist (' ||
                      NVL(v_list, '?') ||
                      '). Resolve duplicates before applying this migration.');
                  END IF;
                END;
                """);

            migrationBuilder.DropIndex(
                name: "IX_persons_document_type_id_document_number",
                table: "persons");

            migrationBuilder.CreateIndex(
                name: "IX_persons_document_number",
                table: "persons",
                column: "document_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_persons_document_number",
                table: "persons");

            migrationBuilder.CreateIndex(
                name: "IX_persons_document_type_id_document_number",
                table: "persons",
                columns: new[] { "document_type_id", "document_number" },
                unique: true);
        }
    }
}
