CREATE TABLE Users
(
	ID int PRIMARY KEY CLUSTERED IDENTITY,
	FirstName nvarchar(30) NOT NULL,
	Email nvarchar(250) NOT NULL UNIQUE,
	Password nvarchar(250) NOT NULL,
	IsRaidTeam bit NOT NULL,
	IsAdmin bit NOT NULL
);

CREATE TABLE Settings
(
	TimeZone int NOT NULL,
	GuildName nvarchar(100) NOT NULL,
	GuildAbbreviation nvarchar(5) NOT NULL
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

INSERT INTO Expansion (Name) VALUES ('Vanilla');
INSERT INTO Expansion (Name) VALUES ('Burning Crusade');
INSERT INTO Expansion (Name) VALUES ('Wrath of the Lich King');
INSERT INTO Expansion (Name) VALUES ('Cataclysm');
INSERT INTO Expansion (Name) VALUES ('Mists of Pandaria');

INSERT INTO Race (Name) VALUES ('Human');
INSERT INTO Race (Name) VALUES ('Dwarf');
INSERT INTO Race (Name) VALUES ('Gnome');
INSERT INTO Race (Name) VALUES ('Night Elf');
INSERT INTO Race (Name) VALUES ('Draenei');
INSERT INTO Race (Name) VALUES ('Worgen');
INSERT INTO Race (Name) VALUES ('Pandaren');

INSERT INTO Role (Name) VALUES ('Tank');
INSERT INTO Role (Name) VALUES ('Melee');
INSERT INTO Role (Name) VALUES ('Ranged');
INSERT INTO Role (Name) VALUES ('Healer');

INSERT INTO Class (Name) VALUES ('Warrior');
INSERT INTO Class (Name) VALUES ('Mage');
INSERT INTO Class (Name) VALUES ('Druid');
INSERT INTO Class (Name) VALUES ('Death Knight');
INSERT INTO Class (Name) VALUES ('Paladin');
INSERT INTO Class (Name) VALUES ('Warlock');
INSERT INTO Class (Name) VALUES ('Priest');
INSERT INTO Class (Name) VALUES ('Shaman');
INSERT INTO Class (Name) VALUES ('Hunter');
INSERT INTO Class (Name) VALUES ('Rogue');
INSERT INTO Class (Name) VALUES ('Monk');

INSERT INTO Specialization (Name, Role) VALUES ('Arms', 'Melee');
INSERT INTO Specialization (Name, Role) VALUES ('Fury', 'Melee');
INSERT INTO Specialization (Name, Role) VALUES ('Protection', 'Tank');

INSERT INTO Specialization (Name, Role) VALUES ('Fire', 'Ranged');
INSERT INTO Specialization (Name, Role) VALUES ('Arcane', 'Ranged');
INSERT INTO Specialization (Name, Role) VALUES ('Frost', 'Ranged');

INSERT INTO Specialization (Name, Role) VALUES ('Balance', 'Ranged');
INSERT INTO Specialization (Name, Role) VALUES ('Feral', 'Melee');
INSERT INTO Specialization (Name, Role) VALUES ('Guardian', 'Tank');
INSERT INTO Specialization (Name, Role) VALUES ('Restoration', 'Healer');

INSERT INTO Specialization (Name, Role) VALUES ('Blood', 'Tank');
INSERT INTO Specialization (Name, Role) VALUES ('Frost', 'Melee');
INSERT INTO Specialization (Name, Role) VALUES ('Unholy', 'Melee');

INSERT INTO Specialization (Name, Role) VALUES ('Holy', 'Healer');
INSERT INTO Specialization (Name, Role) VALUES ('Protection', 'Tank');
INSERT INTO Specialization (Name, Role) VALUES ('Retribution', 'Melee');

INSERT INTO Specialization (Name, Role) VALUES ('Affliction', 'Ranged');
INSERT INTO Specialization (Name, Role) VALUES ('Destruction', 'Ranged');
INSERT INTO Specialization (Name, Role) VALUES ('Demonology', 'Ranged');

INSERT INTO Specialization (Name, Role) VALUES ('Discipline', 'Healer');
INSERT INTO Specialization (Name, Role) VALUES ('Holy', 'Healer');
INSERT INTO Specialization (Name, Role) VALUES ('Shadow', 'Ranged');

INSERT INTO Specialization (Name, Role) VALUES ('Restoration', 'Healer');
INSERT INTO Specialization (Name, Role) VALUES ('Enhancement', 'Melee');
INSERT INTO Specialization (Name, Role) VALUES ('Elemental', 'Ranged');

INSERT INTO Specialization (Name, Role) VALUES ('Beast Mastery', 'Ranged');
INSERT INTO Specialization (Name, Role) VALUES ('Marksmanship', 'Ranged');
INSERT INTO Specialization (Name, Role) VALUES ('Survival', 'Ranged');

INSERT INTO Specialization (Name, Role) VALUES ('Assassination', 'Melee');
INSERT INTO Specialization (Name, Role) VALUES ('Combat', 'Melee');
INSERT INTO Specialization (Name, Role) VALUES ('Subtlety', 'Melee');

INSERT INTO Specialization (Name, Role) VALUES ('Brewmaster', 'Tank');
INSERT INTO Specialization (Name, Role) VALUES ('Mistweaver', 'Healer');
INSERT INTO Specialization (Name, Role) VALUES ('Windwalker', 'Melee');

INSERT INTO Specialization (Name, Role) VALUES ('None', 'Tank');

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warrior', 1);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warrior', 2);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warrior', 3);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warrior', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Mage', 4);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Mage', 5);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Mage', 6);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Mage', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Druid', 7);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Druid', 8);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Druid', 9);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Druid', 10);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Druid', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Death Knight', 11);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Death Knight', 12);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Death Knight', 13);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Death Knight', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Paladin', 14);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Paladin', 15);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Paladin', 16);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Paladin', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warlock', 17);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warlock', 18);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warlock', 19);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Warlock', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Priest', 20);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Priest', 21);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Priest', 22);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Priest', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Shaman', 23);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Shaman', 24);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Shaman', 25);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Shaman', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Hunter', 26);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Hunter', 27);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Hunter', 28);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Hunter', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Rogue', 29);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Rogue', 30);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Rogue', 31);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Rogue', 35);

INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Monk', 32);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Monk', 33);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Monk', 34);
INSERT INTO SpecializationToClass (Class, Specialization) VALUES ('Monk', 35);

-- Vanilla

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Molten Core', 'Vanilla', 40, 60, 10);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Blackwing Lair', 'Vanilla', 40, 60, 10);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ruins of Ahn Qiraj', 'Vanilla', 10, 60, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ahn Qiraj Temple', 'Vanilla', 40, 60, 9);

-- Burning Crusade

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Karazhan', 'Burning Crusade', 10, 70, 12);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Magtheridons Lair', 'Burning Crusade', 25, 70, 1);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Gruuls Lair', 'Burning Crusade', 25, 70, 2);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Serpentshrine Cavern', 'Burning Crusade', 25, 70, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Tempest Keep: The Eye', 'Burning Crusade', 25, 70, 4);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Battle for Mount Hyjal', 'Burning Crusade', 25, 70, 5);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Black Temple', 'Burning Crusade', 25, 70, 9);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Sunwell Plateu', 'Burning Crusade', 25, 70, 6);

-- Wrath of the Lich King

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Vault of Archavon, 10-Man', 'Wrath of the Lich King', 10, 80, 4);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Vault of Archavon, 25-Man', 'Wrath of the Lich King', 25, 80, 4);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Naxxramas, 10-Man', 'Wrath of the Lich King', 10, 80, 15);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Naxxramas, 25-Man', 'Wrath of the Lich King', 25, 80, 15);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Obsidian Sanctum, 10-Man', 'Wrath of the Lich King', 10, 80, 1);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Obsidian Sanctum, 25-Man', 'Wrath of the Lich King', 25, 80, 1);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Eye of Eternity, 10-Man', 'Wrath of the Lich King', 10, 80, 1);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Eye of Eternity, 25-Man', 'Wrath of the Lich King', 25, 80, 1);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ulduar, 10-Man', 'Wrath of the Lich King', 10, 80, 14);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ulduar, 10-Man Hardmodes', 'Wrath of the Lich King', 10, 80, 14);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ulduar, 25-Man', 'Wrath of the Lich King', 25, 80, 14);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ulduar, 25-Man Hardmodes', 'Wrath of the Lich King', 25, 80, 14);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Onyxias Lair, 10-Man', 'Wrath of the Lich King', 10, 80, 1);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Onyxias Lair, 25-Man', 'Wrath of the Lich King', 25, 80, 1);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Trial of the Crusader, 10-Man', 'Wrath of the Lich King', 10, 80, 5);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Trial of the Grand Crusader, 10-Man (Heroic)', 'Wrath of the Lich King', 10, 80, 5);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Trial of the Crusader, 25-Man', 'Wrath of the Lich King', 25, 80, 5);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Trial of the Grand Crusader, 25-Man (Heroic)', 'Wrath of the Lich King', 25, 80, 5);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Icecrown Citadel, 10-Man', 'Wrath of the Lich King', 10, 80, 12);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Icecrown Citadel, 10-Man Heroic', 'Wrath of the Lich King', 10, 80, 12);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Icecrown Citadel, 25-Man', 'Wrath of the Lich King', 25, 80, 12);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Icecrown Citadel, 25-Man Heroic', 'Wrath of the Lich King', 25, 80, 12);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ruby Sanctum, 10-Man', 'Wrath of the Lich King', 10, 80, 1);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ruby Sanctum, 10-Man Heroic', 'Wrath of the Lich King', 10, 80, 1);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ruby Sanctum, 25-Man', 'Wrath of the Lich King', 25, 80, 1);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Ruby Sanctum, 25-Man Heroic', 'Wrath of the Lich King', 25, 80, 1);

