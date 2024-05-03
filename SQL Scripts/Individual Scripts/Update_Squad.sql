-- ================================================
-- 
-- This procedure will update details on the
-- specified squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Update_Squad
	@Id uniqueidentifier,
	@SquadName nvarchar(50),
	@SquadDesc nvarchar(500),
	@SquadLocation nvarchar(25)

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Squads WHERE SquadPK = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
		UPDATE Squads 
		SET 
			SquadName = @SquadName, 
			SquadDesc = @SquadDesc,
			SquadLocation = @SquadLocation
		WHERE SquadPK = @Id
	RETURN 1
	END
END
GO