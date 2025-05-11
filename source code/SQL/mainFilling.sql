use TechnoSphere_2025
go

insert into Users (Username, PasswordHash, PasswordSalt, Email, AvatarPath, Role, IsActive)
values ('admin', 0x7C69C71AADDDD7AC9FE7565E5682B65B495249303B9B8BE3C5FFB5B4F7BA04D1, 0xF1173165DA1CE4129E9FB764B5C03E5A, 'admin@gmail.com', null, 1, 1),
	   ('user', 0x2C204F0C6E770A271E729F59354F27A29160BC68EA385DE51CB8C966E22BA566, 0x9435F34D5998896711C5AB15740C6C29, 'user@gmail.com', null, 0, 1)
go

insert into Categories (Name_Ru, Name_Eng, ParentCategoryID, SortOrder, IsActive)
values
  -- Корневые категории (станут ID = 1, 2, 3)
  ('Холодильники',       'Refrigerators',       NULL, 1, 1),
  ('Стиральные машины',  'Washing Machines',    NULL, 2, 1),
  ('Пылесосы',           'Vacuum Cleaners',     NULL, 3, 1),

  -- Подкатегории для 'Холодильники' (ParentCategoryID = 1)
  ('Однокамерные',       'Single-chamber',      1,    1, 1),
  ('Двухкамерные',       'Double-chamber',      1,    2, 1),

  -- Подкатегории для 'Стиральные машины' (ParentCategoryID = 2)
  ('Фронтальная загрузка','Front-loading',       2,    1, 1),
  ('Вертикальная загрузка','Top-loading',        2,    2, 1),

  -- Подкатегории для 'Пылесосы' (ParentCategoryID = 3)
  ('Ручные пылесосы',    'Handheld vacuums',    3,    1, 1),
  ('Роботы-пылесосы',    'Robot vacuums',       3,    2, 1);
go