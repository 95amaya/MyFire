CREATE TABLE
    bill_transactions(
        id int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
        transaction_date DATETIME COMMENT 'Transaction Date Time',
        amount FLOAT COMMENT '$$ Money' transaction_type NVARCHAR(25) transaction_account NVARCHAR(25)
    ) COMMENT '';