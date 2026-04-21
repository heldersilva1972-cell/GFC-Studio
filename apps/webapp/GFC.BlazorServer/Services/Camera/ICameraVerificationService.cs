// [NEW]
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public interface ICameraVerificationService
    {
        Task<List<VerificationResult>> RunAllVerificationsAsync();
    }
}


