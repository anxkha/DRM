USE DRM;

CREATE TABLE Users
(
	ID int PRIMARY KEY CLUSTERED IDENTITY,
	FirstName nvarchar(30) NOT NULL,
	Email nvarchar(250) NOT NULL UNIQUE,
	Password nvarchar(250) NOT NULL,
	IsRaidTeam bit NOT NULL,
	IsAdmin bit NOT NULL
);

CREATE TABLE Class
(
	Name nvarchar(20) NOT NULL UNIQUE NONCLUSTERED
);

CREATE TABLE Race
(
	Name nvarchar(20) NOT NULL UNIQUE NONCLUSTERED
);

CREATE TABLE Role
(
	Name nvarchar(20) NOT NULL UNIQUE NONCLUSTERED
);

CREATE TABLE Expansion
(
	Name nvarchar(50) NOT NULL UNIQUE NONCLUSTERED
);

CREATE TABLE Specialization
(
	ID int PRIMARY KEY CLUSTERED IDENTITY,
	Name nvarchar(20) NOT NULL,
	Role nvarchar(20) NOT NULL REFERENCES Role(Name)
);

CREATE TABLE Character
(
	Name nvarchar(12) PRIMARY KEY CLUSTERED,
	Class nvarchar(20) NOT NULL REFERENCES Class(Name),
	Race nvarchar(20) NOT NULL REFERENCES Race(Name),
	Level int NOT NULL CHECK (Level >= 1),
	PrimarySpecializationID int REFERENCES Specialization(ID),
	SecondarySpecializationID int REFERENCES Specialization(ID),
	AccountID int NOT NULL REFERENCES Users(ID)
);

CREATE TABLE SpecializationToClass
(
	Class nvarchar(20) NOT NULL REFERENCES Class(Name),
	Specialization int NOT NULL REFERENCES Specialization(ID)
);

CREATE TABLE ClassToRace
(
	Class nvarchar(20) NOT NULL REFERENCES Class(Name),
	Race nvarchar(20) NOT NULL REFERENCES Race(Name)
);

CREATE TABLE Raid
(
	Name nvarchar(50) PRIMARY KEY CLUSTERED,
	Expansion nvarchar(50) NOT NULL REFERENCES Expansion(Name),
	MaxPlayers int NOT NULL CHECK (MaxPlayers >= 10 AND MaxPlayers <= 40),
	MinimumLevel int NOT NULL CHECK (MinimumLevel >= 60),
	NumberOfBosses int NOT NULL CHECK (NumberOfBosses >= 1)
);

CREATE TABLE RaidInstance
(
	ID int PRIMARY KEY CLUSTERED IDENTITY,
	Raid nvarchar(50) NOT NULL REFERENCES Raid(Name),
	Name nvarchar(100) NOT NULL,
	Description nvarchar(1000) NOT NULL,
	InviteTime datetime NOT NULL,
	StartTime datetime NOT NULL,
	IsArchived bit NOT NULL
);

CREATE TABLE RaidSignup
(
	RaidInstanceID int NOT NULL REFERENCES RaidInstance(ID),
	Character nvarchar(12) NOT NULL REFERENCES Character(Name),
	Comment nvarchar(200) NOT NULL,
	IsRostered bit NOT NULL DEFAULT 'FALSE',
	IsCancelled bit NOT NULL DEFAULT 'FALSE',
	RosteredSpecialization int NOT NULL CHECK (RosteredSpecialization >= 1 AND RosteredSpecialization <= 2),
	SignupDate datetime NOT NULL
);
