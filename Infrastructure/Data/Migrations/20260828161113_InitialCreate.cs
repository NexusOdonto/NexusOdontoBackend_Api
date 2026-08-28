using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appointment_origins",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment_origins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "appointment_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    color_hex = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: true),
                    is_final = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_event_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    category = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                    risk_level = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_event_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "background_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_background_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_channels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chatbot_conversation_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    is_final = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatbot_conversation_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chatbot_message_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatbot_message_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dentition_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dentition_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "document_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "finding_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    color_hex = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finding_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "finding_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    color_hex = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: true),
                    visual_symbol = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    requires_surface = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finding_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hub_notification_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hub_notification_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hub_notification_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    icon = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hub_notification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job_positions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_positions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_priorities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    color_hex = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: true),
                    level = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_priorities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission_actions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_actions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    duration_minutes = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    price = table.Column<decimal>(type: "DECIMAL(12,2)", precision: 12, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "session_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "specialties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "support_ticket_reasons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_ticket_reasons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "support_ticket_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    is_final = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "0"),
                    allows_assignment = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "0"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_ticket_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tooth_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    color_hex = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: true),
                    visual_code = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tooth_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tooth_surfaces",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    applies_to_anterior = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    applies_to_posterior = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    visual_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tooth_surfaces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "whatsapp_notification_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_whatsapp_notification_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "whatsapp_notification_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    default_template = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    code = table.Column<string>(type: "NVARCHAR2(60)", maxLength: 60, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    display_order = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_whatsapp_notification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teeth",
                columns: table => new
                {
                    code = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    name = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    dentition_type_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    quadrant = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    position = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DentitionTypeEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teeth", x => x.code);
                    table.ForeignKey(
                        name: "FK_teeth_dentition_types_DentitionTypeEntityId",
                        column: x => x.DentitionTypeEntityId,
                        principalTable: "dentition_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    document_type_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    document_number = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    first_name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    birth_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    gender_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    phone = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    address = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: true),
                    DocumentTypeEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    SexEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.id);
                    table.ForeignKey(
                        name: "FK_persons_document_types_DocumentTypeEntityId",
                        column: x => x.DocumentTypeEntityId,
                        principalTable: "document_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_persons_genders_SexEntityId",
                        column: x => x.SexEntityId,
                        principalTable: "genders",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    module = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    permission_action_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    ActionPermissionEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_permissions_permission_actions_ActionPermissionEntityId",
                        column: x => x.ActionPermissionEntityId,
                        principalTable: "permission_actions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    person_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    job_position_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    hire_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    JobTitleEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_job_positions_JobTitleEntityId",
                        column: x => x.JobTitleEntityId,
                        principalTable: "job_positions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_employees_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    person_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    emergency_contact = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    emergency_phone = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    registration_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.id);
                    table.ForeignKey(
                        name: "FK_patients_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    person_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    password_hash = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    last_access_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_persons_person_id",
                        column: x => x.person_id,
                        principalTable: "persons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    permission_id = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professionals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    employee_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    professional_license = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professionals", x => x.id);
                    table.ForeignKey(
                        name: "FK_professionals_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chatbot_conversations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    patient_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    chat_identifier = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    chat_channel_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    conversation_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    started_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_interaction_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    closed_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    ChatChannelEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatbot_conversations", x => x.id);
                    table.ForeignKey(
                        name: "FK_chatbot_conversations_chat_channels_ChatChannelEntityId",
                        column: x => x.ChatChannelEntityId,
                        principalTable: "chat_channels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_chatbot_conversations_chatbot_conversation_statuses_conversation_status_id",
                        column: x => x.conversation_status_id,
                        principalTable: "chatbot_conversation_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chatbot_conversations_patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "patients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "clinical_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    patient_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    opening_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    general_notes = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinical_histories", x => x.id);
                    table.ForeignKey(
                        name: "FK_clinical_histories_patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "patients",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "hub_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    sender_user_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    recipient_user_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    recipient_role_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    hub_notification_type_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    notification_priority_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    title = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    entity_type = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                    entity_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    hub_notification_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    delivered_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    read_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    HubNotificationTypeEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    NotificationPriorityEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    HubNotificationStatusEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hub_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_hub_notifications_hub_notification_statuses_HubNotificationStatusEntityId",
                        column: x => x.HubNotificationStatusEntityId,
                        principalTable: "hub_notification_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_hub_notifications_hub_notification_types_HubNotificationTypeEntityId",
                        column: x => x.HubNotificationTypeEntityId,
                        principalTable: "hub_notification_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_hub_notifications_notification_priorities_NotificationPriorityEntityId",
                        column: x => x.NotificationPriorityEntityId,
                        principalTable: "notification_priorities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_hub_notifications_roles_recipient_role_id",
                        column: x => x.recipient_role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_hub_notifications_users_recipient_user_id",
                        column: x => x.recipient_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_hub_notifications_users_sender_user_id",
                        column: x => x.sender_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "patient_backgrounds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    patient_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    background_type_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    registered_by_user_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    registered_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PatientEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    AntecedentTypeEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_backgrounds", x => x.id);
                    table.ForeignKey(
                        name: "FK_patient_backgrounds_background_types_AntecedentTypeEntityId",
                        column: x => x.AntecedentTypeEntityId,
                        principalTable: "background_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_patient_backgrounds_patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "patients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_patient_backgrounds_users_registered_by_user_id",
                        column: x => x.registered_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    user_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    refresh_token_hash = table.Column<string>(type: "NVARCHAR2(512)", maxLength: 512, nullable: false),
                    token_family = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    device = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    ip_address = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    user_agent = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    expiration_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    last_used_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    revoked_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    session_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    SessionStatusEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_session_statuses_SessionStatusEntityId",
                        column: x => x.SessionStatusEntityId,
                        principalTable: "session_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_sessions_users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    role_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    RoleEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_user_roles_roles_RoleEntityId",
                        column: x => x.RoleEntityId,
                        principalTable: "roles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_roles_users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    patient_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    professional_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    service_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    start_date_time = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    end_date_time = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    appointment_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    appointment_origin_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    chief_complaint = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    notes = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    created_by_user_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    canceled_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    cancellation_reason = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    ProfessionalEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    ServiceEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    AppointmentStatusEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    AppointmentOriginEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_appointments_appointment_origins_AppointmentOriginEntityId",
                        column: x => x.AppointmentOriginEntityId,
                        principalTable: "appointment_origins",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_appointments_appointment_statuses_AppointmentStatusEntityId",
                        column: x => x.AppointmentStatusEntityId,
                        principalTable: "appointment_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_appointments_patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "patients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_appointments_professionals_ProfessionalEntityId",
                        column: x => x.ProfessionalEntityId,
                        principalTable: "professionals",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_appointments_services_ServiceEntityId",
                        column: x => x.ServiceEntityId,
                        principalTable: "services",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_appointments_users_created_by_user_id",
                        column: x => x.created_by_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "availabilities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    professional_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    start_day = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    end_day = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    start_time = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: false),
                    lunch_start_time = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: false),
                    lunch_end_time = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: false),
                    end_time = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: false),
                    is_active = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    ProfessionalEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_availabilities", x => x.id);
                    table.ForeignKey(
                        name: "FK_availabilities_professionals_ProfessionalEntityId",
                        column: x => x.ProfessionalEntityId,
                        principalTable: "professionals",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "professional_specialties",
                columns: table => new
                {
                    professional_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    specialty_id = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_specialties", x => new { x.professional_id, x.specialty_id });
                    table.ForeignKey(
                        name: "FK_professional_specialties_professionals_professional_id",
                        column: x => x.professional_id,
                        principalTable: "professionals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_professional_specialties_specialties_specialty_id",
                        column: x => x.specialty_id,
                        principalTable: "specialties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chatbot_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    chatbot_conversation_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    message_role_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    rag_confidence = table.Column<decimal>(type: "DECIMAL(5,4)", precision: 5, scale: 4, nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ChatbotConversationEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatbot_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_chatbot_messages_chatbot_conversations_ChatbotConversationEntityId",
                        column: x => x.ChatbotConversationEntityId,
                        principalTable: "chatbot_conversations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_chatbot_messages_chatbot_message_roles_message_role_id",
                        column: x => x.message_role_id,
                        principalTable: "chatbot_message_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_tickets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    chatbot_conversation_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    patient_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    support_ticket_reason_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    notification_priority_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ticket_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    title = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    chatbot_summary = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    rag_confidence = table.Column<decimal>(type: "DECIMAL(5,4)", precision: 5, scale: 4, nullable: true),
                    created_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    assigned_to_user_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    assigned_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    attended_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    resolved_by_user_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    resolved_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    resolution = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    closed_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ChatbotConversationEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    NotificationPriorityEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_support_tickets_chatbot_conversations_ChatbotConversationEntityId",
                        column: x => x.ChatbotConversationEntityId,
                        principalTable: "chatbot_conversations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_support_tickets_notification_priorities_NotificationPriorityEntityId",
                        column: x => x.NotificationPriorityEntityId,
                        principalTable: "notification_priorities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_support_tickets_patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "patients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_support_tickets_support_ticket_reasons_support_ticket_reason_id",
                        column: x => x.support_ticket_reason_id,
                        principalTable: "support_ticket_reasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_support_tickets_support_ticket_statuses_ticket_status_id",
                        column: x => x.ticket_status_id,
                        principalTable: "support_ticket_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_support_tickets_users_assigned_to_user_id",
                        column: x => x.assigned_to_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_support_tickets_users_resolved_by_user_id",
                        column: x => x.resolved_by_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "audit_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    user_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    session_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    audit_event_type_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    entity_type = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                    entity_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    description = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    previous_data = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    new_data = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ip_address = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: true),
                    user_agent = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    event_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_success = table.Column<bool>(type: "NUMBER(1)", nullable: false, defaultValueSql: "1"),
                    error_detail = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    UserEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    SessionEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_events_audit_event_types_audit_event_type_id",
                        column: x => x.audit_event_type_id,
                        principalTable: "audit_event_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_audit_events_sessions_SessionEntityId",
                        column: x => x.SessionEntityId,
                        principalTable: "sessions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_audit_events_users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "clinical_attentions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    appointment_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    attention_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    clinical_exam = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    evolution_notes = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    treatment_plan = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AppointmentEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinical_attentions", x => x.id);
                    table.ForeignKey(
                        name: "FK_clinical_attentions_appointments_AppointmentEntityId",
                        column: x => x.AppointmentEntityId,
                        principalTable: "appointments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "whatsapp_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    appointment_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    patient_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    whatsapp_notification_type_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    whatsapp_notification_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    destination_phone = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    template_name = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    template_parameters = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    provider = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    provider_message_id = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    scheduled_for = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    sent_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    delivered_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    read_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    failed_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    provider_response = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    error_detail = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    AppointmentEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    PatientEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    WhatsAppNotificationTypeEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    WhatsAppNotificationStatusEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_whatsapp_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_whatsapp_notifications_appointments_AppointmentEntityId",
                        column: x => x.AppointmentEntityId,
                        principalTable: "appointments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_whatsapp_notifications_patients_PatientEntityId",
                        column: x => x.PatientEntityId,
                        principalTable: "patients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_whatsapp_notifications_whatsapp_notification_statuses_WhatsAppNotificationStatusEntityId",
                        column: x => x.WhatsAppNotificationStatusEntityId,
                        principalTable: "whatsapp_notification_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_whatsapp_notifications_whatsapp_notification_types_WhatsAppNotificationTypeEntityId",
                        column: x => x.WhatsAppNotificationTypeEntityId,
                        principalTable: "whatsapp_notification_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "diagnoses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    clinical_attention_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    code = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    type = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false, defaultValue: "ODONTOLOGICO"),
                    ClinicalAttentionEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diagnoses", x => x.id);
                    table.ForeignKey(
                        name: "FK_diagnoses_clinical_attentions_ClinicalAttentionEntityId",
                        column: x => x.ClinicalAttentionEntityId,
                        principalTable: "clinical_attentions",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "odontograms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    clinical_history_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    clinical_attention_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    professional_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    registration_date = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    notes = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClinicalHistoryEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    ClinicalAttentionEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    ProfessionalEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odontograms", x => x.id);
                    table.ForeignKey(
                        name: "FK_odontograms_clinical_attentions_ClinicalAttentionEntityId",
                        column: x => x.ClinicalAttentionEntityId,
                        principalTable: "clinical_attentions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_odontograms_clinical_histories_ClinicalHistoryEntityId",
                        column: x => x.ClinicalHistoryEntityId,
                        principalTable: "clinical_histories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_odontograms_professionals_ProfessionalEntityId",
                        column: x => x.ProfessionalEntityId,
                        principalTable: "professionals",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "performed_procedures",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    clinical_attention_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    service_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    performed_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    charged_amount = table.Column<decimal>(type: "DECIMAL(12,2)", precision: 12, scale: 2, nullable: true),
                    ClinicalAttentionEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    ServiceEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_performed_procedures", x => x.id);
                    table.ForeignKey(
                        name: "FK_performed_procedures_clinical_attentions_ClinicalAttentionEntityId",
                        column: x => x.ClinicalAttentionEntityId,
                        principalTable: "clinical_attentions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_performed_procedures_services_ServiceEntityId",
                        column: x => x.ServiceEntityId,
                        principalTable: "services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "odontogram_teeth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    odontogram_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    tooth_code = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    tooth_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    notes = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OdontogramEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    ToothEntityCode = table.Column<string>(type: "NVARCHAR2(2)", nullable: true),
                    ToothStatusEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_odontogram_teeth", x => x.id);
                    table.ForeignKey(
                        name: "FK_odontogram_teeth_odontograms_OdontogramEntityId",
                        column: x => x.OdontogramEntityId,
                        principalTable: "odontograms",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_odontogram_teeth_teeth_ToothEntityCode",
                        column: x => x.ToothEntityCode,
                        principalTable: "teeth",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_odontogram_teeth_tooth_statuses_ToothStatusEntityId",
                        column: x => x.ToothStatusEntityId,
                        principalTable: "tooth_statuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "dental_findings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    odontogram_tooth_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    tooth_surface_id = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    finding_type_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    finding_status_id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    notes = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    registered_at = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    OdontogramToothEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    DentalSurfaceEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    FindingTypeEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    FindingStatusEntityId = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dental_findings", x => x.id);
                    table.ForeignKey(
                        name: "FK_dental_findings_finding_statuses_FindingStatusEntityId",
                        column: x => x.FindingStatusEntityId,
                        principalTable: "finding_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_dental_findings_finding_types_FindingTypeEntityId",
                        column: x => x.FindingTypeEntityId,
                        principalTable: "finding_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_dental_findings_odontogram_teeth_OdontogramToothEntityId",
                        column: x => x.OdontogramToothEntityId,
                        principalTable: "odontogram_teeth",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_dental_findings_tooth_surfaces_DentalSurfaceEntityId",
                        column: x => x.DentalSurfaceEntityId,
                        principalTable: "tooth_surfaces",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "appointment_origins",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "MANUAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The appointment was created manually by authorized personnel.", true, "Manual", 1, null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "AGENTE_BOT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The appointment was created through the automated assistant.", true, "Bot Agent", 2, null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "WEB", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The appointment was requested through the web application.", true, "Web", 3, null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "TELEFONO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The appointment was requested during a telephone call.", true, "Telephone", 4, null }
                });

            migrationBuilder.InsertData(
                table: "appointment_statuses",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "AGENDADA", "#3B82F6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The appointment has been scheduled and is awaiting confirmation.", true, "Scheduled", 1, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "CONFIRMADA", "#22C55E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The patient has confirmed attendance for the appointment.", true, "Confirmed", 2, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "EN_SALA", "#F59E0B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The patient has arrived and is waiting to receive care.", true, "In Waiting Room", 3, null }
                });

            migrationBuilder.InsertData(
                table: "appointment_statuses",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "is_final", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000004"), "ATENDIDA", "#14B8A6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The patient received the scheduled dental care.", true, true, "Completed", 4, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "CANCELADA", "#EF4444", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The appointment was cancelled before dental care was provided.", true, true, "Cancelled", 5, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "NO_ASISTIO", "#64748B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The patient did not attend the scheduled appointment.", true, true, "No Show", 6, null }
                });

            migrationBuilder.InsertData(
                table: "audit_event_types",
                columns: new[] { "id", "category", "code", "CreatedAt", "description", "is_active", "name", "risk_level", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("16000000-0000-0000-0000-000000000001"), "SEGURIDAD", "INICIO_SESION", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A user signed in to the system.", true, "Sign In", 2, 1, null },
                    { new Guid("16000000-0000-0000-0000-000000000002"), "AGENDA", "AGENDAR_CITA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An appointment was created for a patient.", true, "Schedule Appointment", 1, 2, null },
                    { new Guid("16000000-0000-0000-0000-000000000003"), "SOPORTE", "CERRAR_TICKET", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A support ticket was closed.", true, "Close Ticket", 1, 3, null },
                    { new Guid("16000000-0000-0000-0000-000000000004"), "CLINICO", "MODIFICAR_HISTORIA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A clinical history record was modified.", true, "Modify Clinical History", 3, 4, null }
                });

            migrationBuilder.InsertData(
                table: "background_types",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("17000000-0000-0000-0000-000000000001"), "MEDICO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "General medical history relevant to dental care.", true, "Medical", 1, null },
                    { new Guid("17000000-0000-0000-0000-000000000002"), "ODONTOLOGICO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Previous dental treatments and conditions.", true, "Dental", 2, null },
                    { new Guid("17000000-0000-0000-0000-000000000003"), "ALERGIA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Known allergies of the patient.", true, "Allergy", 3, null },
                    { new Guid("17000000-0000-0000-0000-000000000004"), "MEDICAMENTO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Medication the patient is currently taking.", true, "Medication", 4, null }
                });

            migrationBuilder.InsertData(
                table: "chat_channels",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000001"), "WHATSAPP", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Conversation initiated through the WhatsApp channel.", true, "WhatsApp", 1, null },
                    { new Guid("90000000-0000-0000-0000-000000000002"), "TELEGRAM", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Conversation initiated through the Telegram channel.", true, "Telegram", 2, null },
                    { new Guid("90000000-0000-0000-0000-000000000003"), "WEBCHAT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Conversation initiated through the web chat channel.", true, "Web Chat", 3, null }
                });

            migrationBuilder.InsertData(
                table: "chatbot_conversation_statuses",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), "ACTIVA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The conversation is active and being handled by the chatbot.", true, "Active", 1, null },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), "ESCALADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The conversation was escalated and is waiting for human support.", true, "Escalated", 2, null },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), "ATENDIDA_HUMANO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A human agent is currently handling the conversation.", true, "Handled by Human", 3, null }
                });

            migrationBuilder.InsertData(
                table: "chatbot_conversation_statuses",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "is_final", "name", "display_order", "UpdatedAt" },
                values: new object[] { new Guid("b0000000-0000-0000-0000-000000000004"), "CERRADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The conversation has ended and requires no further action.", true, true, "Closed", 4, null });

            migrationBuilder.InsertData(
                table: "chatbot_message_roles",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "USUARIO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Message sent by the user who initiated the conversation.", true, "User", 1, null },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "CHATBOT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Message generated automatically by the conversational assistant.", true, "Chatbot", 2, null },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "AGENTE_HUMANO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Message sent by a human support agent.", true, "Human Agent", 3, null },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), "SISTEMA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Internal message generated by the platform.", true, "System", 4, null }
                });

            migrationBuilder.InsertData(
                table: "dentition_types",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("19000000-0000-0000-0000-000000000001"), "PERMANENTE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permanent dentition according to the FDI standard.", true, "Permanent", 1, null },
                    { new Guid("19000000-0000-0000-0000-000000000002"), "TEMPORAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Primary or deciduous dentition according to the FDI standard.", true, "Deciduous", 2, null }
                });

            migrationBuilder.InsertData(
                table: "document_types",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("e0000000-0000-0000-0000-000000000001"), "CC", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "National identity document for Colombian citizens.", true, "Citizenship Card", 1, null },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), "CE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Identity document for foreign residents.", true, "Foreigner Card", 2, null },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), "TI", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Identity document for minors.", true, "Identity Card", 3, null },
                    { new Guid("e0000000-0000-0000-0000-000000000004"), "PAS", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "International travel document.", true, "Passport", 4, null }
                });

            migrationBuilder.InsertData(
                table: "finding_statuses",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1d000000-0000-0000-0000-000000000001"), "ACTIVO", "#F44336", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finding is present and pending treatment.", true, "Active", 1, null },
                    { new Guid("1d000000-0000-0000-0000-000000000002"), "TRATADO", "#4CAF50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finding was treated and resolved.", true, "Treated", 2, null },
                    { new Guid("1d000000-0000-0000-0000-000000000003"), "INACTIVO", "#9E9E9E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Finding is no longer relevant for follow up.", true, "Inactive", 3, null }
                });

            migrationBuilder.InsertData(
                table: "finding_types",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "name", "requires_surface", "display_order", "UpdatedAt", "visual_symbol" },
                values: new object[,]
                {
                    { new Guid("1c000000-0000-0000-0000-000000000001"), "CARIES", "#F44336", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Carious lesion on a tooth surface.", true, "Caries", true, 1, null, "CARIES" },
                    { new Guid("1c000000-0000-0000-0000-000000000002"), "RESTAURACION", "#2196F3", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Existing filling or restoration on a tooth surface.", true, "Restoration", true, 2, null, "FILLING" }
                });

            migrationBuilder.InsertData(
                table: "finding_types",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt", "visual_symbol" },
                values: new object[,]
                {
                    { new Guid("1c000000-0000-0000-0000-000000000003"), "FRACTURA", "#FF9800", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fracture affecting the tooth structure.", true, "Fracture", 3, null, "FRACTURE" },
                    { new Guid("1c000000-0000-0000-0000-000000000004"), "PROTESIS", "#9C27B0", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fixed or removable prosthesis on the tooth.", true, "Prosthesis", 4, null, "PROSTHESIS" }
                });

            migrationBuilder.InsertData(
                table: "finding_types",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "name", "requires_surface", "display_order", "UpdatedAt", "visual_symbol" },
                values: new object[] { new Guid("1c000000-0000-0000-0000-000000000005"), "SELLANTE", "#4CAF50", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Preventive sealant applied to a tooth surface.", true, "Sealant", true, 5, null, "SEALANT" });

            migrationBuilder.InsertData(
                table: "genders",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("f0000000-0000-0000-0000-000000000001"), "FEMENINO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Female sex.", true, "Female", 1, null },
                    { new Guid("f0000000-0000-0000-0000-000000000002"), "MASCULINO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Male sex.", true, "Male", 2, null },
                    { new Guid("f0000000-0000-0000-0000-000000000003"), "OTRO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Other or unspecified sex.", true, "Other", 3, null }
                });

            migrationBuilder.InsertData(
                table: "hub_notification_statuses",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("60000000-0000-0000-0000-000000000001"), "PENDIENTE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The notification is waiting to be delivered.", true, "Pending", 1, null },
                    { new Guid("60000000-0000-0000-0000-000000000002"), "ENTREGADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The notification was delivered to the recipient.", true, "Delivered", 2, null },
                    { new Guid("60000000-0000-0000-0000-000000000003"), "LEIDA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The recipient has read the notification.", true, "Read", 3, null },
                    { new Guid("60000000-0000-0000-0000-000000000004"), "CANCELADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The notification was cancelled before delivery.", true, "Cancelled", 4, null }
                });

            migrationBuilder.InsertData(
                table: "hub_notification_types",
                columns: new[] { "id", "code", "CreatedAt", "description", "icon", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), "TICKET_CREADO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A new support ticket has been created.", "ticket", true, "Ticket Created", 1, null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "CITA_CREADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A new dental appointment has been created.", "calendar-plus", true, "Appointment Created", 2, null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "CITA_CANCELADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A scheduled dental appointment has been cancelled.", "calendar-x", true, "Appointment Cancelled", 3, null },
                    { new Guid("40000000-0000-0000-0000-000000000004"), "PACIENTE_REGISTRADO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A new patient has been registered in the system.", "user-plus", true, "Patient Registered", 4, null }
                });

            migrationBuilder.InsertData(
                table: "job_positions",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11000000-0000-0000-0000-000000000001"), "ADMINISTRADOR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staff member responsible for clinic administration.", true, "Administrator", 1, null },
                    { new Guid("11000000-0000-0000-0000-000000000002"), "RECEPCIONISTA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staff member responsible for front desk and scheduling.", true, "Receptionist", 2, null },
                    { new Guid("11000000-0000-0000-0000-000000000003"), "ODONTOLOGO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Licensed professional who performs dental procedures.", true, "Dentist", 3, null },
                    { new Guid("11000000-0000-0000-0000-000000000004"), "ASISTENTE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Staff member who assists during dental procedures.", true, "Dental Assistant", 4, null }
                });

            migrationBuilder.InsertData(
                table: "notification_priorities",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "level", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000001"), "BAJA", "#22C55E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Low-priority information that does not require immediate action.", true, 1, "Low", 1, null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), "NORMAL", "#3B82F6", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Standard-priority information for regular follow-up.", true, 2, "Normal", 2, null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), "ALTA", "#F59E0B", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Important information that should be addressed promptly.", true, 3, "High", 3, null },
                    { new Guid("50000000-0000-0000-0000-000000000004"), "URGENTE", "#EF4444", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Critical information that requires immediate attention.", true, 4, "Urgent", 4, null }
                });

            migrationBuilder.InsertData(
                table: "permission_actions",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("12000000-0000-0000-0000-000000000001"), "VER", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows reading records of a module.", true, "View", 1, null },
                    { new Guid("12000000-0000-0000-0000-000000000002"), "CREAR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows creating records in a module.", true, "Create", 2, null },
                    { new Guid("12000000-0000-0000-0000-000000000003"), "EDITAR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows modifying records of a module.", true, "Edit", 3, null },
                    { new Guid("12000000-0000-0000-0000-000000000004"), "ELIMINAR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Allows deleting or deactivating records of a module.", true, "Delete", 4, null }
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "ActionPermissionEntityId", "permission_action_id", "description", "module" },
                values: new object[,]
                {
                    { new Guid("14000000-0000-0000-0001-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the PACIENTES module.", "PACIENTES" },
                    { new Guid("14000000-0000-0000-0001-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the PACIENTES module.", "PACIENTES" },
                    { new Guid("14000000-0000-0000-0001-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the PACIENTES module.", "PACIENTES" },
                    { new Guid("14000000-0000-0000-0001-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the PACIENTES module.", "PACIENTES" },
                    { new Guid("14000000-0000-0000-0002-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the CITAS module.", "CITAS" },
                    { new Guid("14000000-0000-0000-0002-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the CITAS module.", "CITAS" },
                    { new Guid("14000000-0000-0000-0002-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the CITAS module.", "CITAS" },
                    { new Guid("14000000-0000-0000-0002-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the CITAS module.", "CITAS" },
                    { new Guid("14000000-0000-0000-0003-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the CATALOGOS module.", "CATALOGOS" },
                    { new Guid("14000000-0000-0000-0003-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the CATALOGOS module.", "CATALOGOS" },
                    { new Guid("14000000-0000-0000-0003-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the CATALOGOS module.", "CATALOGOS" },
                    { new Guid("14000000-0000-0000-0003-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the CATALOGOS module.", "CATALOGOS" },
                    { new Guid("14000000-0000-0000-0004-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the USUARIOS module.", "USUARIOS" },
                    { new Guid("14000000-0000-0000-0004-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the USUARIOS module.", "USUARIOS" },
                    { new Guid("14000000-0000-0000-0004-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the USUARIOS module.", "USUARIOS" },
                    { new Guid("14000000-0000-0000-0004-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the USUARIOS module.", "USUARIOS" },
                    { new Guid("14000000-0000-0000-0005-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the HISTORIAS module.", "HISTORIAS" },
                    { new Guid("14000000-0000-0000-0005-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the HISTORIAS module.", "HISTORIAS" },
                    { new Guid("14000000-0000-0000-0005-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the HISTORIAS module.", "HISTORIAS" },
                    { new Guid("14000000-0000-0000-0005-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the HISTORIAS module.", "HISTORIAS" },
                    { new Guid("14000000-0000-0000-0006-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the ODONTOGRAMA module.", "ODONTOGRAMA" },
                    { new Guid("14000000-0000-0000-0006-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the ODONTOGRAMA module.", "ODONTOGRAMA" },
                    { new Guid("14000000-0000-0000-0006-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the ODONTOGRAMA module.", "ODONTOGRAMA" },
                    { new Guid("14000000-0000-0000-0006-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the ODONTOGRAMA module.", "ODONTOGRAMA" },
                    { new Guid("14000000-0000-0000-0007-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the NOTIFICACIONES module.", "NOTIFICACIONES" },
                    { new Guid("14000000-0000-0000-0007-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the NOTIFICACIONES module.", "NOTIFICACIONES" },
                    { new Guid("14000000-0000-0000-0007-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the NOTIFICACIONES module.", "NOTIFICACIONES" },
                    { new Guid("14000000-0000-0000-0007-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the NOTIFICACIONES module.", "NOTIFICACIONES" },
                    { new Guid("14000000-0000-0000-0008-000000000001"), null, new Guid("12000000-0000-0000-0000-000000000001"), "Allows to ver records of the SOPORTE module.", "SOPORTE" },
                    { new Guid("14000000-0000-0000-0008-000000000002"), null, new Guid("12000000-0000-0000-0000-000000000002"), "Allows to crear records of the SOPORTE module.", "SOPORTE" },
                    { new Guid("14000000-0000-0000-0008-000000000003"), null, new Guid("12000000-0000-0000-0000-000000000003"), "Allows to editar records of the SOPORTE module.", "SOPORTE" },
                    { new Guid("14000000-0000-0000-0008-000000000004"), null, new Guid("12000000-0000-0000-0000-000000000004"), "Allows to eliminar records of the SOPORTE module.", "SOPORTE" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "code", "created_at", "description", "is_active", "name", "updated_at" },
                values: new object[,]
                {
                    { new Guid("13000000-0000-0000-0000-000000000001"), "ADMINISTRADOR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Full access to every module of the system.", true, "Administrator", null },
                    { new Guid("13000000-0000-0000-0000-000000000002"), "ODONTOLOGO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Access to clinical records, odontogram and appointments.", true, "Dentist", null },
                    { new Guid("13000000-0000-0000-0000-000000000003"), "RECEPCIONISTA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Access to patient registration and appointment scheduling.", true, "Receptionist", null },
                    { new Guid("13000000-0000-0000-0000-000000000004"), "ASISTENTE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Read access to clinical information for assisting procedures.", true, "Dental Assistant", null },
                    { new Guid("13000000-0000-0000-0000-000000000005"), "PACIENTE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Access limited to the patient's own information.", true, "Patient", null }
                });

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "id", "code", "CreatedAt", "description", "duration_minutes", "is_active", "name", "price", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), "VALORACION_GENERAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Initial oral examination and general dental assessment.", 30, true, "General Assessment", 60000m, 1, null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "PROFILAXIS", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Professional dental cleaning and plaque removal.", 45, true, "Dental Prophylaxis", 120000m, 2, null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "CALZA_RESINA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Restoration of a tooth using composite resin material.", 60, true, "Composite Resin Filling", 180000m, 3, null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "BLANQUEAMIENTO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Professional treatment to improve the color of the teeth.", 90, true, "Teeth Whitening", 450000m, 4, null }
                });

            migrationBuilder.InsertData(
                table: "session_statuses",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("15000000-0000-0000-0000-000000000001"), "ACTIVA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Session is currently valid.", true, "Active", 1, null },
                    { new Guid("15000000-0000-0000-0000-000000000002"), "REVOCADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Session was invalidated before its expiration.", true, "Revoked", 2, null },
                    { new Guid("15000000-0000-0000-0000-000000000003"), "EXPIRADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Session reached its expiration time.", true, "Expired", 3, null }
                });

            migrationBuilder.InsertData(
                table: "specialties",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("18000000-0000-0000-0000-000000000001"), "ODONTOLOGIA_GENERAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comprehensive general dental care.", true, "General Dentistry", 1, null },
                    { new Guid("18000000-0000-0000-0000-000000000002"), "ORTODONCIA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Correction of teeth and jaw alignment.", true, "Orthodontics", 2, null },
                    { new Guid("18000000-0000-0000-0000-000000000003"), "ENDODONCIA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Root canal and dental pulp treatment.", true, "Endodontics", 3, null },
                    { new Guid("18000000-0000-0000-0000-000000000004"), "PERIODONCIA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Treatment of gums and supporting structures.", true, "Periodontics", 4, null },
                    { new Guid("18000000-0000-0000-0000-000000000005"), "ODONTOPEDIATRIA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dental care for children and adolescents.", true, "Pediatric Dentistry", 5, null },
                    { new Guid("18000000-0000-0000-0000-000000000006"), "CIRUGIA_ORAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surgical procedures of the mouth and jaw.", true, "Oral Surgery", 6, null }
                });

            migrationBuilder.InsertData(
                table: "support_ticket_reasons",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d0000000-0000-0000-0000-000000000001"), "SOLICITUD_USUARIO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The user explicitly requested assistance from a human support agent.", true, "User Request", 1, null },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), "BAJA_CONFIANZA_RAG", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The retrieval system could not produce an answer with sufficient confidence.", true, "Low RAG Confidence", 2, null },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), "ERROR_AGENDAMIENTO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The automated assistant could not complete an appointment operation.", true, "Scheduling Error", 3, null },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), "RECLAMO_SERVICIO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The user submitted a complaint about a dental service or received care.", true, "Service Complaint", 4, null },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), "CONSULTA_COMPLEJA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The inquiry requires professional judgment or information beyond the chatbot scope.", true, "Complex Inquiry", 5, null }
                });

            migrationBuilder.InsertData(
                table: "support_ticket_statuses",
                columns: new[] { "id", "allows_assignment", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[] { new Guid("c0000000-0000-0000-0000-000000000001"), true, "ABIERTO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The support ticket is open and available for assignment.", true, "Open", 1, null });

            migrationBuilder.InsertData(
                table: "support_ticket_statuses",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000002"), "ASIGNADO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The support ticket has been assigned to a human agent.", true, "Assigned", 2, null },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), "EN_ATENCION", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A human agent is actively working on the support ticket.", true, "In Progress", 3, null }
                });

            migrationBuilder.InsertData(
                table: "support_ticket_statuses",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "is_final", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000004"), "RESUELTO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The reported support request has been resolved.", true, true, "Resolved", 4, null },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), "CERRADO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The support ticket has been closed and requires no further action.", true, true, "Closed", 5, null }
                });

            migrationBuilder.InsertData(
                table: "teeth",
                columns: new[] { "code", "DentitionTypeEntityId", "dentition_type_id", "name", "position", "quadrant" },
                values: new object[,]
                {
                    { "11", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right Central Incisor", 1, 1 },
                    { "12", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right Lateral Incisor", 2, 1 },
                    { "13", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right Canine", 3, 1 },
                    { "14", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right First Premolar", 4, 1 },
                    { "15", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right Second Premolar", 5, 1 },
                    { "16", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right First Molar", 6, 1 },
                    { "17", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right Second Molar", 7, 1 },
                    { "18", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Right Third Molar", 8, 1 },
                    { "21", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left Central Incisor", 1, 2 },
                    { "22", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left Lateral Incisor", 2, 2 },
                    { "23", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left Canine", 3, 2 },
                    { "24", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left First Premolar", 4, 2 },
                    { "25", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left Second Premolar", 5, 2 },
                    { "26", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left First Molar", 6, 2 },
                    { "27", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left Second Molar", 7, 2 },
                    { "28", null, new Guid("19000000-0000-0000-0000-000000000001"), "Upper Left Third Molar", 8, 2 },
                    { "31", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left Central Incisor", 1, 3 },
                    { "32", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left Lateral Incisor", 2, 3 },
                    { "33", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left Canine", 3, 3 },
                    { "34", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left First Premolar", 4, 3 },
                    { "35", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left Second Premolar", 5, 3 },
                    { "36", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left First Molar", 6, 3 },
                    { "37", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left Second Molar", 7, 3 },
                    { "38", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Left Third Molar", 8, 3 },
                    { "41", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right Central Incisor", 1, 4 },
                    { "42", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right Lateral Incisor", 2, 4 },
                    { "43", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right Canine", 3, 4 },
                    { "44", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right First Premolar", 4, 4 },
                    { "45", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right Second Premolar", 5, 4 },
                    { "46", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right First Molar", 6, 4 },
                    { "47", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right Second Molar", 7, 4 },
                    { "48", null, new Guid("19000000-0000-0000-0000-000000000001"), "Lower Right Third Molar", 8, 4 },
                    { "51", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Right Central Incisor", 1, 5 },
                    { "52", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Right Lateral Incisor", 2, 5 },
                    { "53", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Right Canine", 3, 5 },
                    { "54", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Right First Molar", 4, 5 },
                    { "55", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Right Second Molar", 5, 5 },
                    { "61", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Left Central Incisor", 1, 6 },
                    { "62", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Left Lateral Incisor", 2, 6 },
                    { "63", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Left Canine", 3, 6 },
                    { "64", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Left First Molar", 4, 6 },
                    { "65", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Upper Left Second Molar", 5, 6 },
                    { "71", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Left Central Incisor", 1, 7 },
                    { "72", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Left Lateral Incisor", 2, 7 },
                    { "73", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Left Canine", 3, 7 },
                    { "74", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Left First Molar", 4, 7 },
                    { "75", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Left Second Molar", 5, 7 },
                    { "81", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Right Central Incisor", 1, 8 },
                    { "82", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Right Lateral Incisor", 2, 8 },
                    { "83", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Right Canine", 3, 8 },
                    { "84", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Right First Molar", 4, 8 },
                    { "85", null, new Guid("19000000-0000-0000-0000-000000000002"), "Primary Lower Right Second Molar", 5, 8 }
                });

            migrationBuilder.InsertData(
                table: "tooth_statuses",
                columns: new[] { "id", "code", "color_hex", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt", "visual_code" },
                values: new object[,]
                {
                    { new Guid("1a000000-0000-0000-0000-000000000001"), "SANO", "#FFFFFF", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tooth without any detected condition.", true, "Healthy", 1, null, "HEALTHY" },
                    { new Guid("1a000000-0000-0000-0000-000000000002"), "AUSENTE", "#9E9E9E", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tooth is not present and was never erupted or is congenitally absent.", true, "Missing", 2, null, "MISSING" },
                    { new Guid("1a000000-0000-0000-0000-000000000003"), "EXTRAIDO", "#616161", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tooth was removed through extraction.", true, "Extracted", 3, null, "EXTRACTED" },
                    { new Guid("1a000000-0000-0000-0000-000000000004"), "IMPLANTE", "#3F51B5", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tooth was replaced by a dental implant.", true, "Implant", 4, null, "IMPLANT" },
                    { new Guid("1a000000-0000-0000-0000-000000000005"), "CORONA", "#FFC107", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tooth is restored with a full crown.", true, "Crown", 5, null, "CROWN" }
                });

            migrationBuilder.InsertData(
                table: "tooth_surfaces",
                columns: new[] { "id", "applies_to_anterior", "applies_to_posterior", "code", "CreatedAt", "description", "is_active", "name", "visual_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1b000000-0000-0000-0000-000000000001"), true, true, "MESIAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface facing the midline of the dental arch.", true, "Mesial", 1, null },
                    { new Guid("1b000000-0000-0000-0000-000000000002"), true, true, "DISTAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface facing away from the midline of the dental arch.", true, "Distal", 2, null }
                });

            migrationBuilder.InsertData(
                table: "tooth_surfaces",
                columns: new[] { "id", "applies_to_posterior", "code", "CreatedAt", "description", "is_active", "name", "visual_order", "UpdatedAt" },
                values: new object[] { new Guid("1b000000-0000-0000-0000-000000000003"), true, "OCLUSAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chewing surface of premolars and molars.", true, "Occlusal", 3, null });

            migrationBuilder.InsertData(
                table: "tooth_surfaces",
                columns: new[] { "id", "applies_to_anterior", "code", "CreatedAt", "description", "is_active", "name", "visual_order", "UpdatedAt" },
                values: new object[] { new Guid("1b000000-0000-0000-0000-000000000004"), true, "INCISAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cutting edge of incisors and canines.", true, "Incisal", 4, null });

            migrationBuilder.InsertData(
                table: "tooth_surfaces",
                columns: new[] { "id", "applies_to_anterior", "applies_to_posterior", "code", "CreatedAt", "description", "is_active", "name", "visual_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1b000000-0000-0000-0000-000000000005"), true, true, "VESTIBULAR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface facing the cheek or lips.", true, "Buccal", 5, null },
                    { new Guid("1b000000-0000-0000-0000-000000000006"), true, true, "PALATINO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface of upper teeth facing the palate.", true, "Palatal", 6, null },
                    { new Guid("1b000000-0000-0000-0000-000000000007"), true, true, "LINGUAL", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface of lower teeth facing the tongue.", true, "Lingual", 7, null }
                });

            migrationBuilder.InsertData(
                table: "whatsapp_notification_statuses",
                columns: new[] { "id", "code", "CreatedAt", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000001"), "PENDIENTE", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The message is waiting to be sent.", true, "Pending", 1, null },
                    { new Guid("80000000-0000-0000-0000-000000000002"), "ENVIADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The message was sent to the messaging provider.", true, "Sent", 2, null },
                    { new Guid("80000000-0000-0000-0000-000000000003"), "ENTREGADA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The message was delivered to the recipient device.", true, "Delivered", 3, null },
                    { new Guid("80000000-0000-0000-0000-000000000004"), "LEIDA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The recipient has read the message.", true, "Read", 4, null },
                    { new Guid("80000000-0000-0000-0000-000000000005"), "FALLIDA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The message could not be delivered due to an error.", true, "Failed", 5, null }
                });

            migrationBuilder.InsertData(
                table: "whatsapp_notification_types",
                columns: new[] { "id", "code", "CreatedAt", "default_template", "description", "is_active", "name", "display_order", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), "RECORDATORIO_CITA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "appointment_reminder", "Reminds a patient about an upcoming dental appointment.", true, "Appointment Reminder", 1, null },
                    { new Guid("70000000-0000-0000-0000-000000000002"), "CONFIRMACION_CITA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "appointment_confirmation", "Confirms the details of a scheduled dental appointment.", true, "Appointment Confirmation", 2, null },
                    { new Guid("70000000-0000-0000-0000-000000000003"), "TRANSFERENCIA_A_ASESOR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "advisor_transfer", "Informs the user that the conversation is being transferred to a human advisor.", true, "Advisor Transfer", 3, null },
                    { new Guid("70000000-0000-0000-0000-000000000004"), "BIENVENIDA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "welcome", "Welcomes a user to the dental care communication channel.", true, "Welcome", 4, null }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("14000000-0000-0000-0001-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0001-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0001-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0001-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0002-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0002-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0002-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0002-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0003-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0003-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0003-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0003-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0004-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0004-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0004-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0004-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0005-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0005-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0005-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0005-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0006-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0006-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0006-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0006-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0007-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0007-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0007-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0007-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0008-000000000001"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0008-000000000002"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0008-000000000003"), new Guid("13000000-0000-0000-0000-000000000001") },
                    { new Guid("14000000-0000-0000-0008-000000000004"), new Guid("13000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointment_origins_code",
                table: "appointment_origins",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointment_origins_name",
                table: "appointment_origins",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointment_statuses_code",
                table: "appointment_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointment_statuses_name",
                table: "appointment_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppointmentOriginEntityId",
                table: "appointments",
                column: "AppointmentOriginEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppointmentStatusEntityId",
                table: "appointments",
                column: "AppointmentStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_created_by_user_id",
                table: "appointments",
                column: "created_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_patient_id_start_date_time",
                table: "appointments",
                columns: new[] { "patient_id", "start_date_time" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_PatientEntityId",
                table: "appointments",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_professional_id_start_date_time",
                table: "appointments",
                columns: new[] { "professional_id", "start_date_time" });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ProfessionalEntityId",
                table: "appointments",
                column: "ProfessionalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_ServiceEntityId",
                table: "appointments",
                column: "ServiceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_event_types_code",
                table: "audit_event_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_audit_event_types_name",
                table: "audit_event_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_audit_event_type_id",
                table: "audit_events",
                column: "audit_event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_SessionEntityId",
                table: "audit_events",
                column: "SessionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_events_UserEntityId",
                table: "audit_events",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_availabilities_ProfessionalEntityId",
                table: "availabilities",
                column: "ProfessionalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_background_types_code",
                table: "background_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_background_types_name",
                table: "background_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_channels_code",
                table: "chat_channels",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_channels_name",
                table: "chat_channels",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_conversation_statuses_code",
                table: "chatbot_conversation_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_conversation_statuses_name",
                table: "chatbot_conversation_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_conversations_chat_channel_id_chat_identifier",
                table: "chatbot_conversations",
                columns: new[] { "chat_channel_id", "chat_identifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_conversations_ChatChannelEntityId",
                table: "chatbot_conversations",
                column: "ChatChannelEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_conversations_conversation_status_id",
                table: "chatbot_conversations",
                column: "conversation_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_conversations_PatientEntityId",
                table: "chatbot_conversations",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_message_roles_code",
                table: "chatbot_message_roles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_message_roles_name",
                table: "chatbot_message_roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_messages_ChatbotConversationEntityId",
                table: "chatbot_messages",
                column: "ChatbotConversationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_chatbot_messages_message_role_id",
                table: "chatbot_messages",
                column: "message_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_clinical_attentions_appointment_id",
                table: "clinical_attentions",
                column: "appointment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clinical_attentions_AppointmentEntityId",
                table: "clinical_attentions",
                column: "AppointmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_clinical_histories_patient_id",
                table: "clinical_histories",
                column: "patient_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clinical_histories_PatientEntityId",
                table: "clinical_histories",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_dental_findings_DentalSurfaceEntityId",
                table: "dental_findings",
                column: "DentalSurfaceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_dental_findings_FindingStatusEntityId",
                table: "dental_findings",
                column: "FindingStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_dental_findings_FindingTypeEntityId",
                table: "dental_findings",
                column: "FindingTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_dental_findings_OdontogramToothEntityId",
                table: "dental_findings",
                column: "OdontogramToothEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_dentition_types_code",
                table: "dentition_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dentition_types_name",
                table: "dentition_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_diagnoses_ClinicalAttentionEntityId",
                table: "diagnoses",
                column: "ClinicalAttentionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_document_types_code",
                table: "document_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_types_name",
                table: "document_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_JobTitleEntityId",
                table: "employees",
                column: "JobTitleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_employees_person_id",
                table: "employees",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_finding_statuses_code",
                table: "finding_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_finding_statuses_name",
                table: "finding_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_finding_types_code",
                table: "finding_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_finding_types_name",
                table: "finding_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_genders_code",
                table: "genders",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_genders_name",
                table: "genders",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hub_notification_statuses_code",
                table: "hub_notification_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hub_notification_statuses_name",
                table: "hub_notification_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hub_notification_types_code",
                table: "hub_notification_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hub_notification_types_name",
                table: "hub_notification_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hub_notifications_HubNotificationStatusEntityId",
                table: "hub_notifications",
                column: "HubNotificationStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_hub_notifications_HubNotificationTypeEntityId",
                table: "hub_notifications",
                column: "HubNotificationTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_hub_notifications_NotificationPriorityEntityId",
                table: "hub_notifications",
                column: "NotificationPriorityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_hub_notifications_recipient_role_id",
                table: "hub_notifications",
                column: "recipient_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_hub_notifications_recipient_user_id",
                table: "hub_notifications",
                column: "recipient_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_hub_notifications_sender_user_id",
                table: "hub_notifications",
                column: "sender_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_positions_code",
                table: "job_positions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_positions_name",
                table: "job_positions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_priorities_code",
                table: "notification_priorities",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_priorities_name",
                table: "notification_priorities",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_odontogram_teeth_odontogram_id_tooth_code",
                table: "odontogram_teeth",
                columns: new[] { "odontogram_id", "tooth_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_odontogram_teeth_OdontogramEntityId",
                table: "odontogram_teeth",
                column: "OdontogramEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_odontogram_teeth_ToothEntityCode",
                table: "odontogram_teeth",
                column: "ToothEntityCode");

            migrationBuilder.CreateIndex(
                name: "IX_odontogram_teeth_ToothStatusEntityId",
                table: "odontogram_teeth",
                column: "ToothStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_odontograms_ClinicalAttentionEntityId",
                table: "odontograms",
                column: "ClinicalAttentionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_odontograms_ClinicalHistoryEntityId",
                table: "odontograms",
                column: "ClinicalHistoryEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_odontograms_ProfessionalEntityId",
                table: "odontograms",
                column: "ProfessionalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_patient_backgrounds_AntecedentTypeEntityId",
                table: "patient_backgrounds",
                column: "AntecedentTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_patient_backgrounds_PatientEntityId",
                table: "patient_backgrounds",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_patient_backgrounds_registered_by_user_id",
                table: "patient_backgrounds",
                column: "registered_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_patients_person_id",
                table: "patients",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_performed_procedures_ClinicalAttentionEntityId",
                table: "performed_procedures",
                column: "ClinicalAttentionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_performed_procedures_ServiceEntityId",
                table: "performed_procedures",
                column: "ServiceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_actions_code",
                table: "permission_actions",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permission_actions_name",
                table: "permission_actions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permissions_ActionPermissionEntityId",
                table: "permissions",
                column: "ActionPermissionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_persons_document_type_id_document_number",
                table: "persons",
                columns: new[] { "document_type_id", "document_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_persons_DocumentTypeEntityId",
                table: "persons",
                column: "DocumentTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_persons_email",
                table: "persons",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_persons_SexEntityId",
                table: "persons",
                column: "SexEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_specialties_specialty_id",
                table: "professional_specialties",
                column: "specialty_id");

            migrationBuilder.CreateIndex(
                name: "IX_professionals_employee_id",
                table: "professionals",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_professionals_professional_license",
                table: "professionals",
                column: "professional_license",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_code",
                table: "roles",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_services_code",
                table: "services",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_services_name",
                table: "services",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_session_statuses_code",
                table: "session_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_session_statuses_name",
                table: "session_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_refresh_token_hash",
                table: "sessions",
                column: "refresh_token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_SessionStatusEntityId",
                table: "sessions",
                column: "SessionStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_UserEntityId",
                table: "sessions",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_specialties_code",
                table: "specialties",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_specialties_name",
                table: "specialties",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_support_ticket_reasons_code",
                table: "support_ticket_reasons",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_support_ticket_reasons_name",
                table: "support_ticket_reasons",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_support_ticket_statuses_code",
                table: "support_ticket_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_support_ticket_statuses_name",
                table: "support_ticket_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_assigned_to_user_id",
                table: "support_tickets",
                column: "assigned_to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_ChatbotConversationEntityId",
                table: "support_tickets",
                column: "ChatbotConversationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_NotificationPriorityEntityId",
                table: "support_tickets",
                column: "NotificationPriorityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_PatientEntityId",
                table: "support_tickets",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_resolved_by_user_id",
                table: "support_tickets",
                column: "resolved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_support_ticket_reason_id",
                table: "support_tickets",
                column: "support_ticket_reason_id");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_ticket_status_id",
                table: "support_tickets",
                column: "ticket_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_teeth_DentitionTypeEntityId",
                table: "teeth",
                column: "DentitionTypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_tooth_statuses_code",
                table: "tooth_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tooth_statuses_name",
                table: "tooth_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tooth_surfaces_code",
                table: "tooth_surfaces",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tooth_surfaces_name",
                table: "tooth_surfaces",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_RoleEntityId",
                table: "user_roles",
                column: "RoleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_UserEntityId",
                table: "user_roles",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_users_person_id",
                table: "users",
                column: "person_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notification_statuses_code",
                table: "whatsapp_notification_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notification_statuses_name",
                table: "whatsapp_notification_statuses",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notification_types_code",
                table: "whatsapp_notification_types",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notification_types_name",
                table: "whatsapp_notification_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notifications_AppointmentEntityId",
                table: "whatsapp_notifications",
                column: "AppointmentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notifications_PatientEntityId",
                table: "whatsapp_notifications",
                column: "PatientEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notifications_WhatsAppNotificationStatusEntityId",
                table: "whatsapp_notifications",
                column: "WhatsAppNotificationStatusEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_whatsapp_notifications_WhatsAppNotificationTypeEntityId",
                table: "whatsapp_notifications",
                column: "WhatsAppNotificationTypeEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_events");

            migrationBuilder.DropTable(
                name: "availabilities");

            migrationBuilder.DropTable(
                name: "chatbot_messages");

            migrationBuilder.DropTable(
                name: "dental_findings");

            migrationBuilder.DropTable(
                name: "diagnoses");

            migrationBuilder.DropTable(
                name: "hub_notifications");

            migrationBuilder.DropTable(
                name: "patient_backgrounds");

            migrationBuilder.DropTable(
                name: "performed_procedures");

            migrationBuilder.DropTable(
                name: "professional_specialties");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "support_tickets");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "whatsapp_notifications");

            migrationBuilder.DropTable(
                name: "audit_event_types");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "chatbot_message_roles");

            migrationBuilder.DropTable(
                name: "finding_statuses");

            migrationBuilder.DropTable(
                name: "finding_types");

            migrationBuilder.DropTable(
                name: "odontogram_teeth");

            migrationBuilder.DropTable(
                name: "tooth_surfaces");

            migrationBuilder.DropTable(
                name: "hub_notification_statuses");

            migrationBuilder.DropTable(
                name: "hub_notification_types");

            migrationBuilder.DropTable(
                name: "background_types");

            migrationBuilder.DropTable(
                name: "specialties");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "chatbot_conversations");

            migrationBuilder.DropTable(
                name: "notification_priorities");

            migrationBuilder.DropTable(
                name: "support_ticket_reasons");

            migrationBuilder.DropTable(
                name: "support_ticket_statuses");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "whatsapp_notification_statuses");

            migrationBuilder.DropTable(
                name: "whatsapp_notification_types");

            migrationBuilder.DropTable(
                name: "session_statuses");

            migrationBuilder.DropTable(
                name: "odontograms");

            migrationBuilder.DropTable(
                name: "teeth");

            migrationBuilder.DropTable(
                name: "tooth_statuses");

            migrationBuilder.DropTable(
                name: "permission_actions");

            migrationBuilder.DropTable(
                name: "chat_channels");

            migrationBuilder.DropTable(
                name: "chatbot_conversation_statuses");

            migrationBuilder.DropTable(
                name: "clinical_attentions");

            migrationBuilder.DropTable(
                name: "clinical_histories");

            migrationBuilder.DropTable(
                name: "dentition_types");

            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "appointment_origins");

            migrationBuilder.DropTable(
                name: "appointment_statuses");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "professionals");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "job_positions");

            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "document_types");

            migrationBuilder.DropTable(
                name: "genders");
        }
    }
}
