use TechnoSphere_2025
go

delete from SpecificationTypes;
delete from ProductSpecifications;
delete from Products;
delete from Categories;
delete from Users;
go

dbcc checkident ('Products',   reseed, 0);
dbcc checkident ('Categories', reseed, 0);
dbcc checkident ('Users',      reseed, 0);
dbcc checkident ('SpecificationTypes',      reseed, 0);
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
-- подкатегория 5: Однокамерные холодильники
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

-- подкатегория 6: Двухкамерные холодильники
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

-- подкатегория 7: Стиральные машины с фронтальная загрузкой
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

-- подкатегория 8: Стиральные машины с вертикальной загрузкой
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

-- подкатегория 9: Пылесосы с мешком для пыли
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

-- подкатегория 10: Пылесосы с аквафильтром
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

 -- подкатегория 11: Электрические плиты
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
-- Однокамерные холодильники 
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (5, 'Тип компрессора:',				'Compressor type:',							1, 1),
 (5, 'Система No Frost:',				'No Frost system:',							2, 1),
 (5, 'Зона сохранения свежести:',		'Freshness preservation zone:',				3, 1),
 (5, 'Страна происхождения:',			'Country of origin:',						4, 1),
 (5, 'Габариты (В*Ш*Г):',	'Dimensions (H*W*D):',									5, 0),
 (5, 'Производитель:',					'Manufacturer:',							6, 1),
 (5, 'Тип установки:',					'Installation type:',						7, 0),
 (5, 'Цвет:',							'Color:',									8, 0),
 (5, 'Количество компрессоров:',        'Number of compressors:',				    9, 0),
 (5, 'Дисплей:',						'Display:',								    10, 0);
go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (1,  1, 'инверторный',	'inverter'),
 (1,  2, 'без No frost',	'without No frost'),
 (1,  3, 'да (EasyFresh)',	'yes (EasyFresh)'),
 (1,  4, 'Болгария',	    'Bulgaria'),
 (1,  5, '185.5х59.7х67.5',      '185.5х59.7х67.5'),
 (1,  6, 'Liebherr',    'Liebherr'),
 (1,  7, 'отдельностоящий',     'freestanding'),
 (1,  8, 'черный',     'black'),
 (1,  9, '1',     '1'),
 (1, 10, 'да',   'yes');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (2,  1, 'инверторный',	'inverter'),
 (2,  2, 'без No frost',	'without No frost'),
 (2,  3, 'да (EasyFresh)',	'yes (EasyFresh)'),
 (2,  4, 'Болгария',	    'Bulgaria'),
 (2,  5, '185.5х59.7х67.5',      '185.5х59.7х67.5'),
 (2,  6, 'Liebherr',    'Liebherr'),
 (2,  7, 'отдельностоящий',     'freestanding'),
 (2,  8, 'черный',     'black'),
 (2,  9, '1',     '1'),
 (2, 10, 'да',   'yes');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (3,  1, 'инверторный',	'inverter'),
 (3,  2, 'без No frost',	'without No frost'),
 (3,  3, 'да (EasyFresh)',	'yes (EasyFresh)'),
 (3,  4, 'Болгария',	    'Bulgaria'),
 (3,  5, '185.5х59.7х67.5',      '185.5х59.7х67.5'),
 (3,  6, 'Liebherr',    'Liebherr'),
 (3,  7, 'отдельностоящий',     'freestanding'),
 (3,  8, 'белый',     'White'),
 (3,  9, '1',     '1'),
 (3, 10, 'да',   'yes');
 go

 -- Двухкамерные холодильники
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (6, 'Тип компрессора:',				'Compressor type:',							1, 1),
 (6, 'Система No Frost:',				'No Frost system:',							2, 1),
 (6, 'Зона сохранения свежести:',		'Freshness preservation zone:',				3, 1),
 (6, 'Страна происхождения:',			'Country of origin:',						4, 1),
 (6, 'Габариты (В*Ш*Г):',	'Dimensions (H*W*D):',									5, 0),
 (6, 'Производитель:',					'Manufacturer:',							6, 1),
 (6, 'Тип установки:',					'Installation type:',						7, 0),
 (6, 'Цвет:',							'Color:',									8, 0),
 (6, 'Количество компрессоров:',        'Number of compressors:',				    9, 0),
 (6, 'Дисплей:',						'Display:',								    10, 0);
