use PRA_database

create table [Role]
(
	IdRole int primary key identity(1,1),
	[Name] nvarchar(256) not null
)

create table StudyProgram 
(
	IdStudyProgram int primary key identity(1,1),
	[Name] nvarchar(256) not null,
	Amount decimal not null
)


create table [user] (

	IdUser int primary key identity(1,1),
	RoleId int foreign key references [role](idrole),
	FirstName nvarchar(256) not null,
	Lastname nvarchar(256) not null,
	EmailAddress nvarchar(256) not null,
	PhoneNumber nvarchar(256) not null,
	StudyProgramId int foreign key references StudyProgram(idstudyprogram),
	PasswordHash nvarchar(256) not null,
	PasswordSalt nvarchar(256) not null,
	Temp2FACode nvarchar(256),
	Temp2FACodeExpires nvarchar(256),
)

alter table [user]
add LastName nvarchar(256) not null



insert into [user] values (1, 'Marko', 'Markiæ', 'mm@gmail.com', 


create table BillingAccount (

	IdBillingAccount int primary key identity(1,1),
	UserId int foreign key references [user](IdUser),
	Balance decimal not null,
)

create table CreditCard (

	IdCreditCard int primary key identity(1,1),
	FirstName nvarchar(256) not null,
	Lastname nvarchar(256) not null,
	CreditCardNumber nvarchar(16) not null,
	ExpiryDate datetime not null,
	CVVhash nvarchar(256) not null,
	CVVsalt nvarchar(256) not null
)

create table UserCreditCard
(
	IdUserCreditCard int primary key identity(1,1),
	UserId int foreign key references [user](Iduser),
	CreditCardId int foreign key references creditcard(idcreditcard)
)

create table TransactionType 
(
	IdTransactionType int primary key identity(1,1),
	TypeName nvarchar(256) not null
)

create table [Transaction]
(
	IdTransaction int primary key identity(1,1),
	UserId int foreign key references [user](iduser) not null,
	TransactionTypeId int foreign key references transactiontype(idtransactiontype) not null,
	Amount decimal not null,
	[Date] datetime not null,
	
)

create table ParkingPayment
(
	IdParkingPayment int primary key identity(1,1),
	TransactionId int foreign key references [transaction](idtransaction),
	RegistrationNumber nvarchar(256) not null,
	RegistrationCountryCode nvarchar(256) not null,
	DurationHours int not null,
	StartTime datetime not null,
	EndTime datetime not null
)

create table MoneyTransfer 
(
	IdMoneyTransfer int primary key identity(1,1),
	TransactionId int foreign key references [transaction](idtransaction),
	UserRecieverId int foreign key references [user](iduser), 
)

create table Friend
(
	IdFriend int primary key identity(1,1),
	UserId int foreign key references [user](iduser),
	FriendId int foreign key references [user](iduser)
)

insert into TransactionType values ('Food')
insert into TransactionType values ('Parking')
insert into TransactionType values ('Scholarship')
insert into TransactionType values ('Money Transfer')
insert into TransactionType values ('Wallet Top Up')

insert into [Role] values ('Staff')
insert into [Role] values ('Student')

select * from StudyProgram

insert into StudyProgram values ('Programsko Inzenjerstvo', 5000)
insert into StudyProgram values ('Sistemsko Inzenjerstvo', 5000)
insert into StudyProgram values ('Marketing', 4000)
insert into StudyProgram values ('Dizajn', 4000)
insert into StudyProgram values ('Poslovna Ekonomija', 4000)

delete BillingAccount
where userid < 5
delete [user]
where iduser < 5

select * from [user]

select * from BillingAccount
select * from TransactionType
select * from [Transaction]
select * from ParkingPayment
select * from MoneyTransfer
select * from RequestTransfer
select * from Friend
select * from CreditCard
select * from UserCreditCard

create table RequestTransfer 
(
	IdRequestTransfer int primary key identity(1,1),
	UserRecieverId int foreign key references [user](iduser) not null,
	UserSenderId int foreign key references [user](iduser) not null,
	Date datetime not null,
	Amount decimal not null
)


update BillingAccount
set Balance = 10000
where UserId = 5

delete friend
where IdFriend < 4
