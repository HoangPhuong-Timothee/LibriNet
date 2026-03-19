CREATE DATABASE Libri;
GO
USE Libri;
GO

CREATE SEQUENCE [BookStoreId] AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [ExportHistoryIdSeq] AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [ExportInventoryReceiptIdSeq] AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [ExportInventoryReceiptItemIdSeq] AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [ImageIdSeq] AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [ImportHistoryIdSeq] AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [ImportInventoryReceiptIdSeq] AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [ImportInventoryReceiptItemIdSeq] AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [InventoryAuditDetailsIdSeq] AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [InventoryAuditIdSeq] AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [InventoryAuditResultIdSeq] AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [OrderItemSeq] AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [OrderSeq] AS BIGINT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [RefreshTokenIdSeq] AS INT START WITH 1 INCREMENT BY 1;
CREATE SEQUENCE [UnitOfMeasureSeq] AS BIGINT START WITH 2 INCREMENT BY 1;
GO

CREATE TABLE [Roles] (
    [Id] INT PRIMARY KEY,
    [Title] NVARCHAR(20)
);

CREATE TABLE [Authors] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) DEFAULT ('Updating/Unknown'),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(50),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(50),
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(255)
);
CREATE INDEX [IDX_AuthorName] ON [Authors]([Name]);

CREATE TABLE [Genres] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(20),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(50),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(50),
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(50)
);
CREATE INDEX [IDX_GenreName] ON [Genres]([Name]);

CREATE TABLE [Publishers] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(100) DEFAULT ('Updating/Unknown'),
    [Address] NVARCHAR(255),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(50),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(50),
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(255)
);
CREATE INDEX [IDX_PublisherName] ON [Publishers]([Name]);

CREATE TABLE [BookStores] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [BookStoreId]),
    [StoreName] NVARCHAR(255),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(255),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(255),
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(255)
);
CREATE INDEX [IDX_StoreName] ON [BookStores]([StoreName]);

CREATE TABLE [UnitOfMeasures] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [UnitOfMeasureSeq]),
    [Name] NVARCHAR(50),
    [Description] NVARCHAR(255),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(255),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(255),
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(255)
);

CREATE TABLE [DeliveryMethods] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [ShortName] NVARCHAR(100),
    [DeliveryTime] NVARCHAR(100),
    [Price] DECIMAL(18,2),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(255),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(255),
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(255)
);

CREATE TABLE [Books] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Title] NVARCHAR(255) COLLATE Vietnamese_CI_AS,
    [Isbn] NVARCHAR(255),
    [AuthorId] INT NOT NULL,
    [PublisherId] INT NOT NULL,
    [GenreId] INT,
    [Price] DECIMAL(18,2),
    [Description] NVARCHAR(MAX) COLLATE Vietnamese_CI_AS,
    [ImageUrl] NVARCHAR(255) DEFAULT ('https://www.scottishpoetrylibrary.org.uk/wp-content/uploads/2018/05/SnipImage_6-480x360.jpg'),
    [IsAvailable] BIT DEFAULT ((1)),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(50),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(50),
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(255),
    CONSTRAINT [FK_Books_Authors] FOREIGN KEY ([AuthorId]) REFERENCES [Authors]([Id]),
    CONSTRAINT [FK_Books_Publishers] FOREIGN KEY ([PublisherId]) REFERENCES [Publishers]([Id]),
    CONSTRAINT [FK_Books_Genres] FOREIGN KEY ([GenreId]) REFERENCES [Genres]([Id])
);
CREATE INDEX [IDX_BookTitle] ON [Books]([Title]);

CREATE TABLE [BookImages] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [ImageIdSeq]),
    [BookId] INT NOT NULL,
    [ImageUrl] NVARCHAR(500),
    [PublicId] NVARCHAR(255),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(255),
    CONSTRAINT [FK_BookImages_Books] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id])
);

CREATE TABLE [UserInfo] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [UserName] NVARCHAR(255),
    [PhoneNumber] NVARCHAR(11),
    [Gender] NVARCHAR(50),
    [DateOfBirth] DATETIME,
    [ImageUrl] NVARCHAR(255),
    [ImagePublicId] NVARCHAR(255)
);

CREATE TABLE [UserAuth] (
    [UserInfoId] INT PRIMARY KEY,
    [Email] NVARCHAR(255),
    [Password] NVARCHAR(255),
    [PasswordSalt] NVARCHAR(255),
    [CreatedAt] DATETIME,
    [LastLoggedIn] DATETIME,
    CONSTRAINT [FK_UserInfoId] FOREIGN KEY ([UserInfoId]) REFERENCES [UserInfo]([Id]),
    CONSTRAINT [UQ__UserAuth__A9D10534C7EE0EF7] UNIQUE ([Email])
);
CREATE INDEX [IX_UserAuth_Email] ON [UserAuth]([Email]);

