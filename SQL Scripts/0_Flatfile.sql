
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ScheduSquad')
	BEGIN
		CREATE DATABASE ScheduSquad;		
	END
GO

USE ScheduSquad;
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
	BEGIN
		CREATE TABLE Users (
			UserPk uniqueidentifier NOT NULL PRIMARY KEY, 
			FirstName nvarchar(35) NOT NULL,
			LastName nvarchar(35)  NOT NULL,
			Email nvarchar(75)  NOT NULL,
			PwHash nvarchar(128),
			PwSalt nvarchar(128),
			IsDeleted bit NOT NULL DEFAULT 0
		);
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserAvailability')
	BEGIN
		CREATE TABLE UserAvailability (
			AvailabilityPK uniqueidentifier NOT NULL PRIMARY KEY,
			UserFK uniqueidentifier NOT NULL,
			DayEnum int NOT NULL,
			StartTime time NOT NULL,
			EndTime time NOT NULL,
			FOREIGN KEY (UserFK) REFERENCES Users(UserPK),
			IsDeleted bit NOT NULL DEFAULT 0
		);
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Squads')
	BEGIN
		CREATE TABLE Squads (
			SquadPK uniqueidentifier NOT NULL PRIMARY KEY,
			SquadName nvarchar(50) NOT  NULL,
			SquadDesc nvarchar(500) NOT NULL,
			SquadLocation nvarchar(25),
			IsDeleted bit NOT NULL DEFAULT 0
		);
	END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SquadMembers')
	BEGIN
		CREATE TABLE SquadMembers (
			SquadMemberPK uniqueidentifier NOT NULL PRIMARY KEY,
			SquadFK uniqueidentifier NOT NULL,
			UserFK uniqueidentifier NOT NULL,
			IsSquadMaster bit NOT NULL,
			JoinDate datetime2 NOT NULL,
			IsDeleted bit NOT NULL DEFAULT 0,
			FOREIGN KEY (SquadFK) REFERENCES Squads(SquadPK),
			FOREIGN KEY (UserFK) REFERENCES Users(UserPK)
		);
	END
GO

	-- ================================================
-- 
-- This procedure will add a row of availability 
-- for an individual user
-- 
-- ================================================


CREATE OR ALTER PROCEDURE [dbo].[Add_Availability]
	@Id uniqueidentifier,
	@UserId uniqueidentifier,
	@DayEnum int,
	@StartTime time,
	@EndTime time

AS
BEGIN

DECLARE @IdAlreadyExists bit = (SELECT COUNT(1)
							  FROM UserAvailability u
							  WHERE AvailabilityPK = @Id)
DECLARE @UserExists bit = (SELECT COUNT(1) 
							FROM Users 
							WHERE UserPk = @UserId)

-- Checks if the Availability ID already exists or if the user does not exist
IF @IdAlreadyExists = 1 OR @UserExists = 0
	BEGIN
		RETURN 0;
	END
ELSE
	BEGIN
		INSERT INTO UserAvailability (AvailabilityPk, 
									  UserFK, 
									  DayEnum, 
									  StartTime,
									  EndTime)
		VALUES (
				@Id,
				@UserId,
				@DayEnum,
				@StartTime,
				@EndTime
		)
	END
END
GO

-- ================================================
-- 
-- This procedure will create a new member and adds 
-- them to the User table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Add_Member
	@Id uniqueidentifier,
	@FirstName nvarchar(35),
	@LastName nvarchar(35),
	@Email nvarchar(75)

AS
BEGIN

DECLARE @AlreadyExists bit = (SELECT COUNT(1) from Users WHERE UserPk = @Id)

IF @AlreadyExists = 1
	BEGIN
	RETURN 0;
	END
ELSE
	BEGIN
	INSERT INTO Users ( UserPk, 
						FirstName, 
						LastName, 
						Email)
	VALUES (
			@Id,
			@FirstName,
			@LastName,
			@Email
	)
	END
END
GO

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

