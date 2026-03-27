#!/bin/bash

# Orchestrator: A structured, maintainable dev platform script for managing 
# PostgreSQL and EF Core migrations in a Dockerized environment.

set -e # exit on error

ENV_FILE="infra/docker/postgres/.env"

# Load env
if [ -f "$ENV_FILE" ]; then
  set -a
  source "$ENV_FILE"
  set +a
else
  echo "❌ .env file not found at $ENV_FILE"
  exit 1
fi

# --- Functions ---

start_db() {
  echo "🚀 Starting PostgreSQL..."
  ./infra/scripts/dev/start-db.sh
}

init_db() {
  echo "🧱 Initializing database..."
  ./infra/scripts/dev/init-db.sh
}

migrate_db() {
  echo "📦 Applying EF Core migrations..."
  ./infra/scripts/dev/migrate.sh
}

reset_db() {
  echo "⚠️ Resetting database..."
  ./infra/scripts/dev/reset-db.sh
}

stop_db() {
  echo "🛑 Stopping PostgreSQL..."
  ./infra/scripts/dev/stop-db.sh
}

logs_db() {
  docker compose -f infra/docker/postgres/docker-compose.yml logs postgres
}

# --- Commands ---

case "$1" in
  up)
    start_db
    init_db
    migrate_db
    echo "✅ Dev environment is ready."
    ;;

  reset)
    reset_db
    ;;

  down)
    stop_db
    ;;

  restart)
    stop_db
    start_db
    init_db
    migrate_db
    ;;

  migrate)
    migrate_db
    ;;

  logs)
    logs_db
    ;;

  *)
    echo "Usage: $0 {up|reset|down|restart|migrate|logs}"
    exit 1
    ;;
esac