CREATE TABLE [UserRole] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [PK__UserRole__AF2760AD9E7A240B] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK__UserRole__UserId__1D66518C] FOREIGN KEY ([UserId]) REFERENCES [UserInfo]([Id]),
    CONSTRAINT [FK__UserRole__RoleId__1E5A75C5] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id])
);

CREATE TABLE [Addresses] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [UserId] INT NOT NULL,
    [FullName] NVARCHAR(255),
    [Street] NVARCHAR(255),
    [Ward] NVARCHAR(255),
    [District] NVARCHAR(255),
    [City] NVARCHAR(255),
    [PostalCode] NVARCHAR(20),
    [UpdatedAt] DATETIME,
    CONSTRAINT [FK_Addresses_UserInfo] FOREIGN KEY ([UserId]) REFERENCES [UserInfo]([Id]),
    CONSTRAINT [UQ__Addresse__1788CC4DB0B3205A] UNIQUE ([UserId])
);

CREATE TABLE [Inventories] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [BookId] INT NOT NULL,
    [BookStoreId] INT,
    [Quantity] INT NOT NULL,
    [UnitOfMeasureId] INT,
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(255),
    [UpdatedAt] DATETIME DEFAULT (getdate()),
    [UpdatedBy] NVARCHAR(255),
    CONSTRAINT [FK_Inventories_Books] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id]),
    CONSTRAINT [FK_Inventories_BookStores] FOREIGN KEY ([BookStoreId]) REFERENCES [BookStores]([Id]),
    CONSTRAINT [FK_Inventories_UnitOfMeasures] FOREIGN KEY ([UnitOfMeasureId]) REFERENCES [UnitOfMeasures]([Id])
);
CREATE INDEX [IX_Inventories_BookStoreId] ON [Inventories]([BookId], [BookStoreId]);

CREATE TABLE [UnitMappings] (
    [SrcUnitId] INT NOT NULL,
    [DestUnitId] INT NOT NULL,
    [SrcUnitRate] DECIMAL(18,6),
    [DestUnitRate] DECIMAL(18,6),
    CONSTRAINT [PK__UnitMapp__1230A779431DD2A3] PRIMARY KEY ([SrcUnitId], [DestUnitId]),
    CONSTRAINT [FK_UnitMappings_SrcUnit] FOREIGN KEY ([SrcUnitId]) REFERENCES [UnitOfMeasures]([Id]),
    CONSTRAINT [FK_UnitMappings_DestUnit] FOREIGN KEY ([DestUnitId]) REFERENCES [UnitOfMeasures]([Id])
);

CREATE TABLE [ImportInventoryReceipts] (
    [Id] BIGINT PRIMARY KEY DEFAULT (NEXT VALUE FOR [ImportInventoryReceiptIdSeq]),
    [ImportReceiptCode] NVARCHAR(255),
    [TotalPrice] DECIMAL(18,2),
    [Status] NVARCHAR(255),
    [ImportNotes] NVARCHAR(255),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(255),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(255)
);

CREATE TABLE [ImportReceiptItems] (
    [Id] BIGINT PRIMARY KEY DEFAULT (NEXT VALUE FOR [ImportInventoryReceiptItemIdSeq]),
    [ImportReceiptId] BIGINT NOT NULL,
    CONSTRAINT [FK_ImportReceiptItems_ImportInventoryReceipts] FOREIGN KEY ([ImportReceiptId]) REFERENCES [ImportInventoryReceipts]([Id])
);

CREATE TABLE [ExportInventoryReceipts] (
    [Id] BIGINT PRIMARY KEY DEFAULT (NEXT VALUE FOR [ExportInventoryReceiptIdSeq]),
    [ExportReceiptCode] NVARCHAR(255),
    [TotalPrice] DECIMAL(18,2),
    [Status] NVARCHAR(255),
    [ExportNotes] NVARCHAR(255),
    [CreatedAt] DATETIME,
    [CreatedBy] NVARCHAR(255),
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(255)
);

CREATE TABLE [ExportReceiptItems] (
    [Id] BIGINT PRIMARY KEY DEFAULT (NEXT VALUE FOR [ExportInventoryReceiptItemIdSeq]),
    [ExportReceiptId] BIGINT NOT NULL,
    CONSTRAINT [FK_ExportReceiptItems_ExportInventoryReceipts] FOREIGN KEY ([ExportReceiptId]) REFERENCES [ExportInventoryReceipts]([Id])
);

CREATE TABLE [InventoryAudits] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [InventoryAuditIdSeq]),
    [AuditCode] NVARCHAR(255),
    [AuditDate] DATETIME,
    [AuditStatus] NVARCHAR(50),
    [AudittedBy] NVARCHAR(255),
    [AuditNotes] NVARCHAR(255),
    [CreatedAt] DATETIME,
    [IsDeleted] BIT DEFAULT ((0)),
    [DeletedAt] DATETIME,
    [DeletedBy] NVARCHAR(255)
);

