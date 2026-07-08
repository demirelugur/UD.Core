USE [Portal2025]
GO
/****** Object:  UserDefinedFunction [dbo].[sf_ChecksumEqualitySeoFriendly]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sf_ChecksumEqualitySeoFriendly]
(
    @left VARCHAR(MAX),
	@right VARCHAR(MAX)
)
/*
SELECT [dbo].[sf_ChecksumEqualitySeoFriendly](NULL, NULL)
SELECT [dbo].[sf_ChecksumEqualitySeoFriendly]('Uğur', 'ugur')
*/
RETURNS BIT AS
BEGIN
    RETURN CONVERT(BIT, (CASE
        WHEN ISNULL(@left, '') = '' AND ISNULL(@right, '') = '' THEN 1
        WHEN CHECKSUM([dbo].[sf_ToSeoFriendly](@left)) = CHECKSUM([dbo].[sf_ToSeoFriendly](@right)) THEN 1
        ELSE 0
    END))
END
GO
/****** Object:  UserDefinedFunction [dbo].[sf_GetJObjectPropertyValue]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sf_GetJObjectPropertyValue]
(
    @value NVARCHAR(MAX),
    @property VARCHAR(50),
    @propertyisobject BIT
)
/*
SELECT [dbo].[fnsv_GetJObjectPropertyValue]('{"a":"Uğur","b":"Ece","c":"Duru"}','a', 0)
SELECT [dbo].[fnsv_GetJObjectPropertyValue]('{"a":"Uğur","b":"Ece","c":"Duru"}','f', 0)
SELECT [dbo].[fnsv_GetJObjectPropertyValue]('{}','f', 0)
SELECT [dbo].[fnsv_GetJObjectPropertyValue]('{"yet":[872,166,123],"gun":"Loreim İpsum Gündem","kar":"","ack":"Aaaa","ipt":"","sms":1,"epo":1}','yet', 1)
*/
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @result NVARCHAR(MAX)
    IF (ISNULL(@value, '') = '' OR ISJSON(@value) != 1 OR ISNULL(@property, '') = '') BEGIN SET @result = '' END
    ELSE IF @propertyisobject = 1 BEGIN SET @result = JSON_QUERY(@value, CONCAT('$.', @property)) END
    ELSE BEGIN SET @result = JSON_VALUE(@value, CONCAT('$.', @property)) END
    RETURN ISNULL(@result, '')
END
GO
/****** Object:  UserDefinedFunction [dbo].[sf_GetSeoFullText]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sf_GetSeoFullText](@seoid INT)
RETURNS VARCHAR(267)
AS
/*
SELECT [dbo].[sf_GetSeoFullText](1)
SELECT [dbo].[sf_GetSeoFullText](NULL)
SELECT [dbo].[sf_GetSeoFullText](-1)
*/
BEGIN
RETURN (CASE WHEN @seoid > 0 THEN ISNULL((SELECT TOP 1 CONCAT(S.[SeoText], '-p', CONVERT(VARCHAR(10), S.[SeoID])) FROM [dbo].[Seo] AS S WHERE S.[SeoID] = @seoid), '') ELSE '' END)
END
GO
/****** Object:  UserDefinedFunction [dbo].[sf_HasFlag]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sf_HasFlag]
(
    @enumvalue BIGINT,
	@flag BIGINT
)
-- @flag.HasFlag(enumvalue)
RETURNS BIT AS BEGIN RETURN CONVERT(BIT, CASE WHEN (@flag & @enumvalue) = @enumvalue THEN 1 ELSE 0 END) END
GO
/****** Object:  UserDefinedFunction [dbo].[sf_MatchSeoFriendly]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sf_MatchSeoFriendly]
(
    @type TINYINT,
    @value VARCHAR(MAX),
    @searchseo VARCHAR(MAX)
)
/*
SELECT [dbo].[sf_MatchSeoFriendly](0, 'Uğur ata bak', 'ata')
SELECT [dbo].[sf_MatchSeoFriendly](1, 'Uğur ata bak', 'ugur')
SELECT [dbo].[sf_MatchSeoFriendly](2, 'Uğur ata bak', 'bak')
*/
RETURNS BIT
AS
BEGIN
    RETURN CONVERT(BIT, (CASE
        WHEN (ISNULL(@value, '') = '' OR ISNULL(@searchseo, '') = '') THEN 0
		WHEN @type = 0 AND CHARINDEX(@searchseo, [dbo].[sf_ToSeoFriendly](@value)) > 0 THEN 1 -- Contains
		WHEN @type = 1 AND CHARINDEX(@searchseo, [dbo].[sf_ToSeoFriendly](@value)) = 1 THEN 1 -- StartsWith
		WHEN @type = 2 AND RIGHT([dbo].[sf_ToSeoFriendly](@value), LEN(@searchseo)) = @searchseo THEN 1 --EndsWith
        ELSE 0
    END))