go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (4, 11, 'стандартный',	'inverter'),
 (4, 12, 'в морозильнике',	'in the freezer'),
 (4, 13, 'да (BioFresh)',	'yes (BioFresh)'),
 (4, 14, 'Болгария',	    'Bulgaria'),
 (4, 15, '201.5х59.7х67.5',      '201.5х59.7х67.5'),
 (4, 16, 'Liebherr',    'Liebherr'),
 (4, 17, 'отдельностоящий',     'freestanding'),
 (4, 18, 'серый',     'gray'),
 (4, 19, '1',     '1'),
 (4, 20, 'да',   'yes');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (5, 11, 'инверторный',	'inverter'),
 (5, 12, 'в морозильнике',	'in the freezer'),
 (5, 13, 'да (BioFresh)',	'yes (BioFresh)'),
 (5, 14, 'Болгария',	    'Bulgaria'),
 (5, 15, '201.5х59.7х67.5',      '201.5х59.7х67.5'),
 (5, 16, 'Liebherr',    'Liebherr'),
 (5, 17, 'отдельностоящий',     'freestanding'),
 (5, 18, 'чёрный',     'black'),
 (5, 19, '1',     '1'),
 (5, 20, 'да',   'yes');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (6, 11, 'инверторный',	'inverter'),
 (6, 12, 'в морозильнике',	'in the freezer'),
 (6, 13, 'да (EasyFresh)',	'yes (EasyFresh)'),
 (6, 14, 'Болгария',	    'Bulgaria'),
 (6, 15, '201.5х59.7х67.5',      '201.5х59.7х67.5'),
 (6, 16, 'Liebherr',    'Liebherr'),
 (6, 17, 'отдельностоящий',     'freestanding'),
 (6, 18, 'белый',     'white'),
 (6, 19, '1',     '1'),
 (6, 20, 'да',   'yes');
 go

-- Стиральные машины с фронтальной загрузкой
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (7, 'Максимальная загрузка, кг:',	'Maximum load, kg:',						1, 1),
 (7, 'Макс. скорость отжима:',		'Max. spin speed:',							2, 1),
 (7, 'Тип двигателя:',				'Engine type:',								3, 1),
 (7, 'Общее количество программ, шт:',		'Total number of programs, pcs:',	4, 1),
 (7, 'Страна происхождения:',		'Country of origin:',						5, 1),
 (7, 'Производитель:',				'Manufacturer:',							6, 0),
 (7, 'Тип установки:',				'Installation type:',						7, 0),
 (7, 'Цвет:',						'Color:',									8, 0),
 (7, 'Тип управления:',				'Type of management:',					    9, 0),
 (7, 'Материал барабана:',			'Drum Material:',						    10, 0);
go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (7, 21, '8',	'8'),
 (7, 22, '1400',	'1400'),
 (7, 23, 'инверторный',	'inverter'),
 (7, 24, '14',	    '14'),
 (7, 25, 'Италия',      'Italy'),
 (7, 26, 'Electrolux',    'Electrolux'),
 (7, 27, 'отдельностоящий',     'freestanding'),
 (7, 28, 'белый',     'white'),
 (7, 29, 'электронное',     'electronic'),
 (7, 30, 'нержавеющая сталь',   'stainless steel');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (8, 21, '9',	'9'),
 (8, 22, '1200',	'1200'),
 (8, 23, 'инверторный',	'inverter'),
 (8, 24, '14',	    '14'),
 (8, 25, 'Турция',      'Turkey'),
 (8, 26, 'Bosch',    'Bosch'),
 (8, 27, 'отдельностоящий',     'freestanding'),
 (8, 28, 'белый',     'white'),
 (8, 29, 'электронное',     'electronic'),
 (8, 30, 'нержавеющая сталь',   'stainless steel');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (9, 21, '9',	'9'),
 (9, 22, '1200',	'1200'),
 (9, 23, 'инверторный',	'inverter'),
 (9, 24, '14',	    '14'),
 (9, 25, 'Турция',      'Turkey'),
 (9, 26, 'Bosch',    'Bosch'),
 (9, 27, 'отдельностоящий',     'freestanding'),
 (9, 28, 'белый',     'white'),
 (9, 29, 'электронное',     'electronic'),
 (9, 30, 'нержавеющая сталь',   'stainless steel');
 go

 -- Стиральные машины с вертикальной загрузкой
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (8, 'Максимальная загрузка, кг:',	'Maximum load, kg:',						1, 1),
 (8, 'Макс. скорость отжима:',		'Max. spin speed:',							2, 1),
 (8, 'Расход воды за цикл, л:',		'Water consumption per cycle, l:',			3, 0),
 (8, 'Общее количество программ, шт:',	'Total number of programs, pcs:',		4, 1),
 (8, 'Страна происхождения:',		'Country of origin:',						5, 1),
 (8, 'Производитель:',				'Manufacturer:',							6, 1),
 (8, 'Тип установки:',				'Installation type:',						7, 0),
 (8, 'Цвет:',						'Color:',									8, 0),
 (8, 'Тип управления:',				'Type of management:',					    9, 0),
 (8, 'Материал барабана:',			'Drum Material:',						    10, 0);
