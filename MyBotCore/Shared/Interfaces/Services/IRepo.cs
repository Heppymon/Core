using Microsoft.EntityFrameworkCore;

namespace MyBotCore.Shared.Interfaces.Services
{
    public interface IRepo
    {
        /// <summary>
        /// Get typed collection from database context
        /// </summary>
        public DbSet<T> Get<T>() where T : class;

        /// <summary>
        /// Store history (if needed) then add typed item to Database context
        /// </summary>
        /// <param name="item">typed item</param>
        public Task<T> AddAsync<T>(T item) where T : class;

        /// <summary>
        /// Store history (if needed) then add typed items to Database context
        /// </summary>
        /// <param name="items"></param>
        public Task<List<T>> AddRangeAsync<T>(List<T> items)
            where T : class;

        /// <summary>
        /// Store history (if needed) then update typed item at Database context
        /// </summary>
        /// <param name="item">typed item</param>
        public Task<T> UpdateAsync<T>(T item) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">typed item</param>
        public Task<T> UpsertAsync<T>(T item) where T : class;

        /// <summary>
        /// Store history (if needed) then delete typed item at Database context
        /// </summary>
        /// <param name="item">typed item</param>
        public Task<bool> DeleteAsync<T>(T item) where T : class;
    }
}
