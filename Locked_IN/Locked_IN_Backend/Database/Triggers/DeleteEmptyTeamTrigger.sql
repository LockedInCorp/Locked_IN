CREATE OR REPLACE FUNCTION delete_empty_team_on_member_delete()
RETURNS TRIGGER AS $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM team_member 
        WHERE team_id = OLD.team_id
    ) THEN
        UPDATE chat 
        SET team_id = NULL 
        WHERE team_id = OLD.team_id;

        DELETE FROM team_communication_service WHERE team_id = OLD.team_id;
        DELETE FROM team_preferencetag_relation WHERE team_id = OLD.team_id;

        DELETE FROM team WHERE id = OLD.team_id;
    END IF;
    
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_delete_empty_team
AFTER DELETE ON team_member
FOR EACH ROW
EXECUTE FUNCTION delete_empty_team_on_member_delete();