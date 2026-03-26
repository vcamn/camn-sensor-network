#!/bin/bash

set -a
source infra/docker/postgres/init/.env
set +a

echo "Initializing database..."

# 1. Create the database ONLY if it doesn't exist (This runs outside a DO block)
docker exec -i ${CONTAINER_NAME} psql -U ${POSTGRES_USER} -tc \
"SELECT 1 FROM pg_database WHERE datname = '${CAMN_DB_NAME}'" | grep -q 1 || \
docker exec -i ${CONTAINER_NAME} psql -U ${POSTGRES_USER} -c \
"CREATE DATABASE ${CAMN_DB_NAME} OWNER ${POSTGRES_USER}"


# 2. Inject env vars into SQL template and run it
envsubst < infra/database/postgres/init/001_init.sql.template | \
docker exec -i ${CONTAINER_NAME} psql -U ${POSTGRES_USER} -d ${CAMN_DB_NAME}

echo "Database initialization complete."