
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/20/2023 18:17:36
-- Generated from EDMX file: E:\CDAC\pixelSpot\AspNetPixelSpot\PixelSpot\PixelSpot1\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PixelSpot];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Downlaods_ToPhoto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Downlaods] DROP CONSTRAINT [FK_Downlaods_ToPhoto];
GO
IF OBJECT_ID(N'[dbo].[FK_Downlaods_ToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Downlaods] DROP CONSTRAINT [FK_Downlaods_ToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_Liked_photos_Photos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Liked_photos] DROP CONSTRAINT [FK_Liked_photos_Photos];
GO
IF OBJECT_ID(N'[dbo].[FK_Liked_photos_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Liked_photos] DROP CONSTRAINT [FK_Liked_photos_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Photos_PCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Photos_PCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_Photos_UCollection]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Photos_UCollection];
GO
IF OBJECT_ID(N'[dbo].[FK_Photos_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Photos_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Spam_photos_Photo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Spam_photos] DROP CONSTRAINT [FK_Spam_photos_Photo];
GO
IF OBJECT_ID(N'[dbo].[FK_User_collection_CC]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User_collection] DROP CONSTRAINT [FK_User_collection_CC];
GO
IF OBJECT_ID(N'[dbo].[FK_User_collection_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[User_collection] DROP CONSTRAINT [FK_User_collection_Users];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Collection_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Collection_category];
GO
IF OBJECT_ID(N'[dbo].[Downlaods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Downlaods];
GO
IF OBJECT_ID(N'[dbo].[Liked_photos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Liked_photos];
GO
IF OBJECT_ID(N'[dbo].[Photo_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Photo_category];
GO
IF OBJECT_ID(N'[dbo].[Photos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Photos];
GO
IF OBJECT_ID(N'[dbo].[Spam_photos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Spam_photos];
GO
IF OBJECT_ID(N'[dbo].[User_collection]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User_collection];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Collection_category'
CREATE TABLE [dbo].[Collection_category] (
    [cc_id] int IDENTITY(1,1) NOT NULL,
    [cc_name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Downlaods'
CREATE TABLE [dbo].[Downlaods] (
    [d_id] int IDENTITY(1,1) NOT NULL,
    [p_id] int  NULL,
    [u_id] int  NULL
);
GO

-- Creating table 'Liked_photos'
CREATE TABLE [dbo].[Liked_photos] (
    [l_id] int IDENTITY(1,1) NOT NULL,
    [u_id] int  NOT NULL,
    [p_id] int  NOT NULL
);
GO

-- Creating table 'Photo_category'
CREATE TABLE [dbo].[Photo_category] (
    [pc_id] int IDENTITY(1,1) NOT NULL,
    [pc_name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Photos'
CREATE TABLE [dbo].[Photos] (
    [p_id] int IDENTITY(1,1) NOT NULL,
    [p_name] nvarchar(200)  NOT NULL,
    [p_state] bit  NOT NULL,
    [p_degree] bit  NOT NULL,
    [u_id] int  NOT NULL,
    [uc_id] int  NULL,
    [pc_id] int  NULL,
    [p_datetime] datetime  NOT NULL,
    [p_profilePhoto] bit  NULL
);
GO

-- Creating table 'Spam_photos'
CREATE TABLE [dbo].[Spam_photos] (
    [sp_id] int IDENTITY(1,1) NOT NULL,
    [sp_details] nvarchar(50)  NOT NULL,
    [p_id] int  NOT NULL
);
GO

-- Creating table 'User_collection'
CREATE TABLE [dbo].[User_collection] (
    [uc_id] int IDENTITY(1,1) NOT NULL,
    [u_id] int  NULL,
    [cc_id] int  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [u_id] int IDENTITY(1,1) NOT NULL,
    [u_first_name] nvarchar(20)  NOT NULL,
    [u_last_name] nvarchar(20)  NOT NULL,
    [u_email] nvarchar(50)  NOT NULL,
    [u_password] nvarchar(20)  NOT NULL,
    [u_address] nvarchar(max)  NULL,
    [u_mobile] nchar(10)  NULL,
    [u_about] nvarchar(max)  NULL,
    [u_role] nvarchar(20)  NOT NULL,
    [u_status] bit  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [cc_id] in table 'Collection_category'
ALTER TABLE [dbo].[Collection_category]
ADD CONSTRAINT [PK_Collection_category]
    PRIMARY KEY CLUSTERED ([cc_id] ASC);
GO

-- Creating primary key on [d_id] in table 'Downlaods'
ALTER TABLE [dbo].[Downlaods]
ADD CONSTRAINT [PK_Downlaods]
    PRIMARY KEY CLUSTERED ([d_id] ASC);
GO

-- Creating primary key on [l_id] in table 'Liked_photos'
ALTER TABLE [dbo].[Liked_photos]
ADD CONSTRAINT [PK_Liked_photos]
    PRIMARY KEY CLUSTERED ([l_id] ASC);
GO

-- Creating primary key on [pc_id] in table 'Photo_category'
ALTER TABLE [dbo].[Photo_category]
ADD CONSTRAINT [PK_Photo_category]
    PRIMARY KEY CLUSTERED ([pc_id] ASC);
GO

-- Creating primary key on [p_id] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [PK_Photos]
    PRIMARY KEY CLUSTERED ([p_id] ASC);
GO

-- Creating primary key on [sp_id] in table 'Spam_photos'
ALTER TABLE [dbo].[Spam_photos]
ADD CONSTRAINT [PK_Spam_photos]
    PRIMARY KEY CLUSTERED ([sp_id] ASC);
GO

-- Creating primary key on [uc_id] in table 'User_collection'
ALTER TABLE [dbo].[User_collection]
ADD CONSTRAINT [PK_User_collection]
    PRIMARY KEY CLUSTERED ([uc_id] ASC);
GO

-- Creating primary key on [u_id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([u_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [cc_id] in table 'User_collection'
ALTER TABLE [dbo].[User_collection]
ADD CONSTRAINT [FK_User_collection_CC]
    FOREIGN KEY ([cc_id])
    REFERENCES [dbo].[Collection_category]
        ([cc_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_collection_CC'
CREATE INDEX [IX_FK_User_collection_CC]
ON [dbo].[User_collection]
    ([cc_id]);
GO

-- Creating foreign key on [p_id] in table 'Downlaods'
ALTER TABLE [dbo].[Downlaods]
ADD CONSTRAINT [FK_Downlaods_ToPhoto]
    FOREIGN KEY ([p_id])
    REFERENCES [dbo].[Photos]
        ([p_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Downlaods_ToPhoto'
CREATE INDEX [IX_FK_Downlaods_ToPhoto]
ON [dbo].[Downlaods]
    ([p_id]);
GO

-- Creating foreign key on [u_id] in table 'Downlaods'
ALTER TABLE [dbo].[Downlaods]
ADD CONSTRAINT [FK_Downlaods_ToUser]
    FOREIGN KEY ([u_id])
    REFERENCES [dbo].[Users]
        ([u_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Downlaods_ToUser'
CREATE INDEX [IX_FK_Downlaods_ToUser]
ON [dbo].[Downlaods]
    ([u_id]);
GO

-- Creating foreign key on [p_id] in table 'Liked_photos'
ALTER TABLE [dbo].[Liked_photos]
ADD CONSTRAINT [FK_Liked_photos_Photos]
    FOREIGN KEY ([p_id])
    REFERENCES [dbo].[Photos]
        ([p_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Liked_photos_Photos'
CREATE INDEX [IX_FK_Liked_photos_Photos]
ON [dbo].[Liked_photos]
    ([p_id]);
GO

-- Creating foreign key on [u_id] in table 'Liked_photos'
ALTER TABLE [dbo].[Liked_photos]
ADD CONSTRAINT [FK_Liked_photos_Users]
    FOREIGN KEY ([u_id])
    REFERENCES [dbo].[Users]
        ([u_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Liked_photos_Users'
CREATE INDEX [IX_FK_Liked_photos_Users]
ON [dbo].[Liked_photos]
    ([u_id]);
GO

-- Creating foreign key on [pc_id] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [FK_Photos_PCategory]
    FOREIGN KEY ([pc_id])
    REFERENCES [dbo].[Photo_category]
        ([pc_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Photos_PCategory'
CREATE INDEX [IX_FK_Photos_PCategory]
ON [dbo].[Photos]
    ([pc_id]);
GO

-- Creating foreign key on [uc_id] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [FK_Photos_UCollection]
    FOREIGN KEY ([uc_id])
    REFERENCES [dbo].[User_collection]
        ([uc_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Photos_UCollection'
CREATE INDEX [IX_FK_Photos_UCollection]
ON [dbo].[Photos]
    ([uc_id]);
GO

-- Creating foreign key on [u_id] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [FK_Photos_Users]
    FOREIGN KEY ([u_id])
    REFERENCES [dbo].[Users]
        ([u_id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Photos_Users'
CREATE INDEX [IX_FK_Photos_Users]
ON [dbo].[Photos]
    ([u_id]);
GO

-- Creating foreign key on [p_id] in table 'Spam_photos'
ALTER TABLE [dbo].[Spam_photos]
ADD CONSTRAINT [FK_Spam_photos_Photo]
    FOREIGN KEY ([p_id])
    REFERENCES [dbo].[Photos]
        ([p_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Spam_photos_Photo'
CREATE INDEX [IX_FK_Spam_photos_Photo]
ON [dbo].[Spam_photos]
    ([p_id]);
GO

-- Creating foreign key on [u_id] in table 'User_collection'
ALTER TABLE [dbo].[User_collection]
ADD CONSTRAINT [FK_User_collection_Users]
    FOREIGN KEY ([u_id])
    REFERENCES [dbo].[Users]
        ([u_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_User_collection_Users'
CREATE INDEX [IX_FK_User_collection_Users]
ON [dbo].[User_collection]
    ([u_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------