go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (10, 31, '6',	'6'),
 (10, 32, '1200',	'1200'),
 (10, 33, '43',	'43'),
 (10, 34, '18',	    '18'),
 (10, 35, 'Словакия',      'Slovakia'),
 (10, 36, 'Whirlpool',    'Whirlpool'),
 (10, 37, 'отдельностоящий',     'freestanding'),
 (10, 38, 'белый',     'white'),
 (10, 39, 'электронное',     'electronic'),
 (10, 40, 'нержавеющая сталь',   'stainless steel');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (11, 31, '7',	'7'),
 (11, 32, '1200',	'1200'),
 (11, 33, '45',	'45'),
 (11, 34, '14',	    '14'),
 (11, 35, 'Словакия',      'Slovakia'),
 (11, 36, 'Whirlpool',    'Whirlpool'),
 (11, 37, 'отдельностоящий',     'freestanding'),
 (11, 38, 'белый',     'white'),
 (11, 39, 'электронное',     'electronic'),
 (11, 40, 'нержавеющая сталь',   'stainless steel');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (12, 31, '7.5',	'7.5'),
 (12, 32, '1200',	'1200'),
 (12, 33, '52',	'52'),
 (12, 34, '15',	    '15'),
 (12, 35, 'Китай',      'China'),
 (12, 36, 'Whirlpool',    'Whirlpool'),
 (12, 37, 'отдельностоящий',     'freestanding'),
 (12, 38, 'белый',     'white'),
 (12, 39, 'электронное',     'electronic'),
 (12, 40, 'нержавеющая сталь',   'stainless steel');
 go

-- Пылесосы с мешком для пыли
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (9, 'Объём пылесборника, л:',	'Volume of the dust collector, l:',	1, 1),
 (9, 'Тип уборки:',				'Type of cleaning:',				2, 1),
 (9, 'Страна происхождения:',	'Country of origin:',				3, 1),
 (9, 'Насадка турбощетка:',		'Turbo brush nozzle:',				4, 1),
 (9, 'Производитель:',			'Manufacturer:',					5, 1),
 (9, 'Количество уровней мощности:',	'Number of power levels:',	6, 0),
 (9, 'Тип управления:',			'Type of management:',				7, 0),
 (9, 'Управление мощностью:',	'Power management:',				8, 0),
 (9, 'Тип трубки:',				'Tube Type:',					    9, 0),
 (9, 'Технологии:',				'Technologies:',					10, 0);
go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (13, 41, 'L',	'L'),
 (13, 42, 'сухая',	'dry'),
 (13, 43, 'Германия',	'Germany'),
 (13, 44, 'да',	    'yes'),
 (13, 45, 'Bosch',      'Bosch'),
 (13, 46, '5',    '5'),
 (13, 47, 'электронный',     'electronic'),
 (13, 48, 'на корпусе',     'white'),
 (13, 49, 'телескопическая',     'telescopic'),
 (13, 50, 'PowerProtect',   'PowerProtect');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (14, 41, 'L',	'L'),
 (14, 42, 'сухая',	'dry'),
 (14, 43, 'Германия',	'Germany'),
 (14, 44, 'да',	    'yes'),
 (14, 45, 'Bosch',      'Bosch'),
 (14, 46, '5',    '5'),
 (14, 47, 'электронный',     'electronic'),
 (14, 48, 'на корпусе',     'white'),
 (14, 49, 'телескопическая',     'telescopic'),
 (14, 50, 'PowerProtect, PureAir',   'PowerProtect, PureAir');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (15, 41, 'L',	'L'),
 (15, 42, 'сухая',	'dry'),
 (15, 43, 'Германия',	'Germany'),
 (15, 44, 'да',	    'yes'),
 (15, 45, 'Bosch',      'Bosch'),
 (15, 46, '5',    '5'),
 (15, 47, 'электронный',     'electronic'),
 (15, 48, 'на корпусе',     'white'),
 (15, 49, 'телескопическая',     'telescopic'),
 (15, 50, 'AirFresh, AirTurbo Plus, PowerProtect',   'AirFresh, AirTurbo Plus, PowerProtect');
 go

 -- Пылесосы с аквафильтром
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (10, 'Тип уборки:',					'Type of cleaning:',				1, 1),
 (10, 'Потребляемая мощность, Вт:',	'Power consumption, W:',				2, 0),
 (10, 'Страна происхождения:',		'Country of origin:',					3, 1),
 (10, 'Насадка турбощетка:',			'Turbo brush nozzle:',				4, 1),
 (10, 'Объем резервуара для воды, л:',	'Volume of the water tank, l:',		5, 1),
 (10, 'Производитель:',				'Manufacturer:',						6, 1),
 (10, 'Количество уровней мощности:',	'Number of power levels:',			7, 0),
 (10, 'Тип управления:',				'Type of management:',				8, 0),
 (10, 'Управление мощностью:',		'Power management:',				    9, 0),
 (10, 'Тип трубки:',					'Tube Type:',						10, 0);
