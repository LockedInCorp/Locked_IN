-- Insert roles for chat participants
-- Run this script to create roles if they don't exist

-- Insert roles (using ON CONFLICT to avoid duplicates if running multiple times)
INSERT INTO role (id, rolename)
VALUES
    (1, 'Member'),
    (2, 'Admin'),
    (3, 'Moderator')
ON CONFLICT (id) DO UPDATE SET
    rolename = EXCLUDED.rolename;

-- Verify roles were inserted
SELECT id, rolename FROM role ORDER BY id;

