-- ================================================
-- 
-- This procedure will add a member to an existing 
-- squad
-- 
-- ================================================


CREATE PROCEDURE Add_Member_to_Squad
	@Id uniqueidentifier,		--SquadMemberId
	@SquadId uniqueidentifier,
	@UserId uniqueidentifier,
	@IsSquadMaster bit

AS
BEGIN

DECLARE @AlreadyExists bit = (SELECT COUNT(1)
							  FROM SquadMembers 
							  WHERE @UserId = UserFk AND @SquadId = SquadFK)

-- If the member already exists in the squad, do not add them again
IF @AlreadyExists = 1
	BEGIN
		RETURN 0;
	END
ELSE
	BEGIN
		DECLARE @Today datetime2 = GETDATE()

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