-- ================================================
-- 
-- This procedure will create a new Squad and adds 
-- it to the Squad table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Add_Squad
	@Id uniqueidentifier,
	@SquadName nvarchar(50),
	@SquadDesc nvarchar(500),
	@SquadLocation nvarchar(25)

AS
BEGIN

DECLARE @AlreadyExists bit = (SELECT COUNT(1) from Squads WHERE SquadPK = @Id)

IF @AlreadyExists = 1
	BEGIN
	RETURN 0;
	END
ELSE
	BEGIN
	INSERT INTO Squads (SquadPK, 
						SquadName, 
						SquadDesc, 
						SquadLocation)
	VALUES (
			@Id,
			@SquadName,
			@SquadDesc,
			@SquadLocation
	)
	END
END
GO

-- ================================================
-- 
-- This procedure will delete a Squad from 
-- the Squad table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Delete_Availability
	@Id uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from UserAvailability WHERE AvailabilityPK = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
	UPDATE UserAvailability SET IsDeleted = 1 WHERE AvailabilityPK = @Id
	RETURN 1
	END
END
GO

-- ================================================
-- 
-- This procedure will delete a member from 
-- the Member table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Delete_Member
	@Id uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
	UPDATE Users SET IsDeleted = 1 WHERE UserPk = @Id
	RETURN 1
	END
END
GO

-- ================================================
-- 
-- This procedure will delete a member from 
-- the SquadMember table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Delete_Member_from_Squad
	@userId uniqueidentifier,
	@squadId uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @userId)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
	UPDATE SquadMembers 
	SET IsDeleted = 1 
	WHERE SquadFK = @squadId 
		AND UserFK = @userId
	RETURN 1
	END
END
GO

-- ================================================
-- 
-- This procedure will delete a Squad from 
-- the Squad table
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Delete_Squad
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

-- ================================================
-- 
-- This procedure will return a list of all 
-- availability for all members in a squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE [dbo].[Get_All_Availability]

AS
BEGIN


SELECT	AvailabilityPK as 'Id',
		DayEnum,
		StartTime,
		EndTime
FROM UserAvailability
WHERE IsDeleted = 0

END
GO

-- ================================================
-- 
-- This procedure will return a list of all 
-- availability for all members in a squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_All_Availability_By_Id
	@Id uniqueidentifier --UserId if a certain user's availability is requested

AS
BEGIN

IF @Id IS NOT NULL
	BEGIN
		SELECT ua.AvailabilityPK as 'Id',
				ua.DayEnum,
				ua.StartTime,
				ua.EndTime
		FROM UserAvailability ua
		INNER JOIN Users u ON u.UserPk = ua.UserFK
		WHERE @Id = u.UserPk 
			AND u.IsDeleted = 0
			AND ua.IsDeleted = 0
		ORDER BY ua.DayEnum
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will return details for an 
-- all members 
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_AllMembers

AS

SELECT UserPk AS 'Id',
		FirstName,
		LastName,
		Email
FROM Users
WHERE IsDeleted = 0

GO

-- ================================================
-- 
-- This procedure will return details for a 
-- singularly requested availability
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Availability
	@Id uniqueidentifier

AS
BEGIN

IF @Id IS NOT NULL
	BEGIN

		SELECT AvailabilityPK AS 'Id',
			   DayEnum,
			   StartTime,
			   EndTime
		FROM UserAvailability 
		WHERE @Id = AvailabilityPK AND IsDeleted = 0

	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will return a list of all 
-- availability for all members in a squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Availability_for_SquadMembers
	@Id uniqueidentifier --Squad Id

AS
BEGIN

IF @Id IS NOT NULL
	BEGIN
		SELECT u.UserPk AS 'Id',
			   u.UserPk,
			   ua.DayEnum,
			   ua.StartTime,
			   ua.EndTime
		FROM SquadMembers sm
		INNER JOIN Users u ON u.UserPk = sm.UserFK
		INNER JOIN UserAvailability ua ON ua.UserFK = u.UserPk
		WHERE @Id = sm.SquadFK 
			AND sm.IsDeleted = 0
			AND u.IsDeleted = 0
			AND ua.IsDeleted = 0
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will return the joined date 
-- for a member in a squad.
-- 
-- ================================================