go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (16, 51, 'сухая, влажная',	'dry, wet'),
 (16, 52, '2100',	'2100'),
 (16, 53, 'Польша',	'Poland'),
 (16, 54, 'да',	    'yes'),
 (16, 55, '5',      '5'),
 (16, 56, 'Bosch',    'Bosch'),
 (16, 57, '5',     '5'),
 (16, 58, 'электронный',     'electronic'),
 (16, 59, 'на корпусе',     'on the case'),
 (16, 60, 'телескопическая',   'telescopic');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (17, 51, 'сухая, влажная',	'dry, wet'),
 (17, 52, '1700',	'1700'),
 (17, 53, 'Польша',	'Poland'),
 (17, 54, 'да',	    'yes'),
 (17, 55, '5',      '5'),
 (17, 56, 'Bosch',    'Bosch'),
 (17, 57, '5',     '5'),
 (17, 58, 'электронный',     'electronic'),
 (17, 59, 'на корпусе',     'on the case'),
 (17, 60, 'телескопическая',   'telescopic');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (18, 51, 'сухая, влажная',	'dry, wet'),
 (18, 52, '2100',	'2100'),
 (18, 53, 'Польша',	'Poland'),
 (18, 54, 'да',	    'yes'),
 (18, 55, '5',      '5'),
 (18, 56, 'Bosch',    'Bosch'),
 (18, 57, '5',     '5'),
 (18, 58, 'электронный',     'electronic'),
 (18, 59, 'на корпусе',     'on the case'),
 (18, 60, 'телескопическая',   'telescopic');
 go

-- Электрические плиты
insert into SpecificationTypes (CategoryID, Name_Ru, Name_Eng, SortOrder, IsMain)
values
 (11, 'Тип конфорок:',					'Type of burners:',					1, 0),
 (11, 'Тип духового шкафа:',			'Type of oven:',					2, 0),
 (11, 'Объем духового шкафа:',			'Oven volume:',						3, 1),
 (11, 'Материал поверхности:',			'Surface material:',				4, 1),
 (11, 'Количество режимов работы, шт:',	'Number of operating modes, pcs:',	5, 1),
 (11, 'Страна происхождения:',			'Country of origin:',				6, 1),
 (11, 'Производитель:',					'Manufacturer:',					7, 1),
 (11, 'Цвет фурнитуры:',				'Hardware color:',					8, 0),
 (11, 'Дисплей:',						'Display:',						    9, 0),
 (11, 'Тип управления:',				'Type of management:',				10, 0);
go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (19, 61, 'электрические',	'electric'),
 (19, 62, 'электрический',	'electric'),
 (19, 63, '65',	'65'),
 (19, 64, 'стеклокерамика',	    'glass ceramics'),
 (19, 65, '8',      '8'),
 (19, 66, 'Польша',    'Poland'),
 (19, 67, 'Hansa',     'Hansa'),
 (19, 68, 'белый',     'white'),
 (19, 69, 'да',     'yes'),
 (19, 70, 'поворотные переключатели',   'rotary switches');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (20, 61, 'электрические',	'electric'),
 (20, 62, 'электрический',	'electric'),
 (20, 63, '62',	'62'),
 (20, 64, 'стеклокерамика',	    'glass ceramics'),
 (20, 65, '6',      '6'),
 (20, 66, 'Польша',    'Poland'),
 (20, 67, 'Hansa',     'Hansa'),
 (20, 68, 'белый',     'white'),
 (20, 69, 'нет',     'нет'),
 (20, 70, 'поворотные переключатели',   'rotary switches');
 go

insert into ProductSpecifications (ProductID, SpecTypeID, Value_Ru, Value_Eng)
values
 (21, 61, 'электрические',	'electric'),
 (21, 62, 'электрический',	'electric'),
 (21, 63, '67',	'67'),
 (21, 64, 'стеклокерамика',	    'glass ceramics'),
 (21, 65, '4',      '4'),
 (21, 66, 'Польша',    'Poland'),
 (21, 67, 'Hansa',     'Hansa'),
 (21, 68, 'белый',     'white'),
 (21, 69, 'нет',     'нет'),
 (21, 70, 'поворотные переключатели',   'rotary switches');
 go