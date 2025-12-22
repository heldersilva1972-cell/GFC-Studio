// [NEW]
using GFC.Core.Models;
using GFC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public class RecordingService : IRecordingService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RecordingService(ApplicationDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<Recording>> GetRecordingsForCameraAsync(int cameraId, DateTime start, DateTime end)
        {
            return await _context.Recordings
                .Where(r => r.CameraId == cameraId && r.StartTime >= start && r.EndTime <= end)
                .ToListAsync();
        }

        public async Task<Recording> GetRecordingAsync(Guid recordingId)
        {
            return await _context.Recordings.FindAsync(recordingId);
        }

        public async Task<Recording> StartRecordingAsync(int cameraId)
        {
            var videoAgentBaseUrl = _configuration["VideoAgent:BaseUrl"];
            var response = await _httpClient.PostAsync($"{videoAgentBaseUrl}/record/start/{cameraId}", null);
            response.EnsureSuccessStatusCode();

            var recording = await response.Content.ReadFromJsonAsync<Recording>();
            _context.Recordings.Add(recording);
            await _context.SaveChangesAsync();
            return recording;
        }

        public async Task StopRecordingAsync(Guid recordingId)
        {
            var videoAgentBaseUrl = _configuration["VideoAgent:BaseUrl"];
            var recording = await _context.Recordings.FindAsync(recordingId);
            if (recording != null)
            {
                var response = await _httpClient.PostAsync($"{videoAgentBaseUrl}/record/stop/{recording.CameraId}", null);
                response.EnsureSuccessStatusCode();

                var updatedRecording = await response.Content.ReadFromJsonAsync<Recording>();
                recording.EndTime = updatedRecording.EndTime;
                recording.FileSize = updatedRecording.FileSize;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRecordingAsync(Guid recordingId)
        {
            var recording = await _context.Recordings.FindAsync(recordingId);
            if (recording != null)
            {
                _context.Recordings.Remove(recording);
                await _context.SaveChangesAsync();
            }
        }
    }
}
