USE master 
GO 

IF EXISTS(SELECT * FROM sys.databases WHERE name='Network') 
BEGIN 
DROP DATABASE Network
END 
GO 

CREATE DATABASE Network
GO

USE Network
GO

ALTER AUTHORIZATION ON DATABASE::Network TO sa;

CREATE TABLE [User](
	[IDUser] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_User] primary key,
	[Surname] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Patronymic] [varchar](50) NULL,
	[Gender] [bit] NULL,
	[PhoneNumber] [varchar](15) NULL,
	[YearOfBirth] [int] NULL,
	[Town] [varchar](35) NULL,
	[Login] [varchar](35) NOT NULL,
	[Password] [varchar](35) NOT NULL
);

CREATE TABLE [UserRole](
	[IDUser] [int],
	[Role][varchar](15) NOT NULL
);

CREATE TABLE [Friendship](
	[IDUser] [int] NOT NULL,
	[IDFriend] [int] NOT NULL,
	[Term_Friends] [datetime] NOT NULL DEFAULT GETDATE()
						
);

CREATE TABLE [Messages](
	[IDUser] [int] NOT NULL,
	[IDFriend] [int] NOT NULL,
	[Message] [varchar](100) NOT NULL,
	[DateOfMessage] [datetime]	NOT NULL
);

ALTER TABLE [UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole] 
FOREIGN KEY([IDUser]) REFERENCES [User] ([IDUser])

ALTER TABLE [Friendship]  WITH CHECK ADD  CONSTRAINT [FK_Friendship] 
FOREIGN KEY([IDUser]) REFERENCES [User] ([IDUser])

ALTER TABLE [Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages]
FOREIGN KEY([IDUser]) REFERENCES [User] ([IDUser])
GO

INSERT [User] 
([Surname], [Name], [Login], [Password])
Values
('Burdavitsyn', 'Artyom', 'admin', 'admin')
INSERT [UserRole]
([IDUser], [Role])
Values
(1, 'admin')

GO
CREATE TRIGGER Insert_New_User ON [User] AFTER INSERT
AS 
BEGIN 
	DECLARE @Id int
	SET @Id = (SELECT IDUser FROM inserted); 
	INSERT UserRole
	([IDUser], [Role])
	VALUES
	(@Id, 'user')
END

GO
CREATE PROCEDURE [dbo].[AddUser]
	@Login nvarchar(35),
	@Password nvarchar(35),
	@Name nvarchar(50),
	@Surname nvarchar(50),
	@Patronymic nvarchar(50),
	@Town nvarchar(50),
	@YearOfBirth int,
	@PhoneNumber nvarchar(12),
	@Gender bit,
	@Id int out
AS
BEGIN
	 INSERT INTO [User]([Name], [Surname], Patronymic, YearOfBirth, Town, PhoneNumber, [Password],[Login], Gender )
		VALUES(@Name, @Surname, @Patronymic, @YearOfBirth, @Town, @PhoneNumber, @Password, @Login, @Gender)

		SET @Id = SCOPE_IDENTITY();
END

GO
CREATE PROCEDURE [dbo].[AddFriend]
	@IDUser int, 
	@IDFriend int
AS
BEGIN
	 INSERT INTO Friendship (IDUser, IDFriend)
		VALUES(@IDUser, @IDFriend)
END
GO
CREATE PROCEDURE [dbo].DeleteFriend
	@IDUser int, 
	@IDFriend int
AS
BEGIN
	 DELETE FROM Friendship WHERE IDFriend = @IDFriend AND IDUser = @IDUser
END

GO
CREATE PROCEDURE [dbo].[GetAllFriends]
	@Login nvarchar(35)
AS
BEGIN
	 SELECT f.IDFriend, u.[Name], u.Surname, u.Gender, u.YearOfBirth, u.Patronymic, u.PhoneNumber, u.Town, f.Term_Friends,u.[Login]
	 FROM [User] u INNER JOIN (SELECT [IDFriend], Term_Friends FROM [Friendship] WHERE IDUser = 
	 (SELECT [IDUser] FROM [User] WHERE [Login] = @Login)
	 ) f ON f.IDFriend = u.IDUser 
END

GO
CREATE PROCEDURE [dbo].SearchByName
	@Name [varchar](50)
AS
BEGIN
	 SELECT  u.IDUser, u.[Name], u.Surname, u.Gender, u.YearOfBirth, u.Patronymic, u.PhoneNumber, u.Town
	 FROM [User] u 
	 WHERE u.[Name] = @Name
	 
END
GO
CREATE PROCEDURE [dbo].SearchBySurname
	@Surname [varchar](50)
AS
BEGIN
	 SELECT  u.IDUser, u.[Name], u.Surname, u.Gender, u.YearOfBirth, u.Patronymic, u.PhoneNumber, u.Town
	 FROM [User] u 
	 WHERE u.Surname = @Surname
	 
END
GO
CREATE PROCEDURE [dbo].SearchByPhone
	@PhoneNumber [varchar](15)
