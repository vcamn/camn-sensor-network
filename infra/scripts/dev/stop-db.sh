#!/bin/bash

echo "Stopping PostgreSQL container..."

docker compose -f infra/docker/postgres/init/docker-compose.yml down

echo "PostgreSQL stopped."