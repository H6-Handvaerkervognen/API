-- Run this line first
CREATE DATABASE haandvaerkervognen;

USE haandvaerkervognen;

CREATE TABLE [Users] 
(
	[Username] VARCHAR(40) PRIMARY KEY,
	[Password] VARCHAR(200) NOT NULL,
	[Salt] VARBINARY(200) NOT NULL,
	[Token] VARCHAR(MAX) NOT NULL
)

CREATE TABLE [Alarms]
(
	[Id] VARCHAR(200) PRIMARY KEY,
	[StartTime] VARBINARY(MAX), -- encrypted
	[EndTime] VARBINARY(MAX), -- encrypted
	[Name] VARBINARY(MAX), -- encrypted
	[AlarmOn] BIT,
)

CREATE TABLE [Pairs]
(
	-- Username
	[UserId] VARCHAR(40) NOT NULL,
	[AlarmId] VARCHAR(200) NOT NULL,

	
FOREIGN KEY ([UserId]) REFERENCES [Users]([Username]),
FOREIGN KEY ([AlarmId]) REFERENCES [Alarms]([Id])
)

--Tabel med krypt navn