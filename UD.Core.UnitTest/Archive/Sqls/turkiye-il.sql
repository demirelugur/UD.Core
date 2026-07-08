/****** Object:  Table [dbo].[City] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[Id] [tinyint] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[IsMetropolitanMunicipality] [bit] NOT NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (1, N'ADANA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (2, N'ADIYAMAN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (3, N'AFYONKARAHİSAR', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (4, N'AĞRI', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (5, N'AMASYA', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (6, N'ANKARA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (7, N'ANTALYA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (8, N'ARTVİN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (9, N'AYDIN', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (10, N'BALIKESİR', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (11, N'BİLECİK', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (12, N'BİNGÖL', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (13, N'BİTLİS', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (14, N'BOLU', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (15, N'BURDUR', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (16, N'BURSA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (17, N'ÇANAKKALE', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (18, N'ÇANKIRI', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (19, N'ÇORUM', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (20, N'DENİZLİ', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (21, N'DİYARBAKIR', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (22, N'EDİRNE', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (23, N'ELAZIĞ', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (24, N'ERZİNCAN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (25, N'ERZURUM', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (26, N'ESKİŞEHİR', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (27, N'GAZİANTEP', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (28, N'GİRESUN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (29, N'GÜMÜŞHANE', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (30, N'HAKKARİ', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (31, N'HATAY', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (32, N'ISPARTA', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (33, N'MERSİN', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (34, N'İSTANBUL', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (35, N'İZMİR', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (36, N'KARS', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (37, N'KASTAMONU', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (38, N'KAYSERİ', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (39, N'KIRKLARELİ', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (40, N'KIRŞEHİR', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (41, N'KOCAELİ', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (42, N'KONYA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (43, N'KÜTAHYA', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (44, N'MALATYA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (45, N'MANİSA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (46, N'KAHRAMANMARAŞ', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (47, N'MARDİN', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (48, N'MUĞLA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (49, N'MUŞ', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (50, N'NEVŞEHİR', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (51, N'NİĞDE', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (52, N'ORDU', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (53, N'RİZE', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (54, N'SAKARYA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (55, N'SAMSUN', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (56, N'SİİRT', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (57, N'SİNOP', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (58, N'SİVAS', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (59, N'TEKİRDAĞ', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (60, N'TOKAT', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (61, N'TRABZON', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (62, N'TUNCELİ', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (63, N'ŞANLIURFA', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (64, N'UŞAK', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (65, N'VAN', 1)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (66, N'YOZGAT', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (67, N'ZONGULDAK', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (68, N'AKSARAY', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (69, N'BAYBURT', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (70, N'KARAMAN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (71, N'KIRIKKALE', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (72, N'BATMAN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (73, N'ŞIRNAK', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (74, N'BARTIN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (75, N'ARDAHAN', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (76, N'IĞDIR', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (77, N'YALOVA', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (78, N'KARABÜK', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (79, N'KİLİS', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (80, N'OSMANİYE', 0)
GO
INSERT [dbo].[City] ([Id], [Name], [IsMetropolitanMunicipality]) VALUES (81, N'DÜZCE', 0)