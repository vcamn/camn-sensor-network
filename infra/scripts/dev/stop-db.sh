#!/bin/bash

echo "Stopping PostgreSQL container..."

docker compose -f infra/docker/postgres/docker-compose.yml down

echo "PostgreSQL stopped."