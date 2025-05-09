use master
go
create database TechnoSphere_2025
go

use TechnoSphere_2025
go

-- Пользователи
create table Users (
    UserID			int					identity(1,1)	primary key,
    Username		nvarchar(32)		not null unique,
    PasswordHash	varbinary(64)		not null,
    PasswordSalt	varbinary(16)		not null,
    Email			nvarchar(255)		not null unique,
    FirstName		nvarchar(65)		null,
    LastName		nvarchar(64)		null,
    Phone			nvarchar(20)		null,
    AddressLine		nvarchar(255)		null,
	AvatarPath		nvarchar(255)		null,
    Role			tinyint			not null default(0),  -- 0 = покупатель, 1 = админ
    IsActive		bit				not null default(1),  -- 1 = активен, 0 = заблокирован
	RememberToken	uniqueidentifier	null
);
go

-- Категории
create table Categories (
    CategoryID         int identity(1,1) constraint PK_Category primary key,
    Name_Ru            nvarchar(100) not null,
    Name_Eng           nvarchar(100) not null,
    ParentCategoryID   int null,
    SortOrder          int not null constraint DF_Category_SortOrder default(0),
    IsActive           bit not null constraint DF_Category_IsActive default(1),
    constraint FK_Category_Parent
        foreign key (ParentCategoryID)
        references Categories(CategoryID)
        on delete no action
        on update no action
);

create nonclustered index IX_Category_Name_Ru  on Categories(Name_Ru);
create nonclustered index IX_Category_Name_Eng on Categories(Name_Eng);
create nonclustered index IX_Category_Parent   on Categories(ParentCategoryID);