-- =============================================
-- REPAIR POS ITEM VISIBILITY
-- Purpose: Force-enables all active liquor items for display in the POS terminal.
-- Run this if your POS terminal is showing an empty menu even when connected.
-- =============================================

UPDATE LiquorItems 
SET ShowInPos = 1 
WHERE IsActive = 1 
  AND (RetailPrice > 0 OR Category IS NOT NULL);

PRINT 'POS Item Visibility Repaired. Please refresh your POS terminals.';
