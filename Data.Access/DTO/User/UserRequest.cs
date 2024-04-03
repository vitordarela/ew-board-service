using Domain.Model.Enum;

namespace Domain.Model.DTO.User
{
    public class UserRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }
    }
}
