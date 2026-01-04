-- Insert test users for chat testing (Safe version - only inserts if users don't exist)
-- This version won't delete existing users, just inserts missing ones

-- First, check if avatar_url column exists, if not add it
DO $$ 
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'User' AND column_name = 'avatar_url'
    ) THEN
        ALTER TABLE "User" ADD COLUMN avatar_url VARCHAR(255);
    END IF;
END $$;

-- Fix column sizes to match entity definitions
ALTER TABLE "User" ALTER COLUMN email TYPE VARCHAR(50);
ALTER TABLE "User" ALTER COLUMN nickname TYPE VARCHAR(50);
ALTER TABLE "User" ALTER COLUMN hashed_pass TYPE VARCHAR(100);

-- Insert users one by one, skipping if they already exist
DO $$
BEGIN
    -- User 1
    IF NOT EXISTS (SELECT 1 FROM "User" WHERE id = 1) THEN
        INSERT INTO "User" (id, email, nickname, hashed_pass, availability, avatar_url)
        VALUES (1, 'john.doe@example.com', 'JohnDoe', 'hashed_password_1', '{"monday": ["18:00", "22:00"], "friday": ["19:00", "23:00"]}', 'https://example.com/avatars/john.png');
    END IF;
    
    -- User 2
    IF NOT EXISTS (SELECT 1 FROM "User" WHERE id = 2) THEN
        INSERT INTO "User" (id, email, nickname, hashed_pass, availability, avatar_url)
        VALUES (2, 'jane.smith@example.com', 'JaneSmith', 'hashed_password_2', '{"tuesday": ["17:00", "21:00"], "saturday": ["14:00", "18:00"]}', NULL);
    END IF;
    
    -- User 3
    IF NOT EXISTS (SELECT 1 FROM "User" WHERE id = 3) THEN
        INSERT INTO "User" (id, email, nickname, hashed_pass, availability, avatar_url)
        VALUES (3, 'mike.wilson@example.com', 'MikeWilson', 'hashed_password_3', '{"wednesday": ["20:00", "24:00"], "sunday": ["16:00", "20:00"]}', NULL);
    END IF;
    
    -- User 4
    IF NOT EXISTS (SELECT 1 FROM "User" WHERE id = 4) THEN
        INSERT INTO "User" (id, email, nickname, hashed_pass, availability, avatar_url)
        VALUES (4, 'sarah.johnson@example.com', 'SarahJ', 'hashed_password_4', '{"thursday": ["19:00", "23:00"], "saturday": ["15:00", "19:00"]}', NULL);
    END IF;
    
    -- User 5
    IF NOT EXISTS (SELECT 1 FROM "User" WHERE id = 5) THEN
        INSERT INTO "User" (id, email, nickname, hashed_pass, availability, avatar_url)
        VALUES (5, 'test.user5@example.com', 'TestUser5', 'hashed_password_5', '{}', NULL);
    END IF;
    
    -- User 6
    IF NOT EXISTS (SELECT 1 FROM "User" WHERE id = 6) THEN
        INSERT INTO "User" (id, email, nickname, hashed_pass, availability, avatar_url)
        VALUES (6, 'test.user6@example.com', 'TestUser6', 'hashed_password_6', '{}', NULL);
    END IF;
END $$;

-- Verify users exist
SELECT id, email, nickname FROM "User" WHERE id IN (1, 2, 3, 4, 5, 6) ORDER BY id;

