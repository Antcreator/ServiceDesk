using ServiceDesk.Data.Model.Enums;

namespace ServiceDesk.Users.Model
{
    public record CreateUserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole Role { get; set; }
    }
}
