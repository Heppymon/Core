using MyBotDb.Models;

namespace MyBotCore.Shared.Interfaces.Services
{
    public interface IUserService
    {
        public Task<DbUser> GetUserByChatIdAsync(long chatId);
        public Task<DbUser> GetUserByIdAsync(Guid userId);
        public Task<bool> IsUserByChatIdExist(long chatId);
        public Task<DbUser> CreateAsync(long chatId, string? username, string? firstName, string? lastName);
        public Task<DbUser> UpdateAsync(long? chatId, string? username, string? firstName, string? lastName);
        public Task<bool> DeleteAsync(Guid userId);
    }
}
