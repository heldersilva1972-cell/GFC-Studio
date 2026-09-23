// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace GFC.BlazorServer.Services
{
    public class WebsiteSettingsService : GFC.Core.Interfaces.IWebsiteSettingsService
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "WebsiteSettings";

        public WebsiteSettingsService(IDbContextFactory<GfcDbContext> contextFactory, IMemoryCache cache)
        {
            _contextFactory = contextFactory;
            _cache = cache;
        }

        public async Task<WebsiteSettings> GetWebsiteSettingsAsync()
        {
            if (_cache.TryGetValue(CacheKey, out object? cachedObj) && cachedObj is WebsiteSettings cachedSettings)
            {
                return cachedSettings;
            }

            WebsiteSettings? settings = null;
            try
            {
                await HealDatabaseAsync();
                await using var _context = await _contextFactory.CreateDbContextAsync();
                try
                {
                    settings = await _context.WebsiteSettings.AsNoTracking().OrderBy(s => s.Id).FirstOrDefaultAsync();
                }
                catch (Exception efEx)
                {
                    Console.WriteLine($"EF query notice: {efEx.Message}. Falling back to direct SQL read.");
                }

                if (settings == null)
                {
                    // Direct SQL Reader Fallback
                    var conn = _context.Database.GetDbConnection();
                    if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync();
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT TOP 1 * FROM [dbo].[WebsiteSettings] ORDER BY [Id]";
                    using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        settings = new WebsiteSettings();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var col = reader.GetName(i);
                            if (reader.IsDBNull(i)) continue;
                            var val = reader.GetValue(i);
                            
                            switch (col.ToLowerInvariant())
                            {
                                case "id": settings.Id = Convert.ToInt32(val); break;
                                case "enablesandboxmode": settings.EnableSandboxMode = Convert.ToBoolean(val); break;
                                case "sandboxcalendarname": settings.SandboxCalendarName = val?.ToString() ?? ""; break;
                                case "sandboxtestemail": settings.SandboxTestEmail = val?.ToString(); break;
                                case "sandboxgooglecalendarid": settings.SandboxGoogleCalendarId = val?.ToString(); break;
                                case "sandboxcalendarfeedurl": settings.SandboxCalendarFeedUrl = val?.ToString(); break;
                                case "roomsjson": settings.RoomsJson = val?.ToString(); break;
                                case "addonsjson": settings.AddonsJson = val?.ToString(); break;
                                case "rentalformtitle": settings.RentalFormTitle = val?.ToString() ?? ""; break;
                                case "rentalformsubtitle": settings.RentalFormSubtitle = val?.ToString() ?? ""; break;
                                case "rentalformintrotext": settings.RentalFormIntroText = val?.ToString() ?? ""; break;
                                case "rentalformrulestext": settings.RentalFormRulesText = val?.ToString() ?? ""; break;
                                case "rentalformsuccessmessage": settings.RentalFormSuccessMessage = val?.ToString() ?? ""; break;
                                case "showaddressfield": settings.ShowAddressField = Convert.ToBoolean(val); break;
                                case "showguestcountfield": settings.ShowGuestCountField = Convert.ToBoolean(val); break;
                                case "requireapplicantname": settings.RequireApplicantName = Convert.ToBoolean(val); break;
                                case "requireemail": settings.RequireEmail = Convert.ToBoolean(val); break;
                                case "requirephone": settings.RequirePhone = Convert.ToBoolean(val); break;
                                case "requireaddress": settings.RequireAddress = Convert.ToBoolean(val); break;
                                case "requireeventtype": settings.RequireEventType = Convert.ToBoolean(val); break;
                                case "requireguestcount": settings.RequireGuestCount = Convert.ToBoolean(val); break;
                                case "formingestionmode": settings.FormIngestionMode = val?.ToString() ?? "NativeForm"; break;
                                case "notificationemaillist": settings.NotificationEmailList = val?.ToString() ?? ""; break;
                                case "notifyonnewsubmission": settings.NotifyOnNewSubmission = Convert.ToBoolean(val); break;
                                case "notifyonpaymentrecorded": settings.NotifyOnPaymentRecorded = Convert.ToBoolean(val); break;
                                case "sendapplicantconfirmation": settings.SendApplicantConfirmation = Convert.ToBoolean(val); break;
                                case "applicantconfirmationemailsubject": settings.ApplicantConfirmationEmailSubject = val?.ToString() ?? "Your Hall Rental Application Confirmation - {ClubName}"; break;
                                case "applicantconfirmationemailbody": settings.ApplicantConfirmationEmailBody = val?.ToString() ?? ""; break;
                                case "rentalemailprovider": settings.RentalEmailProvider = val?.ToString() ?? "SMTP"; break;
                                case "rentalsmtphost": settings.RentalSmtpHost = val?.ToString() ?? "mail.gloucesterfraternityclub.com"; break;
                                case "rentalsmtpport": settings.RentalSmtpPort = Convert.ToInt32(val); break;
                                case "rentalsmtpusername": settings.RentalSmtpUsername = val?.ToString(); break;
                                case "rentalsmtppassword": settings.RentalSmtpPassword = val?.ToString(); break;
                                case "rentalsmtpenablessl": settings.RentalSmtpEnableSsl = Convert.ToBoolean(val); break;
                                case "rentalresendapikey": settings.RentalResendApiKey = val?.ToString(); break;
                                case "rentalsenderemail": settings.RentalSenderEmail = val?.ToString() ?? "rentals@gloucesterfraternityclub.com"; break;
                                case "rentalsendername": settings.RentalSenderName = val?.ToString() ?? "Gloucester Fraternity Club - Hall Rentals"; break;
                                case "rentalemailcc": settings.RentalEmailCc = val?.ToString(); break;
                                case "dayschedulesjson": settings.DaySchedulesJson = val?.ToString(); break;
                                case "notifyonstatuschange": settings.NotifyOnStatusChange = Convert.ToBoolean(val); break;
                                case "requiresecuritydeposit": settings.RequireSecurityDeposit = Convert.ToBoolean(val); break;
                                case "securitydepositamount": settings.SecurityDepositAmount = Convert.ToDecimal(val); break;
                                case "functionhallnonmemberrate": settings.FunctionHallNonMemberRate = Convert.ToDecimal(val); break;
                                case "functionhallmemberrate": settings.FunctionHallMemberRate = Convert.ToDecimal(val); break;
                                case "coalitionnonmemberrate": settings.CoalitionNonMemberRate = Convert.ToDecimal(val); break;
                                case "coalitionmemberrate": settings.CoalitionMemberRate = Convert.ToDecimal(val); break;
                                case "youthorganizationnonmemberrate": settings.YouthOrganizationNonMemberRate = Convert.ToDecimal(val); break;
                                case "youthorganizationmemberrate": settings.YouthOrganizationMemberRate = Convert.ToDecimal(val); break;
                                case "bartenderservicefee": settings.BartenderServiceFee = Convert.ToDecimal(val); break;
                                case "kitchenfee": settings.KitchenFee = Convert.ToDecimal(val); break;
                                case "avequipmentfee": settings.AvEquipmentFee = Convert.ToDecimal(val); break;
                                case "basefunctionhours": settings.BaseFunctionHours = Convert.ToInt32(val); break;
                                case "allowadditionalhours": settings.AllowAdditionalHours = Convert.ToBoolean(val); break;
                                case "additionalhourrate": settings.AdditionalHourRate = Convert.ToDecimal(val); break;
                                case "nonprofitrate": settings.NonProfitRate = Convert.ToDecimal(val); break;
                                case "maxhallrentaldurationhours": settings.MaxHallRentalDurationHours = Convert.ToInt32(val); break;
                                case "isclubopen": settings.IsClubOpen = Convert.ToBoolean(val); break;
                                case "masteremailkillswitch": settings.MasterEmailKillSwitch = Convert.ToBoolean(val); break;
                                case "highaccessibilitymode": settings.HighAccessibilityMode = Convert.ToBoolean(val); break;
                                case "enableonlinerentalspayment": settings.EnableOnlineRentalsPayment = Convert.ToBoolean(val); break;
                                case "paymentgatewayurl": settings.PaymentGatewayUrl = val?.ToString(); break;
                                case "paymentgatewayapikey": settings.PaymentGatewayApiKey = val?.ToString(); break;
                                case "primarycolor": settings.PrimaryColor = val?.ToString() ?? "#0D1B2A"; break;
                                case "secondarycolor": settings.SecondaryColor = val?.ToString() ?? "#FFD700"; break;
                                case "headingfont": settings.HeadingFont = val?.ToString() ?? "Outfit"; break;
                                case "bodyfont": settings.BodyFont = val?.ToString() ?? "Inter"; break;
                                case "clubphone": settings.ClubPhone = val?.ToString() ?? ""; break;
                                case "clubaddress": settings.ClubAddress = val?.ToString() ?? ""; break;
                                case "rentaltermsandconditionstext": settings.RentalTermsAndConditionsText = val?.ToString() ?? ""; break;
                                case "rentalcancellationpolicytext": settings.RentalCancellationPolicyText = val?.ToString() ?? ""; break;
                                case "rentalkitchenpolicytext": settings.RentalKitchenPolicyText = val?.ToString() ?? ""; break;
                                case "rentalpolicyurl": settings.RentalPolicyUrl = val?.ToString() ?? ""; break;
                                case "rentalpaymentwindownotice": settings.RentalPaymentWindowNotice = val?.ToString() ?? ""; break;
                                case "policiesjson": settings.PoliciesJson = val?.ToString(); break;
                                case "seotitle": settings.SeoTitle = val?.ToString() ?? ""; break;
                                case "seodescription": settings.SeoDescription = val?.ToString() ?? ""; break;
                                case "seokeywords": settings.SeoKeywords = val?.ToString() ?? ""; break;
                            }
                        }
                    }
                }

                if (settings != null)
                {
                    // Apply defaults for any NULL pricing fields
                    settings.FunctionHallNonMemberRate ??= 400;
                    settings.FunctionHallMemberRate ??= 300;
                    settings.CoalitionNonMemberRate ??= 200;
                    settings.CoalitionMemberRate ??= 100;
                    settings.YouthOrganizationNonMemberRate ??= 100;
                    settings.YouthOrganizationMemberRate ??= 100;
                    settings.BartenderServiceFee ??= 100;
                    settings.BaseFunctionHours ??= 5;
                    settings.AdditionalHourRate ??= 50;
                    settings.KitchenFee ??= 50;
                    settings.AvEquipmentFee ??= 25;
                    settings.SecurityDepositAmount ??= 200;
                    settings.MaxHallRentalDurationHours ??= 8;
                }
                else
                {
                    settings = new WebsiteSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed reading WebsiteSettings from DB ({ex.Message}), utilizing memory defaults.");
                settings = new WebsiteSettings();
            }

            _cache.Set(CacheKey, settings, TimeSpan.FromMinutes(30));
            return settings;
        }

        public async Task UpdateWebsiteSettingsAsync(WebsiteSettings settings)
        {
            try
            {
                await using var _context = await _contextFactory.CreateDbContextAsync();

                // FORCE all pricing fields to have values
                settings.FunctionHallNonMemberRate ??= 400;
                settings.FunctionHallMemberRate ??= 300;
                settings.CoalitionNonMemberRate ??= 200;
                settings.CoalitionMemberRate ??= 100;
                settings.YouthOrganizationNonMemberRate ??= 100;
                settings.YouthOrganizationMemberRate ??= 100;
                settings.BartenderServiceFee ??= 100;
                settings.BaseFunctionHours ??= 5;
                settings.AdditionalHourRate ??= 50;
                settings.KitchenFee ??= 50;
                settings.AvEquipmentFee ??= 25;
                settings.SecurityDepositAmount ??= 200;
                settings.MemberRate ??= 0;
                settings.NonMemberRate ??= 0;
                settings.NonProfitRate ??= 200;
                settings.MaxHallRentalDurationHours ??= 8;

                var sql = @"
                    IF EXISTS (SELECT 1 FROM [dbo].[WebsiteSettings])
                    BEGIN
                        UPDATE [dbo].[WebsiteSettings]
                        SET 
                            [FunctionHallNonMemberRate] = @p0,
                            [FunctionHallMemberRate] = @p1,
                            [CoalitionNonMemberRate] = @p2,
                            [CoalitionMemberRate] = @p3,
                            [YouthOrganizationNonMemberRate] = @p4,
                            [YouthOrganizationMemberRate] = @p5,
                            [BartenderServiceFee] = @p6,
                            [KitchenFee] = @p7,
                            [AvEquipmentFee] = @p8,
                            [SecurityDepositAmount] = @p9,
                            [BaseFunctionHours] = @p10,
                            [AdditionalHourRate] = @p11,
                            [MemberRate] = @p12,
                            [NonMemberRate] = @p13,
                            [NonProfitRate] = @p14,
                            [MaxHallRentalDurationHours] = @p15,
                            [RequireSecurityDeposit] = @p16,
                            [RoomsJson] = @p17,
                            [AddonsJson] = @p18,
                            [RentalFormTitle] = @p19,
                            [RentalFormSubtitle] = @p20,
                            [RentalFormIntroText] = @p21,
                            [RentalFormRulesText] = @p22,
                            [RentalFormSuccessMessage] = @p23,
                            [ShowAddressField] = @p24,
                            [ShowGuestCountField] = @p25,
                            [EnableSandboxMode] = @p26,
                            [SandboxCalendarName] = @p27,
                            [SandboxTestEmail] = @p28,
                            [SandboxGoogleCalendarId] = @p29,
                            [SandboxCalendarFeedUrl] = @p30,
                            [FormIngestionMode] = @p31,
                            [NotificationEmailList] = @p32,
                            [NotifyOnNewSubmission] = @p33,
                            [NotifyOnPaymentRecorded] = @p34,
                            [SendApplicantConfirmation] = @p35,
                            [NotifyOnStatusChange] = @p36,
                            [DaySchedulesJson] = @p38,
                            [ApplicantConfirmationEmailSubject] = @p39,
                            [ApplicantConfirmationEmailBody] = @p40,
                            [RentalEmailProvider] = @p41,
                            [RentalSmtpHost] = @p42,
                            [RentalSmtpPort] = @p43,
                            [RentalSmtpUsername] = @p44,
                            [RentalSmtpPassword] = @p45,
                            [RentalSmtpEnableSsl] = @p46,
                            [RentalResendApiKey] = @p47,
                            [RentalSenderEmail] = @p48,
                            [RentalSenderName] = @p49,
                            [RentalEmailCc] = @p50,
                            [RequireApplicantName] = @p51,
                            [RequireEmail] = @p52,
                            [RequirePhone] = @p53,
                            [RequireAddress] = @p54,
                            [RequireEventType] = @p55,
                            [RequireGuestCount] = @p56,
                            [AllowAdditionalHours] = @p57,
                            [RentalTermsAndConditionsText] = @p58,
                            [RentalCancellationPolicyText] = @p59,
                            [RentalKitchenPolicyText] = @p60,
                            [RentalPolicyUrl] = @p61,
                            [RentalPaymentWindowNotice] = @p62,
                            [PoliciesJson] = @p63
                        WHERE [Id] = CASE 
                            WHEN @p37 IS NOT NULL AND @p37 > 0 AND EXISTS (SELECT 1 FROM [dbo].[WebsiteSettings] WHERE [Id] = @p37) THEN @p37
                            ELSE (SELECT TOP 1 [Id] FROM [dbo].[WebsiteSettings] ORDER BY [Id])
                        END;
                    END
                    ELSE
                    BEGIN
                        INSERT INTO [dbo].[WebsiteSettings] (
                            [FunctionHallNonMemberRate], [FunctionHallMemberRate], [CoalitionNonMemberRate], [CoalitionMemberRate],
                            [YouthOrganizationNonMemberRate], [YouthOrganizationMemberRate], [BartenderServiceFee], [KitchenFee],
                            [AvEquipmentFee], [SecurityDepositAmount], [BaseFunctionHours], [AdditionalHourRate],
                            [MemberRate], [NonMemberRate], [NonProfitRate], [MaxHallRentalDurationHours],
                            [RequireSecurityDeposit], [RoomsJson], [AddonsJson], [RentalFormTitle],
                            [RentalFormSubtitle], [RentalFormIntroText], [RentalFormRulesText], [RentalFormSuccessMessage],
                            [ShowAddressField], [ShowGuestCountField], [EnableSandboxMode], [SandboxCalendarName],
                            [SandboxTestEmail], [SandboxGoogleCalendarId], [SandboxCalendarFeedUrl], [FormIngestionMode],
                            [NotificationEmailList], [NotifyOnNewSubmission], [NotifyOnPaymentRecorded],
                            [SendApplicantConfirmation], [NotifyOnStatusChange], [IsClubOpen], [MasterEmailKillSwitch],
                            [HighAccessibilityMode], [EnableOnlineRentalsPayment], [DaySchedulesJson],
                            [ApplicantConfirmationEmailSubject], [ApplicantConfirmationEmailBody],
                            [RentalEmailProvider], [RentalSmtpHost], [RentalSmtpPort], [RentalSmtpUsername],
                            [RentalSmtpPassword], [RentalSmtpEnableSsl], [RentalResendApiKey], [RentalSenderEmail],
                            [RentalSenderName], [RentalEmailCc],
                            [RequireApplicantName], [RequireEmail], [RequirePhone], [RequireAddress], [RequireEventType], [RequireGuestCount],
                            [AllowAdditionalHours],
                            [RentalTermsAndConditionsText], [RentalCancellationPolicyText], [RentalKitchenPolicyText], [RentalPolicyUrl], [RentalPaymentWindowNotice],
                            [PoliciesJson]
                        ) VALUES (
                            @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15,
                            @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29,
                            @p30, @p31, @p32, @p33, @p34, @p35, @p36, 1, 0, 0, 0, @p38, @p39, @p40,
                            @p41, @p42, @p43, @p44, @p45, @p46, @p47, @p48, @p49, @p50,
                            @p51, @p52, @p53, @p54, @p55, @p56, @p57,
                            @p58, @p59, @p60, @p61, @p62,
                            @p63
                        );
                    END";

                var parameters = new[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@p0", System.Data.SqlDbType.Decimal) { Value = (object?)settings.FunctionHallNonMemberRate ?? 400m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p1", System.Data.SqlDbType.Decimal) { Value = (object?)settings.FunctionHallMemberRate ?? 300m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p2", System.Data.SqlDbType.Decimal) { Value = (object?)settings.CoalitionNonMemberRate ?? 200m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p3", System.Data.SqlDbType.Decimal) { Value = (object?)settings.CoalitionMemberRate ?? 100m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p4", System.Data.SqlDbType.Decimal) { Value = (object?)settings.YouthOrganizationNonMemberRate ?? 100m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p5", System.Data.SqlDbType.Decimal) { Value = (object?)settings.YouthOrganizationMemberRate ?? 100m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p6", System.Data.SqlDbType.Decimal) { Value = (object?)settings.BartenderServiceFee ?? 100m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p7", System.Data.SqlDbType.Decimal) { Value = (object?)settings.KitchenFee ?? 50m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p8", System.Data.SqlDbType.Decimal) { Value = (object?)settings.AvEquipmentFee ?? 25m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p9", System.Data.SqlDbType.Decimal) { Value = (object?)settings.SecurityDepositAmount ?? 200m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p10", System.Data.SqlDbType.Int) { Value = (object?)settings.BaseFunctionHours ?? 5 },
                    new Microsoft.Data.SqlClient.SqlParameter("@p11", System.Data.SqlDbType.Decimal) { Value = (object?)settings.AdditionalHourRate ?? 50m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p12", System.Data.SqlDbType.Decimal) { Value = (object?)settings.MemberRate ?? 0m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p13", System.Data.SqlDbType.Decimal) { Value = (object?)settings.NonMemberRate ?? 0m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p14", System.Data.SqlDbType.Decimal) { Value = (object?)settings.NonProfitRate ?? 200m },
                    new Microsoft.Data.SqlClient.SqlParameter("@p15", System.Data.SqlDbType.Int) { Value = (object?)settings.MaxHallRentalDurationHours ?? 8 },
                    new Microsoft.Data.SqlClient.SqlParameter("@p16", System.Data.SqlDbType.Bit) { Value = settings.RequireSecurityDeposit },
                    new Microsoft.Data.SqlClient.SqlParameter("@p17", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RoomsJson ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p18", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.AddonsJson ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p19", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalFormTitle ?? "Hall Rental Application" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p20", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalFormSubtitle ?? "Gloucester Fraternity Club" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p21", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalFormIntroText ?? "" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p22", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalFormRulesText ?? "" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p23", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalFormSuccessMessage ?? "" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p24", System.Data.SqlDbType.Bit) { Value = settings.ShowAddressField },
                    new Microsoft.Data.SqlClient.SqlParameter("@p25", System.Data.SqlDbType.Bit) { Value = settings.ShowGuestCountField },
                    new Microsoft.Data.SqlClient.SqlParameter("@p26", System.Data.SqlDbType.Bit) { Value = settings.EnableSandboxMode },
                    new Microsoft.Data.SqlClient.SqlParameter("@p27", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.SandboxCalendarName ?? "GFC Test Rental Calendar" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p28", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.SandboxTestEmail ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p29", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.SandboxGoogleCalendarId ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p30", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.SandboxCalendarFeedUrl ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p31", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.FormIngestionMode ?? "NativeForm" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p32", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.NotificationEmailList ?? "hnsilva@comcast.net" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p33", System.Data.SqlDbType.Bit) { Value = settings.NotifyOnNewSubmission },
                    new Microsoft.Data.SqlClient.SqlParameter("@p34", System.Data.SqlDbType.Bit) { Value = settings.NotifyOnPaymentRecorded },
                    new Microsoft.Data.SqlClient.SqlParameter("@p35", System.Data.SqlDbType.Bit) { Value = settings.SendApplicantConfirmation },
                    new Microsoft.Data.SqlClient.SqlParameter("@p36", System.Data.SqlDbType.Bit) { Value = settings.NotifyOnStatusChange },
                    new Microsoft.Data.SqlClient.SqlParameter("@p37", System.Data.SqlDbType.Int) { Value = settings.Id > 0 ? (object)settings.Id : DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p38", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.DaySchedulesJson ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p39", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.ApplicantConfirmationEmailSubject ?? "Your Gloucester Fraternity Club Hall Rental Request" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p40", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.ApplicantConfirmationEmailBody ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p41", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalEmailProvider ?? "SMTP" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p42", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalSmtpHost ?? "mail.gloucesterfraternityclub.com" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p43", System.Data.SqlDbType.Int) { Value = settings.RentalSmtpPort },
                    new Microsoft.Data.SqlClient.SqlParameter("@p44", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalSmtpUsername ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p45", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalSmtpPassword ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p46", System.Data.SqlDbType.Bit) { Value = settings.RentalSmtpEnableSsl },
                    new Microsoft.Data.SqlClient.SqlParameter("@p47", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalResendApiKey ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p48", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalSenderEmail ?? "rentals@gloucesterfraternityclub.com" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p49", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalSenderName ?? "Gloucester Fraternity Club - Hall Rentals" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p50", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalEmailCc ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p51", System.Data.SqlDbType.Bit) { Value = settings.RequireApplicantName },
                    new Microsoft.Data.SqlClient.SqlParameter("@p52", System.Data.SqlDbType.Bit) { Value = settings.RequireEmail },
                    new Microsoft.Data.SqlClient.SqlParameter("@p53", System.Data.SqlDbType.Bit) { Value = settings.RequirePhone },
                    new Microsoft.Data.SqlClient.SqlParameter("@p54", System.Data.SqlDbType.Bit) { Value = settings.RequireAddress },
                    new Microsoft.Data.SqlClient.SqlParameter("@p55", System.Data.SqlDbType.Bit) { Value = settings.RequireEventType },
                    new Microsoft.Data.SqlClient.SqlParameter("@p56", System.Data.SqlDbType.Bit) { Value = settings.RequireGuestCount },
                    new Microsoft.Data.SqlClient.SqlParameter("@p57", System.Data.SqlDbType.Bit) { Value = settings.AllowAdditionalHours },
                    new Microsoft.Data.SqlClient.SqlParameter("@p58", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalTermsAndConditionsText ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p59", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalCancellationPolicyText ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p60", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalKitchenPolicyText ?? DBNull.Value },
                    new Microsoft.Data.SqlClient.SqlParameter("@p61", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalPolicyUrl ?? "https://gloucesterfraternityclub.com/hall-rentals/hall-rental-policy/" },
                    new Microsoft.Data.SqlClient.SqlParameter("@p62", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.RentalPaymentWindowNotice ?? "After electronically signing this application, and once your date is approved by the Gloucester Fraternity Club (GFC), you will have two (2) business days to complete payment to reserve your date." },
                    new Microsoft.Data.SqlClient.SqlParameter("@p63", System.Data.SqlDbType.NVarChar) { Value = (object?)settings.PoliciesJson ?? DBNull.Value }
                };

                var connection = _context.Database.GetDbConnection();
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                foreach (var p in parameters)
                {
                    cmd.Parameters.Add(p);
                }

                await cmd.ExecuteNonQueryAsync();

                _cache.Set(CacheKey, settings, TimeSpan.FromMinutes(30));
            }
            catch (Exception ex)
            {
                var fullError = ex.InnerException != null ? $"{ex.Message} (Inner: {ex.InnerException.Message})" : ex.Message;
                Console.WriteLine($"ERROR in UpdateWebsiteSettingsAsync: {fullError}");
                throw new Exception($"Database update error: {fullError}", ex);
            }
        }

        private async Task HealDatabaseAsync()
        {
            try
            {
                await using var _context = await _contextFactory.CreateDbContextAsync();
                var schemaMigrationSql = @"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PoliciesJson')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [PoliciesJson] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalTermsAndConditionsText')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalTermsAndConditionsText] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalCancellationPolicyText')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalCancellationPolicyText] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalKitchenPolicyText')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalKitchenPolicyText] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalPolicyUrl')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalPolicyUrl] NVARCHAR(500) NULL DEFAULT 'https://gloucesterfraternityclub.com/hall-rentals/hall-rental-policy/';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalPaymentWindowNotice')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalPaymentWindowNotice] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'EnableSandboxMode')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [EnableSandboxMode] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxCalendarName')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxCalendarName] NVARCHAR(150) NULL DEFAULT 'GFC Test Rental Calendar';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxTestEmail')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxTestEmail] NVARCHAR(200) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxGoogleCalendarId')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxGoogleCalendarId] NVARCHAR(500) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SandboxCalendarFeedUrl')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SandboxCalendarFeedUrl] NVARCHAR(1000) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RoomsJson')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RoomsJson] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AddonsJson')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [AddonsJson] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'DaySchedulesJson')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [DaySchedulesJson] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'ApplicantConfirmationEmailSubject')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [ApplicantConfirmationEmailSubject] NVARCHAR(300) NULL DEFAULT 'Your Gloucester Fraternity Club Hall Rental Request';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'ApplicantConfirmationEmailBody')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [ApplicantConfirmationEmailBody] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalEmailProvider')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalEmailProvider] NVARCHAR(50) NOT NULL DEFAULT 'SMTP';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalSmtpHost')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalSmtpHost] NVARCHAR(200) NULL DEFAULT 'mail.gloucesterfraternityclub.com';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalSmtpPort')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalSmtpPort] INT NOT NULL DEFAULT 587;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalSmtpUsername')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalSmtpUsername] NVARCHAR(200) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalSmtpPassword')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalSmtpPassword] NVARCHAR(200) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalSmtpEnableSsl')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalSmtpEnableSsl] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalResendApiKey')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalResendApiKey] NVARCHAR(500) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalSenderEmail')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalSenderEmail] NVARCHAR(200) NOT NULL DEFAULT 'rentals@gloucesterfraternityclub.com';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalSenderName')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalSenderName] NVARCHAR(200) NOT NULL DEFAULT 'Gloucester Fraternity Club - Hall Rentals';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalEmailCc')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalEmailCc] NVARCHAR(500) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequireSecurityDeposit')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequireSecurityDeposit] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormTitle')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormTitle] NVARCHAR(200) NULL DEFAULT 'Hall Rental Application';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormSubtitle')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormSubtitle] NVARCHAR(200) NULL DEFAULT 'Gloucester Fraternity Club';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormIntroText')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormIntroText] NVARCHAR(500) NULL DEFAULT '27 Webster Street, Gloucester, MA 01930 | (978) 283-2889';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormRulesText')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormRulesText] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RentalFormSuccessMessage')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RentalFormSuccessMessage] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'ShowAddressField')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [ShowAddressField] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'ShowGuestCountField')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [ShowGuestCountField] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequireApplicantName')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequireApplicantName] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequireEmail')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequireEmail] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequirePhone')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequirePhone] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequireAddress')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequireAddress] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequireEventType')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequireEventType] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'RequireGuestCount')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [RequireGuestCount] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AllowAdditionalHours')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [AllowAdditionalHours] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'FormIngestionMode')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [FormIngestionMode] NVARCHAR(50) NOT NULL DEFAULT 'NativeForm';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'WebhookApiKey')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [WebhookApiKey] NVARCHAR(100) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotificationEmailList')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotificationEmailList] NVARCHAR(500) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotifyOnNewSubmission')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotifyOnNewSubmission] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotifyOnPaymentRecorded')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotifyOnPaymentRecorded] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SendApplicantConfirmation')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SendApplicantConfirmation] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NotifyOnStatusChange')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [NotifyOnStatusChange] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'MaxHallRentalDurationHours')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [MaxHallRentalDurationHours] INT NULL DEFAULT 8;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'NonProfitRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [NonProfitRate] DECIMAL(18,2) NULL DEFAULT 200.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'FunctionHallNonMemberRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [FunctionHallNonMemberRate] DECIMAL(18,2) NULL DEFAULT 400.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'FunctionHallMemberRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [FunctionHallMemberRate] DECIMAL(18,2) NULL DEFAULT 300.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'CoalitionNonMemberRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [CoalitionNonMemberRate] DECIMAL(18,2) NULL DEFAULT 200.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'CoalitionMemberRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [CoalitionMemberRate] DECIMAL(18,2) NULL DEFAULT 100.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'YouthOrganizationNonMemberRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [YouthOrganizationNonMemberRate] DECIMAL(18,2) NULL DEFAULT 100.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'YouthOrganizationMemberRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [YouthOrganizationMemberRate] DECIMAL(18,2) NULL DEFAULT 100.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BartenderServiceFee')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [BartenderServiceFee] DECIMAL(18,2) NULL DEFAULT 100.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'KitchenFee')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [KitchenFee] DECIMAL(18,2) NULL DEFAULT 50.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AvEquipmentFee')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [AvEquipmentFee] DECIMAL(18,2) NULL DEFAULT 25.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecurityDepositAmount')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SecurityDepositAmount] DECIMAL(18,2) NULL DEFAULT 200.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BaseFunctionHours')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [BaseFunctionHours] INT NULL DEFAULT 5;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'AdditionalHourRate')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [AdditionalHourRate] DECIMAL(18,2) NULL DEFAULT 50.00;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'EnableOnlineRentalsPayment')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [EnableOnlineRentalsPayment] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayUrl')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayUrl] NVARCHAR(500) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PaymentGatewayApiKey')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [PaymentGatewayApiKey] NVARCHAR(500) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'PrimaryColor')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [PrimaryColor] NVARCHAR(50) NULL DEFAULT '#0D1B2A';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SecondaryColor')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SecondaryColor] NVARCHAR(50) NULL DEFAULT '#FFD700';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'HeadingFont')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [HeadingFont] NVARCHAR(100) NULL DEFAULT 'Outfit';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'BodyFont')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [BodyFont] NVARCHAR(100) NULL DEFAULT 'Inter';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'HighAccessibilityMode')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [HighAccessibilityMode] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'IsClubOpen')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [IsClubOpen] BIT NOT NULL DEFAULT 1;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'ClubPhone')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [ClubPhone] NVARCHAR(100) NULL DEFAULT '';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'ClubAddress')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [ClubAddress] NVARCHAR(500) NULL DEFAULT '';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'MasterEmailKillSwitch')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [MasterEmailKillSwitch] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoTitle')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoTitle] NVARCHAR(300) NULL DEFAULT '';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoDescription')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoDescription] NVARCHAR(1000) NULL DEFAULT '';

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteSettings]') AND name = 'SeoKeywords')
                        ALTER TABLE [dbo].[WebsiteSettings] ADD [SeoKeywords] NVARCHAR(1000) NULL DEFAULT '';

                    IF NOT EXISTS (SELECT 1 FROM [dbo].[WebsiteSettings])
                    BEGIN
                        INSERT INTO [dbo].[WebsiteSettings] ([MemberRate], [NonMemberRate], [IsClubOpen], [MasterEmailKillSwitch], [HighAccessibilityMode], [RequireSecurityDeposit], [SecurityDepositAmount], [FormIngestionMode], [NotifyOnNewSubmission], [NotifyOnPaymentRecorded], [SendApplicantConfirmation], [NotifyOnStatusChange], [EnableSandboxMode], [ShowAddressField], [ShowGuestCountField], [RequireApplicantName], [RequireEmail], [RequirePhone], [RequireAddress], [RequireEventType], [RequireGuestCount], [AllowAdditionalHours], [MaxHallRentalDurationHours], [NonProfitRate], [SandboxGoogleCalendarId], [SandboxCalendarFeedUrl], [RentalFormTitle], [RentalFormSubtitle], [RentalFormIntroText], [RentalFormRulesText], [RentalFormSuccessMessage])
                        VALUES (0, 0, 1, 0, 0, 1, 200, 'NativeForm', 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 0, 1, 8, 200, '7f6dc846d25ed3bd70a9801c40223ca4b977567b247a53575a93662d77ec8e7d@group.calendar.google.com', 'https://calendar.google.com/calendar/ical/7f6dc846d25ed3bd70a9801c40223ca4b977567b247a53575a93662d77ec8e7d%40group.calendar.google.com/public/basic.ics', 'Hall Rental Application', 'Gloucester Fraternity Club', '27 Webster Street, Gloucester, MA 01930 | (978) 283-2889', 'I have read and agree to the Gloucester Fraternity Club Hall Rental Policy & Rules. I understand that date confirmation is subject to committee approval and receipt of the security deposit.', 'Your rental request has been received and added to our calendar as Pending. The Hall Rental Committee will review your date and reach out to you directly to confirm booking.');
                    END

                    UPDATE [dbo].[WebsiteSettings] SET [MemberRate] = 0 WHERE [MemberRate] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [NonMemberRate] = 0 WHERE [NonMemberRate] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [IsClubOpen] = 1 WHERE [IsClubOpen] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [MasterEmailKillSwitch] = 0 WHERE [MasterEmailKillSwitch] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [HighAccessibilityMode] = 0 WHERE [HighAccessibilityMode] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RequireSecurityDeposit] = 1 WHERE [RequireSecurityDeposit] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [SecurityDepositAmount] = 200 WHERE [SecurityDepositAmount] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [FormIngestionMode] = 'NativeForm' WHERE [FormIngestionMode] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [NotifyOnNewSubmission] = 1 WHERE [NotifyOnNewSubmission] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [NotifyOnPaymentRecorded] = 1 WHERE [NotifyOnPaymentRecorded] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [SendApplicantConfirmation] = 1 WHERE [SendApplicantConfirmation] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [NotifyOnStatusChange] = 1 WHERE [NotifyOnStatusChange] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [ShowAddressField] = 1 WHERE [ShowAddressField] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [ShowGuestCountField] = 1 WHERE [ShowGuestCountField] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RequireApplicantName] = 1 WHERE [RequireApplicantName] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RequireEmail] = 1 WHERE [RequireEmail] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RequirePhone] = 1 WHERE [RequirePhone] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RequireAddress] = 0 WHERE [RequireAddress] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RequireEventType] = 1 WHERE [RequireEventType] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RequireGuestCount] = 0 WHERE [RequireGuestCount] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [AllowAdditionalHours] = 1 WHERE [AllowAdditionalHours] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [MaxHallRentalDurationHours] = 8 WHERE [MaxHallRentalDurationHours] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [NonProfitRate] = 200 WHERE [NonProfitRate] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [SandboxGoogleCalendarId] = '7f6dc846d25ed3bd70a9801c40223ca4b977567b247a53575a93662d77ec8e7d@group.calendar.google.com' WHERE [SandboxGoogleCalendarId] IS NULL OR [SandboxGoogleCalendarId] = '';
                    UPDATE [dbo].[WebsiteSettings] SET [SandboxCalendarFeedUrl] = 'https://calendar.google.com/calendar/ical/7f6dc846d25ed3bd70a9801c40223ca4b977567b247a53575a93662d77ec8e7d%40group.calendar.google.com/public/basic.ics' WHERE [SandboxCalendarFeedUrl] IS NULL OR [SandboxCalendarFeedUrl] = '';
                    UPDATE [dbo].[WebsiteSettings] SET [RentalFormTitle] = 'Hall Rental Application' WHERE [RentalFormTitle] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RentalFormSubtitle] = 'Gloucester Fraternity Club' WHERE [RentalFormSubtitle] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RentalFormIntroText] = '27 Webster Street, Gloucester, MA 01930 | (978) 283-2889' WHERE [RentalFormIntroText] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RentalFormRulesText] = 'I have read and agree to the Gloucester Fraternity Club Hall Rental Policy & Rules. I understand that date confirmation is subject to committee approval and receipt of the security deposit.' WHERE [RentalFormRulesText] IS NULL;
                    UPDATE [dbo].[WebsiteSettings] SET [RentalFormSuccessMessage] = 'Your rental request has been received and added to our calendar as Pending. The Hall Rental Committee will review your date and reach out to you directly to confirm booking.' WHERE [RentalFormSuccessMessage] IS NULL;
                ";
                await _context.Database.ExecuteSqlRawAsync(schemaMigrationSql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HealDatabaseAsync notice: {ex.Message}");
            }
        }
    }
}


