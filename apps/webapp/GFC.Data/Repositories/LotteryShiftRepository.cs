using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories
{
    public class LotteryShiftRepository : ILotteryShiftRepository
    {
        public LotteryShift? GetById(int shiftId)
        {
            try
            {
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = @"
                    SELECT 
                        s.ShiftId, s.ShiftDate, s.ShiftType, s.MachineId,
                        s.StartingCash, s.EndingCash, s.TotalSales, s.TotalPayouts, s.TotalCancels,
                        s.Commission, s.CashBonus, s.ClaimsBonus, s.NetDue,
                        s.BackupBagAmount, s.EnvelopeAmount, s.BagRefillAmount,
                        s.NetSales, s.ExpectedCash, s.Variance, s.LotteryIncome, s.NetIncome,
                        s.ShiftSalesActivity, s.ShiftPayoutsActivity, s.ShiftCancelsActivity, s.ShiftNetDueActivity,
                        s.Notes, s.Status, s.IsReconciled, s.ReconciledBy, s.ReconciledDate,
                        s.CreatedBy, s.CreatedDate, s.ModifiedBy, s.ModifiedDate, s.TicketImageUrl,
                        ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) as ResolvedEmployeeName
                    FROM LotteryShifts s
                    LEFT JOIN AppUsers u ON s.EmployeeName = u.Username
                    LEFT JOIN Members m ON u.MemberId = m.MemberID
                    WHERE s.ShiftId = @ShiftId";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShiftId", shiftId);
                using var reader = command.ExecuteReader();
                return reader.Read() ? MapReaderToShift(reader) : null;
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                // Table doesn't exist yet - return null
                return null;
            }
        }

        public List<LotteryShift> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var shifts = new List<LotteryShift>();
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = @"
                    SELECT 
                        s.ShiftId, s.ShiftDate, s.ShiftType, s.MachineId,
                        s.StartingCash, s.EndingCash, s.TotalSales, s.TotalPayouts, s.TotalCancels,
                        s.Commission, s.CashBonus, s.ClaimsBonus, s.NetDue,
                        s.BackupBagAmount, s.EnvelopeAmount, s.BagRefillAmount,
                        s.NetSales, s.ExpectedCash, s.Variance, s.LotteryIncome, s.NetIncome,
                        s.ShiftSalesActivity, s.ShiftPayoutsActivity, s.ShiftCancelsActivity, s.ShiftNetDueActivity,
                        s.Notes, s.Status, s.IsReconciled, s.ReconciledBy, s.ReconciledDate,
                        s.CreatedBy, s.CreatedDate, s.ModifiedBy, s.ModifiedDate, s.TicketImageUrl,
                        ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) as ResolvedEmployeeName
                    FROM LotteryShifts s
                    LEFT JOIN AppUsers u ON s.EmployeeName = u.Username
                    LEFT JOIN Members m ON u.MemberId = m.MemberID
                    WHERE s.ShiftDate >= @StartDate AND s.ShiftDate < DATEADD(day, 1, @EndDate)
                    ORDER BY s.ShiftDate DESC, ResolvedEmployeeName";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@StartDate", startDate.Date);
                command.Parameters.AddWithValue("@EndDate", endDate.Date);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    shifts.Add(MapReaderToShift(reader));
                }
                return shifts;
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                // Table doesn't exist yet - return empty list
                return new List<LotteryShift>();
            }
        }

        public List<LotteryShift> GetByEmployee(string employeeName, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var shifts = new List<LotteryShift>();
                using var connection = Db.GetConnection();
                connection.Open();
                var sql = @"
                    SELECT 
                        s.ShiftId, s.ShiftDate, s.ShiftType, s.MachineId,
                        s.StartingCash, s.EndingCash, s.TotalSales, s.TotalPayouts, s.TotalCancels,
                        s.Commission, s.CashBonus, s.ClaimsBonus, s.NetDue,
                        s.BackupBagAmount, s.EnvelopeAmount, s.BagRefillAmount,
                        s.NetSales, s.ExpectedCash, s.Variance, s.LotteryIncome, s.NetIncome,
                        s.ShiftSalesActivity, s.ShiftPayoutsActivity, s.ShiftCancelsActivity, s.ShiftNetDueActivity,
                        s.Notes, s.Status, s.IsReconciled, s.ReconciledBy, s.ReconciledDate,
                        s.CreatedBy, s.CreatedDate, s.ModifiedBy, s.ModifiedDate, s.TicketImageUrl,
                        ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) as ResolvedEmployeeName
                    FROM LotteryShifts s
                    LEFT JOIN AppUsers u ON s.EmployeeName = u.Username
                    LEFT JOIN Members m ON u.MemberId = m.MemberID
                    WHERE (s.EmployeeName = @EmployeeName OR 
                           ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) = @EmployeeName)";
                
                if (startDate.HasValue)
                {
                    sql += " AND ShiftDate >= @StartDate";
                }
                if (endDate.HasValue)
                {
                    sql += " AND ShiftDate < DATEADD(day, 1, @EndDate)";
                }
                sql += " ORDER BY ShiftDate DESC";
                
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@EmployeeName", employeeName);
                if (startDate.HasValue)
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.Value.Date);
                }
                if (endDate.HasValue)
                {
                    command.Parameters.AddWithValue("@EndDate", endDate.Value.Date);
                }
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    shifts.Add(MapReaderToShift(reader));
                }
                return shifts;
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                // Table doesn't exist yet - return empty list
                return new List<LotteryShift>();
            }
        }

        public List<LotteryShift> GetAll()
        {
            try
            {
                var shifts = new List<LotteryShift>();
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = @"
                    SELECT 
                        s.ShiftId, s.ShiftDate, s.ShiftType, s.MachineId,
                        s.StartingCash, s.EndingCash, s.TotalSales, s.TotalPayouts, s.TotalCancels,
                        s.Commission, s.CashBonus, s.ClaimsBonus, s.NetDue,
                        s.BackupBagAmount, s.EnvelopeAmount, s.BagRefillAmount,
                        s.NetSales, s.ExpectedCash, s.Variance, s.LotteryIncome, s.NetIncome,
                        s.ShiftSalesActivity, s.ShiftPayoutsActivity, s.ShiftCancelsActivity, s.ShiftNetDueActivity,
                        s.Notes, s.Status, s.IsReconciled, s.ReconciledBy, s.ReconciledDate,
                        s.CreatedBy, s.CreatedDate, s.ModifiedBy, s.ModifiedDate, s.TicketImageUrl,
                        ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) as ResolvedEmployeeName
                    FROM LotteryShifts s
                    LEFT JOIN AppUsers u ON s.EmployeeName = u.Username
                    LEFT JOIN Members m ON u.MemberId = m.MemberID
                    ORDER BY s.ShiftDate DESC, ResolvedEmployeeName";
                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    shifts.Add(MapReaderToShift(reader));
                }
                return shifts;
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                // Table doesn't exist yet - return empty list
                return new List<LotteryShift>();
            }
        }

        public int Create(LotteryShift shift)
        {
            try
            {
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = @"
                    INSERT INTO LotteryShifts (
                        ShiftDate, EmployeeName, ShiftType, MachineId,
                        StartingCash, EndingCash, TotalSales, TotalPayouts, TotalCancels,
                        Commission, CashBonus, ClaimsBonus, NetDue,
                        BackupBagAmount, EnvelopeAmount, BagRefillAmount,
                        NetSales, ExpectedCash, Variance, LotteryIncome, NetIncome,
                        ShiftSalesActivity, ShiftPayoutsActivity, ShiftCancelsActivity, ShiftNetDueActivity,
                        Notes, Status, IsReconciled, ReconciledBy, ReconciledDate,
                        CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, TicketImageUrl
                    )
                    VALUES (
                        @ShiftDate, @EmployeeName, @ShiftType, @MachineId,
                        @StartingCash, @EndingCash, @TotalSales, @TotalPayouts, @TotalCancels,
                        @Commission, @CashBonus, @ClaimsBonus, @NetDue,
                        @BackupBagAmount, @EnvelopeAmount, @BagRefillAmount,
                        @NetSales, @ExpectedCash, @Variance, @LotteryIncome, @NetIncome,
                        @ShiftSalesActivity, @ShiftPayoutsActivity, @ShiftCancelsActivity, @ShiftNetDueActivity,
                        @Notes, @Status, @IsReconciled, @ReconciledBy, @ReconciledDate,
                        @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate, @TicketImageUrl
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using var command = new SqlCommand(sql, connection);
                MapShiftToParameters(command, shift);
                return (int)command.ExecuteScalar();
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                throw new InvalidOperationException("LotteryShifts table does not exist. Please run CreateLotteryTables.sql script first.", ex);
            }
        }

        public void Update(LotteryShift shift)
        {
            try
            {
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = @"
                    UPDATE LotteryShifts
                    SET ShiftDate = @ShiftDate,
                        EmployeeName = @EmployeeName,
                        ShiftType = @ShiftType,
                        MachineId = @MachineId,
                        StartingCash = @StartingCash,
                        EndingCash = @EndingCash,
                        TotalSales = @TotalSales,
                        TotalPayouts = @TotalPayouts,
                        TotalCancels = @TotalCancels,
                        Commission = @Commission,
                        CashBonus = @CashBonus,
                        ClaimsBonus = @ClaimsBonus,
                        NetDue = @NetDue,
                        BackupBagAmount = @BackupBagAmount,
                        EnvelopeAmount = @EnvelopeAmount,
                        BagRefillAmount = @BagRefillAmount,
                        NetSales = @NetSales,
                        ExpectedCash = @ExpectedCash,
                        Variance = @Variance,
                        LotteryIncome = @LotteryIncome,
                        NetIncome = @NetIncome,
                        ShiftSalesActivity = @ShiftSalesActivity,
                        ShiftPayoutsActivity = @ShiftPayoutsActivity,
                        ShiftCancelsActivity = @ShiftCancelsActivity,
                        ShiftNetDueActivity = @ShiftNetDueActivity,
                        Notes = @Notes,
                        Status = @Status,
                        IsReconciled = @IsReconciled,
                        ReconciledBy = @ReconciledBy,
                        ReconciledDate = @ReconciledDate,
                        CreatedBy = @CreatedBy,
                        ModifiedBy = @ModifiedBy,
                        ModifiedDate = @ModifiedDate,
                        TicketImageUrl = @TicketImageUrl
                    WHERE ShiftId = @ShiftId";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShiftId", shift.ShiftId);
                MapShiftToParameters(command, shift);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                throw new InvalidOperationException("LotteryShifts table does not exist. Please run CreateLotteryTables.sql script first.", ex);
            }
        }

        public void Delete(int shiftId)
        {
            try
            {
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = "DELETE FROM LotteryShifts WHERE ShiftId = @ShiftId";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShiftId", shiftId);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                throw new InvalidOperationException("LotteryShifts table does not exist. Please run CreateLotteryTables.sql script first.", ex);
            }
        }

        public List<LotteryShift> GetUnreconciled()
        {
            try
            {
                var shifts = new List<LotteryShift>();
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = @"
                    SELECT 
                        s.ShiftId, s.ShiftDate, s.ShiftType, s.MachineId,
                        s.StartingCash, s.EndingCash, s.TotalSales, s.TotalPayouts, s.TotalCancels,
                        s.Commission, s.CashBonus, s.ClaimsBonus, s.NetDue,
                        s.BackupBagAmount, s.EnvelopeAmount, s.BagRefillAmount,
                        s.NetSales, s.ExpectedCash, s.Variance, s.LotteryIncome, s.NetIncome,
                        s.ShiftSalesActivity, s.ShiftPayoutsActivity, s.ShiftCancelsActivity, s.ShiftNetDueActivity,
                        s.Notes, s.Status, s.IsReconciled, s.ReconciledBy, s.ReconciledDate,
                        s.CreatedBy, s.CreatedDate, s.ModifiedBy, s.ModifiedDate, s.TicketImageUrl,
                        ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) as ResolvedEmployeeName
                    FROM LotteryShifts s
                    LEFT JOIN AppUsers u ON s.EmployeeName = u.Username
                    LEFT JOIN Members m ON u.MemberId = m.MemberID
                    WHERE IsReconciled = 0
                    ORDER BY s.ShiftDate DESC, ResolvedEmployeeName";
                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    shifts.Add(MapReaderToShift(reader));
                }
                return shifts;
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                // Table doesn't exist yet - return empty list
                return new List<LotteryShift>();
            }
        }

        public LotteryShift? GetDuplicateShift(string employeeName, DateTime date, string? shiftType)
        {
            try
            {
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = @"
                    SELECT TOP 1 
                        s.ShiftId, s.ShiftDate, s.ShiftType, s.MachineId,
                        s.StartingCash, s.EndingCash, s.TotalSales, s.TotalPayouts, s.TotalCancels,
                        s.Commission, s.CashBonus, s.ClaimsBonus, s.NetDue,
                        s.BackupBagAmount, s.EnvelopeAmount, s.BagRefillAmount,
                        s.NetSales, s.ExpectedCash, s.Variance, s.LotteryIncome, s.NetIncome,
                        s.ShiftSalesActivity, s.ShiftPayoutsActivity, s.ShiftCancelsActivity, s.ShiftNetDueActivity,
                        s.Notes, s.Status, s.IsReconciled, s.ReconciledBy, s.ReconciledDate,
                        s.CreatedBy, s.CreatedDate, s.ModifiedBy, s.ModifiedDate, s.TicketImageUrl,
                        ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) as ResolvedEmployeeName
                    FROM LotteryShifts s
                    LEFT JOIN AppUsers u ON s.EmployeeName = u.Username
                    LEFT JOIN Members m ON u.MemberId = m.MemberID
                    WHERE CAST(s.ShiftDate AS DATE) = CAST(@ShiftDate AS DATE)
                      AND ISNULL(s.ShiftType, '') = ISNULL(@ShiftType, '')
                      AND (s.EmployeeName = @EmployeeName 
                           OR ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) = @EmployeeName)";

                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@EmployeeName", employeeName);
                command.Parameters.AddWithValue("@ShiftDate", date.Date);
                command.Parameters.AddWithValue("@ShiftType", (object?)shiftType ?? DBNull.Value);
                
                using var reader = command.ExecuteReader();
                return reader.Read() ? MapReaderToShift(reader) : null;
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                return null;
            }
        }

        public bool Exists(int shiftId)
        {
            try
            {
                using var connection = Db.GetConnection();
                connection.Open();
                const string sql = "SELECT COUNT(*) FROM LotteryShifts WHERE ShiftId = @ShiftId";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShiftId", shiftId);
                return (int)command.ExecuteScalar() > 0;
            }
            catch (SqlException ex) when (ex.Number == 208) // Invalid object name
            {
                // Table doesn't exist yet - return false
                return false;
            }
        }

        public List<(string Username, string FullName)> GetEmployeeMetadata()
        {
            try
            {
                var metadata = new List<(string Username, string FullName)>();
                using var connection = Db.GetConnection();
                connection.Open();
                // Fetch active users linked to members for full names
                // Fetch unique FullNames that exist in the shifts table
                // Group by FullName to prevent duplicates (e.g. if one shift has 'mcosta' and another has 'Michael Costa')
                const string sql = @"
                    SELECT DISTINCT FullName FROM (
                        SELECT 
                            ISNULL(m.FirstName + ' ' + m.LastName + ISNULL(' ' + m.Suffix, ''), s.EmployeeName) as FullName
                        FROM LotteryShifts s
                        LEFT JOIN AppUsers u ON s.EmployeeName = u.Username
                        LEFT JOIN Members m ON u.MemberId = m.MemberID
                    ) a
                    ORDER BY FullName";
                using var command = new SqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string fullName = reader["FullName"].ToString();
                    metadata.Add((fullName, fullName)); // Use FullName for both to simplify matching
                }
                return metadata;
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                return new List<(string, string)>();
            }
        }

        public void UpdateBarSaleOwner(DateTime date, string shiftType, string oldUsername, string newUsername)
        {
            try
            {
                using var connection = Db.GetConnection();
                connection.Open();
                
                // Find and update the bar entry linked to this lottery shift
                // Matches on Date, Shift Type, and the OLD username to ensure we hit the right one
                const string sql = @"
                    UPDATE BarSaleEntries
                    SET CreatedBy = @NewUsername,
                        ModifiedDate = @ModifiedAt, -- Primary audit field
                        ModifiedAt = @ModifiedAt,   -- Secondary/Legacy audit field
                        ModifiedBy = 'AdminReassignment'
                    WHERE (CAST(ISNULL(AdjustedSaleDate, SaleDate) AS DATE) = @Date)
                      AND Shift = @ShiftType
                      AND CreatedBy = @OldUsername";
                      
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@NewUsername", newUsername);
                command.Parameters.AddWithValue("@Date", date.Date);
                command.Parameters.AddWithValue("@ShiftType", shiftType);
                command.Parameters.AddWithValue("@OldUsername", oldUsername);
                command.Parameters.AddWithValue("@ModifiedAt", DateTime.UtcNow);
                
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // Non-critical failure; if a bar entry isn't found or fails, we don't block the lottery save
            }
        }

        private static LotteryShift MapReaderToShift(SqlDataReader reader)
        {
            return new LotteryShift
            {
                ShiftId = (int)reader["ShiftId"],
                ShiftDate = (DateTime)reader["ShiftDate"],
                EmployeeName = (string)reader["ResolvedEmployeeName"],
                ShiftType = reader["ShiftType"] as string,
                MachineId = reader["MachineId"] as string,
                StartingCash = (decimal)reader["StartingCash"],
                EndingCash = (decimal)reader["EndingCash"],
                TotalSales = (decimal)reader["TotalSales"],
                TotalPayouts = (decimal)reader["TotalPayouts"],
                TotalCancels = (decimal)reader["TotalCancels"],
                Commission = reader["Commission"] != DBNull.Value ? (decimal)reader["Commission"] : 0m,
                CashBonus = reader["CashBonus"] != DBNull.Value ? (decimal)reader["CashBonus"] : 0m,
                ClaimsBonus = reader["ClaimsBonus"] != DBNull.Value ? (decimal)reader["ClaimsBonus"] : 0m,
                NetDue = reader["NetDue"] != DBNull.Value ? (decimal)reader["NetDue"] : 0m,
                BackupBagAmount = reader["BackupBagAmount"] != DBNull.Value ? (decimal)reader["BackupBagAmount"] : 0m,
                EnvelopeAmount = reader["EnvelopeAmount"] != DBNull.Value ? (decimal)reader["EnvelopeAmount"] : 0m,
                BagRefillAmount = reader["BagRefillAmount"] != DBNull.Value ? (decimal)reader["BagRefillAmount"] : 0m,
                NetSales = reader["NetSales"] != DBNull.Value ? (decimal)reader["NetSales"] : 0m,
                ExpectedCash = reader["ExpectedCash"] != DBNull.Value ? (decimal)reader["ExpectedCash"] : 0m,
                Variance = reader["Variance"] != DBNull.Value ? (decimal)reader["Variance"] : 0m,
                LotteryIncome = reader["LotteryIncome"] != DBNull.Value ? (decimal)reader["LotteryIncome"] : 0m,
                NetIncome = reader["NetIncome"] != DBNull.Value ? (decimal)reader["NetIncome"] : 0m,
                ShiftSalesActivity = reader["ShiftSalesActivity"] != DBNull.Value ? (decimal)reader["ShiftSalesActivity"] : 0m,
                ShiftPayoutsActivity = reader["ShiftPayoutsActivity"] != DBNull.Value ? (decimal)reader["ShiftPayoutsActivity"] : 0m,
                ShiftCancelsActivity = reader["ShiftCancelsActivity"] != DBNull.Value ? (decimal)reader["ShiftCancelsActivity"] : 0m,
                ShiftNetDueActivity = reader["ShiftNetDueActivity"] != DBNull.Value ? (decimal)reader["ShiftNetDueActivity"] : 0m,
                Notes = reader["Notes"] as string,
                Status = reader["Status"] as string,
                IsReconciled = (bool)reader["IsReconciled"],
                ReconciledBy = reader["ReconciledBy"] as string,
                ReconciledDate = reader["ReconciledDate"] as DateTime?,
                CreatedBy = reader["CreatedBy"] as string,
                CreatedDate = (DateTime)reader["CreatedDate"],
                ModifiedBy = reader["ModifiedBy"] as string,
                ModifiedDate = reader["ModifiedDate"] as DateTime?,
                TicketImageUrl = reader["TicketImageUrl"] as string
            };
        }

        private static void MapShiftToParameters(SqlCommand command, LotteryShift shift)
        {
            command.Parameters.AddWithValue("@ShiftDate", shift.ShiftDate);
            command.Parameters.AddWithValue("@EmployeeName", shift.EmployeeName);
            command.Parameters.AddWithValue("@ShiftType", (object?)shift.ShiftType ?? DBNull.Value);
            command.Parameters.AddWithValue("@MachineId", (object?)shift.MachineId ?? DBNull.Value);
            command.Parameters.AddWithValue("@StartingCash", shift.StartingCash);
            command.Parameters.AddWithValue("@EndingCash", shift.EndingCash);
            command.Parameters.AddWithValue("@TotalSales", shift.TotalSales);
            command.Parameters.AddWithValue("@TotalPayouts", shift.TotalPayouts);
            command.Parameters.AddWithValue("@TotalCancels", shift.TotalCancels);
            command.Parameters.AddWithValue("@Commission", shift.Commission);
            command.Parameters.AddWithValue("@CashBonus", shift.CashBonus);
            command.Parameters.AddWithValue("@ClaimsBonus", shift.ClaimsBonus);
            command.Parameters.AddWithValue("@NetDue", shift.NetDue);
            command.Parameters.AddWithValue("@BackupBagAmount", shift.BackupBagAmount);
            command.Parameters.AddWithValue("@EnvelopeAmount", shift.EnvelopeAmount);
            command.Parameters.AddWithValue("@BagRefillAmount", shift.BagRefillAmount);
            command.Parameters.AddWithValue("@NetSales", shift.NetSales);
            command.Parameters.AddWithValue("@ExpectedCash", shift.ExpectedCash);
            command.Parameters.AddWithValue("@Variance", shift.Variance);
            command.Parameters.AddWithValue("@LotteryIncome", shift.LotteryIncome);
            command.Parameters.AddWithValue("@NetIncome", shift.NetIncome);
            command.Parameters.AddWithValue("@ShiftSalesActivity", shift.ShiftSalesActivity);
            command.Parameters.AddWithValue("@ShiftPayoutsActivity", shift.ShiftPayoutsActivity);
            command.Parameters.AddWithValue("@ShiftCancelsActivity", shift.ShiftCancelsActivity);
            command.Parameters.AddWithValue("@ShiftNetDueActivity", shift.ShiftNetDueActivity);
            command.Parameters.AddWithValue("@Notes", (object?)shift.Notes ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", (object?)shift.Status ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsReconciled", shift.IsReconciled);
            command.Parameters.AddWithValue("@ReconciledBy", (object?)shift.ReconciledBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReconciledDate", (object?)shift.ReconciledDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedBy", (object?)shift.CreatedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedDate", shift.CreatedDate);
            command.Parameters.AddWithValue("@ModifiedBy", (object?)shift.ModifiedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@ModifiedDate", (object?)shift.ModifiedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@TicketImageUrl", (object?)shift.TicketImageUrl ?? DBNull.Value);
        }
    }
}
