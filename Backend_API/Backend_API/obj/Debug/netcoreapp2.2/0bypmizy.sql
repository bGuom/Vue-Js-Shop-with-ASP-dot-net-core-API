IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Dishes] (
    [DishId] uniqueidentifier NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [DishName] nvarchar(max) NOT NULL,
    [DishDescrition] nvarchar(max) NOT NULL,
    [DishPrice] real NOT NULL,
    [DishTotalRating] int NOT NULL,
    CONSTRAINT [PK_Dishes] PRIMARY KEY ([DishId])
);

GO

CREATE TABLE [Images] (
    [ImageId] uniqueidentifier NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Path] nvarchar(max) NOT NULL,
    [DishId] uniqueidentifier NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY ([ImageId]),
    CONSTRAINT [FK_Images_Dishes_DishId] FOREIGN KEY ([DishId]) REFERENCES [Dishes] ([DishId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Ratings] (
    [RatingId] uniqueidentifier NOT NULL,
    [IsDeleted] bit NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [DishId] uniqueidentifier NOT NULL,
    [UserName] nvarchar(max) NULL,
    [UserEmail] nvarchar(max) NULL,
    [Rate] int NOT NULL,
    [Review] nvarchar(max) NULL,
    CONSTRAINT [PK_Ratings] PRIMARY KEY ([RatingId]),
    CONSTRAINT [FK_Ratings_Dishes_DishId] FOREIGN KEY ([DishId]) REFERENCES [Dishes] ([DishId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Images_DishId] ON [Images] ([DishId]);

GO

CREATE INDEX [IX_Ratings_DishId] ON [Ratings] ([DishId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200414185801_initialCreate', N'2.2.4-servicing-10062');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dishes]') AND [c].[name] = N'DishTotalRating');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Dishes] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Dishes] ALTER COLUMN [DishTotalRating] decimal(18,2) NOT NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dishes]') AND [c].[name] = N'DishPrice');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Dishes] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Dishes] ALTER COLUMN [DishPrice] decimal(18,2) NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200417105348_DB-Update', N'2.2.4-servicing-10062');

GO