END
GO
/****** Object:  UserDefinedFunction [dbo].[sf_ToInt32]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sf_ToInt32](@value VARCHAR(10)) 
RETURNS INT
AS
/*
SELECT [dbo].[sf_ToInt32]('1')
SELECT [dbo].[sf_ToInt32](NULL)
SELECT [dbo].[sf_ToInt32]('ASDAD')
SELECT [dbo].[sf_ToInt32]('-2147483648')
*/
BEGIN RETURN CONVERT(INT, (CASE WHEN ISNUMERIC(@value) = 1 THEN @value ELSE '0' END)) END
GO
/****** Object:  UserDefinedFunction [dbo].[sf_ToSeoFriendly]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[sf_ToSeoFriendly](@value VARCHAR(MAX)) 
RETURNS VARCHAR(MAX)
AS
/*
SELECT [dbo].[sf_ToSeoFriendly]('')
SELECT [dbo].[sf_ToSeoFriendly](NULL)
SELECT [dbo].[sf_ToSeoFriendly]('657 S.K. 4/B - Sözleşmeli Personel')
*/
BEGIN
SET @value = LTRIM(RTRIM(ISNULL(@value, '')))
SET @value = REPLACE(@value, 'ş', 's');
SET @value = REPLACE(@value, 'Ş', 's');
SET @value = REPLACE(@value, 'ö', 'o');
SET @value = REPLACE(@value, 'Ö', 'o');
SET @value = REPLACE(@value, 'ü', 'u');
SET @value = REPLACE(@value, 'Ü', 'u');
SET @value = REPLACE(@value, 'ç', 'c');
SET @value = REPLACE(@value, 'Ç', 'c');
SET @value = REPLACE(@value, 'ğ', 'g');
SET @value = REPLACE(@value, 'Ğ', 'g');
SET @value = REPLACE(@value, 'ı', 'i');
SET @value = REPLACE(@value, 'I', 'i');
SET @value = REPLACE(@value, 'İ', 'i');
SET @value = REPLACE(@value, ' ', '-');
SET @value = REPLACE(@value, '?', '');
SET @value = REPLACE(@value, '/', '');
SET @value = REPLACE(@value, '.', '');
SET @value = REPLACE(@value, '''', '');
SET @value = REPLACE(@value, '"', '');
SET @value = REPLACE(@value, '#', '');
SET @value = REPLACE(@value, '%', '');
SET @value = REPLACE(@value, '&', '');
SET @value = REPLACE(@value, '*', '');
SET @value = REPLACE(@value, '!', '');
SET @value = REPLACE(@value, '@', '');
SET @value = REPLACE(@value, '+', '');
SET @value = LOWER(@value);
DECLARE @i INT = 1, @len INT = LEN(@value), @c CHAR(1)
WHILE (@i <= @len)
BEGIN
    SET @c = SUBSTRING(@value, @i, 1)
    IF NOT (@c LIKE '[a-z0-9-]')
    BEGIN
        SET @value = STUFF(@value, @i, 1, '-')
        SET @len = LEN(@value)
    END
    SET @i = @i + 1
END
WHILE (CHARINDEX('--', @value) > 0) BEGIN  SET @value = REPLACE(@value, '--', '-') END
WHILE (LEFT(@value, 1) = '-') BEGIN SET @value = SUBSTRING(@value, 2, LEN(@value) - 1) END
WHILE (RIGHT(@value, 1) = '-') BEGIN SET @value = SUBSTRING(@value, 1, LEN(@value) - 1) END
RETURN @value;
END
GO
/****** Object:  UserDefinedFunction [dbo].[tvf_helper_createproperty]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[tvf_helper_createproperty]
(@PROPERTIES VARCHAR(MAX))
RETURNS @RETURNTABLE TABLE ([data] VARCHAR(1000))
AS
/*
SELECT * FROM [dbo].[tvf_helper_createproperty]('private int _Id')
SELECT * FROM [dbo].[tvf_helper_createproperty]('private int _id')
*/
BEGIN
DECLARE @E68BA062 TABLE(CharKod INT, [Value] CHAR(1))
DECLARE @SAYAC INT = ASCII('A')
WHILE(@SAYAC <= ASCII('Z'))
BEGIN
INSERT INTO @E68BA062 SELECT @SAYAC, CHAR(@SAYAC + 32)
INSERT INTO @E68BA062 SELECT @SAYAC + 32, CHAR(@SAYAC)
SET @SAYAC = @SAYAC + 1
END
DECLARE @PROPERTYNAME VARCHAR(200), @FIELDS VARCHAR(200), @FIRSTCHAR CHAR(1) /* SELECT * FROM @E68BA062 */
DECLARE CRS_PERSONEL CURSOR FOR SELECT C.[data] FROM [dbo].[tvf_Split](REPLACE(REPLACE(@PROPERTIES, CHAR(13), ''), CHAR(10), ''), ';') AS C WHERE C.[data] != ''
OPEN CRS_PERSONEL
FETCH NEXT FROM CRS_PERSONEL INTO @FIELDS
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @PROPERTYNAME = LTRIM(RTRIM(@FIELDS))
	SET @FIRSTCHAR = (SELECT F.[Value] FROM @E68BA062 AS F WHERE F.CharKod = ASCII(SUBSTRING(@PROPERTYNAME, CHARINDEX('_', @PROPERTYNAME) + 1, 1)))
	INSERT INTO @RETURNTABLE ([data]) VALUES (
	CONCAT('public '
	, LTRIM(SUBSTRING(@PROPERTYNAME, CHARINDEX(SPACE(1), @PROPERTYNAME),CHARINDEX(SPACE(1), @PROPERTYNAME, CHARINDEX(SPACE(1), @PROPERTYNAME) + 1) - CHARINDEX(SPACE(1), @PROPERTYNAME)))
	, SPACE(1)
	, @FIRSTCHAR
	, SUBSTRING(@PROPERTYNAME, CHARINDEX('_', @PROPERTYNAME) + 2, LEN(@PROPERTYNAME) - CHARINDEX('_', @PROPERTYNAME) + 1)
	,' { get { return '
	, SUBSTRING(@PROPERTYNAME, CHARINDEX('_', @PROPERTYNAME), LEN(@PROPERTYNAME) - CHARINDEX('_', @PROPERTYNAME) + 1)
	, '; } set { '
	, SUBSTRING(@PROPERTYNAME, CHARINDEX('_', @PROPERTYNAME), LEN(@PROPERTYNAME) - CHARINDEX('_', @PROPERTYNAME) + 1)
	, ' = value; } }')
	)
    FETCH NEXT FROM CRS_PERSONEL INTO @FIELDS
END
CLOSE CRS_PERSONEL
DEALLOCATE CRS_PERSONEL
RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[tvf_Split]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[tvf_Split]
(
	@rowdata VARCHAR(MAX),
	@spliton CHAR(1)
)  
/*
SELECT * FROM [dbo].[tvf_Split]('Lorem;Ipsum;Generator',';')
Not: SELECT [value] FROM [string_split]('Abc;def;fgh;  ;;;;', ';') WHERE LTRIM(RTRIM([value])) != '' sorgusuyla aynı işlem (alternatif)
*/
RETURNS @RETURNTABLE TABLE
(
	id INT IDENTITY(1, 1),
	[data] VARCHAR(255)
)
AS
BEGIN
	DECLARE @count INT = 1
	WHILE (CHARINDEX(@spliton, @rowdata) > 0)
	BEGIN
		INSERT INTO @RETURNTABLE ([data]) VALUES (TRIM(SUBSTRING(@rowdata, 1, CHARINDEX(@spliton, @rowdata) - 1)))
		SET @rowdata = SUBSTRING(@rowdata, CHARINDEX(@spliton, @rowdata) + 1, LEN(@rowdata))
		SET @count = @count + 1
	END
	INSERT INTO @RETURNTABLE ([data]) VALUES (TRIM(@rowdata))
	RETURN
END
GO
/****** Object:  View [dbo].[vw_SqlModule]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SqlModule]
AS
/*
SELECT S.* FROM [dbo].[vw_SqlModule] AS S
*/
SELECT O.[type] 
, O.[type_desc]
, CONCAT('[', SCHEMA_NAME(O.[schema_id]), '].[', O.[name], ']') AS objectname
, LJ.[parent_objectname]
, SM.[definition]
FROM [sys].[sql_modules] AS SM
INNER JOIN [sys].[objects] AS O ON SM.[object_id] = O.[object_id]
LEFT JOIN (
SELECT PO.[object_id], CONCAT('[', SCHEMA_NAME(PO.[schema_id]), '].[', PO.[name], ']') AS parent_objectname FROM [sys].[objects] AS PO
) AS LJ ON O.[parent_object_id] = LJ.[object_id]
GO
/****** Object:  StoredProcedure [dbo].[sp_BackupAl]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_BackupAl]
@PATH VARCHAR(255) = NULL
AS
/*
EXEC [dbo].[sp_BackupAl]
*/
DECLARE @NAME NVARCHAR(255), @P NVARCHAR(255), @FILENAME NVARCHAR(255)
SET @NAME = DB_NAME()
SET @P = (CASE WHEN (@PATH IS NOT NULL) THEN @PATH
               WHEN @@SERVERNAME = 'UDemirel' THEN N'C:\Dosyalar\YardimciProgramlar\Backups\'
               WHEN @@SERVERNAME = 'UGUR-DEMIREL' THEN N'C:\UDemirelPage\Backups\'
               WHEN (@@SERVERNAME IN ('bayuniwebdb', 'APP', 'WEBUNINEW')) THEN 'C:\Backups\'
			   ELSE 'D:\Backups\' END)
SET @FILENAME = CONCAT(@P, @NAME, '_', CONVERT(NVARCHAR, GETDATE(), 112), '_', REPLACE(CONVERT(NVARCHAR, GETDATE(), 24), ':', ''), '.bak')
BACKUP DATABASE @NAME TO DISK = @FILENAME
GO
/****** Object:  StoredProcedure [dbo].[sp_helper_createtableproperty]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_helper_createtableproperty]
@tablename VARCHAR(255)
AS
/*
EXEC [dbo].[sp_helper_createtableproperty] @tablename = 'dbo.Uye'
*/
IF(ISNULL(OBJECT_ID('tempdb..#TBL_VAL9C2FC'),0) != 0) BEGIN DROP TABLE #TBL_VAL9C2FC END
IF(ISNULL(OBJECT_ID('tempdb..#TBL_RET8A2BF'),0) != 0) BEGIN DROP TABLE #TBL_RET8A2BF END
CREATE TABLE #TBL_VAL9C2FC(property_name NVARCHAR(255), fieldname NVARCHAR(255), user_type_id INT, property_typename NVARCHAR(255), property_typename_csharp NVARCHAR(255), is_chartype BIT, max_length INT, is_max_lengthmax BIT, is_identity BIT, [precision] VARCHAR(2), scale VARCHAR(2), is_nullable BIT, is_primary_key BIT)
CREATE TABLE #TBL_RET8A2BF(id INT IDENTITY(1, 1), deger NVARCHAR(4000))
DECLARE @property_name NVARCHAR(255), @fieldname NVARCHAR(255), @user_type_id INT, @property_typename NVARCHAR(255), @property_typename_csharp NVARCHAR(255), @is_chartype BIT, @max_length INT, @is_max_lengthmax BIT, @is_identity BIT, @prescion TINYINT, @scale TINYINT, @is_nullable BIT, @is_primary_key BIT
DECLARE @hascompositekey BIT = 0
/*********************/
DECLARE @object_id INT = (SELECT TOP 1 T.[object_id] FROM sys.tables AS T WHERE CONCAT(SCHEMA_NAME(T.[schema_id]), '.', T.[name]) = @tablename)
/*********************/
DECLARE CRS_MAIN CURSOR FOR 
SELECT C.[name] AS property_name
, CONCAT('_', (CASE WHEN LOWER(SUBSTRING(C.[name], 1, 1)) = 'ı' THEN 'i' ELSE LOWER(SUBSTRING(C.[name], 1, 1)) END), SUBSTRING(C.[name], 2, LEN(C.[name]) - 1)) AS fieldname
, C.user_type_id
, T.[name] AS property_typename
, (CASE WHEN C.user_type_id = 36 THEN 'Guid' /* uniqueidentifier */
     WHEN C.user_type_id = 40 THEN 'DateTime' /* date */
	 WHEN C.user_type_id = 48 THEN 'byte' /* tinyint */
     WHEN C.user_type_id = 52 THEN 'short' /* smallint */
	 WHEN C.user_type_id = 56 THEN 'int' /* int */
	 WHEN C.user_type_id = 60 THEN 'decimal' /* money */
     WHEN C.user_type_id = 61 THEN 'DateTime' /* datetime */
	 WHEN C.user_type_id = 104 THEN 'bool' /* bit */
	 WHEN C.user_type_id = 106 THEN 'decimal' /* decimal */
	 WHEN C.user_type_id = 108 THEN 'decimal' /* numeric */
	 WHEN C.user_type_id = 127 THEN 'long' /* bigint */
	 WHEN C.user_type_id = 165 THEN 'byte[]' /* varbinary */
ELSE 'string' END) AS property_typename_csharp
, CONVERT(BIT, (CASE WHEN C.user_type_id IN (167, 175, 231) THEN 1 ELSE 0 END)) AS is_chartype
, (CASE WHEN ((C.user_type_id IN (167, 175)) AND C.max_length > 0) THEN C.max_length 
        WHEN (C.user_type_id = 231 AND C.max_length > 0) THEN (C.max_length / 2) 
        ELSE 0 END) AS max_length /* varchar(167), char(175), nvarchar(231) */
, CONVERT(BIT, (CASE WHEN C.max_length = -1 THEN 1 ELSE 0 END)) AS is_max_lengthmax /* nvarchar(max), varchar(max) değerlerini yakalamak için */
, C.is_identity
, CONVERT(VARCHAR(2), (CASE WHEN C.user_type_id = 106 THEN C.[precision] ELSE 0 END)) AS [precision]
, CONVERT(VARCHAR(2), (CASE WHEN C.user_type_id = 106 THEN C.scale ELSE 0 END)) AS scale
, C.is_nullable
, CONVERT(BIT, (CASE WHEN (PK.objectid IS NULL) THEN 0 ELSE 1 END)) AS is_primary_key
FROM sys.columns AS C
LEFT JOIN (
SELECT I.[object_id] AS objectid, IC.column_id FROM sys.indexes AS I 
INNER JOIN sys.index_columns AS IC ON I.[object_id] = IC.[object_id] AND I.index_id = IC.index_id
WHERE I.is_primary_key = 1
) AS PK ON C.[object_id] = PK.objectid AND C.column_id = PK.column_id
INNER JOIN sys.types AS T ON C.user_type_id = T.user_type_id
WHERE C.[object_id] = @object_id
ORDER BY C.column_id
OPEN CRS_MAIN
FETCH NEXT FROM CRS_MAIN INTO @property_name, @fieldname, @user_type_id, @property_typename, @property_typename_csharp, @is_chartype, @max_length, @is_max_lengthmax, @is_identity, @prescion, @scale, @is_nullable, @is_primary_key
WHILE @@FETCH_STATUS = 0
BEGIN
 INSERT INTO #TBL_VAL9C2FC SELECT @property_name, @fieldname, @user_type_id, @property_typename, @property_typename_csharp, @is_chartype, @max_length, @is_max_lengthmax, @is_identity, @prescion, @scale, @is_nullable, @is_primary_key
 FETCH NEXT FROM CRS_MAIN INTO @property_name, @fieldname, @user_type_id, @property_typename, @property_typename_csharp, @is_chartype, @max_length, @is_max_lengthmax, @is_identity, @prescion, @scale, @is_nullable, @is_primary_key
END
CLOSE CRS_MAIN
DEALLOCATE CRS_MAIN
/*********************/
INSERT INTO #TBL_RET8A2BF
SELECT CONCAT('private '
, T.property_typename_csharp
, (CASE WHEN T.is_nullable = 0 THEN '' ELSE '?' END)
, ' '
, T.fieldname
-- , (CASE WHEN (T.is_chartype = 1 AND T.is_nullable = 0) THEN ' = ""' ELSE '' END)
, ';') FROM #TBL_VAL9C2FC AS T /* private string? _src; private string _src = ""; */
SET @tablename = (CASE WHEN SUBSTRING(@tablename, 1, 3) = SCHEMA_NAME(1) THEN REPLACE(@tablename, CONCAT(SCHEMA_NAME(1), '.'), '') ELSE REPLACE(@tablename, '.', '_') END)
/*********************/
INSERT INTO #TBL_RET8A2BF
SELECT CONCAT('public ', @tablename, '() { }')
/*********************/
DECLARE CRS_DETAY CURSOR FOR SELECT * FROM #TBL_VAL9C2FC AS V
OPEN CRS_DETAY
FETCH NEXT FROM CRS_DETAY INTO @property_name, @fieldname, @user_type_id, @property_typename, @property_typename_csharp, @is_chartype, @max_length, @is_max_lengthmax, @is_identity, @prescion, @scale, @is_nullable, @is_primary_key
WHILE @@FETCH_STATUS = 0
BEGIN
 /*********************/
 IF(@is_primary_key = 1)
 BEGIN
 INSERT INTO #TBL_RET8A2BF SELECT CONCAT('[DatabaseGenerated(DatabaseGeneratedOption.', (CASE WHEN @is_identity = 1 THEN 'Identity' ELSE 'None' END), ')]')
 INSERT INTO #TBL_RET8A2BF SELECT '[Key]'
 END
 /*********************/
 IF(@user_type_id = 40) BEGIN INSERT INTO #TBL_RET8A2BF SELECT '[Column(TypeName = "date")]' END /* date */
 /*********************/
 IF(@user_type_id = 61) BEGIN INSERT INTO #TBL_RET8A2BF SELECT '[Column(TypeName = "datetime")]' END /* datetime */
 /*********************/
 IF(@user_type_id = 106) 
 BEGIN 
   INSERT INTO #TBL_RET8A2BF SELECT CONCAT('[Precision(', @prescion, ', ', @scale, ')]') /* decimal */
   INSERT INTO #TBL_RET8A2BF SELECT CONCAT('[Column(TypeName = "decimal(', @prescion, ',', @scale, ')")]')
 END 
 /*********************/
 ELSE IF(@is_chartype = 1)
 BEGIN
   IF(@is_nullable = 0) BEGIN INSERT INTO #TBL_RET8A2BF SELECT '[Required]' END
   IF(@max_length > 0) BEGIN INSERT INTO #TBL_RET8A2BF SELECT CONCAT('[MaxLength(', CONVERT(VARCHAR(4), @max_length),')]') END
   INSERT INTO #TBL_RET8A2BF SELECT CONCAT('[Column(TypeName = "', @property_typename, '(', (CASE WHEN @is_max_lengthmax = 1 THEN 'max' ELSE CONVERT(VARCHAR(4), @max_length) END), ')")]')
 END
 /*********************/
 INSERT INTO #TBL_RET8A2BF SELECT CONCAT('public ', @property_typename_csharp, (CASE WHEN @is_nullable = 1 THEN '?' ELSE '' END), ' ', @property_name, ' {  get { return ', @fieldname, '; } set { ', @fieldname, ' = value; } }') /* public int? MyVar {  get { return _myVar; } set { _myVar = value; } } */
 /*********************/
 FETCH NEXT FROM CRS_DETAY INTO @property_name, @fieldname, @user_type_id, @property_typename, @property_typename_csharp, @is_chartype, @max_length, @is_max_lengthmax, @is_identity, @prescion, @scale, @is_nullable, @is_primary_key
END
CLOSE CRS_DETAY
DEALLOCATE CRS_DETAY
SELECT R.deger FROM #TBL_RET8A2BF AS R
IF(ISNULL(OBJECT_ID('tempdb..#TBL_VAL9C2FC'), 0) != 0) BEGIN DROP TABLE #TBL_VAL9C2FC END
IF(ISNULL(OBJECT_ID('tempdb..#TBL_RET8A2BF'), 0) != 0) BEGIN DROP TABLE #TBL_RET8A2BF END
GO
/****** Object:  StoredProcedure [dbo].[sp_helper_foreignkeys]    Script Date: 21.10.2025 16:19:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_helper_foreignkeys]
@tablename VARCHAR(30)
AS
/*
EXEC [dbo].[sp_helper_foreignkeys] @tablename = 'dbo.Uye'
EXEC [dbo].[sp_helper_foreignkeys] @tablename = 'dbo.Dep'
*/
SELECT CONCAT('[', SCHEMA_NAME(T.[schema_id]), '].[', T.[name], ']') AS tablename
, C.[name] AS column_name
, C.[is_nullable] AS column_isnullable
FROM [sys].[foreign_key_columns] AS FK
INNER JOIN [sys].[tables] AS T ON FK.[parent_object_id] = T.[object_id]
INNER JOIN [sys].[all_columns] AS C ON FK.[parent_object_id] = C.[object_id] AND FK.[parent_column_id] = C.[column_id]
WHERE FK.referenced_object_id = OBJECT_ID(@tablename)
ORDER BY tablename
GO