-- Cataclysm

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Baradin Hold, 10-Man', 'Cataclysm', 10, 85, 3);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Baradin Hold, 25-Man', 'Cataclysm', 25, 85, 3);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Bastion of Twilight, 10-Man', 'Cataclysm', 10, 85, 4);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Bastion of Twilight, 10-Man Heroic', 'Cataclysm', 10, 85, 5);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Bastion of Twilight, 25-Man', 'Cataclysm', 25, 85, 4);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Bastion of Twilight, 25-Man Heroic', 'Cataclysm', 25, 85, 5);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Blackwing Descent, 10-Man', 'Cataclysm', 10, 85, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Blackwing Descent, 10-Man Heroic', 'Cataclysm', 10, 85, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Blackwing Descent, 25-Man', 'Cataclysm', 25, 85, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Blackwing Descent, 25-Man Heroic', 'Cataclysm', 25, 85, 6);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Throne of the Four Winds, 10-Man', 'Cataclysm', 10, 85, 2);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Throne of the Four Winds, 10-Man Heroic', 'Cataclysm', 10, 85, 2);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Throne of the Four Winds, 25-Man', 'Cataclysm', 25, 85, 2);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Throne of the Four Winds, 25-Man Heroic', 'Cataclysm', 25, 85, 2);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Firelands, 10-Man', 'Cataclysm', 10, 85, 7);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Firelands, 10-Man Heroic', 'Cataclysm', 10, 85, 7);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Firelands, 25-Man', 'Cataclysm', 25, 85, 7);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Firelands, 25-Man Heroic', 'Cataclysm', 25, 85, 7);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Dragon Soul, 10-Man', 'Cataclysm', 10, 85, 8);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Dragon Soul, 10-Man Heroic', 'Cataclysm', 10, 85, 8);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Dragon Soul, 25-Man', 'Cataclysm', 25, 85, 8);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Dragon Soul, 25-Man Heroic', 'Cataclysm', 25, 85, 8);

