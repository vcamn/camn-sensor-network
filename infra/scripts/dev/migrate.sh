#!/bin/bash

echo "Applying EF Core migrations..."

dotnet ef database update \
  --project services/cloud/api/fleet-api/Fleet.Infrastructure \
  --startup-project services/cloud/api/fleet-api/Fleet.Api

echo "Migrations applied."