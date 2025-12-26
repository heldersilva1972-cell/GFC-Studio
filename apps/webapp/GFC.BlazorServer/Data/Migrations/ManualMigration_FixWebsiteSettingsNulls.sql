-- Fix NULL values in WebsiteSettings required string fields
-- Run this on your ClubMembership database

UPDATE [dbo].[WebsiteSettings]
SET 
    [ClubPhone] = ISNULL([ClubPhone], '(978) 283-3204'),
    [ClubAddress] = ISNULL([ClubAddress], '27 Webster Street, Gloucester, MA 01930'),
    [PrimaryColor] = ISNULL([PrimaryColor], '#D4AF37'),
    [SecondaryColor] = ISNULL([SecondaryColor], '#1A2332'),
    [HeadingFont] = ISNULL([HeadingFont], 'Playfair Display'),
    [BodyFont] = ISNULL([BodyFont], 'Inter'),
    [SeoTitle] = ISNULL([SeoTitle], 'Gloucester Fraternity Club'),
    [SeoDescription] = ISNULL([SeoDescription], 'Historic social club in Gloucester, Massachusetts'),
    [SeoKeywords] = ISNULL([SeoKeywords], 'gloucester, fraternity, club, social, massachusetts')
WHERE [Id] = 1;

PRINT 'Fixed NULL values in WebsiteSettings';
GO
