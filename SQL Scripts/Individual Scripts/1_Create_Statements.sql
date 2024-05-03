
CREATE DATABASE ScheduSquad; 
GO

USE ScheduSquad ;

CREATE TABLE Users (
	UserPk uniqueidentifier NOT NULL PRIMARY KEY, 
	FirstName nvarchar(35) NOT NULL,
	LastName nvarchar(35)  NOT NULL,
	Email nvarchar(75)  NOT NULL,
	PwHash nvarchar(128),
	PwSalt nvarchar(128),
	IsDeleted bit NOT NULL DEFAULT 0
);

CREATE TABLE UserAvailability (
	AvailabilityPK uniqueidentifier NOT NULL PRIMARY KEY,
	UserFK uniqueidentifier NOT NULL,
	DayEnum int NOT NULL,
	StartTime time NOT NULL,
	EndTime time NOT NULL,
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
