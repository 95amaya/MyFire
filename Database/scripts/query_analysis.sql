-- Active: 1691857044362@@127.0.0.1@3306@localdb

SELECT *
from bill_transactions
ORDER BY transaction_date DESC
LIMIT 100;

SELECT
    MONTH(transaction_date) as month_num,
    transaction_type,
    CONCAT('$', FORMAT(sum(amount), 2)) as `Monthly Total Income`
FROM bill_transactions
WHERE
    transaction_type = 'DEBIT'
    AND transaction_account = 'NEEDS'
    AND amount > 0
    AND description NOT LIKE '%ONLINE TRANSFER%'
GROUP BY
    MONTH(transaction_date),
    transaction_type;

SELECT @curr_month := MONTH(CURRENT_DATE) - 1;

SELECT
    'Total Income By Month',
    CONCAT('$', FORMAT(Jan_Total, 2)) as 'Jan',
    CONCAT('$', FORMAT(Feb_Total, 2)) as 'Jan',
    CONCAT('$', FORMAT(March_Total, 2)) as 'March',
    CONCAT('$', FORMAT(April_Total, 2)) as 'April',
    CONCAT('$', FORMAT(May_Total, 2)) as 'May',
    CONCAT('$', FORMAT(June_Total, 2)) as 'June',
    CONCAT('$', FORMAT(July_Total, 2)) as 'July',
    CONCAT('$', FORMAT(Aug_Total, 2)) as 'Aug',
    CONCAT('$', FORMAT(Sept_Total, 2)) as 'Sept',
    CONCAT('$', FORMAT(Oct_Total, 2)) as 'Oct',
    CONCAT('$', FORMAT(Nov_Total, 2)) as 'Nov',
    CONCAT('$', FORMAT(Dec_Total, 2)) as 'Dec',
    CONCAT('$', FORMAT(`YTD_Total`, 2)) as 'YTD Total',
    CONCAT('$', FORMAT(`Monthly_Avg`, 2)) as 'Monthly Average'
FROM (
        SELECT
            sum(
                case
                    when month_num = 1 then amount
                    else 0
                end
            ) as 'Jan_Total',
            sum(
                case
                    when month_num = 2 then amount
                    else 0
                end
            ) as 'Feb_Total',
            sum(
                case
                    when month_num = 3 then amount
                    else 0
                end
            ) as 'March_Total',
            sum(
                case
                    when month_num = 4 then amount
                    else 0
                end
            ) as 'April_Total',
            sum(
                case
                    when month_num = 5 then amount
                    else 0
                end
            ) as 'May_Total',
            sum(
                case
                    when month_num = 6 then amount
                    else 0
                end
            ) as 'June_Total',
            sum(
                case
                    when month_num = 7 then amount
                    else 0
                end
            ) as 'July_Total',
            sum(
                case
                    when month_num = 8 then amount
                    else 0
                end
            ) as 'Aug_Total',
            sum(
                case
                    when month_num = 9 then amount
                    else 0
                end
            ) as 'Sept_Total',
            sum(
                case
                    when month_num = 10 then amount
                    else 0
                end
            ) as 'Oct_Total',
            sum(
                case
                    when month_num = 11 then amount
                    else 0
                end
            ) as 'Nov_Total',
            sum(
                case
                    when month_num = 12 then amount
                    else 0
                end
            ) as 'Dec_Total',
            sum(amount) as 'YTD_Total',
            sum(amount) / @curr_month as 'Monthly_Avg'
        FROM (
                SELECT
                    MONTH(transaction_date) as month_num,
                    transaction_type,
                    amount
                FROM
                    bill_transactions
                WHERE
                    transaction_type = 'DEBIT'
                    AND transaction_account = 'NEEDS'
                    AND amount > 0
                    AND description NOT LIKE '%ONLINE TRANSFER%'
            ) as sub
    ) as total;

SELECT *
FROM bill_transactions
WHERE
    transaction_type = 'DEBIT'
    AND transaction_account = 'NEEDS'
    AND amount > 0
    AND MONTH(transaction_date) = 1;

-- AND description NOT LIKE '%ONLINE TRANSFER%';

SELECT *
FROM bill_transactions
WHERE
    transaction_type = 'DEBIT'
    AND MONTH(transaction_date) = 1;

SELECT *
FROM bill_transactions
WHERE
    MONTH(transaction_date) = 1;