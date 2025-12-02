-- SQLite Database Schema for User and ConnectionString tables

-- Create User table
CREATE TABLE IF NOT EXISTS User (
    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
    UserName TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL
);

-- Create ConnectionString table with Type field
CREATE TABLE IF NOT EXISTS ConnectionString (
    ConnectionStringId INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    ConnectionStringName TEXT NOT NULL,
    ConnectionStringValue TEXT NOT NULL,
    Type TEXT NOT NULL CHECK(Type IN ('postgresql', 'mysql', 'sqlserver')),
    FOREIGN KEY (UserId) REFERENCES User(UserId) ON DELETE CASCADE
);

-- Create index for better performance
CREATE INDEX IF NOT EXISTS idx_connectionstring_userid ON ConnectionString(UserId);
CREATE INDEX IF NOT EXISTS idx_connectionstring_type ON ConnectionString(Type);