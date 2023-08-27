-- Active: 1691857044362@@127.0.0.1@3306@localdb

DROP TABLE IF EXISTS bill_transactions;

CREATE TABLE
    bill_transactions(
        id int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
        transaction_date DATETIME NOT NULL COMMENT 'Transaction Date Time',
        amount FLOAT NOT NULL COMMENT '$$$ Money',
        `description` NVARCHAR(250) NOT NULL,
        transaction_type NVARCHAR(25) NOT NULL,
        transaction_account NVARCHAR(25) NOT NULL,
        CONSTRAINT uc_transaction UNIQUE (
            transaction_date,
            amount,
            description
        )
    ) COMMENT 'consolidated list of transactions from various accounts';

ALTER TABLE bill_transactions ADD is_noise BIT(1) NOT NULL DEFAULT 0;