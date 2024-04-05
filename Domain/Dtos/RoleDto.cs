using System.Collections.Generic;

namespace Domain.Dtos
{
    public class RoleDto
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Users { get; set; } = new List<string>();
        public string Parent { get; set; } = string.Empty;
        public List<RoleDto> Child { get; set; } = new List<RoleDto>();
    }
}
