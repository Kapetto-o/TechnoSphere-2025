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

-- Пользователи
insert into Users (Username, PasswordHash, PasswordSalt, Email, AvatarPath, Role, IsActive)
values ('admin', 0x7C69C71AADDDD7AC9FE7565E5682B65B495249303B9B8BE3C5FFB5B4F7BA04D1, 0xF1173165DA1CE4129E9FB764B5C03E5A, 'admin@gmail.com', null, 1, 1),
	   ('user', 0x2C204F0C6E770A271E729F59354F27A29160BC68EA385DE51CB8C966E22BA566, 0x9435F34D5998896711C5AB15740C6C29, 'user@gmail.com', null, 0, 1)
go

-- Категории
insert into Categories (Name_Ru, Name_Eng, ParentCategoryID, SortOrder, IsActive)
values
  -- Корневые категории (станут ID = 1, 2, 3, 4)
  ('Холодильники',       'Refrigerators',       NULL, 1, 1),
  ('Стиральные машины',  'Washing Machines',    NULL, 2, 1),
  ('Пылесосы',           'Vacuum Cleaners',     NULL, 3, 1),
  ('Кухонные плиты',     'Kitchen stoves',		NULL, 4, 1),

  -- Подкатегории для 'Холодильники' (ParentCategoryID = 1)
  ('Однокамерные',       'Single-chamber',      1,    1, 1),
  ('С морозильной камерой',       'Double-chamber',      1,    2, 1),

  -- Подкатегории для 'Стиральные машины' (ParentCategoryID = 2)
  ('Фронтальная загрузка','Front-loading',       2,    1, 1),
  ('Вертикальная загрузка','Top-loading',        2,    2, 1),

  -- Подкатегории для 'Пылесосы' (ParentCategoryID = 3)
  ('С мешком для пыли',    'Handheld vacuums',    3,    1, 1),
  ('С аквафильтром',    'Robot vacuums',       3,    2, 1),

    -- Подкатегории для 'Кухонные плиты' (ParentCategoryID = 4)
  ('Электрические плиты',    'Electric stoves',    4,    1, 1);
go

-- Подкатегории
insert into products
    (sku, name_ru, name_eng, description_ru, description_eng,
     price, installmentprice, promoprice, stockquantity, isactive,
     mainimagepath, mainimagealt_ru, mainimagealt_eng, categoryid)
values
-- подкатегория 5: однокамерные
('11111111', 'Liebherr SRbde 5220', 'Liebherr SRbde 5220',
 'Описание холодильника однокамерного 1', 'Description of single-chamber fridge 1',
 5310, 193, null, 10, 1,	
 '/images/products/Liebherr_SRbde_5220.jpg', 'Однокам. холодильник 1', 'Single-chamber fridge 1', 5),

('11111112', 'Liebherr SRsfd 5220 Plus 5220-22001', 'Liebherr SRsfd 5220 Plus 5220-22001',
 'Описание холодильника однокамерного 2', 'Description of single-chamber fridge 2',
 3851, 140, 1540, 5, 1,
 '/images/products/Liebherr_SRsfd_5220_Plus_5220-22001.jpg', 'Однокам. холодильник 2', 'Single-chamber fridge 2', 5),
	
('11111113', 'Liebherr SRd 5220 Plus 5220-22001', 'Liebherr SRd 5220 Plus 5220-22001',
 'Описание холодильника однокамерного 3', 'Description of single-chamber fridge 3',
 2938, null, null, 0, 1,
 '/images/products/Liebherr_SRd_5220_Plus_5220-22001.jpg', 'Однокам. холодильник 3', 'Single-chamber fridge 3', 5),

-- подкатегория 6: двухкамерные
('11111114', ' Liebherr CNsff 5703 Pure NoFrost 5703', 'Liebherr CNsff 5703 Pure NoFrost 5703',
 'Описание холодильника двухкамерного 1', 'Description of double-chamber fridge 1',
 3199, 116, 1215, 8, 1,
 '/images/products/Liebherr_CNsff_5703_Pure_NoFrost_5703.jpg', 'Двухкам. холодильник 1', 'Double-chamber fridge 1', 6),

