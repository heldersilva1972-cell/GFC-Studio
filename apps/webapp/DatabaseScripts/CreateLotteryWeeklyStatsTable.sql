
CREATE TABLE [dbo].[LotteryWeeklyStats](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [WeekEndingDate] [date] NOT NULL,
    
    -- Online Section
    [OnlineNetSales] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineCommission] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineCashes] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineCashBonus] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineClaimsBonus] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineAdjustments] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineServiceFee] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineBondingFee] [decimal](18, 2) NOT NULL DEFAULT 0,
    [OnlineDue] [decimal](18, 2) NOT NULL DEFAULT 0,

    -- Instant Section
    [InstantGrossSales] [decimal](18, 2) NOT NULL DEFAULT 0,
    [InstantReturnSales] [decimal](18, 2) NOT NULL DEFAULT 0,
    [InstantCommission] [decimal](18, 2) NOT NULL DEFAULT 0,
    [InstantCashes] [decimal](18, 2) NOT NULL DEFAULT 0,
    [InstantCashBonus] [decimal](18, 2) NOT NULL DEFAULT 0,
    [InstantClaimsBonus] [decimal](18, 2) NOT NULL DEFAULT 0,
    [InstantAdjustments] [decimal](18, 2) NOT NULL DEFAULT 0,
    [InstantDue] [decimal](18, 2) NOT NULL DEFAULT 0,

    -- Totals
    [TotalDue] [decimal](18, 2) NOT NULL DEFAULT 0,

    -- Audit
    [CreatedBy] [nvarchar](100) NULL,
    [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
    
 CONSTRAINT [PK_LotteryWeeklyStats] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)
) ON [PRIMARY]
GO
