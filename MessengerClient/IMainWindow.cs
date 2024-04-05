using Domain.Dtos;
using Repository;

namespace MessengerClient
{
    public interface IMainWindow
    {
        Repository<MessageDto> MessageRepo { get; set; }
        Repository<UserDto> UserRepo { get; set; }
        UserDto SelectedUser { get; set; }
        string MyLogin { get; set; }
    }
}