AS
BEGIN
	 SELECT  u.IDUser, u.[Name], u.Surname, u.Gender, u.YearOfBirth, u.Patronymic, u.PhoneNumber, u.Town
	 FROM [User] u 
	 WHERE u.[PhoneNumber] = @PhoneNumber
	 
END
GO
CREATE PROCEDURE [dbo].SearchByTown
	@Town [varchar](35)
AS
BEGIN
	 SELECT  u.IDUser, u.[Name], u.Surname, u.Gender, u.YearOfBirth, u.Patronymic, u.PhoneNumber, u.Town
	 FROM [User] u 
	 WHERE u.[Town] = @Town
	 
END

GO
CREATE PROCEDURE [dbo].[EditUser]
	@Login nvarchar(35),
	@Password nvarchar(35),
	@Name nvarchar(50),
	@Surname nvarchar(50),
	@Patronymic nvarchar(50),
	@Town nvarchar(50),
	@YearOfBirth int,
	@PhoneNumber nvarchar(12),
	@Gender bit,
	@Id int
AS
BEGIN
	 Update [User] 
	Set [Login] = @Login, 
		[Password] = @Password, 
		[Name] = @Name, 
		Surname = @Surname,
		Patronymic = @Patronymic, 
		Town = @Town, 
		YearOfBirth = @YearOfBirth,
		PhoneNumber = @PhoneNumber, 
		Gender = @Gender

		Where IDUser = @Id
END

GO
CREATE PROCEDURE [dbo].GetUsetRoles
	@UserName [varchar](50)
AS
BEGIN
	 SELECT r.[Role]
	 FROM [User] u INNER JOIN UserRole r ON r.IDUser = u.IDUser
	 WHERE  u.[Login] = @UserName
END

GO
CREATE PROCEDURE [dbo].SendMessage
	@IDUser int, 
	@IDFriend int,
	@Message [varchar](100),
	@DateOfMessage datetime
AS
BEGIN
	 INSERT INTO [Messages] (IDUser, IDFriend, [Message], DateOfMessage)
		VALUES(@IDUser, @IDFriend, @Message, @DateOfMessage)
END

GO
CREATE PROCEDURE [dbo].GetMessagesByIds
	@IDUser int, 
	@IDFriend int
AS
BEGIN
	 SELECT * 
	 FROM [Messages] 
	 WHERE (IDUser = @IDUser AND IDFriend = @IDFriend) OR  (IDUser = @IDFriend AND IDFriend = @IDUser)
END

GO
CREATE PROCEDURE [dbo].[LogIn]
	@Login nvarchar(35), 
	@Password nvarchar(35)
AS
BEGIN
	 SELECT * 
	 FROM [User] u
	 WHERE u.[Login] = @Login AND u.[Password] = @Password
END

Go
CREATE PROCEDURE [dbo].[GetByLogin]
	@Login [varchar](35)
AS
BEGIN
	 SELECT u.IDUser, u.[Name], u.Surname, u.Gender, u.YearOfBirth, u.Patronymic, u.PhoneNumber, u.Town, u.[Login]
	 FROM [User] u 
	 WHERE u.[Login] = @Login
	 
END
GO
CREATE PROCEDURE [dbo].[GetById]
	@Id int
AS
BEGIN
	 SELECT u.IDUser, u.[Name], u.Surname, u.Gender, u.YearOfBirth, u.Patronymic, u.PhoneNumber, u.Town, u.[Login]
	 FROM [User] u 
	 WHERE u.[IDUser] = @Id
	 
END

GO
CREATE PROCEDURE [dbo].[GetAllUsers]
AS
BEGIN
	 SELECT *
	 FROM [User]  
END

GO
CREATE PROCEDURE [dbo].GetAllMessages
AS
BEGIN
	 SELECT *
	 FROM [Messages]	 
END

GO
CREATE PROCEDURE [dbo].DeleteMessages
AS
BEGIN
	 ALTER TABLE [User]  nocheck constraint all
	 DELETE FROM [Messages] 
END
GO
CREATE PROCEDURE [dbo].DeleteUsers
AS
BEGIN

	 ALTER TABLE [Friendship]  nocheck constraint all
	 ALTER TABLE [UserRole]  nocheck constraint all
	 ALTER TABLE [Messages]  nocheck constraint all
	 DELETE u
	 FROM [User] u INNER JOIN [UserRole] ur ON u.IDUser=ur.IDUser
	 WHERE ur.[Role] != 'admin'

	 DELETE ur
	 FROM [User] u RIGHT JOIN [UserRole] ur ON u.IDUser=ur.IDUser
	 WHERE u.IDUser is Null
	 
	 DELETE m
	 FROM [User] u RIGHT JOIN [Messages] m ON u.IDUser=m.IDUser
	 WHERE u.IDUser is Null

	 DELETE f
	 FROM [User] u RIGHT JOIN [Friendship] f ON u.IDUser=f.IDUser
	 WHERE u.IDUser is Null
END
