using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public interface IStudioCmsService
    {
        Task<List<StudioCollection>> GetCollectionsAsync();
        Task<StudioCollection?> GetCollectionWithFieldsAsync(int id);
        Task<StudioCollectionItem?> GetItemAsync(int id);
        Task<StudioCollection> CreateCollectionAsync(StudioCollection collection);
        Task<StudioCollectionField> AddFieldAsync(StudioCollectionField field);
        Task<StudioCollectionItem> SaveItemAsync(StudioCollectionItem item);
        Task DeleteItemAsync(int id);
    }

    public class StudioCmsService : IStudioCmsService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;
        private readonly ILogger<StudioCmsService> _logger;

        public StudioCmsService(IDbContextFactory<GfcDbContext> dbContextFactory, ILogger<StudioCmsService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<List<StudioCollection>> GetCollectionsAsync()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.StudioCollections
                .Include(c => c.Fields)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<StudioCollection?> GetCollectionWithFieldsAsync(int id)
        {
             using var context = await _dbContextFactory.CreateDbContextAsync();
             return await context.StudioCollections
                .Include(c => c.Fields)
                .Include(c => c.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

         public async Task<StudioCollectionItem?> GetItemAsync(int id)
        {
             using var context = await _dbContextFactory.CreateDbContextAsync();
             return await context.StudioCollectionItems
                .Include(i => i.Collection)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<StudioCollection> CreateCollectionAsync(StudioCollection collection)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            context.StudioCollections.Add(collection);
            await context.SaveChangesAsync();
            return collection;
        }

        public async Task<StudioCollectionField> AddFieldAsync(StudioCollectionField field)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            context.StudioCollectionFields.Add(field);
            await context.SaveChangesAsync();
            return field;
        }

        public async Task<StudioCollectionItem> SaveItemAsync(StudioCollectionItem item)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            if (item.Id == 0)
            {
                context.StudioCollectionItems.Add(item);
            }
            else
            {
                context.Entry(item).State = EntityState.Modified;
            }
            await context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
             using var context = await _dbContextFactory.CreateDbContextAsync();
             var item = new StudioCollectionItem { Id = id };
             context.StudioCollectionItems.Attach(item);
             context.StudioCollectionItems.Remove(item);
             await context.SaveChangesAsync();
        }
    }
}