CREATE OR ALTER PROCEDURE Get_JoinedDateForSquadMember
	@MemberId uniqueidentifier,
	@SquadId uniqueidentifier

AS
BEGIN

IF @MemberId IS NOT NULL AND @SquadId IS NOT NULL
	
	BEGIN
		Select JoinDate 
		From SquadMembers 
		Where SquadFK = @SquadId 
			AND UserFK = @MemberId 
			AND IsDeleted = 0
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will return details for an 
-- individual member or all members if no Id is given
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Member
	@Id uniqueidentifier

AS
BEGIN

DECLARE @EXISTS bit = (SELECT COUNT(1) FROM Users WHERE UserPk = @Id)

IF @EXISTS = 1
	BEGIN
		SELECT UserPk AS 'Id',
			   FirstName,
			   LastName,
			   Email
		FROM Users 
		WHERE @Id = UserPk
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will return a list of all 
-- members not in a specific squad
-- 
-- ================================================

CREATE OR ALTER PROCEDURE Get_MembersNotInSquad
	@SquadId uniqueidentifier

AS
BEGIN

IF @SquadId IS NOT NULL
	
	BEGIN
	Select DISTINCT * from (
		SELECT u.UserPk AS 'Id',
			   u.FirstName,
			   u.LastName,
			   u.Email
		FROM Users u
		LEFT JOIN SquadMembers sm ON u.UserPK = sm.UserFK 
		AND sm.SquadFK = @SquadID
		WHERE sm.SquadMemberPK IS NULL
		UNION
		SELECT u.UserPk AS 'Id',
			   u.FirstName,
			   u.LastName,
			   u.Email
		FROM Users u
		JOIN SquadMembers sm ON u.UserPK = sm.UserFK 
		AND sm.SquadFK = @SquadID 
		WHERE sm.IsDeleted = 1
		) a
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will get the password hash based
-- on the user id
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Password
	@memberId uniqueidentifier

AS
BEGIN


DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @memberId)

IF @Exists = 0
	BEGIN
		RETURN 0
	END
ELSE
	BEGIN
		SELECT TOP 1 u.PwHash as item
		FROM USERS u
		WHERE u.UserPk = @memberId
	END
END
GO

-- ================================================
-- 
-- This procedure will get the password salt based
-- on the user id
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Salt
	@memberId uniqueidentifier

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @memberId)

IF @Exists = 0
	BEGIN
		RETURN 0
	END
ELSE
	BEGIN
		SELECT TOP 1 u.PwSalt as item
		FROM USERS u
		WHERE u.UserPk = @memberId
	END
END
GO

-- ================================================
-- 
-- This procedure will return details for an 
-- individual squad or all squads if no Id is given
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Squad
	@Id uniqueidentifier

AS
BEGIN

DECLARE @EXISTS bit = (SELECT COUNT(1) FROM Squads WHERE SquadPk = @Id)

IF @EXISTS = 1
	BEGIN
	SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation,
			   sm.UserFK AS 'SquadMasterId'
		FROM Squads s
		INNER JOIN SquadMembers sm ON sm.SquadFK = s.SquadPK
		WHERE sm.IsDeleted = 0
			AND s.IsDeleted = 0
			AND sm.IsSquadMaster = 1
			AND s.SquadPK = @Id
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will return a list of all 
-- members in a squad
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_SquadMembers
	@Id uniqueidentifier

AS
BEGIN

IF @Id IS NOT NULL
	BEGIN
		SELECT u.UserPk AS 'Id',
			   u.FirstName,
			   u.LastName,
			   u.Email
		FROM SquadMembers sm
		INNER JOIN Users u ON u.UserPk = sm.UserFK
		WHERE @Id = sm.SquadFK 
			AND sm.IsDeleted = 0
			AND u.IsDeleted = 0
	END
ELSE
	BEGIN
		RETURN 0
	END
END
GO

-- ================================================
-- 
-- This procedure will return details for  
-- all squads 
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_Squads

