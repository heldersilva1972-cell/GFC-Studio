-- Fix existing ActiveEvents that are missing their TemplateId link.
-- This happened because StartBanquet() in the standalone terminal was not
-- setting TemplateId when creating the ActiveEvent object.
--
-- This update links each open ActiveEvent to its EventTemplate by matching on Name.
-- Run this once to fix any banquets that were already started before the code fix.

UPDATE ae
SET ae.TemplateId = et.Id
FROM [dbo].[ActiveEvents] ae
INNER JOIN [dbo].[EventTemplates] et ON et.Name = ae.Name AND et.IsDeleted = 0
WHERE ae.TemplateId IS NULL
  AND ae.Status = 0  -- 0 = Open
  AND ae.IsDeleted = 0;

SELECT 
    ae.Id        AS EventId,
    ae.Name      AS EventName,
    ae.TemplateId AS LinkedTemplateId,
    et.ItemsOverrideJson AS HasOverrideJson
FROM [dbo].[ActiveEvents] ae
LEFT JOIN [dbo].[EventTemplates] et ON et.Id = ae.TemplateId
WHERE ae.Status = 0 AND ae.IsDeleted = 0;
