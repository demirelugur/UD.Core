namespace UD.Core.Helper.Services
{
    using System;
    using System.Collections.Concurrent;
    using UD.Core.Extensions;
    public interface ITokenBlacklistService // AddSingleton
    {
        Task<bool> Any(string token);
        Task Add(string token, TimeSpan expiration);
        Task TryAdd(string token, TimeSpan expiration);
    }
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private static readonly ConcurrentDictionary<string, DateTime> blackListedTokens = [];
        public Task<bool> Any(string token)
        {
            blackListedTokens.RemoveWhere(x => x.Value < DateTime.UtcNow);
            return Task.FromResult(blackListedTokens.ContainsKey(token));
        }
        public Task Add(string token, TimeSpan expiration)
        {
            blackListedTokens.AddOrUpdate(token, DateTime.UtcNow.Add(expiration));
            return Task.CompletedTask;
        }
        public async Task TryAdd(string token, TimeSpan expiration)
        {
            if (!await this.Any(token)) { await this.Add(token, expiration); }
        }
    }
}