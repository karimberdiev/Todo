namespace Todo.API.Auth
{
    public interface ITokenStore
    {
        string CreateToken(string userId);
        bool TryGetUserId(string token, out string? userId);
    }
}