CREATE TABLE [InventoryAuditDetails] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [InventoryAuditDetailsIdSeq]),
    [InventoryAuditId] INT NOT NULL,
    [BookId] INT NOT NULL,
    [BookStoreId] INT NOT NULL,
    [ISBN] NVARCHAR(255),
    [UnitOfMeasureId] INT NOT NULL,
    [UpdatedAt] DATETIME,
    [UpdatedBy] NVARCHAR(255),
    CONSTRAINT [FK_InventoryAuditDetails_InventoryAudits] FOREIGN KEY ([InventoryAuditId]) REFERENCES [InventoryAudits]([Id]),
    CONSTRAINT [FK_InventoryAuditDetails_Books] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id]),
    CONSTRAINT [FK_InventoryAuditDetails_BookStores] FOREIGN KEY ([BookStoreId]) REFERENCES [BookStores]([Id]),
    CONSTRAINT [FK_InventoryAuditDetails_UnitOfMeasures] FOREIGN KEY ([UnitOfMeasureId]) REFERENCES [UnitOfMeasures]([Id])
);

CREATE TABLE [InventoryAuditResults] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [InventoryAuditResultIdSeq]),
    [InventoryAuditDetailsId] INT NOT NULL,
    [BookId] INT NOT NULL,
    [BookStoreId] INT NOT NULL,
    [UnitOfMeasureId] INT NOT NULL,
    [ResultDetails] NVARCHAR(255),
    [ConductedBy] NVARCHAR(255),
    [ConductedAt] DATETIME,
    CONSTRAINT [FK_InventoryAuditResults_InventoryAuditDetails] FOREIGN KEY ([InventoryAuditDetailsId]) REFERENCES [InventoryAuditDetails]([Id]),
    CONSTRAINT [FK_InventoryAuditResults_Books] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id]),
    CONSTRAINT [FK_InventoryAuditResults_BookStores] FOREIGN KEY ([BookStoreId]) REFERENCES [BookStores]([Id]),
    CONSTRAINT [FK_InventoryAuditResults_UnitOfMeasures] FOREIGN KEY ([UnitOfMeasureId]) REFERENCES [UnitOfMeasures]([Id])
);

CREATE TABLE [Orders] (
    [Id] BIGINT PRIMARY KEY DEFAULT (NEXT VALUE FOR [OrderSeq]),
    [OrderDate] DATETIME,
    [UserEmail] NVARCHAR(255),
    [FullName] NVARCHAR(255),
    [Street] NVARCHAR(255),
    [Ward] NVARCHAR(255),
    [District] NVARCHAR(255),
    [City] NVARCHAR(255),
    [Subtotal] DECIMAL(18,2),
    [Status] NVARCHAR(255),
    [DeliveryMethodId] INT NOT NULL,
    CONSTRAINT [FK_Orders_DeliveryMethods] FOREIGN KEY ([DeliveryMethodId]) REFERENCES [DeliveryMethods]([Id])
);

CREATE TABLE [OrderItems] (
    [Id] BIGINT PRIMARY KEY DEFAULT (NEXT VALUE FOR [OrderItemSeq]),
    [OrderId] BIGINT NOT NULL,
    [BookTitle] NVARCHAR(255),
    [Price] DECIMAL(18,2),
    CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]) ON DELETE CASCADE
);

CREATE TABLE [LibriLogs] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [TimeStamp] DATETIME,
    [Level] INT,
    [Message] NVARCHAR(MAX),
    [Exception] NVARCHAR(MAX)
);

CREATE TABLE [ImportHistories] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [ImportHistoryIdSeq]),
    [BookId] INT NOT NULL,
    [BookStoreId] INT NOT NULL,
    [UnitOfMeasureId] INT NOT NULL,
    [ImportDate] DATETIME,
    [PerformedBy] NVARCHAR(255),
    CONSTRAINT [ImportHistories_Books] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id]),
    CONSTRAINT [ImportHistories_BookStores] FOREIGN KEY ([BookStoreId]) REFERENCES [BookStores]([Id]),
    CONSTRAINT [ImportHistories_UnitOfMeasureId] FOREIGN KEY ([UnitOfMeasureId]) REFERENCES [UnitOfMeasures]([Id])
);

CREATE TABLE [ExportHistories] (
    [Id] INT PRIMARY KEY DEFAULT (NEXT VALUE FOR [ExportHistoryIdSeq]),
    [BookId] INT NOT NULL,
    [BookStoreId] INT NOT NULL,
    [UnitOfMeasureId] INT NOT NULL,
    [ExportDate] DATETIME,
    [PerformedBy] NVARCHAR(255),
    CONSTRAINT [ExportHistories_Books] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id]),
    CONSTRAINT [ExportHistories_BookStores] FOREIGN KEY ([BookStoreId]) REFERENCES [BookStores]([Id]),
    CONSTRAINT [ExportHistories_UnitOfMeasureId] FOREIGN KEY ([UnitOfMeasureId]) REFERENCES [UnitOfMeasures]([Id])
);
GO