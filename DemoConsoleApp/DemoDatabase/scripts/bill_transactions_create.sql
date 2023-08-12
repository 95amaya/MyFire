-- Active: 1691857044362@@127.0.0.1@3306@localdb

CREATE TABLE
    bill_transactions(
        id int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
        transaction_date DATETIME COMMENT 'Transaction Date Time',
        amount FLOAT COMMENT '$$$ Money',
        `description` NVARCHAR(250),
        transaction_type NVARCHAR(25),
        transaction_account NVARCHAR(25)
    ) COMMENT 'consolidated list of transactions from various accounts';