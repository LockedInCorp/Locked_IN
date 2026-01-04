#!/bin/bash
# Script to insert test users into the database

DB_NAME="locked_in"
DB_USER="${PGUSER:-postgres}"
DB_HOST="${PGHOST:-localhost}"
DB_PORT="${PGPORT:-5432}"

echo "Inserting test users into database: $DB_NAME"
echo "Using connection: $DB_USER@$DB_HOST:$DB_PORT"
echo ""

# Get the directory where this script is located
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
SQL_FILE="$SCRIPT_DIR/InsertTestUsers.sql"

if [ ! -f "$SQL_FILE" ]; then
    echo "Error: SQL file not found at $SQL_FILE"
    exit 1
fi

# Run the SQL script
psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -f "$SQL_FILE"

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Test users inserted successfully!"
    echo ""
    echo "Available test users:"
    echo "  User ID 1: JohnDoe (john.doe@example.com)"
    echo "  User ID 2: JaneSmith (jane.smith@example.com)"
    echo "  User ID 3: MikeWilson (mike.wilson@example.com)"
    echo "  User ID 4: SarahJ (sarah.johnson@example.com)"
    echo "  User ID 5: TestUser5"
    echo "  User ID 6: TestUser6"
else
    echo ""
    echo "❌ Error inserting test users. Check your database connection."
    echo ""
    echo "Make sure:"
    echo "  1. PostgreSQL is running"
    echo "  2. Database 'locked_in' exists"
    echo "  3. User has proper permissions"
    echo ""
    echo "You can also run manually:"
    echo "  psql -d locked_in -f $SQL_FILE"
fi

