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

select * from UserCreditCard

delete UserCreditCard
where IDUserCreditCard > 2

