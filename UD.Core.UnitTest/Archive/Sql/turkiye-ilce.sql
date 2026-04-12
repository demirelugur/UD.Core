/****** Object:  Table [dbo].[District] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[Id] [smallint] NOT NULL,
	[CityId] [tinyint] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[TelephoneCode] [smallint] NOT NULL,
 CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (101, 1, N'ALADAĞ', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (102, 1, N'CEYHAN', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (103, 1, N'ÇUKUROVA', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (104, 1, N'FEKE', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (105, 1, N'İMAMOĞLU', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (106, 1, N'KARAİSALI', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (107, 1, N'KARATAŞ', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (108, 1, N'KOZAN', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (109, 1, N'POZANTI', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (110, 1, N'SAİMBEYLİ', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (111, 1, N'SARIÇAM', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (112, 1, N'SEYHAN', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (113, 1, N'TUFANBEYLİ', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (114, 1, N'YUMURTALIK', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (115, 1, N'YÜREĞİR', 322)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (201, 2, N'MERKEZ', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (202, 2, N'BESNİ', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (203, 2, N'ÇELİKHAN', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (204, 2, N'GERGER', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (205, 2, N'GÖLBAŞI', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (206, 2, N'KAHTA', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (207, 2, N'SAMSAT', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (208, 2, N'SİNCİK', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (209, 2, N'TUT', 416)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (301, 3, N'MERKEZ', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (302, 3, N'BAŞMAKÇI', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (303, 3, N'BAYAT', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (304, 3, N'BOLVADİN', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (305, 3, N'ÇAY', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (306, 3, N'ÇOBANLAR', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (307, 3, N'DAZKIRI', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (308, 3, N'DİNAR', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (309, 3, N'EMİRDAĞ', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (310, 3, N'EVCİLER', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (311, 3, N'HOCALAR', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (312, 3, N'İHSANİYE', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (313, 3, N'İSCEHİSAR', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (314, 3, N'KIZILÖREN', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (315, 3, N'SANDIKLI', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (316, 3, N'SİNANPAŞA', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (317, 3, N'SULTANDAĞI', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (318, 3, N'ŞUHUT', 272)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (401, 4, N'MERKEZ', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (402, 4, N'DİYADİN', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (403, 4, N'DOĞUBAYAZIT', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (404, 4, N'ELEŞKİRT', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (405, 4, N'HAMUR', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (406, 4, N'PATNOS', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (407, 4, N'TAŞLIÇAY', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (408, 4, N'TUTAK', 472)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (501, 5, N'MERKEZ', 358)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (502, 5, N'GÖYNÜCEK', 358)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (503, 5, N'GÜMÜŞHACIKÖY', 358)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (504, 5, N'HAMAMÖZÜ', 358)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (505, 5, N'MERZİFON', 358)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (506, 5, N'SULUOVA', 358)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (507, 5, N'TAŞOVA', 358)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (601, 6, N'AKYURT', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (602, 6, N'ALTINDAĞ', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (603, 6, N'AYAŞ', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (604, 6, N'BALA', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (605, 6, N'BEYPAZARI', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (606, 6, N'ÇAMLIDERE', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (607, 6, N'ÇANKAYA', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (608, 6, N'ÇUBUK', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (609, 6, N'ELMADAĞ', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (610, 6, N'ETİMESGUT', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (611, 6, N'EVREN', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (612, 6, N'GÖLBAŞI', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (613, 6, N'GÜDÜL', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (614, 6, N'HAYMANA', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (615, 6, N'KAHRAMANKAZAN', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (616, 6, N'KALECİK', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (617, 6, N'KEÇİÖREN', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (618, 6, N'KIZILCAHAMAM', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (619, 6, N'MAMAK', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (620, 6, N'NALLIHAN', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (621, 6, N'POLATLI', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (622, 6, N'PURSAKLAR', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (623, 6, N'SİNCAN', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (624, 6, N'ŞEREFLİKOÇHİSAR', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (625, 6, N'YENİMAHALLE', 312)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (701, 7, N'AKSEKİ', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (702, 7, N'AKSU', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (703, 7, N'ALANYA', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (704, 7, N'DEMRE', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (705, 7, N'DÖŞEMEALTI', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (706, 7, N'ELMALI', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (707, 7, N'FİNİKE', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (708, 7, N'GAZİPAŞA', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (709, 7, N'GÜNDOĞMUŞ', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (710, 7, N'İBRADI', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (711, 7, N'KAŞ', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (712, 7, N'KEMER', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (713, 7, N'KEPEZ', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (714, 7, N'KONYAALTI', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (715, 7, N'KORKUTELİ', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (716, 7, N'KUMLUCA', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (717, 7, N'MANAVGAT', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (718, 7, N'MURATPAŞA', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (719, 7, N'SERİK', 242)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (801, 8, N'MERKEZ', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (802, 8, N'ARDANUÇ', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (803, 8, N'ARHAVİ', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (804, 8, N'BORÇKA', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (805, 8, N'HOPA', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (806, 8, N'KEMALPAŞA', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (807, 8, N'MURGUL', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (808, 8, N'ŞAVŞAT', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (809, 8, N'YUSUFELİ', 466)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (901, 9, N'BOZDOĞAN', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (902, 9, N'BUHARKENT', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (903, 9, N'ÇİNE', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (904, 9, N'DİDİM', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (905, 9, N'EFELER', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (906, 9, N'GERMENCİK', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (907, 9, N'İNCİRLİOVA', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (908, 9, N'KARACASU', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (909, 9, N'KARPUZLU', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (910, 9, N'KOÇARLI', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (911, 9, N'KÖŞK', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (912, 9, N'KUŞADASI', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (913, 9, N'KUYUCAK', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (914, 9, N'NAZİLLİ', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (915, 9, N'SÖKE', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (916, 9, N'SULTANHİSAR', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (917, 9, N'YENİPAZAR', 256)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1001, 10, N'ALTIEYLÜL', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1002, 10, N'AYVALIK', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1003, 10, N'BALYA', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1004, 10, N'BANDIRMA', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1005, 10, N'BİGADİÇ', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1006, 10, N'BURHANİYE', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1007, 10, N'DURSUNBEY', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1008, 10, N'EDREMİT', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1009, 10, N'ERDEK', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1010, 10, N'GÖMEÇ', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1011, 10, N'GÖNEN', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1012, 10, N'HAVRAN', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1013, 10, N'İVRİNDİ', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1014, 10, N'KARESİ', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1015, 10, N'KEPSUT', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1016, 10, N'MANYAS', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1017, 10, N'MARMARA', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1018, 10, N'SAVAŞTEPE', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1019, 10, N'SINDIRGI', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1020, 10, N'SUSURLUK', 266)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1101, 11, N'MERKEZ', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1102, 11, N'BOZÜYÜK', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1103, 11, N'GÖLPAZARI', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1104, 11, N'İNHİSAR', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1105, 11, N'OSMANELİ', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1106, 11, N'PAZARYERİ', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1107, 11, N'SÖĞÜT', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1108, 11, N'YENİPAZAR', 228)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1201, 12, N'MERKEZ', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1202, 12, N'ADAKLI', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1203, 12, N'GENÇ', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1204, 12, N'KARLIOVA', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1205, 12, N'KİĞI', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1206, 12, N'SOLHAN', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1207, 12, N'YAYLADERE', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1208, 12, N'YEDİSU', 426)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1301, 13, N'MERKEZ', 434)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1302, 13, N'ADİLCEVAZ', 434)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1303, 13, N'AHLAT', 434)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1304, 13, N'GÜROYMAK', 434)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1305, 13, N'HİZAN', 434)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1306, 13, N'MUTKİ', 434)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1307, 13, N'TATVAN', 434)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1401, 14, N'MERKEZ', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1402, 14, N'DÖRTDİVAN', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1403, 14, N'GEREDE', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1404, 14, N'GÖYNÜK', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1405, 14, N'KIBRISCIK', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1406, 14, N'MENGEN', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1407, 14, N'MUDURNU', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1408, 14, N'SEBEN', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1409, 14, N'YENİÇAĞA', 374)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1501, 15, N'MERKEZ', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1502, 15, N'AĞLASUN', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1503, 15, N'ALTINYAYLA', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1504, 15, N'BUCAK', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1505, 15, N'ÇAVDIR', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1506, 15, N'ÇELTİKÇİ', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1507, 15, N'GÖLHİSAR', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1508, 15, N'KARAMANLI', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1509, 15, N'KEMER', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1510, 15, N'TEFENNİ', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1511, 15, N'YEŞİLOVA', 248)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1601, 16, N'BÜYÜKORHAN', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1602, 16, N'GEMLİK', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1603, 16, N'GÜRSU', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1604, 16, N'HARMANCIK', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1605, 16, N'İNEGÖL', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1606, 16, N'İZNİK', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1607, 16, N'KARACABEY', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1608, 16, N'KELES', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1609, 16, N'KESTEL', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1610, 16, N'MUDANYA', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1611, 16, N'MUSTAFAKEMALPAŞA', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1612, 16, N'NİLÜFER', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1613, 16, N'ORHANELİ', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1614, 16, N'ORHANGAZİ', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1615, 16, N'OSMANGAZİ', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1616, 16, N'YENİŞEHİR', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1617, 16, N'YILDIRIM', 224)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1701, 17, N'MERKEZ', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1702, 17, N'AYVACIK', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1703, 17, N'BAYRAMİÇ', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1704, 17, N'BİGA', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1705, 17, N'BOZCAADA', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1706, 17, N'ÇAN', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1707, 17, N'ECEABAT', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1708, 17, N'EZİNE', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1709, 17, N'GELİBOLU', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1710, 17, N'GÖKÇEADA', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1711, 17, N'LAPSEKİ', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1712, 17, N'YENİCE', 286)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1801, 18, N'MERKEZ', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1802, 18, N'ATKARACALAR', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1803, 18, N'BAYRAMÖREN', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1804, 18, N'ÇERKEŞ', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1805, 18, N'ELDİVAN', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1806, 18, N'ILGAZ', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1807, 18, N'KIZILIRMAK', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1808, 18, N'KORGUN', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1809, 18, N'KURŞUNLU', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1810, 18, N'ORTA', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1811, 18, N'ŞABANÖZÜ', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1812, 18, N'YAPRAKLI', 376)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1901, 19, N'MERKEZ', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1902, 19, N'ALACA', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1903, 19, N'BAYAT', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1904, 19, N'BOĞAZKALE', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1905, 19, N'DODURGA', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1906, 19, N'İSKİLİP', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1907, 19, N'KARGI', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1908, 19, N'LAÇİN', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1909, 19, N'MECİTÖZÜ', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1910, 19, N'OĞUZLAR', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1911, 19, N'ORTAKÖY', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1912, 19, N'OSMANCIK', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1913, 19, N'SUNGURLU', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (1914, 19, N'UĞURLUDAĞ', 364)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2001, 20, N'ACIPAYAM', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2002, 20, N'BABADAĞ', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2003, 20, N'BAKLAN', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2004, 20, N'BEKİLLİ', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2005, 20, N'BEYAĞAÇ', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2006, 20, N'BOZKURT', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2007, 20, N'BULDAN', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2008, 20, N'ÇAL', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2009, 20, N'ÇAMELİ', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2010, 20, N'ÇARDAK', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2011, 20, N'ÇİVRİL', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2012, 20, N'GÜNEY', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2013, 20, N'HONAZ', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2014, 20, N'KALE', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2015, 20, N'MERKEZEFENDİ', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2016, 20, N'PAMUKKALE', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2017, 20, N'SARAYKÖY', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2018, 20, N'SERİNHİSAR', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2019, 20, N'TAVAS', 258)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2101, 21, N'BAĞLAR', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2102, 21, N'BİSMİL', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2103, 21, N'ÇERMİK', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2104, 21, N'ÇINAR', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2105, 21, N'ÇÜNGÜŞ', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2106, 21, N'DİCLE', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2107, 21, N'EĞİL', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2108, 21, N'ERGANİ', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2109, 21, N'HANİ', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2110, 21, N'HAZRO', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2111, 21, N'KAYAPINAR', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2112, 21, N'KOCAKÖY', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2113, 21, N'KULP', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2114, 21, N'LİCE', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2115, 21, N'SİLVAN', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2116, 21, N'SUR', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2117, 21, N'YENİŞEHİR', 412)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2201, 22, N'MERKEZ', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2202, 22, N'ENEZ', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2203, 22, N'HAVSA', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2204, 22, N'İPSALA', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2205, 22, N'KEŞAN', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2206, 22, N'LALAPAŞA', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2207, 22, N'MERİÇ', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2208, 22, N'SÜLOĞLU', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2209, 22, N'UZUNKÖPRÜ', 284)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2301, 23, N'MERKEZ', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2302, 23, N'AĞIN', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2303, 23, N'ALACAKAYA', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2304, 23, N'ARICAK', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2305, 23, N'BASKİL', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2306, 23, N'KARAKOÇAN', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2307, 23, N'KEBAN', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2308, 23, N'KOVANCILAR', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2309, 23, N'MADEN', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2310, 23, N'PALU', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2311, 23, N'SİVRİCE', 424)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2401, 24, N'MERKEZ', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2402, 24, N'ÇAYIRLI', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2403, 24, N'İLİÇ', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2404, 24, N'KEMAH', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2405, 24, N'KEMALİYE', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2406, 24, N'OTLUKBELİ', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2407, 24, N'REFAHİYE', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2408, 24, N'TERCAN', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2409, 24, N'ÜZÜMLÜ', 446)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2501, 25, N'AŞKALE', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2502, 25, N'AZİZİYE', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2503, 25, N'ÇAT', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2504, 25, N'HINIS', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2505, 25, N'HORASAN', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2506, 25, N'İSPİR', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2507, 25, N'KARAÇOBAN', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2508, 25, N'KARAYAZI', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2509, 25, N'KÖPRÜKÖY', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2510, 25, N'NARMAN', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2511, 25, N'OLTU', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2512, 25, N'OLUR', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2513, 25, N'PALANDÖKEN', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2514, 25, N'PASİNLER', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2515, 25, N'PAZARYOLU', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2516, 25, N'ŞENKAYA', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2517, 25, N'TEKMAN', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2518, 25, N'TORTUM', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2519, 25, N'UZUNDERE', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2520, 25, N'YAKUTİYE', 42)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2601, 26, N'ALPU', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2602, 26, N'BEYLİKOVA', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2603, 26, N'ÇİFTELER', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2604, 26, N'GÜNYÜZÜ', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2605, 26, N'HAN', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2606, 26, N'İNÖNÜ', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2607, 26, N'MAHMUDİYE', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2608, 26, N'MİHALGAZİ', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2609, 26, N'MİHALIÇÇIK', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2610, 26, N'ODUNPAZARI', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2611, 26, N'SARICAKAYA', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2612, 26, N'SEYİTGAZİ', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2613, 26, N'SİVRİHİSAR', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2614, 26, N'TEPEBAŞI', 222)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2701, 27, N'ARABAN', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2702, 27, N'İSLAHİYE', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2703, 27, N'KARKAMIŞ', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2704, 27, N'NİZİP', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2705, 27, N'NURDAĞI', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2706, 27, N'OĞUZELİ', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2707, 27, N'ŞAHİNBEY', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2708, 27, N'ŞEHİTKAMİL', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2709, 27, N'YAVUZELİ', 342)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2801, 28, N'MERKEZ', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2802, 28, N'ALUCRA', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2803, 28, N'BULANCAK', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2804, 28, N'ÇAMOLUK', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2805, 28, N'ÇANAKÇI', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2806, 28, N'DERELİ', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2807, 28, N'DOĞANKENT', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2808, 28, N'ESPİYE', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2809, 28, N'EYNESİL', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2810, 28, N'GÖRELE', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2811, 28, N'GÜCE', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2812, 28, N'KEŞAP', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2813, 28, N'PİRAZİZ', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2814, 28, N'ŞEBİNKARAHİSAR', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2815, 28, N'TİREBOLU', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2816, 28, N'YAĞLIDERE', 454)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2901, 29, N'MERKEZ', 456)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2902, 29, N'KELKİT', 456)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2903, 29, N'KÖSE', 456)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2904, 29, N'KÜRTÜN', 456)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2905, 29, N'ŞİRAN', 456)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (2906, 29, N'TORUL', 456)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3001, 30, N'MERKEZ', 438)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3002, 30, N'ÇUKURCA', 438)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3003, 30, N'DERECİK', 438)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3004, 30, N'ŞEMDİNLİ', 438)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3005, 30, N'YÜKSEKOVA', 438)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3101, 31, N'ALTINÖZÜ', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3102, 31, N'ANTAKYA', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3103, 31, N'ARSUZ', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3104, 31, N'BELEN', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3105, 31, N'DEFNE', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3106, 31, N'DÖRTYOL', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3107, 31, N'ERZİN', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3108, 31, N'HASSA', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3109, 31, N'İSKENDERUN', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3110, 31, N'KIRIKHAN', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3111, 31, N'KUMLU', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3112, 31, N'PAYAS', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3113, 31, N'REYHANLI', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3114, 31, N'SAMANDAĞ', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3115, 31, N'YAYLADAĞI', 326)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3201, 32, N'MERKEZ', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3202, 32, N'AKSU', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3203, 32, N'ATABEY', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3204, 32, N'EĞİRDİR', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3205, 32, N'GELENDOST', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3206, 32, N'GÖNEN', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3207, 32, N'KEÇİBORLU', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3208, 32, N'SENİRKENT', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3209, 32, N'SÜTÇÜLER', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3210, 32, N'ŞARKİKARAAĞAÇ', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3211, 32, N'ULUBORLU', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3212, 32, N'YALVAÇ', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3213, 32, N'YENİŞARBADEMLİ', 246)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3301, 33, N'AKDENİZ', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3302, 33, N'ANAMUR', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3303, 33, N'AYDINCIK', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3304, 33, N'BOZYAZI', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3305, 33, N'ÇAMLIYAYLA', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3306, 33, N'ERDEMLİ', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3307, 33, N'GÜLNAR', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3308, 33, N'MEZİTLİ', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3309, 33, N'MUT', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3310, 33, N'SİLİFKE', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3311, 33, N'TARSUS', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3312, 33, N'TOROSLAR', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3313, 33, N'YENİŞEHİR', 324)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3401, 34, N'ADALAR', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3402, 34, N'ARNAVUTKÖY', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3403, 34, N'ATAŞEHİR', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3404, 34, N'AVCILAR', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3405, 34, N'BAĞCILAR', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3406, 34, N'BAHÇELİEVLER', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3407, 34, N'BAKIRKÖY', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3408, 34, N'BAŞAKŞEHİR', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3409, 34, N'BAYRAMPAŞA', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3410, 34, N'BEŞİKTAŞ', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3411, 34, N'BEYKOZ', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3412, 34, N'BEYLİKDÜZÜ', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3413, 34, N'BEYOĞLU', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3414, 34, N'BÜYÜKÇEKMECE', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3415, 34, N'ÇATALCA', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3416, 34, N'ÇEKMEKÖY', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3417, 34, N'ESENLER', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3418, 34, N'ESENYURT', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3419, 34, N'EYÜPSULTAN', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3420, 34, N'FATİH', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3421, 34, N'GAZİOSMANPAŞA', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3422, 34, N'GÜNGÖREN', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3423, 34, N'KADIKÖY', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3424, 34, N'KAĞITHANE', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3425, 34, N'KARTAL', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3426, 34, N'KÜÇÜKÇEKMECE', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3427, 34, N'MALTEPE', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3428, 34, N'PENDİK', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3429, 34, N'SANCAKTEPE', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3430, 34, N'SARIYER', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3431, 34, N'SİLİVRİ', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3432, 34, N'SULTANBEYLİ', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3433, 34, N'SULTANGAZİ', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3434, 34, N'ŞİLE', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3435, 34, N'ŞİŞLİ', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3436, 34, N'TUZLA', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3437, 34, N'ÜMRANİYE', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3438, 34, N'ÜSKÜDAR', 216)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3439, 34, N'ZEYTİNBURNU', 212)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3501, 35, N'ALİAĞA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3502, 35, N'BALÇOVA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3503, 35, N'BAYINDIR', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3504, 35, N'BAYRAKLI', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3505, 35, N'BERGAMA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3506, 35, N'BEYDAĞ', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3507, 35, N'BORNOVA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3508, 35, N'BUCA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3509, 35, N'ÇEŞME', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3510, 35, N'ÇİĞLİ', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3511, 35, N'DİKİLİ', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3512, 35, N'FOÇA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3513, 35, N'GAZİEMİR', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3514, 35, N'GÜZELBAHÇE', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3515, 35, N'KARABAĞLAR', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3516, 35, N'KARABURUN', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3517, 35, N'KARŞIYAKA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3518, 35, N'KEMALPAŞA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3519, 35, N'KINIK', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3520, 35, N'KİRAZ', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3521, 35, N'KONAK', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3522, 35, N'MENDERES', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3523, 35, N'MENEMEN', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3524, 35, N'NARLIDERE', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3525, 35, N'ÖDEMİŞ', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3526, 35, N'SEFERİHİSAR', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3527, 35, N'SELÇUK', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3528, 35, N'TİRE', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3529, 35, N'TORBALI', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3530, 35, N'URLA', 232)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3601, 36, N'MERKEZ', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3602, 36, N'AKYAKA', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3603, 36, N'ARPAÇAY', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3604, 36, N'DİGOR', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3605, 36, N'KAĞIZMAN', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3606, 36, N'SARIKAMIŞ', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3607, 36, N'SELİM', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3608, 36, N'SUSUZ', 474)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3701, 37, N'MERKEZ', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3702, 37, N'ABANA', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3703, 37, N'AĞLI', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3704, 37, N'ARAÇ', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3705, 37, N'AZDAVAY', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3706, 37, N'BOZKURT', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3707, 37, N'CİDE', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3708, 37, N'ÇATALZEYTİN', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3709, 37, N'DADAY', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3710, 37, N'DEVREKANİ', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3711, 37, N'DOĞANYURT', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3712, 37, N'HANÖNÜ', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3713, 37, N'İHSANGAZİ', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3714, 37, N'İNEBOLU', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3715, 37, N'KÜRE', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3716, 37, N'PINARBAŞI', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3717, 37, N'SEYDİLER', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3718, 37, N'ŞENPAZAR', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3719, 37, N'TAŞKÖPRÜ', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3720, 37, N'TOSYA', 366)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3801, 38, N'AKKIŞLA', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3802, 38, N'BÜNYAN', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3803, 38, N'DEVELİ', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3804, 38, N'FELAHİYE', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3805, 38, N'HACILAR', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3806, 38, N'İNCESU', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3807, 38, N'KOCASİNAN', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3808, 38, N'MELİKGAZİ', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3809, 38, N'ÖZVATAN', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3810, 38, N'PINARBAŞI', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3811, 38, N'SARIOĞLAN', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3812, 38, N'SARIZ', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3813, 38, N'TALAS', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3814, 38, N'TOMARZA', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3815, 38, N'YAHYALI', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3816, 38, N'YEŞİLHİSAR', 352)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3901, 39, N'MERKEZ', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3902, 39, N'BABAESKİ', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3903, 39, N'DEMİRKÖY', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3904, 39, N'KOFÇAZ', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3905, 39, N'LÜLEBURGAZ', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3906, 39, N'PEHLİVANKÖY', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3907, 39, N'PINARHİSAR', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (3908, 39, N'VİZE', 0)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4001, 40, N'MERKEZ', 386)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4002, 40, N'AKÇAKENT', 386)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4003, 40, N'AKPINAR', 386)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4004, 40, N'BOZTEPE', 386)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4005, 40, N'ÇİÇEKDAĞI', 386)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4006, 40, N'KAMAN', 386)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4007, 40, N'MUCUR', 386)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4101, 41, N'BAŞİSKELE', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4102, 41, N'ÇAYIROVA', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4103, 41, N'DARICA', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4104, 41, N'DERİNCE', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4105, 41, N'DİLOVASI', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4106, 41, N'GEBZE', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4107, 41, N'GÖLCÜK', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4108, 41, N'İZMİT', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4109, 41, N'KANDIRA', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4110, 41, N'KARAMÜRSEL', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4111, 41, N'KARTEPE', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4112, 41, N'KÖRFEZ', 262)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4201, 42, N'AHIRLI', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4202, 42, N'AKÖREN', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4203, 42, N'AKŞEHİR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4204, 42, N'ALTINEKİN', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4205, 42, N'BEYŞEHİR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4206, 42, N'BOZKIR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4207, 42, N'CİHANBEYLİ', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4208, 42, N'ÇELTİK', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4209, 42, N'ÇUMRA', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4210, 42, N'DERBENT', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4211, 42, N'DEREBUCAK', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4212, 42, N'DOĞANHİSAR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4213, 42, N'EMİRGAZİ', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4214, 42, N'EREĞLİ', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4215, 42, N'GÜNEYSINIR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4216, 42, N'HADİM', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4217, 42, N'HALKAPINAR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4218, 42, N'HÜYÜK', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4219, 42, N'ILGIN', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4220, 42, N'KADINHANI', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4221, 42, N'KARAPINAR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4222, 42, N'KARATAY', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4223, 42, N'KULU', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4224, 42, N'MERAM', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4225, 42, N'SARAYÖNÜ', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4226, 42, N'SELÇUKLU', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4227, 42, N'SEYDİŞEHİR', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4228, 42, N'TAŞKENT', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4229, 42, N'TUZLUKÇU', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4230, 42, N'YALIHÜYÜK', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4231, 42, N'YUNAK', 332)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4301, 43, N'MERKEZ', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4302, 43, N'ALTINTAŞ', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4303, 43, N'ASLANAPA', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4304, 43, N'ÇAVDARHİSAR', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4305, 43, N'DOMANİÇ', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4306, 43, N'DUMLUPINAR', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4307, 43, N'EMET', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4308, 43, N'GEDİZ', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4309, 43, N'HİSARCIK', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4310, 43, N'PAZARLAR', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4311, 43, N'SİMAV', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4312, 43, N'ŞAPHANE', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4313, 43, N'TAVŞANLI', 274)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4401, 44, N'AKÇADAĞ', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4402, 44, N'ARAPGİR', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4403, 44, N'ARGUVAN', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4404, 44, N'BATTALGAZİ', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4405, 44, N'DARENDE', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4406, 44, N'DOĞANŞEHİR', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4407, 44, N'DOĞANYOL', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4408, 44, N'HEKİMHAN', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4409, 44, N'KALE', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4410, 44, N'KULUNCAK', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4411, 44, N'PÜTÜRGE', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4412, 44, N'YAZIHAN', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4413, 44, N'YEŞİLYURT', 422)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4501, 45, N'AHMETLİ', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4502, 45, N'AKHİSAR', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4503, 45, N'ALAŞEHİR', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4504, 45, N'DEMİRCİ', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4505, 45, N'GÖLMARMARA', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4506, 45, N'GÖRDES', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4507, 45, N'KIRKAĞAÇ', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4508, 45, N'KÖPRÜBAŞI', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4509, 45, N'KULA', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4510, 45, N'SALİHLİ', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4511, 45, N'SARIGÖL', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4512, 45, N'SARUHANLI', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4513, 45, N'SELENDİ', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4514, 45, N'SOMA', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4515, 45, N'ŞEHZADELER', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4516, 45, N'TURGUTLU', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4517, 45, N'YUNUSEMRE', 236)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4601, 46, N'AFŞİN', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4602, 46, N'ANDIRIN', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4603, 46, N'ÇAĞLAYANCERİT', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4604, 46, N'DULKADİROĞLU', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4605, 46, N'EKİNÖZÜ', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4606, 46, N'ELBİSTAN', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4607, 46, N'GÖKSUN', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4608, 46, N'NURHAK', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4609, 46, N'ONİKİŞUBAT', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4610, 46, N'PAZARCIK', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4611, 46, N'TÜRKOĞLU', 344)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4701, 47, N'ARTUKLU', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4702, 47, N'DARGEÇİT', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4703, 47, N'DERİK', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4704, 47, N'KIZILTEPE', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4705, 47, N'MAZIDAĞI', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4706, 47, N'MİDYAT', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4707, 47, N'NUSAYBİN', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4708, 47, N'ÖMERLİ', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4709, 47, N'SAVUR', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4710, 47, N'YEŞİLLİ', 482)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4801, 48, N'BODRUM', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4802, 48, N'DALAMAN', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4803, 48, N'DATÇA', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4804, 48, N'FETHİYE', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4805, 48, N'KAVAKLIDERE', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4806, 48, N'KÖYCEĞİZ', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4807, 48, N'MARMARİS', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4808, 48, N'MENTEŞE', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4809, 48, N'MİLAS', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4810, 48, N'ORTACA', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4811, 48, N'SEYDİKEMER', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4812, 48, N'ULA', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4813, 48, N'YATAĞAN', 252)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4901, 49, N'MERKEZ', 436)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4902, 49, N'BULANIK', 436)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4903, 49, N'HASKÖY', 436)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4904, 49, N'KORKUT', 436)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4905, 49, N'MALAZGİRT', 436)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (4906, 49, N'VARTO', 436)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5001, 50, N'MERKEZ', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5002, 50, N'ACIGÖL', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5003, 50, N'AVANOS', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5004, 50, N'DERİNKUYU', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5005, 50, N'GÜLŞEHİR', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5006, 50, N'HACIBEKTAŞ', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5007, 50, N'KOZAKLI', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5008, 50, N'ÜRGÜP', 384)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5101, 51, N'MERKEZ', 388)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5102, 51, N'ALTUNHİSAR', 388)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5103, 51, N'BOR', 388)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5104, 51, N'ÇAMARDI', 388)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5105, 51, N'ÇİFTLİK', 388)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5106, 51, N'ULUKIŞLA', 388)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5201, 52, N'AKKUŞ', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5202, 52, N'ALTINORDU', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5203, 52, N'AYBASTI', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5204, 52, N'ÇAMAŞ', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5205, 52, N'ÇATALPINAR', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5206, 52, N'ÇAYBAŞI', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5207, 52, N'FATSA', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5208, 52, N'GÖLKÖY', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5209, 52, N'GÜLYALI', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5210, 52, N'GÜRGENTEPE', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5211, 52, N'İKİZCE', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5212, 52, N'KABADÜZ', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5213, 52, N'KABATAŞ', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5214, 52, N'KORGAN', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5215, 52, N'KUMRU', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5216, 52, N'MESUDİYE', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5217, 52, N'PERŞEMBE', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5218, 52, N'ULUBEY', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5219, 52, N'ÜNYE', 452)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5301, 53, N'MERKEZ', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5302, 53, N'ARDEŞEN', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5303, 53, N'ÇAMLIHEMŞİN', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5304, 53, N'ÇAYELİ', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5305, 53, N'DEREPAZARI', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5306, 53, N'FINDIKLI', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5307, 53, N'GÜNEYSU', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5308, 53, N'HEMŞİN', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5309, 53, N'İKİZDERE', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5310, 53, N'İYİDERE', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5311, 53, N'KALKANDERE', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5312, 53, N'PAZAR', 464)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5401, 54, N'ADAPAZARI', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5402, 54, N'AKYAZI', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5403, 54, N'ARİFİYE', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5404, 54, N'ERENLER', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5405, 54, N'FERİZLİ', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5406, 54, N'GEYVE', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5407, 54, N'HENDEK', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5408, 54, N'KARAPÜRÇEK', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5409, 54, N'KARASU', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5410, 54, N'KAYNARCA', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5411, 54, N'KOCAALİ', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5412, 54, N'PAMUKOVA', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5413, 54, N'SAPANCA', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5414, 54, N'SERDİVAN', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5415, 54, N'SÖĞÜTLÜ', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5416, 54, N'TARAKLI', 264)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5501, 55, N'ALAÇAM', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5502, 55, N'ASARCIK', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5503, 55, N'ATAKUM', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5504, 55, N'AYVACIK', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5505, 55, N'BAFRA', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5506, 55, N'CANİK', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5507, 55, N'ÇARŞAMBA', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5508, 55, N'HAVZA', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5509, 55, N'İLKADIM', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5510, 55, N'KAVAK', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5511, 55, N'LADİK', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5512, 55, N'19 MAYIS', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5513, 55, N'SALIPAZARI', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5514, 55, N'TEKKEKÖY', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5515, 55, N'TERME', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5516, 55, N'VEZİRKÖPRÜ', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5517, 55, N'YAKAKENT', 362)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5601, 56, N'MERKEZ', 484)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5602, 56, N'BAYKAN', 484)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5603, 56, N'ERUH', 484)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5604, 56, N'KURTALAN', 484)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5605, 56, N'PERVARİ', 484)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5606, 56, N'ŞİRVAN', 484)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5607, 56, N'TİLLO', 484)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5701, 57, N'MERKEZ', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5702, 57, N'AYANCIK', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5703, 57, N'BOYABAT', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5704, 57, N'DİKMEN', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5705, 57, N'DURAĞAN', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5706, 57, N'ERFELEK', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5707, 57, N'GERZE', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5708, 57, N'SARAYDÜZÜ', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5709, 57, N'TÜRKELİ', 368)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5801, 58, N'MERKEZ', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5802, 58, N'AKINCILAR', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5803, 58, N'ALTINYAYLA', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5804, 58, N'DİVRİĞİ', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5805, 58, N'DOĞANŞAR', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5806, 58, N'GEMEREK', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5807, 58, N'GÖLOVA', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5808, 58, N'GÜRÜN', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5809, 58, N'HAFİK', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5810, 58, N'İMRANLI', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5811, 58, N'KANGAL', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5812, 58, N'KOYULHİSAR', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5813, 58, N'SUŞEHRİ', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5814, 58, N'ŞARKIŞLA', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5815, 58, N'ULAŞ', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5816, 58, N'YILDIZELİ', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5817, 58, N'ZARA', 346)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5901, 59, N'ÇERKEZKÖY', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5902, 59, N'ÇORLU', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5903, 59, N'ERGENE', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5904, 59, N'HAYRABOLU', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5905, 59, N'KAPAKLI', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5906, 59, N'MALKARA', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5907, 59, N'MARMARAEREĞLİSİ', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5908, 59, N'MURATLI', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5909, 59, N'SARAY', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5910, 59, N'SÜLEYMANPAŞA', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (5911, 59, N'ŞARKÖY', 282)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6001, 60, N'MERKEZ', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6002, 60, N'ALMUS', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6003, 60, N'ARTOVA', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6004, 60, N'BAŞÇİFTLİK', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6005, 60, N'ERBAA', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6006, 60, N'NİKSAR', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6007, 60, N'PAZAR', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6008, 60, N'REŞADİYE', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6009, 60, N'SULUSARAY', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6010, 60, N'TURHAL', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6011, 60, N'YEŞİLYURT', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6012, 60, N'ZİLE', 356)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6101, 61, N'AKÇAABAT', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6102, 61, N'ARAKLI', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6103, 61, N'ARSİN', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6104, 61, N'BEŞİKDÜZÜ', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6105, 61, N'ÇARŞIBAŞI', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6106, 61, N'ÇAYKARA', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6107, 61, N'DERNEKPAZARI', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6108, 61, N'DÜZKÖY', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6109, 61, N'HAYRAT', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6110, 61, N'KÖPRÜBAŞI', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6111, 61, N'MAÇKA', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6112, 61, N'OF', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6113, 61, N'ORTAHİSAR', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6114, 61, N'SÜRMENE', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6115, 61, N'ŞALPAZARI', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6116, 61, N'TONYA', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6117, 61, N'VAKFIKEBİR', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6118, 61, N'YOMRA', 462)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6201, 62, N'MERKEZ', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6202, 62, N'ÇEMİŞGEZEK', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6203, 62, N'HOZAT', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6204, 62, N'MAZGİRT', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6205, 62, N'NAZIMİYE', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6206, 62, N'OVACIK', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6207, 62, N'PERTEK', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6208, 62, N'PÜLÜMÜR', 428)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6301, 63, N'AKÇAKALE', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6302, 63, N'BİRECİK', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6303, 63, N'BOZOVA', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6304, 63, N'CEYLANPINAR', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6305, 63, N'EYYÜBİYE', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6306, 63, N'HALFETİ', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6307, 63, N'HALİLİYE', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6308, 63, N'HARRAN', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6309, 63, N'HİLVAN', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6310, 63, N'KARAKÖPRÜ', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6311, 63, N'SİVEREK', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6312, 63, N'SURUÇ', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6313, 63, N'VİRANŞEHİR', 414)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6401, 64, N'MERKEZ', 276)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6402, 64, N'BANAZ', 276)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6403, 64, N'EŞME', 276)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6404, 64, N'KARAHALLI', 276)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6405, 64, N'SİVASLI', 276)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6406, 64, N'ULUBEY', 276)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6501, 65, N'BAHÇESARAY', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6502, 65, N'BAŞKALE', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6503, 65, N'ÇALDIRAN', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6504, 65, N'ÇATAK', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6505, 65, N'EDREMİT', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6506, 65, N'ERCİŞ', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6507, 65, N'GEVAŞ', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6508, 65, N'GÜRPINAR', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6509, 65, N'İPEKYOLU', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6510, 65, N'MURADİYE', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6511, 65, N'ÖZALP', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6512, 65, N'SARAY', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6513, 65, N'TUŞBA', 432)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6601, 66, N'MERKEZ', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6602, 66, N'AKDAĞMADENİ', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6603, 66, N'AYDINCIK', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6604, 66, N'BOĞAZLIYAN', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6605, 66, N'ÇANDIR', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6606, 66, N'ÇAYIRALAN', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6607, 66, N'ÇEKEREK', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6608, 66, N'KADIŞEHRİ', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6609, 66, N'SARAYKENT', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6610, 66, N'SARIKAYA', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6611, 66, N'SORGUN', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6612, 66, N'ŞEFAATLİ', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6613, 66, N'YENİFAKILI', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6614, 66, N'YERKÖY', 354)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6701, 67, N'MERKEZ', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6702, 67, N'ALAPLI', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6703, 67, N'ÇAYCUMA', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6704, 67, N'DEVREK', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6705, 67, N'EREĞLİ', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6706, 67, N'GÖKÇEBEY', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6707, 67, N'KİLİMLİ', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6708, 67, N'KOZLU', 372)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6801, 68, N'MERKEZ', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6802, 68, N'AĞAÇÖREN', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6803, 68, N'ESKİL', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6804, 68, N'GÜLAĞAÇ', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6805, 68, N'GÜZELYURT', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6806, 68, N'ORTAKÖY', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6807, 68, N'SARIYAHŞİ', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6808, 68, N'SULTANHANI', 382)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6901, 69, N'MERKEZ', 458)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6902, 69, N'AYDINTEPE', 458)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (6903, 69, N'DEMİRÖZÜ', 458)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7001, 70, N'MERKEZ', 338)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7002, 70, N'AYRANCI', 338)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7003, 70, N'BAŞYAYLA', 338)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7004, 70, N'ERMENEK', 338)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7005, 70, N'KAZIMKARABEKİR', 338)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7006, 70, N'SARIVELİLER', 338)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7101, 71, N'MERKEZ', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7102, 71, N'BAHŞILI', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7103, 71, N'BALIŞEYH', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7104, 71, N'ÇELEBİ', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7105, 71, N'DELİCE', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7106, 71, N'KARAKEÇİLİ', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7107, 71, N'KESKİN', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7108, 71, N'SULAKYURT', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7109, 71, N'YAHŞİHAN', 318)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7201, 72, N'MERKEZ', 488)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7202, 72, N'BEŞİRİ', 488)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7203, 72, N'GERCÜŞ', 488)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7204, 72, N'HASANKEYF', 488)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7205, 72, N'KOZLUK', 488)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7206, 72, N'SASON', 488)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7301, 73, N'MERKEZ', 486)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7302, 73, N'BEYTÜŞŞEBAP', 486)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7303, 73, N'CİZRE', 486)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7304, 73, N'GÜÇLÜKONAK', 486)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7305, 73, N'İDİL', 486)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7306, 73, N'SİLOPİ', 486)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7307, 73, N'ULUDERE', 486)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7401, 74, N'MERKEZ', 378)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7402, 74, N'AMASRA', 378)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7403, 74, N'KURUCAŞİLE', 378)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7404, 74, N'ULUS', 378)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7501, 75, N'MERKEZ', 478)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7502, 75, N'ÇILDIR', 478)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7503, 75, N'DAMAL', 478)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7504, 75, N'GÖLE', 478)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7505, 75, N'HANAK', 478)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7506, 75, N'POSOF', 478)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7601, 76, N'MERKEZ', 476)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7602, 76, N'ARALIK', 476)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7603, 76, N'KARAKOYUNLU', 476)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7604, 76, N'TUZLUCA', 476)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7701, 77, N'MERKEZ', 226)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7702, 77, N'ALTINOVA', 226)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7703, 77, N'ARMUTLU', 226)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7704, 77, N'ÇINARCIK', 226)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7705, 77, N'ÇİFTLİKKÖY', 226)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7706, 77, N'TERMAL', 226)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7801, 78, N'MERKEZ', 370)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7802, 78, N'EFLANİ', 370)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7803, 78, N'ESKİPAZAR', 370)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7804, 78, N'OVACIK', 370)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7805, 78, N'SAFRANBOLU', 370)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7806, 78, N'YENİCE', 370)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7901, 79, N'MERKEZ', 348)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7902, 79, N'ELBEYLİ', 348)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7903, 79, N'MUSABEYLİ', 348)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (7904, 79, N'POLATELİ', 348)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8001, 80, N'MERKEZ', 328)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8002, 80, N'BAHÇE', 328)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8003, 80, N'DÜZİÇİ', 328)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8004, 80, N'HASANBEYLİ', 328)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8005, 80, N'KADİRLİ', 328)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8006, 80, N'SUMBAS', 328)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8007, 80, N'TOPRAKKALE', 328)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8101, 81, N'MERKEZ', 380)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8102, 81, N'AKÇAKOCA', 380)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8103, 81, N'CUMAYERİ', 380)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8104, 81, N'ÇİLİMLİ', 380)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8105, 81, N'GÖLYAKA', 380)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8106, 81, N'GÜMÜŞOVA', 380)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8107, 81, N'KAYNAŞLI', 380)
GO
INSERT [dbo].[District] ([Id], [CityId], [Name], [TelephoneCode]) VALUES (8108, 81, N'YIĞILCA', 380)
GO
/****** Object:  Index [IX_District_CityId] ******/
CREATE NONCLUSTERED INDEX [IX_District_CityId] ON [dbo].[District]
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[District]  WITH CHECK ADD  CONSTRAINT [FK_District_City_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[District] CHECK CONSTRAINT [FK_District_City_CityId]