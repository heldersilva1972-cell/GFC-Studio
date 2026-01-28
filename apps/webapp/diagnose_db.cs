using System;
using GFC.Core.Interfaces;
using GFC.Data.Repositories;
using GFC.Core.Models;

namespace DBCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try 
            {
                var repo = new UserRepository();
                var users = repo.GetAllUsers();
                Console.WriteLine($"Found {users.Count} users.");
                foreach (var user in users)
                {
                    Console.WriteLine($"User: {user.Username} (ID: {user.UserId})");
                    Console.WriteLine($"  PassCodeHash: '{(user.PassCodeHash ?? "NULL")}'");
                    Console.WriteLine($"  PassCodeHash IsNullOrWhiteSpace: {string.IsNullOrWhiteSpace(user.PassCodeHash)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
    }
}
