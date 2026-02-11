-- 1. Update the function to handle dependencies before deletion
CREATE OR REPLACE FUNCTION delete_empty_team_on_member_delete()
RETURNS TRIGGER AS $$
BEGIN
    -- Check if there are any members left in the team
    IF NOT EXISTS (
        SELECT 1 
        FROM team_member 
        WHERE team_id = OLD.team_id
    ) THEN
        -- 1. Set team_id to NULL in chat table to satisfy FK constraint
        UPDATE chat 
        SET team_id = NULL 
        WHERE team_id = OLD.team_id;

        -- 2. Clear other team-related relations (to avoid similar FK errors)
        DELETE FROM team_communication_service WHERE team_id = OLD.team_id;
        DELETE FROM team_preferencetag_relation WHERE team_id = OLD.team_id;

        -- 3. Finally, delete the team
        DELETE FROM team WHERE id = OLD.team_id;
    END IF;
    
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

-- 2. Create the trigger on the team_member table
CREATE OR REPLACE TRIGGER trigger_delete_empty_team
AFTER DELETE ON team_member
FOR EACH ROW
EXECUTE FUNCTION delete_empty_team_on_member_delete();