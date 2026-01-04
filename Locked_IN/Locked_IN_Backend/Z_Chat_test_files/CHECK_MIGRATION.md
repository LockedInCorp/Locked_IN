# Database Migration Check

The error "Npgsql.Pos..." suggests that the database schema doesn't match your entities. 

## Quick Fix:

Run the database migration to add the new chat fields:

```bash
cd Locked_IN_Backend
dotnet ef database update
```

## Verify Migration:

After running the migration, verify the new columns exist:

```sql
-- Check if new columns exist in chat table
SELECT column_name, data_type 
FROM information_schema.columns 
WHERE table_name = 'chat' 
AND column_name IN ('last_message_at');

-- Check if new columns exist in message table
SELECT column_name, data_type 
FROM information_schema.columns 
WHERE table_name = 'message' 
AND column_name IN ('edited_at', 'is_deleted', 'deleted_at', 'attachment_url');

-- Check if new columns exist in chatparticipant table
SELECT column_name, data_type 
FROM information_schema.columns 
WHERE table_name = 'chatparticipant' 
AND column_name IN ('last_read_at', 'unread_count');
```

## Common Issues:

1. **Migration not run**: Run `dotnet ef database update`
2. **Connection string wrong**: Check `appsettings.json`
3. **Database doesn't exist**: Create it first
4. **User doesn't have permissions**: Check PostgreSQL user permissions

## After Migration:

1. Restart your backend server
2. Try sending a message again
3. Check the error message - it should now be JSON formatted

