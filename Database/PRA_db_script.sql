USE [master]
GO
/****** Object:  Database [PRA_database]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[BillingAccount]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[CreditCard]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[Friend]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[MoneyTransfer]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[ParkingPayment]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[RequestTransfer]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[StudyProgram]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[Transaction]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[TransactionType]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[User]    Script Date: 23/05/2025 22:35:43 ******/
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
/****** Object:  Table [dbo].[UserCreditCard]    Script Date: 23/05/2025 22:35:43 ******/
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
