-- Add PaymentType column to DuesPayments table
-- This fixes the "Past Due" issue where payments are not being recognized

-- Check if column exists before adding
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[DuesPayments]') 
    AND name = 'PaymentType'
)
BEGIN
    ALTER TABLE [dbo].[DuesPayments]
    ADD [PaymentType] NVARCHAR(50) NULL;
    
    PRINT 'PaymentType column added to DuesPayments table';
END
ELSE
BEGIN
    PRINT 'PaymentType column already exists in DuesPayments table';
END
GO

-- Update existing records to set PaymentType to 'CASH' where PaidDate is not null
-- This ensures existing payments are recognized
UPDATE [dbo].[DuesPayments]
SET [PaymentType] = 'CASH'
WHERE [PaidDate] IS NOT NULL 
  AND ([PaymentType] IS NULL OR [PaymentType] = '');

PRINT 'Updated existing payment records with default PaymentType';
GO