AS
BEGIN

SELECT SquadPK AS 'Id',
		SquadName,
		SquadDesc,
		SquadLocation,
		sm.UserFK AS 'SquadMasterId'
FROM Squads s
INNER JOIN SquadMembers sm ON sm.SquadFK = s.SquadPK
WHERE sm.IsDeleted = 0
	AND s.IsDeleted = 0
	AND sm.IsSquadMaster = 1
END
GO

-- ================================================
-- 
-- This procedure will return details for  
-- all squads a member is a part of
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Get_SquadsByMember
	@Id uniqueidentifier

AS
BEGIN

IF @Id IS NULL
	BEGIN
		RETURN 0;
	END
ELSE
	BEGIN
		SELECT SquadPK AS 'Id',
			   SquadName,
			   SquadDesc,
			   SquadLocation,
			   (Select UserFK From SquadMembers sm1 Where SquadFK = s.SquadPK AND sm1.IsSquadMaster = 1) as SquadMasterId
		FROM Squads s
		INNER JOIN SquadMembers sm ON sm.SquadFK = s.SquadPK
		WHERE s.IsDeleted = 0 
			AND sm.IsDeleted = 0	
			AND sm.UserFK = @Id

	END
END
GO

-- ================================================
-- 
-- This procedure will update details on the
-- specified availability
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Update_Availability
	@Id uniqueidentifier,
	@DayEnum int,
	@StartTime time,
	@EndTime time

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from UserAvailability WHERE AvailabilityPK = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
		UPDATE UserAvailability 
		SET 
			DayEnum = @DayEnum, 
			StartTime = @StartTime,
			EndTime = @EndTime
		WHERE AvailabilityPK = @Id
	RETURN 1
	END
END
GO

-- ================================================
-- 
-- This procedure will update details on the
-- specified member
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Update_Member
	@Id uniqueidentifier,
	@FirstName nvarchar(35),
	@LastName nvarchar(35),
	@Email nvarchar(75)

AS
BEGIN

DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @Id)

IF @Exists = 0
	BEGIN
	RETURN 0
	END
ELSE
	BEGIN
		UPDATE Users 
		SET 
			FirstName = @FirstName, 
			LastName = @LastName,
			Email = @Email
		WHERE UserPk = @Id
	RETURN 1
	END
END
GO

-- ================================================
-- 
-- This procedure will update the password hash and
-- salt based on the user id
-- 
-- ================================================


CREATE OR ALTER PROCEDURE Update_Password
	@memberId uniqueidentifier,
	@PwHash varchar(128),
	@PwSalt varchar(128)

AS
BEGIN


DECLARE @Exists bit = (SELECT COUNT(1) from Users WHERE UserPk = @memberId)

IF @Exists = 0
	BEGIN
		RETURN 0
	END
ELSE
	BEGIN
		UPDATE Users 
		SET 
			PwHash = @PwHash, 
			PwSalt = @PwSalt
		WHERE UserPk = @memberId
	END
END
GO

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

---- This file must run after the database and all tables have been created.
---- This seed script was generated by ChatGPT, with minor tweaks by the team to fix issues.
---- START OF CHATGPT SEED SCRIPT
DELETE FROM UserAvailability;
DELETE FROM SquadMembers;
DELETE FROM Users;
DELETE FROM Squads;


