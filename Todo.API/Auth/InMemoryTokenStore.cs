using System.Collections.Concurrent;

namespace Todo.API.Auth
{
    public class InMemoryTokenStore : ITokenStore
    {
        private readonly ConcurrentDictionary<string, string> _tokens = new();

        public string CreateToken(string userId)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            _tokens[token] = userId;
            return token;
        }

        public bool TryGetUserId(string token, out string? userId)
        {
            return _tokens.TryGetValue(token, out userId);
        }
    }
}
