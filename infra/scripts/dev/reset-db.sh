#!/bin/bash

set -a
source infra/docker/postgres/.env
set +a

echo "Resetting database..."

docker exec -i ${CONTAINER_NAME} psql -U ${POSTGRES_USER} <<EOF
DROP DATABASE IF EXISTS ${CAMN_DB_NAME} WITH (FORCE);
DROP ROLE IF EXISTS ${CAMN_APP_USER};
EOF

echo "Re-initializing database..."
./infra/scripts/dev/init-db.sh

echo "Applying migrations..."
./infra/scripts/dev/migrate.sh

echo "Database reset complete."