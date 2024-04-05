using System.Collections.Generic;

namespace Domain.Dtos
{
    public class UserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public UserState State { get; set; }
    }
}
