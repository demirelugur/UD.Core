namespace UD.Core.Helper.Services
{
    using System;
    using System.Collections.Concurrent;
    using UD.Core.Extensions;
    public interface ITokenBlacklistService // AddSingleton
    {
        bool Any(string token);
        void Add(string token, TimeSpan expiration);
        void TryAdd(string token, TimeSpan expiration);
    }
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private static readonly ConcurrentDictionary<string, DateTime> _blackListedTokens = [];
        public TokenBlacklistService() { }
        public bool Any(string token)
        {
            _blackListedTokens.RemoveWhere(x => x.Value < DateTime.UtcNow);
            return _blackListedTokens.ContainsKey(token);
        }
        public void Add(string token, TimeSpan expiration) => _blackListedTokens.AddOrUpdate(token, DateTime.UtcNow.Add(expiration));
        public void TryAdd(string token, TimeSpan expiration)
        {
            if (!this.Any(token)) { this.Add(token, expiration); }
        }
    }
}