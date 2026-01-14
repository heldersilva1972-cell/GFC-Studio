
-- Create BylawDocuments table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BylawDocuments')
BEGIN
    CREATE TABLE [dbo].[BylawDocuments] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Category] NVARCHAR(50) NOT NULL DEFAULT 'General',
        [Title] NVARCHAR(200) NOT NULL,
        [Content] NVARCHAR(MAX) NULL,
        [LastUpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastUpdatedBy] NVARCHAR(100) NULL,
        [CurrentVersion] INT NOT NULL DEFAULT 1
    );

    -- Insert default record if empty
    DECLARE @InitialContent NVARCHAR(MAX) = '<div class=''text-center mb-5''><h1>Constitution and By-Laws</h1><h2 class=''text-muted''>Gloucester Fraternity Club, Inc.</h2><p><strong>Revised:</strong> October 28, 1999<br><strong>Revised:</strong> February 27, 2003</p></div><hr><h3>Index</h3><ul><li>Preamble</li><li>Order of Business</li><li>Constitution</li><li>By-Laws</li></ul><hr><h3>Preamble</h3><p>The purpose and object of this club shall be to bring together at frequent intervals those who are interested in the civic and fraternal betterment of any nature, with the purpose of promoting the cause of good citizenship.</p><hr><h3>Order of Business</h3><ol><li>Call meeting to order</li><li>Salute to the flag</li><li>Moment of silence for all departed members</li><li>Roll call of Officers</li><li>Reading of minutes of previous meeting</li><li>Communications</li><li>Treasurer''s Report</li><li>Board of Directors'' Report</li><li>Report of regular and special committees, Hall Rental and Lottery</li><li>Unfinished business</li><li>New business</li><li>Good of the club<ul><li>Sick call</li><li>Bank night</li></ul></li><li>Adjournment</li></ol><hr><h2>Constitution</h2><h3>Article I – Name of Organization</h3><p>This organization shall be known as the <strong>Gloucester Fraternity Club, Inc.</strong> This official title shall be used in all written transactions of this organization.</p><hr><h3>Article II – Membership</h3><h4>Section I – Regular Membership</h4><p>To become a regular member of this organization, one must:</p><ul><li>Be a male of Portuguese descent</li><li>Be of good moral character</li><li>Have reached the age of twenty-one (21)</li></ul><p>There shall be forty-five (45) non-Portuguese regular members with American citizenship within the organization at any one time.</p><h4>Section II – Guest Membership</h4><p>To become a guest member, one must:</p><ul><li>Be a male American citizen</li><li>Be of good moral character</li><li>Have reached the age of twenty-one (21)</li></ul><p>Guest members:</p><ul><li>May use clubrooms when available</li><li>May attend club festivities</li><li>May serve on committees by invitation only</li><li>Shall <strong>not</strong> attend regular or special meetings</li><li>Shall <strong>not</strong> vote</li></ul><h4>Section III – Armed Services</h4><p>A regular member joining the Armed Forces shall be granted a leave of absence for four years, provided he remains in the service. Members shall be notified of any new laws pertaining to leave of absence.</p><hr><h3>Article III – Officers</h3><p>The officers shall consist of:</p><ul><li>President</li><li>Vice-President</li><li>Recording Secretary</li><li>Financial Secretary-Treasurer</li><li>Collector of Dues</li><li>Guide</li><li>Four Directors</li></ul><hr><h3>Article IV – Board of Directors</h3><p>Four regular members shall be elected as Directors. Together with the officers, they form the nine-man Board of Directors.</p><p>The Board shall:</p><ul><li>Formulate plans</li><li>Recommend procedures</li><li>Elect a Chairman and Vice-Chairman from among the Directors</li></ul><hr><h3>Article V – Membership Investigating Committee</h3><p>A five (5) member standing committee consisting of:</p><ul><li>Two (2) Directors</li><li>Collector of Dues</li><li>Two (2) regular members-at-large</li></ul><p>The committee elects its own Chairman and Vice-Chairman.</p><hr><h3>Article VI – Meetings</h3><h4>Section I</h4><p>Regular meetings shall be held monthly on the fourth Thursday at 7:00 p.m., unless rescheduled by the Board.</p><h4>Section II</h4><p>Special meetings may be called by:</p><ul><li>The Board of Directors, or</li><li>Written request of at least twenty (20) regular members</li></ul><h4>Section III</h4><p>A quorum of fifteen (15) members is required.</p><hr><h3>Article VII – Amendments</h3><p>Amendments must:</p><ul><li>Be submitted in writing</li><li>Be signed by at least three (3) members</li><li>Be read at three consecutive meetings</li></ul><p>Approval requires a two-thirds majority vote.</p><hr><h2>By-Laws</h2><h3>Article I – Membership</h3><h4>Section I – Application Procedure</h4><p>Applicants must:</p><ul><li>Be sponsored by a regular member in good standing</li><li>Complete an application</li><li>Submit the application and fee to the Collector of Dues</li></ul><h4>Section II – Election Procedure</h4><ul><li>Approved applications proceed to ballot vote</li><li>White balls elect; three (3) black balls reject</li><li>Rejected applicants may reapply after six months</li></ul><h4>Section III – Membership Limits</h4><ul><li>Regular membership capped at 300 paying members</li><li>Guest membership capped at 50% of regular membership</li></ul><h4>Section IV – Life Membership</h4><p>Life Membership is granted to any regular member who:</p><ul><li>Has served fifteen (15) consecutive years in good standing</li><li>Has attained age 65</li></ul><hr><h3>Article II – Dues, Fines, and Membership Fees</h3><ul><li>Membership fee: $10.00</li><li>Dues payable by January 1</li><li>Non-payment by February meeting results in expulsion unless illness is reported</li></ul><hr><h3>Article III – Nominations and Elections of Officers</h3><p>Includes:</p><ul><li>October nominations</li><li>Eligibility requirements</li><li>Absentee voting rules</li><li>Term limits</li><li>Tie election procedures</li></ul><hr><h3>Article IV – Duties of Officers</h3><p>Duties defined for:</p><ul><li>President</li><li>Vice-President</li><li>Recording Secretary</li><li>Financial Secretary-Treasurer</li><li>Collector of Dues</li><li>Guide</li><li>Board of Directors</li></ul><hr><h3>Article V – Membership Investigating Committee Duties</h3><p>Responsibilities include:</p><ul><li>Verifying applications</li><li>Conducting interviews</li><li>Investigating character references</li><li>Voting on application approval</li></ul><hr><h3>Article VI – Privileges and Conduct of Members</h3><p>Includes rules regarding:</p><ul><li>Meeting conduct</li><li>Discipline</li><li>Suspensions and appeals</li><li>Probation restrictions</li></ul><hr><h3>Article VII – Committees</h3><p>Committee chairmen are appointed by the President unless otherwise approved by membership vote.</p><hr><h3>Article VIII – Expenditures and Appropriations</h3><ul><li>Donations over $10 require two-thirds approval</li><li>Expenditures over $50 require approval</li><li>Withdrawals require authorized signatures</li></ul><hr><h3>Article IX – Interpretation</h3><p>Interpretation of these By-Laws shall be made by the Board of Directors.</p><hr><h3>Article X – Pledge</h3><blockquote class=''blockquote p-3 bg-light border-start border-4 border-primary''>I do most solemnly on my honor, promise to observe and abide by the rules and regulations of the Gloucester Fraternity Club, Inc., and that I will attend meetings as often as possible and strive to create a brotherly feeling among members.</blockquote><hr><h3>Constitution and By-Laws Committee</h3><ul><li>Ralph E. Goulart, Chairman</li><li>Richard A. Duwart</li><li>William P. Goulart III</li><li>David A. Rose, P.P.</li><li>John A. Vestal, P.P.</li></ul><p><strong>Accepted:</strong> October 1994</p>';

    IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[BylawDocuments])
    BEGIN
        INSERT INTO [dbo].[BylawDocuments] ([Category], [Title], [Content], [LastUpdatedBy], [CurrentVersion])
        VALUES ('Club', 'GFC Bylaws', @InitialContent, 'System', 1);
    END
    ELSE
    BEGIN
        -- Update the placeholder if it exists
        UPDATE [dbo].[BylawDocuments] 
        SET [Content] = @InitialContent 
        WHERE [Title] = 'GFC Bylaws' AND [Content] LIKE '%Welcome. Please edit this content%';
    END
END
GO

-- Create BylawRevisions table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BylawRevisions')
BEGIN
    CREATE TABLE [dbo].[BylawRevisions] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [DocumentId] INT NOT NULL,
        [Content] NVARCHAR(MAX) NULL,
        [RevisionDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [RevisionBy] NVARCHAR(100) NULL,
        [Version] INT NOT NULL,
        [ChangeReason] NVARCHAR(500) NULL,
        CONSTRAINT [FK_BylawRevisions_BylawDocuments] FOREIGN KEY ([DocumentId]) REFERENCES [dbo].[BylawDocuments] ([Id]) ON DELETE CASCADE
    );
END
GO
