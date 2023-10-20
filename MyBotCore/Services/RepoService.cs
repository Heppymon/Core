using Microsoft.EntityFrameworkCore;
using MyBotCore.Shared.Interfaces.Services;
using MyBotDb;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace MyBotCore.Services
{
    public class RepoService : IRepo
    {
        private readonly MainBotContext context;

        public RepoService(MainBotContext context)
        {
            this.context = context;
        }

        /// <inheritdoc />
        public async Task<T> AddAsync<T>(T item) where T : class
        {
            context.Add(item);
            await context.SaveChangesAsync();
            Log.Information($"Item {nameof(T)} added successfully");

            return item;
        }

        /// <inheritdoc />
        public async Task<List<T>> AddRangeAsync<T>(List<T> items) where T : class
        {
            context.AddRange(items);
            await context.SaveChangesAsync();
            Log.Information($"Range of items {nameof(T)} added successfully");

            return items;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync<T>(T item) where T : class
        {
            if (await FindItem(item) == null)
            {
                Log.Warning($"Item {nameof(T)} was not found");
                return false;
            }

            context.Remove(item);
            Log.Information($"Item {nameof(T)} was successfully removed");
            await context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc />
        public DbSet<T> Get<T>() where T : class
        {
            return context.Set<T>();
        }

        /// <inheritdoc />
        public async Task<T> UpdateAsync<T>(T item) where T : class
        {
            context.Update(item);
            Log.Information($"Item {nameof(T)} was successfully updated");
            await context.SaveChangesAsync();

            return item;
        }

        /// <inheritdoc />
        public async Task<T> UpsertAsync<T>(T item) where T : class
        {
            var set = context.Set<T>();

            // Type: get key property
            var keyProperty = typeof(T).GetProperties()
                .Select(pi => new
                {
                    Property = pi,
                    Attribute = pi.GetCustomAttributes(typeof(KeyAttribute), true).FirstOrDefault() as KeyAttribute
                })
                .First(x => x.Attribute != null)
                .Property;

            var target = await set.FindAsync(keyProperty.GetValue(item));

            if (target is null)
            {
                return await AddAsync(item);
            }
            else
            {
                context.Entry(target).State = EntityState.Detached;
                context.Attach(item);
                return await UpdateAsync(item);
            }
        }

        private async Task<T?> FindItem<T>(T item) where T : class
        {
            var entityEntry = await context.Entry(item).GetDatabaseValuesAsync();
            return (T)entityEntry?.ToObject()!;
        }
    }
}
