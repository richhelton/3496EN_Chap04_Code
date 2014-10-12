
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/25/2014 20:44:27
-- Generated from EDMX file: F:\3496_Chap04\SagaPaymentClient\MVCApp - WCF\MVCApp\DAL\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MVCApp];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Paymessage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Paymessage];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Paymessages'
CREATE TABLE [dbo].[Paymessages] (
    [id] int  NOT NULL,
    [EventId] uniqueidentifier  NOT NULL,
    [error] varchar(50)  NULL,
    [state] varchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'Paymessages'
ALTER TABLE [dbo].[Paymessages]
ADD CONSTRAINT [PK_Paymessages]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------