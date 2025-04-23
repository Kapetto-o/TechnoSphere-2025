use master
go
create database TechnoSphere_2025
go

use TechnoSphere_2025
go

create Table Users (
    UserID         int            identity(1,1) primary key,
    Username       nvarchar(50)   not null unique,
    PasswordHash   varbinary(256) not null,
    Email          nvarchar(100)  not null unique,
    FirstName      nvarchar(50),
	LastName       nvarchar(50),
    Phone          nvarchar(20),
    Role           nvarchar(20)   not null default 'Ñustomer',
    IsActive       bit            not null default 1,
    AddressLine    nvarchar(200)
);
go