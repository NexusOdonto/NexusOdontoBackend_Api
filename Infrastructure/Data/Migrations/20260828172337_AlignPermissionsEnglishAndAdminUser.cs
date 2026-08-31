using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlignPermissionsEnglishAndAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000001"),
                column: "code",
                value: "VIEW");

            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000002"),
                column: "code",
                value: "CREATE");

            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000003"),
                column: "code",
                value: "EDIT");

            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000004"),
                column: "code",
                value: "DELETE");

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the PATIENTS module.", "PATIENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the PATIENTS module.", "PATIENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the PATIENTS module.", "PATIENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the PATIENTS module.", "PATIENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the APPOINTMENTS module.", "APPOINTMENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the APPOINTMENTS module.", "APPOINTMENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the APPOINTMENTS module.", "APPOINTMENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the APPOINTMENTS module.", "APPOINTMENTS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the CATALOGS module.", "CATALOGS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the CATALOGS module.", "CATALOGS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the CATALOGS module.", "CATALOGS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the CATALOGS module.", "CATALOGS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the USERS module.", "USERS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the USERS module.", "USERS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the USERS module.", "USERS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the USERS module.", "USERS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the CLINICAL module.", "CLINICAL" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the CLINICAL module.", "CLINICAL" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the CLINICAL module.", "CLINICAL" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the CLINICAL module.", "CLINICAL" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the ODONTOGRAM module.", "ODONTOGRAM" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the ODONTOGRAM module.", "ODONTOGRAM" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the ODONTOGRAM module.", "ODONTOGRAM" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the ODONTOGRAM module.", "ODONTOGRAM" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the NOTIFICATIONS module.", "NOTIFICATIONS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the NOTIFICATIONS module.", "NOTIFICATIONS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the NOTIFICATIONS module.", "NOTIFICATIONS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the NOTIFICATIONS module.", "NOTIFICATIONS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to view records of the CHATBOT module.", "CHATBOT" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to create records of the CHATBOT module.", "CHATBOT" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to edit records of the CHATBOT module.", "CHATBOT" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to delete records of the CHATBOT module.", "CHATBOT" });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "ActionPermissionEntityId", "permission_action_id", "description", "module" },
                values: new object[,]
                {
                    { new Guid("14000000-0000-0000-0009-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to view records of the AUDIT module.", "AUDIT" },
                    { new Guid("14000000-0000-0000-0009-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to create records of the AUDIT module.", "AUDIT" },
                    { new Guid("14000000-0000-0000-0009-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to edit records of the AUDIT module.", "AUDIT" },
                    { new Guid("14000000-0000-0000-0009-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to delete records of the AUDIT module.", "AUDIT" },
                    { new Guid("14000000-0000-0000-0010-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to view records of the PEOPLE module.", "PEOPLE" },
                    { new Guid("14000000-0000-0000-0010-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to create records of the PEOPLE module.", "PEOPLE" },
                    { new Guid("14000000-0000-0000-0010-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to edit records of the PEOPLE module.", "PEOPLE" },
                    { new Guid("14000000-0000-0000-0010-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to delete records of the PEOPLE module.", "PEOPLE" },
                    { new Guid("14000000-0000-0000-0011-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to view records of the ROLES module.", "ROLES" },
                    { new Guid("14000000-0000-0000-0011-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to create records of the ROLES module.", "ROLES" },
                    { new Guid("14000000-0000-0000-0011-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to edit records of the ROLES module.", "ROLES" },
                    { new Guid("14000000-0000-0000-0011-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to delete records of the ROLES module.", "ROLES" },
                    { new Guid("14000000-0000-0000-0012-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to view records of the PERMISSIONS module.", "PERMISSIONS" },
                    { new Guid("14000000-0000-0000-0012-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to create records of the PERMISSIONS module.", "PERMISSIONS" },
                    { new Guid("14000000-0000-0000-0012-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to edit records of the PERMISSIONS module.", "PERMISSIONS" },
                    { new Guid("14000000-0000-0000-0012-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to delete records of the PERMISSIONS module.", "PERMISSIONS" }
                });

            migrationBuilder.InsertData(
                table: "persons",
                columns: new[] { "id", "address", "created_at", "birth_date", "document_number", "DocumentTypeEntityId", "document_type_id", "email", "first_name", "is_active", "last_name", "phone", "SexEntityId", "gender_id", "updated_at" },
                values: new object[] { new Guid("1b000000-0000-0000-0000-000000000001"), null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "1234567890", null, new Guid("e0000000-0000-0000-0000-000000000001"), "admin@nexus.local", "Admin", true, "Sistema", null, null, null, null });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "role_id", "user_id", "assigned_at", "RoleEntityId", "UserEntityId" },
                values: new object[] { new Guid("13000000-0000-0000-0000-000000000001"), new Guid("1b000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("14000000-0000-0000-0009-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0009-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0009-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0009-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0010-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0010-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0010-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0010-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0011-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0011-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0011-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0011-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0012-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0012-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0012-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0012-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "is_active", "last_access_at", "password_hash", "person_id", "updated_at" },
                values: new object[] { new Guid("1b000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, "$2a$11$20hYXWKcqvIPrfsNyCCB1eJJZgRWoTXIA2JWITxYKRY3KJajuY0Aq", new Guid("1b000000-0000-0000-0000-000000000001"), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0009-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0009-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0009-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0009-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0010-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0010-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0010-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0010-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0011-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0011-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0011-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0011-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0012-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0012-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0012-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "permission_id", "role_id" },
                keyValues: new object[] { new Guid("14000000-0000-0000-0012-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumns: new[] { "role_id", "user_id" },
                keyValues: new object[] { new Guid("13000000-0000-0000-0000-000000000001"), new Guid("1b000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("1b000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0009-000000000001"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0009-000000000002"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0009-000000000003"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0009-000000000004"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0010-000000000001"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0010-000000000002"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0010-000000000003"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0010-000000000004"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0011-000000000001"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0011-000000000002"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0011-000000000003"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0011-000000000004"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0012-000000000001"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0012-000000000002"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0012-000000000003"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0012-000000000004"));

            migrationBuilder.DeleteData(
                table: "persons",
                keyColumn: "id",
                keyValue: new Guid("1b000000-0000-0000-0000-000000000001"));

            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000001"),
                column: "code",
                value: "VER");

            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000002"),
                column: "code",
                value: "CREAR");

            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000003"),
                column: "code",
                value: "EDITAR");

            migrationBuilder.UpdateData(
                table: "permission_actions",
                keyColumn: "id",
                keyValue: new Guid("12000000-0000-0000-0000-000000000004"),
                column: "code",
                value: "ELIMINAR");

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the PACIENTES module.", "PACIENTES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the PACIENTES module.", "PACIENTES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the PACIENTES module.", "PACIENTES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0001-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the PACIENTES module.", "PACIENTES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the CITAS module.", "CITAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the CITAS module.", "CITAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the CITAS module.", "CITAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0002-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the CITAS module.", "CITAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the CATALOGOS module.", "CATALOGOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the CATALOGOS module.", "CATALOGOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the CATALOGOS module.", "CATALOGOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0003-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the CATALOGOS module.", "CATALOGOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the USUARIOS module.", "USUARIOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the USUARIOS module.", "USUARIOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the USUARIOS module.", "USUARIOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0004-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the USUARIOS module.", "USUARIOS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the HISTORIAS module.", "HISTORIAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the HISTORIAS module.", "HISTORIAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the HISTORIAS module.", "HISTORIAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0005-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the HISTORIAS module.", "HISTORIAS" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the ODONTOGRAMA module.", "ODONTOGRAMA" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the ODONTOGRAMA module.", "ODONTOGRAMA" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the ODONTOGRAMA module.", "ODONTOGRAMA" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0006-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the ODONTOGRAMA module.", "ODONTOGRAMA" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the NOTIFICACIONES module.", "NOTIFICACIONES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the NOTIFICACIONES module.", "NOTIFICACIONES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the NOTIFICACIONES module.", "NOTIFICACIONES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0007-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the NOTIFICACIONES module.", "NOTIFICACIONES" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000001"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to ver records of the SOPORTE module.", "SOPORTE" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000002"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to crear records of the SOPORTE module.", "SOPORTE" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000003"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to editar records of the SOPORTE module.", "SOPORTE" });

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: new Guid("14000000-0000-0000-0008-000000000004"),
                columns: new[] { "description", "module" },
                values: new object[] { "Allows to eliminar records of the SOPORTE module.", "SOPORTE" });
        }
    }
}
