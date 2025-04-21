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

insert into [User] values ('Tanja', 'Tanjuric', 'tanjica1996', '123456', null, 'tanjica.tanjuric@algebra.hr', '099-123-4556')
insert into [User] values ('Luka', 'Baric', 'bara97', 'kakosi123', null, 'luka.baric@algebra.hr', '097-777-4556')
insert into [User] values ('Kristijan', 'Golubovic', 'kiki-metak', 'karbonara', null, 'kristijan.golubovic@algebra.hr', '091-123-9855')

select * from [User]

delete [user]
where lastname = 'lucic'