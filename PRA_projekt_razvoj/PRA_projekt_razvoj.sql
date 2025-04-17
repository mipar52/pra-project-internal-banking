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

CREATE TABLE CanteenPayment (
    IdCanteenPayment INT PRIMARY KEY IDENTITY,
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

CREATE TABLE Role (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE UserAccountRole (
    UserAccountId INT NOT NULL FOREIGN KEY REFERENCES UserAccount(Id),
    RoleId INT NOT NULL FOREIGN KEY REFERENCES Role(Id),
    PRIMARY KEY (UserAccountId, RoleId)
);



insert into StudyProgram values ('Programsko inženjerstvo', 'Prijediplomski studij', 'Sveuèilište Algebra Bernays', 5000)
insert into StudyProgram values ('Sistemsko inženjerstvo', 'Prijediplomski studij', 'Sveuèilište Algebra Bernays', 5000)

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

insert into BillingAccount values (8, 50);


select * from ParkingPayment
select * from [User]
select * from CarRegistration
select * from UserCarRegistration
select * from BillingAccount

select * FROM ScholarshipPayment

update BillingAccount
set Balance = 10000
where IdBillingAcount = 1