-- Fix chat.id column to use auto-increment sequence
-- This script ensures the chat table's id column has a proper sequence for auto-incrementing

-- Check if sequence exists, if not create it
DO $$ 
BEGIN
    -- Check if the sequence exists
    IF NOT EXISTS (
        SELECT 1 FROM pg_sequences WHERE schemaname = 'public' AND sequencename = 'chat_id_seq'
    ) THEN
        -- Create sequence starting from the max id + 1, or 1 if table is empty
        CREATE SEQUENCE chat_id_seq;
        
        -- Set the sequence to start from the max id + 1 (or 1 if no rows)
        SELECT setval('chat_id_seq', COALESCE((SELECT MAX(id) FROM chat), 0) + 1, false);
        
        -- Set the column to use the sequence as default
        ALTER TABLE chat 
            ALTER COLUMN id 
            SET DEFAULT nextval('chat_id_seq');
        
        -- Make sure the sequence is owned by the column
        ALTER SEQUENCE chat_id_seq OWNED BY chat.id;
    ELSE
        -- Sequence exists, but make sure the column uses it
        ALTER TABLE chat 
            ALTER COLUMN id 
            SET DEFAULT nextval('chat_id_seq');
    END IF;
END $$;

-- Verify the sequence exists and is configured correctly
SELECT 
    schemaname,
    sequencename,
    last_value,
    is_called
FROM pg_sequences 
WHERE sequencename = 'chat_id_seq';

-- Verify the column default
SELECT 
    column_name,
    column_default,
    is_nullable,
    data_type
FROM information_schema.columns
WHERE table_name = 'chat' AND column_name = 'id';

