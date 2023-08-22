CREATE OR REPLACE FUNCTION public."NotifyCommentChange"()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$ 
DECLARE 
  data JSON; notification JSON;
BEGIN	

  IF (TG_OP = 'INSERT')     THEN
	 data = row_to_json(NEW);
  ELSIF (TG_OP = 'UPDATE')  THEN
	 data = row_to_json(NEW);
  ELSIF (TG_OP = 'DELETE')  THEN
	 data = row_to_json(OLD);
  END IF;
  
  notification = json_build_object(
            'table',TG_TABLE_NAME,
            'action', TG_OP,
            'data', data);  
			
   PERFORM pg_notify('commentchange', notification::TEXT);
   
  RETURN NEW;
END
$function$
;


ALTER FUNCTION public."NotifyCommentChange"()
    OWNER TO admin;
    
CREATE TRIGGER "OnCommentChange"
    AFTER INSERT OR DELETE OR UPDATE 
    ON public."comment" 
    FOR EACH ROW
    EXECUTE PROCEDURE public."NotifyCommentChange"();