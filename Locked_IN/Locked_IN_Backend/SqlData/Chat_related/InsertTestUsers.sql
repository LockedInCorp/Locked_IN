-- Insert test users for chat testing
-- Run this script if users don't exist in your database

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

-- Delete existing test users (IDs 1-6) to avoid conflicts
DELETE FROM "User" WHERE id IN (1, 2, 3, 4, 5, 6);

-- Insert test users
INSERT INTO "User" (id, email, nickname, hashed_pass, availability, avatar_url)
VALUES
    (1, 'john.doe@example.com', 'JohnDoe', 'hashed_password_1', '{"monday": ["18:00", "22:00"], "friday": ["19:00", "23:00"]}', 'https://example.com/avatars/john.png'),
    (2, 'jane.smith@example.com', 'JaneSmith', 'hashed_password_2', '{"tuesday": ["17:00", "21:00"], "saturday": ["14:00", "18:00"]}', NULL),
    (3, 'mike.wilson@example.com', 'MikeWilson', 'hashed_password_3', '{"wednesday": ["20:00", "24:00"], "sunday": ["16:00", "20:00"]}', NULL),
    (4, 'sarah.johnson@example.com', 'SarahJ', 'hashed_password_4', '{"thursday": ["19:00", "23:00"], "saturday": ["15:00", "19:00"]}', NULL),
    (5, 'test.user5@example.com', 'TestUser5', 'hashed_password_5', '{}', NULL),
    (6, 'test.user6@example.com', 'TestUser6', 'hashed_password_6', '{}', NULL);

-- Insert roles if they don't exist
INSERT INTO role (id, rolename)
VALUES
    (1, 'Member'),
    (2, 'Admin'),
    (3, 'Moderator')
ON CONFLICT (id) DO UPDATE SET
    rolename = EXCLUDED.rolename;

-- Verify users were inserted
SELECT id, email, nickname FROM "User" WHERE id IN (1, 2, 3, 4, 5, 6) ORDER BY id;

-- Verify roles were inserted
SELECT id, rolename FROM role ORDER BY id;

