-- ================================================
-- 
-- This procedure will add a member to an existing 
-- squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Add_Member_to_Squad
	@Id uniqueidentifier,		--SquadMemberId
	@SquadId uniqueidentifier,
	@UserId uniqueidentifier,
	@IsSquadMaster bit

AS
BEGIN

DECLARE @AlreadyExists bit = (SELECT COUNT(1)
							  FROM SquadMembers 
							  WHERE @UserId = UserFk AND @SquadId = SquadFK)


DECLARE @Today datetime2 = GETDATE()
-- If the member already exists in the squad, update their record.
IF @AlreadyExists = 1
	BEGIN
		UPDATE SquadMembers SET IsDeleted = 0, JoinDate = @Today WHERE @UserId = UserFk AND @SquadId = SquadFK 
	END
ELSE
	BEGIN
		

		INSERT INTO SquadMembers(SquadMemberPK, 
								 SquadFK, 
								 UserFK,
								 IsSquadMaster,
								 JoinDate)
		VALUES (
				@Id,
				@SquadId,
				@UserId,
				@IsSquadMaster,
				@Today
		)
	END
END
GO