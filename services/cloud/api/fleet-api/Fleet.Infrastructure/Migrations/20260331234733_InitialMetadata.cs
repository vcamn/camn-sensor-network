using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fleet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "metadata");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "device_statuses",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    status_name = table.Column<string>(type: "citext", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_device_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "integration_types",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_integration_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sensor_statuses",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    status_name = table.Column<string>(type: "citext", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sensor_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sensor_types",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sensor_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_statuses",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    status_name = table.Column<string>(type: "citext", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_site_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "station_statuses",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "citext", maxLength: 16, nullable: false),
                    status_name = table.Column<string>(type: "citext", maxLength: 32, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    display_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_station_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "integration_templates",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    integration_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    default_config_json = table.Column<string>(type: "jsonb", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_integration_templates", x => x.id);
                    table.ForeignKey(
                        name: "fk_integration_templates_integration_types_integration_type_id",
                        column: x => x.integration_type_id,
                        principalSchema: "metadata",
                        principalTable: "integration_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_integration_templates_sensor_types_sensor_type_id",
                        column: x => x.sensor_type_id,
                        principalSchema: "metadata",
                        principalTable: "sensor_types",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sites",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    address_line1 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    address_line2 = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    city = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    state = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false, defaultValue: "CA"),
                    postal_code = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    country = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false, defaultValue: "USA"),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    installed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_maintenance_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    contact_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    contact_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sites", x => x.id);
                    table.ForeignKey(
                        name: "fk_sites_site_statuses_site_status_id",
                        column: x => x.site_status_id,
                        principalSchema: "metadata",
                        principalTable: "site_statuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "stations",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    site_id = table.Column<Guid>(type: "uuid", nullable: false),
                    station_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    station_code = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stations", x => x.id);
                    table.ForeignKey(
                        name: "fk_stations_sites_site_id",
                        column: x => x.site_id,
                        principalSchema: "metadata",
                        principalTable: "sites",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_stations_station_statuses_station_status_id",
                        column: x => x.station_status_id,
                        principalSchema: "metadata",
                        principalTable: "station_statuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "devices",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    station_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_identifier = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    device_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    serial_number = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    mac_address = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    iot_hub_device_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    firmware_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    last_active_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_devices", x => x.id);
                    table.ForeignKey(
                        name: "fk_devices_device_statuses_device_status_id",
                        column: x => x.device_status_id,
                        principalSchema: "metadata",
                        principalTable: "device_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_devices_stations_station_id",
                        column: x => x.station_id,
                        principalSchema: "metadata",
                        principalTable: "stations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sensors",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    station_id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: true),
                    sensor_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_identifier = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    unit_of_measure = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    calibration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sensors", x => x.id);
                    table.ForeignKey(
                        name: "fk_sensors_devices_device_id",
                        column: x => x.device_id,
                        principalSchema: "metadata",
                        principalTable: "devices",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sensors_sensor_statuses_sensor_status_id",
                        column: x => x.sensor_status_id,
                        principalSchema: "metadata",
                        principalTable: "sensor_statuses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sensors_sensor_types_sensor_type_id",
                        column: x => x.sensor_type_id,
                        principalSchema: "metadata",
                        principalTable: "sensor_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sensors_stations_station_id",
                        column: x => x.station_id,
                        principalSchema: "metadata",
                        principalTable: "stations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sensor_calibrations",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    calibration_version = table.Column<int>(type: "integer", nullable: false),
                    calibration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    calibration_method = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    performed_by = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sensor_calibrations", x => x.id);
                    table.ForeignKey(
                        name: "fk_sensor_calibrations_sensors_sensor_id",
                        column: x => x.sensor_id,
                        principalSchema: "metadata",
                        principalTable: "sensors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sensor_integrations",
                schema: "metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sensor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    integration_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    integration_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    config_json = table.Column<string>(type: "jsonb", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sensor_integrations", x => x.id);
                    table.ForeignKey(
                        name: "fk_sensor_integrations_integration_templates_integration_templ",
                        column: x => x.integration_template_id,
                        principalSchema: "metadata",
                        principalTable: "integration_templates",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sensor_integrations_integration_types_integration_type_id",
                        column: x => x.integration_type_id,
                        principalSchema: "metadata",
                        principalTable: "integration_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sensor_integrations_sensors_sensor_id",
                        column: x => x.sensor_id,
                        principalSchema: "metadata",
                        principalTable: "sensors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_device_statuses_code",
                schema: "metadata",
                table: "device_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_device_statuses_status_name",
                schema: "metadata",
                table: "device_statuses",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_devices_device_status_id",
                schema: "metadata",
                table: "devices",
                column: "device_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_devices_station_id",
                schema: "metadata",
                table: "devices",
                column: "station_id");

            migrationBuilder.CreateIndex(
                name: "ix_integration_templates_integration_type_id",
                schema: "metadata",
                table: "integration_templates",
                column: "integration_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_integration_templates_sensor_type_id",
                schema: "metadata",
                table: "integration_templates",
                column: "sensor_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensor_calibrations_sensor_id",
                schema: "metadata",
                table: "sensor_calibrations",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensor_integrations_integration_template_id",
                schema: "metadata",
                table: "sensor_integrations",
                column: "integration_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensor_integrations_integration_type_id",
                schema: "metadata",
                table: "sensor_integrations",
                column: "integration_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensor_integrations_sensor_id",
                schema: "metadata",
                table: "sensor_integrations",
                column: "sensor_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensor_statuses_code",
                schema: "metadata",
                table: "sensor_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sensor_statuses_status_name",
                schema: "metadata",
                table: "sensor_statuses",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sensors_device_id",
                schema: "metadata",
                table: "sensors",
                column: "device_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensors_sensor_status_id",
                schema: "metadata",
                table: "sensors",
                column: "sensor_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensors_sensor_type_id",
                schema: "metadata",
                table: "sensors",
                column: "sensor_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_sensors_station_id",
                schema: "metadata",
                table: "sensors",
                column: "station_id");

            migrationBuilder.CreateIndex(
                name: "ix_site_statuses_code",
                schema: "metadata",
                table: "site_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_site_statuses_status_name",
                schema: "metadata",
                table: "site_statuses",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sites_site_status_id",
                schema: "metadata",
                table: "sites",
                column: "site_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_station_statuses_code",
                schema: "metadata",
                table: "station_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_station_statuses_status_name",
                schema: "metadata",
                table: "station_statuses",
                column: "status_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_stations_site_id",
                schema: "metadata",
                table: "stations",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_stations_station_status_id",
                schema: "metadata",
                table: "stations",
                column: "station_status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sensor_calibrations",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "sensor_integrations",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "integration_templates",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "sensors",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "integration_types",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "devices",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "sensor_statuses",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "sensor_types",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "device_statuses",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "stations",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "sites",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "station_statuses",
                schema: "metadata");

            migrationBuilder.DropTable(
                name: "site_statuses",
                schema: "metadata");
        }
    }
}
