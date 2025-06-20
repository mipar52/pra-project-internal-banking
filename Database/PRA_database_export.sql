USE [master]
GO
/****** Object:  Database [PRA_database]    Script Date: 11/06/2025 21:16:25 ******/
CREATE DATABASE [PRA_database]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PRA_database', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PRA_database.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PRA_database_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\PRA_database_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [PRA_database] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PRA_database].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PRA_database] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PRA_database] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PRA_database] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PRA_database] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PRA_database] SET ARITHABORT OFF 
GO
ALTER DATABASE [PRA_database] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PRA_database] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PRA_database] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PRA_database] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PRA_database] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PRA_database] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PRA_database] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PRA_database] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PRA_database] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PRA_database] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PRA_database] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PRA_database] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PRA_database] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PRA_database] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PRA_database] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PRA_database] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PRA_database] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PRA_database] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PRA_database] SET  MULTI_USER 
GO
ALTER DATABASE [PRA_database] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PRA_database] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PRA_database] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PRA_database] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PRA_database] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PRA_database] SET QUERY_STORE = OFF
GO
USE [PRA_database]
GO
/****** Object:  Table [dbo].[BillingAccount]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingAccount](
	[IdBillingAccount] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Balance] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdBillingAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditCard]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditCard](
	[IdCreditCard] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[Lastname] [nvarchar](256) NOT NULL,
	[CreditCardNumber] [nvarchar](16) NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[CVVhash] [nvarchar](256) NOT NULL,
	[CVVsalt] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCreditCard] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Friend]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friend](
	[IdFriend] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[FriendId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdFriend] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MoneyTransfer]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MoneyTransfer](
	[IdMoneyTransfer] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NULL,
	[UserRecieverId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdMoneyTransfer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParkingPayment]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParkingPayment](
	[IdParkingPayment] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NULL,
	[RegistrationNumber] [nvarchar](256) NOT NULL,
	[RegistrationCountryCode] [nvarchar](256) NOT NULL,
	[DurationHours] [int] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdParkingPayment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestTransfer]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestTransfer](
	[IdRequestTransfer] [int] IDENTITY(1,1) NOT NULL,
	[UserRecieverId] [int] NOT NULL,
	[UserSenderId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRequestTransfer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[IdRole] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudyProgram]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudyProgram](
	[IdStudyProgram] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdStudyProgram] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[IdTransaction] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionTypeId] [int] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[Date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTransaction] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionType]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionType](
	[IdTransactionType] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTransactionType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[IdUser] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[EmailAddress] [nvarchar](256) NOT NULL,
	[PhoneNumber] [nvarchar](256) NOT NULL,
	[StudyProgramId] [int] NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[PasswordSalt] [nvarchar](256) NOT NULL,
	[Temp2FACodeExpires] [datetime] NULL,
	[Temp2FACode] [nvarchar](256) NULL,
	[ProfilePictureUrl] [nvarchar](256) NULL,
	[LastName] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCreditCard]    Script Date: 11/06/2025 21:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCreditCard](
	[IdUserCreditCard] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[CreditCardId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUserCreditCard] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BillingAccount] ON 

INSERT [dbo].[BillingAccount] ([IdBillingAccount], [UserId], [Balance]) VALUES (4, 5, CAST(45314 AS Decimal(18, 0)))
INSERT [dbo].[BillingAccount] ([IdBillingAccount], [UserId], [Balance]) VALUES (1002, 1002, CAST(550 AS Decimal(18, 0)))
INSERT [dbo].[BillingAccount] ([IdBillingAccount], [UserId], [Balance]) VALUES (1003, 1003, CAST(0 AS Decimal(18, 0)))
INSERT [dbo].[BillingAccount] ([IdBillingAccount], [UserId], [Balance]) VALUES (1004, 1004, CAST(0 AS Decimal(18, 0)))
INSERT [dbo].[BillingAccount] ([IdBillingAccount], [UserId], [Balance]) VALUES (1005, 1005, CAST(0 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[BillingAccount] OFF
GO
SET IDENTITY_INSERT [dbo].[CreditCard] ON 

INSERT [dbo].[CreditCard] ([IdCreditCard], [FirstName], [Lastname], [CreditCardNumber], [ExpiryDate], [CVVhash], [CVVsalt]) VALUES (1, N'Ksenija', N'Ratnica', N'1234567812345678', CAST(N'2025-05-23T19:38:24.577' AS DateTime), N'ELVRNVg44eGIJVFZLU9rfcKFWiF2ISvXqRDFFfIU4uI=', N'DeGHlJ6OABEURykiI5MSig==')
SET IDENTITY_INSERT [dbo].[CreditCard] OFF
GO
SET IDENTITY_INSERT [dbo].[Friend] ON 

INSERT [dbo].[Friend] ([IdFriend], [UserId], [FriendId]) VALUES (6, 5, 1003)
INSERT [dbo].[Friend] ([IdFriend], [UserId], [FriendId]) VALUES (7, 1003, 5)
INSERT [dbo].[Friend] ([IdFriend], [UserId], [FriendId]) VALUES (8, 5, 1004)
INSERT [dbo].[Friend] ([IdFriend], [UserId], [FriendId]) VALUES (9, 1004, 5)
INSERT [dbo].[Friend] ([IdFriend], [UserId], [FriendId]) VALUES (10, 5, 1002)
INSERT [dbo].[Friend] ([IdFriend], [UserId], [FriendId]) VALUES (11, 1002, 5)
SET IDENTITY_INSERT [dbo].[Friend] OFF
GO
SET IDENTITY_INSERT [dbo].[MoneyTransfer] ON 

INSERT [dbo].[MoneyTransfer] ([IdMoneyTransfer], [TransactionId], [UserRecieverId]) VALUES (1, 2003, 1002)
SET IDENTITY_INSERT [dbo].[MoneyTransfer] OFF
GO
SET IDENTITY_INSERT [dbo].[ParkingPayment] ON 

INSERT [dbo].[ParkingPayment] ([IdParkingPayment], [TransactionId], [RegistrationNumber], [RegistrationCountryCode], [DurationHours], [StartTime], [EndTime]) VALUES (1, NULL, N'ZG1234AA', N'HR', 2, CAST(N'2025-05-19T23:24:38.333' AS DateTime), CAST(N'2025-05-20T01:24:38.333' AS DateTime))
INSERT [dbo].[ParkingPayment] ([IdParkingPayment], [TransactionId], [RegistrationNumber], [RegistrationCountryCode], [DurationHours], [StartTime], [EndTime]) VALUES (2, NULL, N'ZG1234AA', N'HR', 3, CAST(N'2025-05-23T15:04:34.473' AS DateTime), CAST(N'2025-05-23T18:04:34.473' AS DateTime))
INSERT [dbo].[ParkingPayment] ([IdParkingPayment], [TransactionId], [RegistrationNumber], [RegistrationCountryCode], [DurationHours], [StartTime], [EndTime]) VALUES (3, NULL, N'PU1234D', N'HR', 5, CAST(N'2025-05-23T17:05:50.613' AS DateTime), CAST(N'2025-05-23T22:05:50.613' AS DateTime))
INSERT [dbo].[ParkingPayment] ([IdParkingPayment], [TransactionId], [RegistrationNumber], [RegistrationCountryCode], [DurationHours], [StartTime], [EndTime]) VALUES (4, NULL, N'ST4545BB', N'HR', 5, CAST(N'2025-05-23T17:15:15.970' AS DateTime), CAST(N'2025-05-23T22:15:15.970' AS DateTime))
SET IDENTITY_INSERT [dbo].[ParkingPayment] OFF
GO
SET IDENTITY_INSERT [dbo].[RequestTransfer] ON 

INSERT [dbo].[RequestTransfer] ([IdRequestTransfer], [UserRecieverId], [UserSenderId], [Date], [Amount]) VALUES (1, 5, 1002, CAST(N'2025-05-23T18:02:11.393' AS DateTime), CAST(2000 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[RequestTransfer] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([IdRole], [Name]) VALUES (1, N'Staff')
INSERT [dbo].[Role] ([IdRole], [Name]) VALUES (2, N'Student')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[StudyProgram] ON 

INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (1, N'Programsko Inzenjerstvo', CAST(5000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (2, N'Sistemsko Inzenjerstvo', CAST(5000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (3, N'Marketing', CAST(4000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (4, N'Dizajn', CAST(4000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (5, N'Poslovna Ekonomija', CAST(4000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (6, N'Programsko Inzenjerstvo', CAST(5000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (7, N'Sistemsko Inzenjerstvo', CAST(5000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (8, N'Marketing', CAST(4000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (9, N'Dizajn', CAST(4000 AS Decimal(18, 0)))
INSERT [dbo].[StudyProgram] ([IdStudyProgram], [Name], [Amount]) VALUES (10, N'Poslovna Ekonomija', CAST(4000 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[StudyProgram] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (1, 5, 1, CAST(500 AS Decimal(18, 0)), CAST(N'2025-05-19T21:13:29.670' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (2, 5, 1, CAST(500 AS Decimal(18, 0)), CAST(N'2025-05-19T21:13:29.670' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (3, 5, 1, CAST(500 AS Decimal(18, 0)), CAST(N'2025-05-19T21:13:29.670' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (4, 5, 1, CAST(500 AS Decimal(18, 0)), CAST(N'2025-05-19T21:13:29.670' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (5, 5, 3, CAST(5000 AS Decimal(18, 0)), CAST(N'2025-05-19T21:20:17.513' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (6, 5, 2, CAST(5 AS Decimal(18, 0)), CAST(N'2025-05-19T21:24:07.363' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (1002, 5, 2, CAST(8 AS Decimal(18, 0)), CAST(N'2025-05-23T15:04:15.177' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (1003, 5, 2, CAST(13 AS Decimal(18, 0)), CAST(N'2025-05-23T15:05:23.550' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (1004, 5, 2, CAST(13 AS Decimal(18, 0)), CAST(N'2025-05-23T17:15:15.967' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (2002, 5, 4, CAST(250 AS Decimal(18, 0)), CAST(N'2025-05-23T19:23:15.420' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (2003, 5, 4, CAST(300 AS Decimal(18, 0)), CAST(N'2025-05-23T19:24:41.117' AS DateTime))
INSERT [dbo].[Transaction] ([IdTransaction], [UserId], [TransactionTypeId], [Amount], [Date]) VALUES (2004, 5, 5, CAST(15000 AS Decimal(18, 0)), CAST(N'2025-05-23T22:31:30.990' AS DateTime))
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
SET IDENTITY_INSERT [dbo].[TransactionType] ON 

INSERT [dbo].[TransactionType] ([IdTransactionType], [TypeName]) VALUES (1, N'Food')
INSERT [dbo].[TransactionType] ([IdTransactionType], [TypeName]) VALUES (2, N'Parking')
INSERT [dbo].[TransactionType] ([IdTransactionType], [TypeName]) VALUES (3, N'Scholarship')
INSERT [dbo].[TransactionType] ([IdTransactionType], [TypeName]) VALUES (4, N'Money Transfer')
INSERT [dbo].[TransactionType] ([IdTransactionType], [TypeName]) VALUES (5, N'Wallet Top Up')
SET IDENTITY_INSERT [dbo].[TransactionType] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([IdUser], [RoleId], [FirstName], [EmailAddress], [PhoneNumber], [StudyProgramId], [PasswordHash], [PasswordSalt], [Temp2FACodeExpires], [Temp2FACode], [ProfilePictureUrl], [LastName]) VALUES (5, 1, N'Dombač', N'dada@gmail.com', N'0991231234', 1, N'wHtvKBzJGeqbUzt7cEC5dRoaPcVjrBItgJpWwxI/vuU=', N'2PXOnVCrwyWeowferKHyRQ==', NULL, NULL, NULL, N'Dombić')
INSERT [dbo].[User] ([IdUser], [RoleId], [FirstName], [EmailAddress], [PhoneNumber], [StudyProgramId], [PasswordHash], [PasswordSalt], [Temp2FACodeExpires], [Temp2FACode], [ProfilePictureUrl], [LastName]) VALUES (1002, 1, N'Ivan', N'ivo@gmail.com', N'09121314567', 1, N'VCia/SHRVsLdWiF+XBePQFMFjIkil0zoG7CGaYWAZBQ=', N'lidKyg2Pt6x0cRv+2Z+Nfw==', NULL, NULL, NULL, N'Ivanić')
INSERT [dbo].[User] ([IdUser], [RoleId], [FirstName], [EmailAddress], [PhoneNumber], [StudyProgramId], [PasswordHash], [PasswordSalt], [Temp2FACodeExpires], [Temp2FACode], [ProfilePictureUrl], [LastName]) VALUES (1003, 1, N'Luka', N'luka@gmail.com', N'0956781234', 1, N'JVuenvwVu4nxGTwfG7QW8aKFGp+4IsKsXcnBe3BqscA=', N's0fWRVFaakKTbFCBeE9/iA==', NULL, NULL, NULL, N'Lukić')
INSERT [dbo].[User] ([IdUser], [RoleId], [FirstName], [EmailAddress], [PhoneNumber], [StudyProgramId], [PasswordHash], [PasswordSalt], [Temp2FACodeExpires], [Temp2FACode], [ProfilePictureUrl], [LastName]) VALUES (1004, 1, N'Gabi', N'gabi@gmail.com', N'0979781234', 1, N'ibaBoyfNAhnYbnx+nghZB5wV+1PxlSorjbO6XocOoAk=', N'3jtHS94DJHgUHFjr9YsHqw==', NULL, NULL, NULL, N'Gabić')
INSERT [dbo].[User] ([IdUser], [RoleId], [FirstName], [EmailAddress], [PhoneNumber], [StudyProgramId], [PasswordHash], [PasswordSalt], [Temp2FACodeExpires], [Temp2FACode], [ProfilePictureUrl], [LastName]) VALUES (1005, 1, N'Tomo', N'tomo@gmail.com', N'0951234123', 1, N'LVHTFeGNGYsWvEupzcBEQgO5j+3rZr9ibpb5VV1qflc=', N'nMMS72CEkSIIYkeIHPzgdw==', NULL, NULL, N'C:\Users\Korisnik\Documents\Faks\Algebra_2_G\PRA\PRA_projekt_službeno\PRA_project\PRA_project\ProfilePictureRepo\default_picture.jpg', N'Tomić')
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserCreditCard] ON 

INSERT [dbo].[UserCreditCard] ([IdUserCreditCard], [UserId], [CreditCardId]) VALUES (1, 5, 1)
SET IDENTITY_INSERT [dbo].[UserCreditCard] OFF
GO
ALTER TABLE [dbo].[BillingAccount]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[Friend]  WITH CHECK ADD FOREIGN KEY([FriendId])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[Friend]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[MoneyTransfer]  WITH CHECK ADD FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([IdTransaction])
GO
ALTER TABLE [dbo].[MoneyTransfer]  WITH CHECK ADD FOREIGN KEY([UserRecieverId])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[ParkingPayment]  WITH CHECK ADD FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([IdTransaction])
GO
ALTER TABLE [dbo].[RequestTransfer]  WITH CHECK ADD FOREIGN KEY([UserRecieverId])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[RequestTransfer]  WITH CHECK ADD FOREIGN KEY([UserSenderId])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([TransactionTypeId])
REFERENCES [dbo].[TransactionType] ([IdTransactionType])
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([IdUser])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([IdRole])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([StudyProgramId])
REFERENCES [dbo].[StudyProgram] ([IdStudyProgram])
GO
ALTER TABLE [dbo].[UserCreditCard]  WITH CHECK ADD FOREIGN KEY([CreditCardId])
REFERENCES [dbo].[CreditCard] ([IdCreditCard])
GO
ALTER TABLE [dbo].[UserCreditCard]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([IdUser])
GO
USE [master]
GO
ALTER DATABASE [PRA_database] SET  READ_WRITE 
GO
