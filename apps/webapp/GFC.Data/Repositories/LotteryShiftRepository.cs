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
                    SELECT ShiftId, ShiftDate, EmployeeName, ShiftType, MachineId,
                           StartingCash, EndingCash, TotalSales, TotalPayouts, TotalCancels,
                           Notes, Status, IsReconciled, ReconciledBy, ReconciledDate,
                           CreatedBy, CreatedDate, ModifiedBy, ModifiedDate
                    FROM LotteryShifts
                    WHERE ShiftId = @ShiftId";
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
                    SELECT ShiftId, ShiftDate, EmployeeName, ShiftType, MachineId,
                           StartingCash, EndingCash, TotalSales, TotalPayouts, TotalCancels,
                           Notes, Status, IsReconciled, ReconciledBy, ReconciledDate,
                           CreatedBy, CreatedDate, ModifiedBy, ModifiedDate
                    FROM LotteryShifts
                    WHERE ShiftDate >= @StartDate AND ShiftDate < DATEADD(day, 1, @EndDate)
                    ORDER BY ShiftDate DESC, EmployeeName";
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
                    SELECT ShiftId, ShiftDate, EmployeeName, ShiftType, MachineId,
                           StartingCash, EndingCash, TotalSales, TotalPayouts, TotalCancels,
                           Notes, Status, IsReconciled, ReconciledBy, ReconciledDate,
                           CreatedBy, CreatedDate, ModifiedBy, ModifiedDate
                    FROM LotteryShifts
                    WHERE EmployeeName = @EmployeeName";
                
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
                    SELECT ShiftId, ShiftDate, EmployeeName, ShiftType, MachineId,
                           StartingCash, EndingCash, TotalSales, TotalPayouts, TotalCancels,
                           Notes, Status, IsReconciled, ReconciledBy, ReconciledDate,
                           CreatedBy, CreatedDate, ModifiedBy, ModifiedDate
                    FROM LotteryShifts
                    ORDER BY ShiftDate DESC, EmployeeName";
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
                        Notes, Status, IsReconciled, ReconciledBy, ReconciledDate,
                        CreatedBy, CreatedDate, ModifiedBy, ModifiedDate
                    )
                    VALUES (
                        @ShiftDate, @EmployeeName, @ShiftType, @MachineId,
                        @StartingCash, @EndingCash, @TotalSales, @TotalPayouts, @TotalCancels,
                        @Notes, @Status, @IsReconciled, @ReconciledBy, @ReconciledDate,
                        @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate
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
                        Notes = @Notes,
                        Status = @Status,
                        IsReconciled = @IsReconciled,
                        ReconciledBy = @ReconciledBy,
                        ReconciledDate = @ReconciledDate,
                        ModifiedBy = @ModifiedBy,
                        ModifiedDate = @ModifiedDate
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
                    SELECT ShiftId, ShiftDate, EmployeeName, ShiftType, MachineId,
                           StartingCash, EndingCash, TotalSales, TotalPayouts, TotalCancels,
                           Notes, Status, IsReconciled, ReconciledBy, ReconciledDate,
                           CreatedBy, CreatedDate, ModifiedBy, ModifiedDate
                    FROM LotteryShifts
                    WHERE IsReconciled = 0
                    ORDER BY ShiftDate DESC";
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

        private static LotteryShift MapReaderToShift(SqlDataReader reader)
        {
            return new LotteryShift
            {
                ShiftId = (int)reader["ShiftId"],
                ShiftDate = (DateTime)reader["ShiftDate"],
                EmployeeName = (string)reader["EmployeeName"],
                ShiftType = reader["ShiftType"] as string,
                MachineId = reader["MachineId"] as string,
                StartingCash = (decimal)reader["StartingCash"],
                EndingCash = (decimal)reader["EndingCash"],
                TotalSales = (decimal)reader["TotalSales"],
                TotalPayouts = (decimal)reader["TotalPayouts"],
                TotalCancels = (decimal)reader["TotalCancels"],
                Notes = reader["Notes"] as string,
                Status = reader["Status"] as string,
                IsReconciled = (bool)reader["IsReconciled"],
                ReconciledBy = reader["ReconciledBy"] as string,
                ReconciledDate = reader["ReconciledDate"] as DateTime?,
                CreatedBy = reader["CreatedBy"] as string,
                CreatedDate = (DateTime)reader["CreatedDate"],
                ModifiedBy = reader["ModifiedBy"] as string,
                ModifiedDate = reader["ModifiedDate"] as DateTime?
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
            command.Parameters.AddWithValue("@Notes", (object?)shift.Notes ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", (object?)shift.Status ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsReconciled", shift.IsReconciled);
            command.Parameters.AddWithValue("@ReconciledBy", (object?)shift.ReconciledBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReconciledDate", (object?)shift.ReconciledDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedBy", (object?)shift.CreatedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedDate", shift.CreatedDate);
            command.Parameters.AddWithValue("@ModifiedBy", (object?)shift.ModifiedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@ModifiedDate", (object?)shift.ModifiedDate ?? DBNull.Value);
        }
    }
}
