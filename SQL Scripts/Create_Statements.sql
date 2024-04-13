
--CREATE DATABASE ScheduSquad;
USE ScheduSquad ;

DECLARE @COMMIT bit = 0

BEGIN TRANSACTION A

CREATE TABLE Users (
	UserPk uniqueidentifier NOT NULL PRIMARY KEY, 
	FirstName nvarchar(35) NOT NULL,
	LastName nvarchar(35)  NOT NULL,
	Email nvarchar(75)  NOT NULL,
	PwHash char(128)  NOT NULL,
	PwSalt char(128)  NOT NULL,
	IsDeleted bit NOT NULL DEFAULT 0
);

CREATE TABLE UserAvailability (
	AvailabilityPK uniqueidentifier NOT NULL PRIMARY KEY,
	UserFK uniqueidentifier NOT NULL,
	DayEnum int NOT NULL,
	StartTime datetime2 NOT NULL,
	EndTime datetime2 NOT NULL,
	FOREIGN KEY (UserFK) REFERENCES Users(UserPK),
	IsDeleted bit NOT NULL DEFAULT 0
);

CREATE TABLE Squads (
	SquadPK uniqueidentifier NOT NULL PRIMARY KEY,
	SquadName nvarchar(50) NOT  NULL,
	SquadDesc nvarchar(500) NOT NULL,
	SquadLocation nvarchar(25),
	IsDeleted bit NOT NULL DEFAULT 0
);

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

IF @COMMIT = 1
	BEGIN
	COMMIT TRANSACTION A;
	SELECT 'COMMITTED'
	END
ELSE
	BEGIN
	ROLLBACK TRANSACTION A;
	SELECT 'ROLLED BACK'
	END

	
--INSERT INTO Users VALUES ('Joe','Jones', 'a@b.com', 'cc9e2e77533aa12149211e19c682f2b5', 'cc9e2e77533aa12149211e19c682f2b5');
--SELECT * FROM Users
--DROP TABLE DaysOfTheWeek