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
go

-- Товары
create table Products (
    ProductID           int identity(1,1) primary key,
    SKU                 nvarchar(50)    not null unique,
    Name_Ru             nvarchar(200)   not null,
    Name_Eng            nvarchar(200)   not null,
    Description_Ru      nvarchar(max)   null,
    Description_Eng     nvarchar(max)   null,
    Price               decimal(12,2)   not null,
    InstallmentPrice    decimal(12,2)   null,
    PromoPrice          decimal(12,2)   null,
    StockQuantity       int             not null default 0,
    IsActive            bit             not null default 1,
    CreatedAt           datetime2       not null default sysutcdatetime(),
    UpdatedAt           datetime2       not null default sysutcdatetime(),
    MainImagePath       nvarchar(500)   null,
    MainImageAlt_Ru     nvarchar(200)   null,
    MainImageAlt_Eng    nvarchar(200)   null,
    CategoryID          int             not null,
    constraint FK_Products_Categories foreign key (CategoryID) references Categories(CategoryID)
);
go

-- Избранное
create table Favorites (
    UserID				int				not null
       constraint FK_Fav_User foreign key references Users(UserID) on delete cascade,
    ProductID			int				not null
       constraint FK_Fav_Product foreign key references Products(ProductID) on delete cascade,
    AddedAt				datetime2		not null default sysutcdatetime(),
    constraint PK_Favorites primary key(UserID, ProductID)
);
go

-- Характеристики
create table SpecificationTypes (
    SpecTypeID			int identity(1,1) primary key,
    CategoryID			int				not null
        constraint FK_SpecType_Category references Categories(CategoryID),
    Name_Ru				nvarchar(100)	not null,
    Name_Eng			nvarchar(100)	not null,
    SortOrder			int				not null default 0,
    IsMain				bit				not null default 0  -- 0 = скрыт , 1 = показан - в карточке товара
);
create index IX_SpecType_Category on SpecificationTypes(CategoryID);
go

-- Значения характеристик
create table ProductSpecifications (
    ProductID			int				not null
        constraint FK_ProdSpec_Product references Products(ProductID) on delete cascade,
    SpecTypeID			int				not null
        constraint FK_ProdSpec_SpecType references SpecificationTypes(SpecTypeID) on delete cascade,
    Value_Ru			nvarchar(200)	not null,
    Value_Eng			nvarchar(200)	not null,
    constraint PK_ProductSpecifications primary key(ProductID, SpecTypeID)
);
create index IX_ProdSpec_Product on ProductSpecifications(ProductID);
go

-- Корзина
create table BasketItems (
    UserID				int			    not null
      constraint FK_BasketItems_User references Users(UserID) on delete cascade,
    ProductID			int			    not null
      constraint FK_BasketItems_Product references Products(ProductID) on delete cascade,
    Quantity			int				not null constraint CK_BasketItems_Quantity check(Quantity > 0),
    AddedAt				datetime2		not null default sysutcdatetime(),
    constraint PK_BasketItems primary key(UserID, ProductID)
);
create index IX_BasketItems_User on BasketItems(UserID);
create index IX_BasketItems_Product on BasketItems(ProductID);
go

-- Таблица сравнений
create table Comparisons (
    UserID    int not null
       constraint FK_Comparisons_User foreign key references Users(UserID) on delete cascade,
    ProductID int not null
       constraint FK_Comparisons_Product foreign key references Products(ProductID) on delete cascade,
    AddedAt   datetime2 not null default sysutcdatetime(),
    constraint PK_Comparisons primary key(UserID, ProductID)
);
create index IX_Comparisons_User    on Comparisons(UserID);
create index IX_Comparisons_Product on Comparisons(ProductID);
go

-- Статусы заказов
create table OrderStatuses (
    StatusID    tinyint         primary key,
    Name_Ru     nvarchar(50)    not null,
    Name_Eng    nvarchar(50)    not null
);
go

-- Заказы
create table Orders (
    OrderID     int             identity(1,1) primary key,
    UserID      int             not null
        constraint FK_Orders_User references Users(UserID) on delete cascade,
    StatusID    tinyint         not null
        constraint FK_Orders_Status references OrderStatuses(StatusID),
    CreatedAt   datetime2       not null default sysutcdatetime(),
    Delivery    nvarchar(500)   null, 
    ContactName nvarchar(100)   not null,
    ContactPhone nvarchar(20)   not null,
	TotalAmount decimal(12,2)   not null constraint DF_Orders_TotalAmount default (0)
);
go

-- Позиции заказа
create table OrderItems (
    OrderID     int     not null
        constraint FK_OrderItems_Order references Orders(OrderID) on delete cascade,
    ProductID   int     not null
        constraint FK_OrderItems_Product references Products(ProductID),
    Quantity    int     not null check (Quantity > 0),
    UnitPrice   decimal(12,2) not null,
    constraint PK_OrderItems primary key(OrderID, ProductID)
);