---- Generated Users - PwHash is a hashed version of 'Password'
INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'John', 'Smith', 'johnsmith@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Emily', 'Johnson', 'emilyjohnson@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Michael', 'Williams', 'michaelwilliams@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Sophia', 'Brown', 'sophiabrown@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Mohammed', 'Ali', 'mohammedali@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Anna', 'Martinez', 'annamartinez@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Liam', 'Garcia', 'liamgarcia@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Emma', 'Rodriguez', 'emmarodriguez@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Ethan', 'Lopez', 'ethanlopez@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Olivia', 'Lee', 'olivialee@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Muhammad', 'Wang', 'muhammadwang@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Ava', 'Kim', 'avakim@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Noah', 'Singh', 'noahsingh@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Isabella', 'Hernandez', 'isabellahernandez@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'William', 'Nguyen', 'williamnguyen@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Sofia', 'Takahashi', 'sofiatakahashi@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'James', 'Müller', 'jamesmuller@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Mia', 'Sato', 'miasato@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Benjamin', 'Chen', 'benjaminchen@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Charlotte', 'Kumar', 'charlottekumar@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Daniel', 'Gupta', 'danielgupta@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Amelia', 'Patel', 'ameliapatel@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Logan', 'Tan', 'logantan@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Evelyn', 'Wong', 'evelynwong@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Alexander', 'Kim', 'alexanderkim@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 0);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Yuna', 'Ito', 'yunaito@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 1);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Jacob', 'Wu', 'jacobwu@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 1);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Harper', 'Lee', 'harperlee@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 1);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Aiden', 'Choi', 'aidenchoi@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 1);

INSERT INTO USERS (UserPk, FirstName, LastName, Email, PwHash, PwSalt, IsDeleted) 
VALUES (NEWID(), 'Emma', 'Ivanov', 'emmaivanov@bademail.com', 'WklCAUD9+zUZsCBoTDtzJPhfRtxWzNBZ5fC14zV6vd8=', 'xXhK/PNFxya0Bh+Vd1HOoA==', 1);

---- Generated Squads
INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Mighty Morphin Mangoes', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed id nisi vel risus tincidunt dictum. Mauris eu nulla ut ligula rhoncus convallis. Vestibulum scelerisque, elit vel tempor ultrices, metus felis pulvinar orci, in varius eros justo ac nulla. Nulla facilisi. In tempor, ante et interdum rutrum, magna mi tincidunt odio, ac condimentum tortor ante id quam. ', 'New York, NY', 0);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Dazzling Disco Ducks', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam laoreet, risus et tincidunt placerat, nulla purus suscipit risus, ut pharetra libero turpis eget sem. Pellentesque bibendum lectus libero, ac posuere erat ultricies in. Proin sit amet tempor nunc, id gravida turpis. Duis vitae libero vitae neque fermentum laoreet. In eu magna non enim vulputate lacinia. Aenean fermentum id purus et tincidunt. ', 'Los Angeles, CA', 0);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Funky Chicken Nuggets', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis ac eros sed velit efficitur eleifend sed in urna. Nullam euismod ex vitae quam elementum, nec efficitur nulla facilisis. Vestibulum convallis metus in justo dignissim, vel tempus eros suscipit. In commodo efficitur ligula, non feugiat justo feugiat sit amet. Suspendisse faucibus ante non vehicula fringilla. Integer lacinia hendrerit nunc, nec elementum nibh vehicula vitae.', 'Chicago, IL', 0);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Groovy Gummy Bears', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce at feugiat nisi, eget tincidunt odio. Maecenas ullamcorper dolor in magna commodo feugiat. In ut tellus vitae justo maximus gravida vel ut dui. In ultricies nisl risus, vitae sodales orci commodo non. Sed ac mi ut felis tempus euismod vel sit amet libero. Curabitur auctor, ligula et ultricies fermentum, mi ex tempus leo, vitae congue enim dolor a ante. Sed eleifend nisi et hendrerit lacinia. Nulla facilisi.', 'Miami, FL', 0);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Wacky Watermelons', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec posuere ante at lorem congue ultrices. Nulla porttitor eleifend ligula, eu tempor justo egestas a. Curabitur id diam erat. Morbi pellentesque, sem ac luctus malesuada, mi lectus fermentum lacus, at tristique arcu ligula at justo. Donec non consectetur lorem. Vestibulum convallis sagittis fermentum. Morbi sed nisi quis nisi feugiat congue eu in dui. Proin blandit quam eget orci tempus, at posuere lorem tristique.', 'Seattle, WA', 0);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Silly Strawberry Jams', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer tincidunt velit ut purus sollicitudin, id consequat felis interdum. Ut ut sapien risus. Cras dictum massa vel turpis finibus euismod. Nulla rutrum malesuada fringilla. Nam dictum tempor magna, nec tincidunt urna lobortis quis. Duis iaculis nisl et lectus scelerisque, at molestie orci laoreet. Proin vitae mauris sed lectus cursus dictum. Cras tempor magna eget velit bibendum dictum. Duis in mi quam. ', 'Houston, TX', 0);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Crazy Coconut Crew', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam in lacus eu lorem suscipit interdum eu in nibh. Phasellus non enim non tortor eleifend sodales a sit amet risus. Integer viverra est nec enim scelerisque, nec mattis sapien volutpat. In vitae velit nec ex varius aliquet. Curabitur vestibulum lacus felis, eget tristique massa malesuada a. Pellentesque sit amet tempus felis. ', 'Dallas, TX', 1);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Zany Zucchini Zebras', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum eget est eu libero luctus mattis eu sit amet tortor. Ut ullamcorper tincidunt ipsum at pellentesque. Nulla sed velit vel purus cursus bibendum. Sed ut urna non justo suscipit pretium et ut arcu. Sed varius nibh eu eros dictum, eu malesuada ipsum efficitur. Curabitur facilisis, libero at fermentum vehicula, odio nisi lobortis dolor, sit amet cursus dui nulla in sapien.', 'San Francisco, CA', 1);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Wacky Waffle Wizards', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ac elit elit. Integer ut nulla sit amet mi auctor venenatis. Nam eleifend justo eu sem convallis, eget interdum risus tempus. Maecenas commodo est id libero suscipit, ac congue lorem ullamcorper.', 'Denver, CO', 0);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Jumping Jellybeans', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras fermentum nec dui ac suscipit. Integer a ligula rutrum, consequat ligula in, efficitur sapien. Nulla sit amet nulla eget nulla eleifend viverra. Vestibulum convallis massa in arcu eleifend aliquam.', 'Austin, TX', 1);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Radical Raspberry Rabbits', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam placerat fermentum lorem, a elementum ipsum rutrum nec. Nullam viverra neque vel sapien tristique, id fermentum libero dignissim. Integer varius tellus mi, non posuere est molestie nec. Nam auctor mi id sollicitudin consequat. Donec nec efficitur magna. Integer vitae ante vestibulum, mattis lorem a, vehicula velit. ', 'Portland, OR', 1);

