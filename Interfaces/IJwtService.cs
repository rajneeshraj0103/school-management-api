namespace School_Management.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string email, string password);

        string GenerateRefreshToken();
    }
}
