CREATE TABLE [User] (
	Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
	Username NVARCHAR(50) UNIQUE NOT NULL,
	Email NVARCHAR(50) UNIQUE NOT NULL,
	Password NVARCHAR(100) NOT NULL,
    AvatarUrl VARCHAR(255) NULL
)