-- Mists of Pandaria

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Mogushan Vaults, 10-Man', 'Mists of Pandaria', 10, 90, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Mogushan Vaults, 10-Man Heroic', 'Mists of Pandaria', 10, 90, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Mogushan Vaults, 25-Man', 'Mists of Pandaria', 25, 90, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Mogushan Vaults, 25-Man Heroic', 'Mists of Pandaria', 25, 90, 6);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Heart of Fear, 10-Man', 'Mists of Pandaria', 10, 90, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Heart of Fear, 10-Man Heroic', 'Mists of Pandaria', 10, 90, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Heart of Fear, 25-Man', 'Mists of Pandaria', 25, 90, 6);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Heart of Fear, 25-Man Heroic', 'Mists of Pandaria', 25, 90, 6);

INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Terrace of Endless Spring, 10-Man', 'Mists of Pandaria', 10, 90, 4);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Terrace of Endless Spring, 10-Man Heroic', 'Mists of Pandaria', 10, 90, 4);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Terrace of Endless Spring, 25-Man', 'Mists of Pandaria', 25, 90, 4);
INSERT INTO Raid (Name, Expansion, MaxPlayers, MinimumLevel, NumberOfBosses) VALUES ('Terrace of Endless Spring, 25-Man Heroic', 'Mists of Pandaria', 25, 90, 4);

INSERT INTO ClassToRace (Class, Race) VALUES ('Warrior', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Warrior', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Warrior', 'Gnome');
INSERT INTO ClassToRace (Class, Race) VALUES ('Warrior', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Warrior', 'Draenei');
INSERT INTO ClassToRace (Class, Race) VALUES ('Warrior', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Mage', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Mage', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Mage', 'Gnome');
INSERT INTO ClassToRace (Class, Race) VALUES ('Mage', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Mage', 'Draenei');
INSERT INTO ClassToRace (Class, Race) VALUES ('Mage', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Druid', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Druid', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Death Knight', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Death Knight', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Death Knight', 'Gnome');
INSERT INTO ClassToRace (Class, Race) VALUES ('Death Knight', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Death Knight', 'Draenei');
INSERT INTO ClassToRace (Class, Race) VALUES ('Death Knight', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Paladin', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Paladin', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Paladin', 'Draenei');

INSERT INTO ClassToRace (Class, Race) VALUES ('Warlock', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Warlock', 'Gnome');
INSERT INTO ClassToRace (Class, Race) VALUES ('Warlock', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Priest', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Priest', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Priest', 'Gnome');
INSERT INTO ClassToRace (Class, Race) VALUES ('Priest', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Priest', 'Draenei');
INSERT INTO ClassToRace (Class, Race) VALUES ('Priest', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Shaman', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Shaman', 'Draenei');

INSERT INTO ClassToRace (Class, Race) VALUES ('Hunter', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Hunter', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Hunter', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Hunter', 'Draenei');
INSERT INTO ClassToRace (Class, Race) VALUES ('Hunter', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Rogue', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Rogue', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Rogue', 'Gnome');
INSERT INTO ClassToRace (Class, Race) VALUES ('Rogue', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Rogue', 'Worgen');

INSERT INTO ClassToRace (Class, Race) VALUES ('Monk', 'Human');
INSERT INTO ClassToRace (Class, Race) VALUES ('Monk', 'Dwarf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Monk', 'Night Elf');
INSERT INTO ClassToRace (Class, Race) VALUES ('Monk', 'Gnome');
INSERT INTO ClassToRace (Class, Race) VALUES ('Monk', 'Draenei');
INSERT INTO ClassToRace (Class, Race) VALUES ('Monk', 'Pandaren');
