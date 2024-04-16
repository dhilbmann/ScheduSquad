-- ================================================
-- 
-- This procedure will delete a Squad from 
-- the Squad table
-- 
-- ================================================


CREATE PROCEDURE Delete_Squad
	@Id uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Squads WHERE SquadPK = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
	UPDATE Squads SET IsDeleted = 1 WHERE SquadPK = @Id
	RETURN 1
	END
END
GO