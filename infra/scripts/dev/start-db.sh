#!/bin/bash

echo "Starting PostgreSQL container..."
docker compose -f infra/docker/postgres/docker-compose.yml up -d

echo "Waiting for PostgreSQL to be ready..."
sleep 5

echo "PostgreSQL started."