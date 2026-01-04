#!/bin/bash
# Script to fix chat.id sequence in PostgreSQL database

# Get connection details from appsettings.json or use defaults
DB_HOST="${DB_HOST:-localhost}"
DB_PORT="${DB_PORT:-5432}"
DB_NAME="${DB_NAME:-locked_in}"
DB_USER="${DB_USER:-postgres}"

echo "Fixing chat.id sequence in database: $DB_NAME"
echo "Host: $DB_HOST:$DB_PORT"
echo "User: $DB_USER"
echo ""

# Run the SQL script
psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -f "$(dirname "$0")/FixChatIdSequence.sql"

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Successfully fixed chat.id sequence!"
else
    echo ""
    echo "❌ Error fixing chat.id sequence. Please check your database connection."
    exit 1
fi

