use PRA_db

create table [User] (

	IDUser int primary key identity(1,1),
	FirstName nvarchar(256) not null,
	LastName nvarchar(256) not null,
	UserName nvarchar(256) unique check (len(UserName) > 5) not null,
	UserPassword nvarchar(256) check (len(UserPassword) > 5) not null,
	TestPassword nvarchar(256) null,
	Email nvarchar(256) unique not null,
	Phone nvarchar(256) unique not null
)

create table CreditCard (
	IDCreditCard int primary key identity(1,1),
	FirstName nvarchar(256) not null,
	LastName nvarchar(256) not null,
	CardNumber nvarchar(16) not null unique check (len(CardNumber)=16),
	ExpiryDate datetime not null,
	CvvHash nvarchar(256) not null,
	CvvSalt nvarchar(256) not null,
)

create table CreditCardDataBase (
	IDCreditCardDataBase int primary key identity(1,1),
	FirstName nvarchar(256) not null,
	LastName nvarchar(256) not null,
	CardNumber nvarchar(16) not null unique check (len(CardNumber)=16),
	ExpiryDate datetime not null,
	CvvHash nvarchar(256) not null,
	CvvSalt nvarchar(256) not null,
)

create table UserCreditCard (
	IDUserCreditCard int primary key identity(1,1),
	UserID int foreign key (UserID) references [User](IDUser) not null,
	CreditCardID int foreign key (CreditCardID) references CreditCard(IDCreditCard) not null
)

create table CarRegistration (
	IDCarRegistration int primary key identity(1,1),
	CarBrand nvarchar(256),
	CarModel nvarchar(256),
	RegistrationCountry nvarchar(256) not null,
	RegistrationNumber nvarchar(256) not null,
)

create table UserCarRegistration (
	IDUserCarRegistration int primary key identity(1,1),
	UserID int foreign key(UserID) references [User](IDUser) not null,
	CarRegistrationID int foreign key(CarRegistrationID) references CarRegistration(IDCarRegistration) not null
)

CREATE TABLE ParkingPayment (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES [User](IdUser),
	RegistrationCountryCode nvarchar(256) not null,
    RegistrationNumber nvarchar(256) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    DurationHours INT NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime AS DATEADD(HOUR, DurationHours, StartTime) PERSISTED,
    PaymentDate AS StartTime PERSISTED
)

CREATE TABLE BillingAccount (
    IdBillingAcount INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES [User](IdUser),
    Balance DECIMAL(10, 2) NOT NULL DEFAULT 0
);

CREATE TABLE FoodPayment (
    IdFoodPayment INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES [User](IdUser),
    Amount DECIMAL(10,2) NOT NULL,
    PaymentDate DATETIME NOT NULL
);

CREATE TABLE StudyProgram (
    IdStudyProgram INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(100) NOT NULL,
    DegreeLevel NVARCHAR(50), --preddipl, dipl, doktorat
    Institution NVARCHAR(100),
    FullScholarshipPrice DECIMAL(10, 2) NOT NULL --bez PDV-a
);

CREATE TABLE ScholarshipPayment (
    IdScholarshipPayment INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL FOREIGN KEY REFERENCES [User](IdUser),
    StudyProgramId INT NOT NULL FOREIGN KEY REFERENCES StudyProgram(IdStudyProgram),
    PaymentPlan NVARCHAR(20),
    InstallmentNumber INT,
    TotalInstallments INT,
    Amount DECIMAL(10,2) not null,
    PaymentDate DATETIME NOT NULL
);

CREATE TABLE [Role] (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE UserRole (
	IDUserRole int identity(1,1) PRIMARY KEY (UserId, RoleId),
    UserId INT NOT NULL FOREIGN KEY REFERENCES [User](IdUser),
    RoleId INT NOT NULL FOREIGN KEY REFERENCES Role(Id)
    
);

CREATE TABLE TransactionType (
    IdTransactionType INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

create table SendRequestMoney (
	IdSendRequestMoney int primary key identity(1,1),
	UserSenderId int not null foreign key references [User](IdUser),
	UserRecieverId int not null foreign key references [User](IdUser),
	TransactionTypeId int not null foreign key references TransactionType(IDTransactionType),
	Amount decimal(10,2) not null,
	PaymentDate DATETIME NOT NULL
)

create table UserFriends (
	IdUserFriends int primary key identity(1,1),
	UserId int foreign key references [User](IdUser) not null,
	FriendId int foreign key references [User](IdUser) not null,
)



create table UserAllTransactions (
	IDUserAllTransactions int primary key identity(1,1),
	UserId int not null foreign key references [User](IdUser),
	TransactionTypeId int not null,
	TransactionId int not null,
	TransactionDate datetime not null,
	Amount decimal(10,2) not null
)

INSERT INTO TransactionType (Name) VALUES
('Deposit'),
('Scholarship Payment'),
('Parking Payment'),
('Bills'),
('Food/Drinks Payment'),
('Send/Request Money'),
('Other')

update TransactionType
set name = 'Bills'
where IdTransactionType = 4

select * from TransactionType

select * from [User]


insert into [role] values ('Student')
insert into [role] values ('Osoblje')

select * from [role]

insert into userrole values (8, 1)

select * from userrole

insert into StudyProgram values ('Programsko in�enjerstvo', 'Prijediplomski studij', 'Sveu�ili�te Algebra Bernays', 5000)
insert into StudyProgram values ('Sistemsko in�enjerstvo', 'Prijediplomski studij', 'Sveu�ili�te Algebra Bernays', 5000)

select * from StudyProgram

SELECT * FROM [USER]
SELECT * FROM UserCarRegistration
SELECT * FROM CarRegistration

SELECT * FROM [USER]
SELECT * FROM UserCreditCard
SELECT * FROM CreditCard
select * from CreditCardDataBase

ALTER TABLE USERCARREGISTRATION
add constraint  UQ_User_CarRegistration UNIQUE (UserID, CarRegistrationID)



delete UserCarRegistration
where IDUserCarRegistration = 11

delete [user]
where iduser = 1019

insert into BillingAccount values (7, 10000);

update BillingAccount
set Balance = 10000
where UserId = 8

select * from CreditCardDataBase
select * from BillingAccount

select * from ParkingPayment
select * from [User]
select * from CarRegistration
select * from UserCarRegistration
select * from BillingAccount

select * from BillingAccount
select * from SendRequestMoney
select * from FoodPayment
select * from UserAllTransactions
SELECT * FROM ParkingPayment

select * from [user]
select * from UserFriends

alter table [user]
add RoleId int not null foreign key references [Role](IdRole)
