using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using GFC.Data;

namespace GFC.Data.Repositories
{
    public class LotteryRateRepository : ILotteryRateRepository
    {
        public List<LotteryCommissionRate> GetAll()
        {
            var rates = new List<LotteryCommissionRate>();
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = "SELECT [Year], SalesRate, CashingRate, TicketRate, DailySystemFee, WeeklyBondFee, CreatedBy FROM LotteryCommissionRates ORDER BY [Year] DESC";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                rates.Add(new LotteryCommissionRate
                {
                    Year = (int)reader["Year"],
                    SalesRate = (decimal)reader["SalesRate"],
                    CashingRate = (decimal)reader["CashingRate"],
                    TicketRate = (decimal)reader["TicketRate"],
                    DailySystemFee = (decimal)reader["DailySystemFee"],
                    DailyBondingFee = (decimal)reader["WeeklyBondFee"],
                    CreatedBy = reader["CreatedBy"] as string
                });
            }
            return rates;
        }

        public LotteryCommissionRate? GetByYear(int year)
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = "SELECT [Year], SalesRate, CashingRate, TicketRate, DailySystemFee, WeeklyBondFee, CreatedBy FROM LotteryCommissionRates WHERE [Year] = @Year";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Year", year);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new LotteryCommissionRate
                {
                    Year = (int)reader["Year"],
                    SalesRate = (decimal)reader["SalesRate"],
                    CashingRate = (decimal)reader["CashingRate"],
                    TicketRate = (decimal)reader["TicketRate"],
                    DailySystemFee = (decimal)reader["DailySystemFee"],
                    DailyBondingFee = (decimal)reader["WeeklyBondFee"],
                    CreatedBy = reader["CreatedBy"] as string
                };
            }
            return null;
        }

        public LotteryCommissionRate GetApplicableRate(int year)
        {
            // [SMART LOOKUP]: Find year, or latest previous year
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT TOP 1 [Year], SalesRate, CashingRate, TicketRate, DailySystemFee, WeeklyBondFee, CreatedBy 
                FROM LotteryCommissionRates 
                WHERE [Year] <= @Year 
                ORDER BY [Year] DESC";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Year", year);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new LotteryCommissionRate
                {
                    Year = (int)reader["Year"],
                    SalesRate = (decimal)reader["SalesRate"],
                    CashingRate = (decimal)reader["CashingRate"],
                    TicketRate = (decimal)reader["TicketRate"],
                    DailySystemFee = (decimal)reader["DailySystemFee"],
                    DailyBondingFee = (decimal)reader["WeeklyBondFee"],
                    CreatedBy = reader["CreatedBy"] as string
                };
            }

            // Fallback (Safe defaults)
            return new LotteryCommissionRate { Year = year, SalesRate = 5.00m, CashingRate = 1.00m, TicketRate = 1.00m, DailySystemFee = 2.00m, DailyBondingFee = 1.00m };
        }

        public void Save(LotteryCommissionRate rate)
        {
            using var connection = Db.GetConnection();
            connection.Open();
            // MERGE logic for easy save/update
            const string sql = @"
                IF EXISTS (SELECT 1 FROM LotteryCommissionRates WHERE [Year] = @Year)
                BEGIN
                    UPDATE LotteryCommissionRates 
                    SET SalesRate = @SalesRate, CashingRate = @CashingRate, TicketRate = @TicketRate, 
                        DailySystemFee = @DailySystemFee, WeeklyBondFee = @WeeklyBondFee, CreatedBy = @CreatedBy
                    WHERE [Year] = @Year
                END
                ELSE
                BEGIN
                    INSERT INTO LotteryCommissionRates ([Year], SalesRate, CashingRate, TicketRate, DailySystemFee, WeeklyBondFee, CreatedBy)
                    VALUES (@Year, @SalesRate, @CashingRate, @TicketRate, @DailySystemFee, @WeeklyBondFee, @CreatedBy)
                END";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Year", rate.Year);
            command.Parameters.AddWithValue("@SalesRate", rate.SalesRate);
            command.Parameters.AddWithValue("@CashingRate", rate.CashingRate);
            command.Parameters.AddWithValue("@TicketRate", rate.TicketRate);
            command.Parameters.AddWithValue("@DailySystemFee", rate.DailySystemFee);
            command.Parameters.AddWithValue("@WeeklyBondFee", rate.DailyBondingFee);
            command.Parameters.AddWithValue("@CreatedBy", rate.CreatedBy ?? (object)DBNull.Value);
            command.ExecuteNonQuery();
        }
    }
}
