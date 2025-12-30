using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Locked_IN_Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixChatIdSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create sequence for chat.id if it doesn't exist
            migrationBuilder.Sql(@"
                DO $$ 
                DECLARE
                    max_id INTEGER;
                BEGIN
                    -- Check if the sequence exists
                    IF NOT EXISTS (
                        SELECT 1 FROM pg_sequences WHERE schemaname = 'public' AND sequencename = 'chat_id_seq'
                    ) THEN
                        -- Get max id from table (or 0 if empty)
                        SELECT COALESCE(MAX(id), 0) INTO max_id FROM chat;
                        
                        -- Create sequence starting from the max id + 1, or 1 if table is empty
                        CREATE SEQUENCE chat_id_seq START WITH 1;
                        
                        -- Set the sequence to start from the max id + 1 (or 1 if no rows)
                        PERFORM setval('chat_id_seq', max_id + 1, false);
                        
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
            ");

            // Create sequence for chatparticipant.chatparticipant_id if it doesn't exist
            migrationBuilder.Sql(@"
                DO $$ 
                DECLARE
                    max_id INTEGER;
                BEGIN
                    -- Check if the sequence exists
                    IF NOT EXISTS (
                        SELECT 1 FROM pg_sequences WHERE schemaname = 'public' AND sequencename = 'chatparticipant_chatparticipant_id_seq'
                    ) THEN
                        -- Get max id from table (or 0 if empty)
                        SELECT COALESCE(MAX(chatparticipant_id), 0) INTO max_id FROM chatparticipant;
                        
                        -- Create sequence starting from the max id + 1, or 1 if table is empty
                        CREATE SEQUENCE chatparticipant_chatparticipant_id_seq START WITH 1;
                        
                        -- Set the sequence to start from the max id + 1 (or 1 if no rows)
                        PERFORM setval('chatparticipant_chatparticipant_id_seq', max_id + 1, false);
                        
                        -- Set the column to use the sequence as default
                        ALTER TABLE chatparticipant 
                            ALTER COLUMN chatparticipant_id 
                            SET DEFAULT nextval('chatparticipant_chatparticipant_id_seq');
                        
                        -- Make sure the sequence is owned by the column
                        ALTER SEQUENCE chatparticipant_chatparticipant_id_seq OWNED BY chatparticipant.chatparticipant_id;
                    ELSE
                        -- Sequence exists, but make sure the column uses it
                        ALTER TABLE chatparticipant 
                            ALTER COLUMN chatparticipant_id 
                            SET DEFAULT nextval('chatparticipant_chatparticipant_id_seq');
                    END IF;
                END $$;
            ");

            // Create sequence for message.id if it doesn't exist
            migrationBuilder.Sql(@"
                DO $$ 
                DECLARE
                    max_id INTEGER;
                BEGIN
                    -- Check if the sequence exists
                    IF NOT EXISTS (
                        SELECT 1 FROM pg_sequences WHERE schemaname = 'public' AND sequencename = 'message_id_seq'
                    ) THEN
                        -- Get max id from table (or 0 if empty)
                        SELECT COALESCE(MAX(id), 0) INTO max_id FROM message;
                        
                        -- Create sequence starting from the max id + 1, or 1 if table is empty
                        CREATE SEQUENCE message_id_seq START WITH 1;
                        
                        -- Set the sequence to start from the max id + 1 (or 1 if no rows)
                        PERFORM setval('message_id_seq', max_id + 1, false);
                        
                        -- Set the column to use the sequence as default
                        ALTER TABLE message 
                            ALTER COLUMN id 
                            SET DEFAULT nextval('message_id_seq');
                        
                        -- Make sure the sequence is owned by the column
                        ALTER SEQUENCE message_id_seq OWNED BY message.id;
                    ELSE
                        -- Sequence exists, but make sure the column uses it
                        ALTER TABLE message 
                            ALTER COLUMN id 
                            SET DEFAULT nextval('message_id_seq');
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the sequence (optional - you may want to keep it)
            // migrationBuilder.Sql("DROP SEQUENCE IF EXISTS chat_id_seq CASCADE;");
        }
    }
}
