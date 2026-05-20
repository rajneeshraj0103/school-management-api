namespace School_Management.Entities
{
    public class Student
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }    
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Role { get; set; } = "User";
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
