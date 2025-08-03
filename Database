Create Database
CREATE DATABASE UserLoginDB;
GO

USE UserLoginDB;
GO

-- Create Users Table
CREATE TABLE Users (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    PhoneNumber NVARCHAR(20) NOT NULL UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Insert Sample Data
INSERT INTO Users (Name, Age, PhoneNumber) VALUES 
('John Smith', 25, '01234567890'),
('Sarah Johnson', 30, '01098765432'),
('Mike Wilson', 28, '01555123456');
GO
