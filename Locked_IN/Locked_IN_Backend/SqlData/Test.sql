
DELETE FROM team_member;
DELETE FROM team;
DELETE FROM experience_tag;
DELETE FROM game;
DELETE FROM "User";
DELETE FROM member_status;

INSERT INTO member_status (id, statusname)
VALUES
(1, 'Leader'),
(2, 'Member'),
(3, 'Pending');

INSERT INTO "User" (id, email, nickname, hashed_pass, availability)
DELETE FROM team_member;
DELETE FROM team;
DELETE FROM experience_tag;
DELETE FROM game;
DELETE FROM "User";
DELETE FROM member_status;

INSERT INTO member_status (id, statusname)
VALUES (1, 'Leader'), (2, 'Member'), (3, 'Pending');

INSERT INTO "User" (email, nickname, hashed_pass, availability)
VALUES
    ('admin@test.com', 'AdminUser', 'hash123', '{}'),
    ('user@test.com', 'NormalUser', 'hash456', '{}'),
    ('sdf@test.com', 'fjfj', '111', '{}');

INSERT INTO game (name) VALUES ('Test Game');

INSERT INTO experience_tag (experiencelevel) VALUES ('Casual');

INSERT INTO team (name, max_player_count, description, game_id, isprivate, isblitz, experience_tag_id)
VALUES
    ('Public Team', 5, 123, (SELECT id from game WHERE name='Test Game'), false, false, (SELECT id from experience_tag WHERE experiencelevel='Casual'));
INSERT INTO team (name, max_player_count, description, game_id, isprivate, isblitz, experience_tag_id)
VALUES
    ('Private Team', 5, 456, (SELECT id from game WHERE name='Test Game'), true, false, (SELECT id from experience_tag WHERE experiencelevel='Casual'));

INSERT INTO team_member (isleader, jointimestamp, team_id, user_id, member_status_id)
VALUES
    (true, NOW(), (SELECT id from team WHERE name='Private Team'), (SELECT id from "User" WHERE email='admin@test.com'), 1);

