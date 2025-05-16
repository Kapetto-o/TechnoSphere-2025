use TechnoSphere_2025
go

delete from Products;
delete from Categories;
delete from Users;
go

dbcc checkident ('Products',   reseed, 0);
dbcc checkident ('Categories', reseed, 0);
dbcc checkident ('Users',      reseed, 0);
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

insert into products
    (sku, name_ru, name_eng, description_ru, description_eng,
     price, installmentprice, promoprice, stockquantity, isactive,
     mainimagepath, mainimagealt_ru, mainimagealt_eng, categoryid)
values
-- подкатегория 4: однокамерные
('frig-oc-001', 'Холодильник Однокамерный 1', 'Single-Chamber Fridge 1',
 'Описание холодильника однокамерного 1', 'Description of single-chamber fridge 1',
 25000.00, 2500.00, null, 10, 1,
 '/images/fridge_sc_1.jpg', 'Однокам. холодильник 1', 'Single-chamber fridge 1', 4),

('frig-oc-002', 'Холодильник Однокамерный 2', 'Single-Chamber Fridge 2',
 'Описание холодильника однокамерного 2', 'Description of single-chamber fridge 2',
 27000.00, 2700.00, 26000.00, 5, 1,
 '/images/fridge_sc_2.jpg', 'Однокам. холодильник 2', 'Single-chamber fridge 2', 4),

('frig-oc-003', 'Холодильник Однокамерный 3', 'Single-Chamber Fridge 3',
 'Описание холодильника однокамерного 3', 'Description of single-chamber fridge 3',
 23000.00, null, null, 0, 1,
 '/images/fridge_sc_3.jpg', 'Однокам. холодильник 3', 'Single-chamber fridge 3', 4),

-- подкатегория 5: двухкамерные
('frig-dc-001', 'Холодильник Двухкамерный 1', 'Double-Chamber Fridge 1',
 'Описание холодильника двухкамерного 1', 'Description of double-chamber fridge 1',
 35000.00, 3500.00, 33000.00, 8, 1,
 '/images/fridge_dc_1.jpg', 'Двухкам. холодильник 1', 'Double-chamber fridge 1', 5),

('frig-dc-002', 'Холодильник Двухкамерный 2', 'Double-Chamber Fridge 2',
 'Описание холодильника двухкамерного 2', 'Description of double-chamber fridge 2',
 37000.00, null, null, 12, 1,
 '/images/fridge_dc_2.jpg', 'Двухкам. холодильник 2', 'Double-chamber fridge 2', 5),

('frig-dc-003', 'Холодильник Двухкамерный 3', 'Double-Chamber Fridge 3',
 'Описание холодильника двухкамерного 3', 'Description of double-chamber fridge 3',
 39000.00, 3900.00, null, 3, 1,
 '/images/fridge_dc_3.jpg', 'Двухкам. холодильник 3', 'Double-chamber fridge 3', 5),

-- подкатегория 6: фронтальная загрузка
('wm-fl-001', 'Стиральная машина Фронт. 1', 'Front-Load Washer 1',
 'Описание фронтальной стиралки 1', 'Description of front-load washer 1',
 20000.00, 2000.00, null, 7, 1,
 '/images/washer_fl_1.jpg', 'Фронт. стиралка 1', 'Front-load washer 1', 6),

('wm-fl-002', 'Стиральная машина Фронт. 2', 'Front-Load Washer 2',
 'Описание фронтальной стиралки 2', 'Description of front-load washer 2',
 22000.00, null, 21000.00, 4, 1,
 '/images/washer_fl_2.jpg', 'Фронт. стиралка 2', 'Front-load washer 2', 6),

('wm-fl-003', 'Стиральная машина Фронт. 3', 'Front-Load Washer 3',
 'Описание фронтальной стиралки 3', 'Description of front-load washer 3',
 24000.00, 2400.00, null, 0, 1,
 '/images/washer_fl_3.jpg', 'Фронт. стиралка 3', 'Front-load washer 3', 6),

-- подкатегория 7: вертикальная загрузка
('wm-tl-001', 'Стиральная машина Верхн. 1', 'Top-Load Washer 1',
 'Описание вертикальной стиралки 1', 'Description of top-load washer 1',
 18000.00, null, null, 6, 1,
 '/images/washer_tl_1.jpg', 'Верхн. стиралка 1', 'Top-load washer 1', 7),

('wm-tl-002', 'Стиральная машина Верхн. 2', 'Top-Load Washer 2',
 'Описание вертикальной стиралки 2', 'Description of top-load washer 2',
 19000.00, 1900.00, 18000.00, 2, 1,
 '/images/washer_tl_2.jpg', 'Верхн. стиралка 2', 'Top-load washer 2', 7),

('wm-tl-003', 'Стиральная машина Верхн. 3', 'Top-Load Washer 3',
 'Описание вертикальной стиралки 3', 'Description of top-load washer 3',
 21000.00, null, null, 9, 1,
 '/images/washer_tl_3.jpg', 'Верхн. стиралка 3', 'Top-load washer 3', 7),

-- подкатегория 8: ручные пылесосы
('vac-hh-001', 'Ручной пылесос 1', 'Handheld Vacuum 1',
 'Описание ручного пылесоса 1', 'Description of handheld vacuum 1',
 5000.00, 500.00, null, 15, 1,
 '/images/vac_hh_1.jpg', 'Ручн. пылесос 1', 'Handheld vacuum 1', 8),

('vac-hh-002', 'Ручной пылесос 2', 'Handheld Vacuum 2',
 'Описание ручного пылесоса 2', 'Description of handheld vacuum 2',
 5500.00, null, 5300.00, 11, 1,
 '/images/vac_hh_2.jpg', 'Ручн. пылесос 2', 'Handheld vacuum 2', 8),

('vac-hh-003', 'Ручной пылесос 3', 'Handheld Vacuum 3',
 'Описание ручного пылесоса 3', 'Description of handheld vacuum 3',
 6000.00, 600.00, null, 0, 1,
 '/images/vac_hh_3.jpg', 'Ручн. пылесос 3', 'Handheld vacuum 3', 8),

-- подкатегория 9: роботы-пылесосы
('vac-rb-001', 'Робот-пылесос 1', 'Robot Vacuum 1',
 'Описание робота-пылесоса 1', 'Description of robot vacuum 1',
 20000.00, 2000.00, 18000.00, 5, 1,
 '/images/vac_rb_1.jpg', 'Робот-пылесос 1', 'Robot vacuum 1', 9),

('vac-rb-002', 'Робот-пылесос 2', 'Robot Vacuum 2',
 'Описание робота-пылесоса 2', 'Description of robot vacuum 2',
 22000.00, null, null, 3, 1,
 '/images/vac_rb_2.jpg', 'Робот-пылесос 2', 'Robot vacuum 2', 9),

('vac-rb-003', 'Робот-пылесос 3', 'Robot Vacuum 3',
 'Описание робота-пылесоса 3', 'Description of robot vacuum 3',
 25000.00, 2500.00, null, 0, 1,
 '/images/vac_rb_3.jpg', 'Робот-пылесос 3', 'Robot vacuum 3', 9);
go