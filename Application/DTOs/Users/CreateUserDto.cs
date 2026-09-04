namespace Application.DTOs.Users
{
    public class CreateUserDto
    {
        public Guid PersonId { get; set; }
        public string Password { get; set; } = string.Empty;
        public Guid? RoleId { get; set; }
        public List<Guid>? RoleIds { get; set; }
    }
}
