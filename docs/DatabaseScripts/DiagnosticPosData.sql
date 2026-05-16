-- =============================================
-- DIAGNOSTIC: Check POS Data State
-- =============================================

SELECT COUNT(*) as TotalItems, SUM(CAST(ShowInPos as int)) as VisibleItems 
FROM LiquorItems;

SELECT * FROM PosCategories;

SELECT DISTINCT Category FROM LiquorItems WHERE ShowInPos = 1;
