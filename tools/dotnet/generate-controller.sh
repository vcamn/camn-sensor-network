scripts/generate-controller.sh
#!/bin/bash

# Script: generate-controller.sh
# Purpose: Generate ASP.NET Core API controllers with parameterized inputs
# Usage: ./generate-controller.sh <controller-name> <model-fqn> <dbcontext-fqn>
# Example: ./generate-controller.sh SiteController MyWebApi.Models.Site MyWebApi.Data.AppDbContext

set -e

# Color output for better readability
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to display usage
usage() {
    echo -e "${YELLOW}Usage: $0 <controller-name> <model-fqn> <dbcontext-fqn>${NC}"
    echo ""
    echo "Parameters:"
    echo "  <controller-name>   Name of the controller (e.g., SiteController)"
    echo "  <model-fqn>         Fully qualified model name (e.g., Fleet.Models.Site)"
    echo "  <dbcontext-fqn>     Fully qualified DbContext name (e.g., Fleet.Data.AppDbContext)"
    echo ""
    echo "Example:"
    echo "  $0 SiteController Fleet.Models.Site Fleet.Data.AppDbContext"
    exit 1
}

# Validate parameters
if [ $# -ne 3 ]; then
    echo -e "${RED}Error: Exactly 3 parameters required.${NC}"
    usage
fi

CONTROLLER_NAME="$1"
MODEL_FQN="$2"
DBCONTEXT_FQN="$3"

# Validate that controller name ends with "Controller"
if [[ ! "$CONTROLLER_NAME" =~ Controller$ ]]; then
    echo -e "${RED}Warning: Controller name should end with 'Controller'${NC}"
fi

echo -e "${GREEN}Generating controller...${NC}"
echo "Controller: $CONTROLLER_NAME"
echo "Model: $MODEL_FQN"
echo "DbContext: $DBCONTEXT_FQN"
echo ""

# Run the code generator
dotnet aspnet-codegenerator controller \
    --name "$CONTROLLER_NAME" \
    --async \
    --api \
    --model "$MODEL_FQN" \
    --dataContext "$DBCONTEXT_FQN" \
    --dbProvider postgres \
    --outputDir Controllers \
    --force

echo -e "${GREEN}✓ Controller generated successfully!${NC}"