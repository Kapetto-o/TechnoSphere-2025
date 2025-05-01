use master
go
create database TechnoSphere_2025
go

use TechnoSphere_2025
go

create table Users (
    UserID			int				identity(1,1)	primary key,
    Username		nvarchar(32)	not null unique,
    PasswordHash	varbinary(64)	not null,
    PasswordSalt	varbinary(16)	not null,
    Email			nvarchar(255)	not null unique,
    FirstName		nvarchar(65)	null,
    LastName		nvarchar(64)	null,
    Phone			nvarchar(20)	null,
    AddressLine		nvarchar(255)	null,
	AvatarPath		nvarchar(255)	null,
    Role			tinyint			not null default(0),  -- 0 = покупатель, 1 = админ
    IsActive		bit				not null default(1)   -- 1 = активен, 0 = заблокирован
);
go