INSERT INTO SQUADS (SquadPK, SquadName, SquadDesc, SquadLocation, IsDeleted) 
VALUES (NEWID(), 'The Bouncing Banana Bunch', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ullamcorper, nisi a vestibulum congue, metus lorem fermentum justo, in volutpat purus ipsum nec mauris. Pellentesque mattis mi vitae neque sollicitudin iaculis. Nullam sit amet velit arcu. Vivamus dignissim, arcu at pharetra facilisis, felis nunc fringilla justo, id sollicitudin lacus sem non turpis.', 'Phoenix, AZ', 0);


-- Randomly associate Users to Squads.
-- Declare variables to store the number of users and squads
DECLARE @NumUsers INT
DECLARE @NumSquads INT

-- Get the total number of users
SELECT @NumUsers = COUNT(*) FROM USERS

-- Get the total number of squads
SELECT @NumSquads = COUNT(*) FROM SQUADS

-- Insert random squad members
DECLARE @CounterA INT = 1

WHILE @CounterA <= 100 -- Change 100 to the desired number of random squad members
BEGIN
    -- Generate a random UserPK
    DECLARE @RandomUserPK UNIQUEIDENTIFIER
    SELECT TOP 1 @RandomUserPK = UserPK FROM USERS ORDER BY NEWID()

    -- Generate a random SquadPK
    DECLARE @RandomSquadPK UNIQUEIDENTIFIER
    SELECT TOP 1 @RandomSquadPK = SquadPK FROM SQUADS ORDER BY NEWID()

    -- Check if the selected user is already a member of the chosen squad
    IF NOT EXISTS (SELECT 1 FROM SquadMembers WHERE SquadFK = @RandomSquadPK AND UserFK = @RandomUserPK)
    BEGIN
        -- Determine if the user is the squad master
        DECLARE @IsSquadMaster BIT
        IF NOT EXISTS (SELECT 1 FROM SquadMembers WHERE SquadFK = @RandomSquadPK)
            SET @IsSquadMaster = 1
        ELSE
            SET @IsSquadMaster = 0

        -- Generate a random JoinDate between Jan 1, 2022 and Today
        DECLARE @JoinDate DATE
        SET @JoinDate = DATEADD(DAY, -1 * RAND() * DATEDIFF(DAY, '2022-01-01', GETDATE()), GETDATE())

        -- Generate a random value to determine if the record should be deleted
        DECLARE @IsDeleted FLOAT
        SET @IsDeleted = RAND()

        -- Insert the squad member record
        INSERT INTO SquadMembers (SquadMemberPK, SquadFK, UserFK, IsSquadMaster, JoinDate, IsDeleted)
        VALUES (NEWID(), @RandomSquadPK, @RandomUserPK, @IsSquadMaster, @JoinDate, CASE WHEN @IsDeleted < 0.95 THEN 0 ELSE 1 END)
    END

    SET @CounterA = @CounterA + 1
END


-- Declare variables to store the number of days and users
DECLARE @NumDays INT
DECLARE @NumUsersB INT

-- Get the total number of days (Monday - Sunday)
SET @NumDays = 6

-- Get the total number of users
SELECT @NumUsersB = COUNT(*) FROM USERS

-- Insert random availability records for each user
DECLARE @CounterB INT = 1

WHILE @CounterB <= @NumUsersB
BEGIN
    -- Generate a random number of availability records between 6 and 20
    DECLARE @NumRecords INT
    SET @NumRecords = ROUND(RAND() * (20 - 6) + 6, 0)

    -- Get the UserPK for the current user
    DECLARE @CurrentUserPK UNIQUEIDENTIFIER
    SELECT TOP 1 @CurrentUserPK = u.UserPk
		FROM Users u
		LEFT JOIN UserAvailability ua ON u.UserPk = ua.UserFK
		GROUP BY u.UserPk
		HAVING COUNT(ua.AvailabilityPK) = 0

    -- Insert random availability records for the current user
    DECLARE @DayCounter INT = 1

    WHILE @DayCounter <= @NumRecords
    BEGIN
        -- Generate a random DayEnum value between 1 and 7 (representing Monday - Sunday)
        DECLARE @RandomDayEnum INT
        SET @RandomDayEnum = ROUND(RAND() * (@NumDays), 0)

        -- Generate a random StartTime between 00:00 and 23:45 (in 15-minute increments)
        DECLARE @RandomStartTime TIME
        SET @RandomStartTime = DATEADD(MINUTE, ROUND(RAND() * 95, 0) * 15, '00:00')

        -- Generate a random EndTime after the StartTime
        DECLARE @RandomEndTime TIME
        SET @RandomEndTime = DATEADD(MINUTE, ROUND(RAND() * (96 - DATEPART(MINUTE, @RandomStartTime)), 0) * 15, @RandomStartTime)

        -- If EndTime is before StartTime, swap them
        IF @RandomEndTime <= @RandomStartTime
        BEGIN
            DECLARE @Temp TIME
            SET @Temp = @RandomStartTime
            SET @RandomStartTime = @RandomEndTime
            SET @RandomEndTime = @Temp
        END

        -- Generate a random value to determine if the record should be deleted
        DECLARE @IsDeletedB FLOAT
        SET @IsDeletedB = RAND()

        -- Insert the availability record
        INSERT INTO UserAvailability (AvailabilityPK, UserFK, DayEnum, StartTime, EndTime, IsDeleted)
        VALUES (NEWID(), @CurrentUserPK, @RandomDayEnum, @RandomStartTime, @RandomEndTime, CASE WHEN @IsDeletedB < 0.95 THEN 0 ELSE 1 END)

        SET @DayCounter = @DayCounter + 1
    END

    SET @CounterB = @CounterB + 1
END

---- END OF CHATGPT SEED SCRIPT