('11111115', 'Liebherr CBNbdc 573i Plus BioFresh NoFrost 573i', 'Liebherr CBNbdc 573i Plus BioFresh NoFrost 573i',
 'Описание холодильника двухкамерного 2', 'Description of double-chamber fridge 2',
 5999, null, null, 12, 1,
 '/images/products/Liebherr_CBNbdc_573i_Plus_BioFresh_NoFrost_573i.jpg', 'Двухкам. холодильник 2', 'Double-chamber fridge 2', 6),

('11111116', 'Liebherr CNgwc 5723 Plus NoFrost 5723', 'Liebherr CNgwc 5723 Plus NoFrost 5723',
 'Описание холодильника двухкамерного 3', 'Description of double-chamber fridge 3',
 4856, 176, null, 3, 1,
 '/images/products/Liebherr_CNgwc_5723_Plus_NoFrost_5723.jpg', 'Двухкам. холодильник 3', 'Double-chamber fridge 3', 6),

-- подкатегория 7: фронтальная загрузка
('11111117', 'Bosch WGG0420GPL', 'Bosch WGG0420GPL',
 'Описание фронтальной стиралки 1', 'Description of front-load washer 1',
 2420, 88, null, 7, 1,
 '/images/products/Bosch_WGG0420GPL.jpg', 'Фронт. стиралка 1', 'Front-load washer 1', 7),

('11111118', 'Electrolux EW8FN148BP', 'Electrolux EW8FN148BP',
 'Описание фронтальной стиралки 2', 'Description of front-load washer 2',
 3020, null, 2710, 4, 1,
 '/images/products/Electrolux_EW8FN148BP.jpg', 'Фронт. стиралка 2', 'Front-load washer 2', 7),

('11111119', 'Bosch WGG242ZGPL', 'Bosch WGG242ZGPL',
 'Описание фронтальной стиралки 3', 'Description of front-load washer 3',
 2780, 101, null, 0, 1,
 '/images/products/Bosch_WGG242ZGPL.jpg', 'Фронт. стиралка 3', 'Front-load washer 3', 7),

-- подкатегория 8: вертикальная загрузка
('11111121', 'Whirlpool TDLR 6240L EU/N', 'Whirlpool TDLR 6240L EU/N',
 'Описание вертикальной стиралки 1', 'Description of top-load washer 1',
 1406, null, null, 6, 1,
 '/images/products/Whirlpool_TDLR_6240L_EU-N.jpg', 'Верхн. стиралка 1', 'Top-load washer 1', 8),

('11111122', 'Whirlpool TDLR 7231BS EU', 'Whirlpool TDLR 7231BS EU',
 'Описание вертикальной стиралки 2', 'Description of top-load washer 2',
 1750, 64, 1400, 2, 1,
 '/images/products/Whirlpool_TDLR_7231BS_EU.jpg', 'Верхн. стиралка 2', 'Top-load washer 2', 8),

('11111123', 'Weissgauff WM 40275 TD', 'Weissgauff WM 40275 TD',
 'Описание вертикальной стиралки 3', 'Description of top-load washer 3',
 21000.00, null, null, 9, 1,
 '/images/products/Weissgauff_WM_40275_TD.jpg', 'Верхн. стиралка 3', 'Top-load washer 3', 8),

-- подкатегория 9: с мешком для пыли
('11111124', 'Bosch BGLS4POW2', 'Bosch BGLS4POW2',
 'Описание ручного пылесоса 1', 'Description of handheld vacuum 1',
 695, 26, null, 15, 1,
 '/images/products/Bosch_BGLS4POW2.jpg', 'Ручн. пылесос 1', 'Handheld vacuum 1', 9),

('11111125', 'Bosch BGL38BA2H', 'Bosch BGL38BA2H',
 'Описание ручного пылесоса 2', 'Description of handheld vacuum 2',
 593, null, 474, 11, 1,
 '/images/products/Bosch_BGL38BA2H.jpg', 'Ручн. пылесос 2', 'Handheld vacuum 2', 9),

('11111126', 'Bosch BGLS4PET2', 'Bosch BGLS4PET2',
 'Описание ручного пылесоса 3', 'Description of handheld vacuum 3',
 617, 23, null, 0, 1,
 '/images/products/Bosch_BGLS4PET2.jpg', 'Ручн. пылесос 3', 'Handheld vacuum 3', 9),

-- подкатегория 10: с аквафильтром
('11111127', 'Bosch BWD421PRO', 'Bosch BWD421PRO',
 'Описание робота-пылесоса 1', 'Description of robot vacuum 1',
 1025, 38, 512, 5, 1,
 '/images/products/Bosch_BWD421PRO.jpg', 'Робот-пылесос 1', 'Robot vacuum 1', 10),

('11111128', 'Bosch BWD41740', 'Bosch BWD41740',
 'Описание робота-пылесоса 2', 'Description of robot vacuum 2',
 1193, null, null, 3, 1,
 '/images/products/Bosch_BWD41740.jpg', 'Робот-пылесос 2', 'Robot vacuum 2', 10),

('11111129', 'Bosch BWD421POW', 'Bosch BWD421POW	',
 'Описание робота-пылесоса 3', 'Description of robot vacuum 3',
 1051, 39, null, 0, 1,
 '/images/products/Bosch_BWD421POW.jpg', 'Робот-пылесос 3', 'Robot vacuum 3', 10),

 -- подкатегория 11: электрические
('11111130', 'Hansa FCCW58208', 'Hansa FCCW58208',
 'Описание робота-пылесоса 1', 'Description of robot vacuum 1',
 1230, 45, null, 5, 1,
 '/images/products/Hansa_FCCW58208.jpg', 'Робот-пылесос 1', 'Robot vacuum 1', 11),

('11111131', 'Hansa FCCW56069', 'Hansa FCCW56069',
 'Описание робота-пылесоса 2', 'Description of robot vacuum 2',
 1366, 50, null, 3, 1,
 '/images/products/Hansa_FCCW56069.jpg', 'Робот-пылесос 2', 'Robot vacuum 2', 11),

('11111132', 'Hansa FCCW54000', 'Hansa FCCW54000',
 'Описание робота-пылесоса 3', 'Description of robot vacuum 3',
 999, 37, null, 0, 1,
 '/images/products/Hansa_FCCW54000.jpg', 'Робот-пылесос 3', 'Robot vacuum 3', 11);
go

-- Характеристики
-- Для пылесосов с мешком для пыли
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (9, 'Объём пылесборника',      'Dustbin Volume',      1, 1),
 (9, 'Тип уборки',              'Cleaning Type',       2, 1),
 (9, 'Мощность, Вт',            'Power, W',            3, 1),
 (9, 'Уровень шума, дБ',        'Noise Level, dB',     4, 1),
 (9, 'Длина шнура, м',          'Cord Length, m',      5, 1),
 (9, 'Вес, кг',                 'Weight, kg',          6, 0),
 (9, 'Высота, см',              'Height, cm',          7, 0),
 (9, 'Ширина, см',              'Width, cm',           8, 0),
 (9, 'Глубина, см',             'Depth, cm',           9, 0),
 (9, 'Тип фильтра',             'Filter Type',        10, 0);
go

-- Значения характеристик
-- Для товара ProductID = 13 (первый ручной пылесос):
insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (13,  1, '0.5 Л',  '0.5 L'),
 (13,  2, 'сухая',  'dry'),
 (13,  3, '700',    '700'),
 (13,  4, '75',     '75'),
 (13,  5, '5',      '5'),
 (13,  6, '2.5',    '2.5'),
 (13,  7, '25',     '25'),
 (13,  8, '15',     '15'),
 (13,  9, '10',     '10'),
 (13, 10, 'HEPA',   'HEPA');
 go