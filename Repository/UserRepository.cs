using Domain.Dtos;
using Infrastructure;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : Repository<UserDto>
    {
        private static string fillUsersUrl = "/api/user";
        private static string addUserUrl = "/api/user/add";
        private static string deleteUserUrl = "/api/user/delete";
        private static string editUserUrl = "/api/user/update";
        public UserRepository(string baseUrl, string jwtToken) : base(baseUrl, jwtToken)
        {
        }

        public override void FillRepository()
        {
            Task.Run(async () =>
            {
                Values = await HttpRequester.SendRequestAsync<ObservableCollection<UserDto>>(this.baseUrl + fillUsersUrl, HttpRequestType.GET, jwtToken);
            });
        }

        protected override void SendAddRequestToHub(UserDto value)
        {
            Task.Run(async () =>
            {
                if (hubConnection != null)
                {
                    await hubConnection.InvokeAsync("Add", value);
                }
            });
        }

        protected override void SendAddRequestToServer(UserDto value)
        {
            Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(addUserUrl))
                {
                    var newElem = await HttpRequester.SendRequestAsync<MessageDto>(baseUrl + addUserUrl, HttpRequestType.POST, jwtToken, value);
                }
            });
        }

        protected override void SendDeleteRequestToHub(UserDto value)
        {
            Task.Run(async () =>
            {
                if (hubConnection != null)
                {
                    await hubConnection.InvokeAsync("Delete", value);
                }
            });
        }

        protected override void SendDeleteRequestToServer(UserDto value)
        {
            Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(addUserUrl))
                {
                    var newElem = await HttpRequester.SendRequestAsync<MessageDto>(baseUrl + deleteUserUrl, HttpRequestType.POST, jwtToken, value);
                }
            });
        }

        protected override void SendEditRequestToHub(UserDto value)
        {
            Task.Run(async () => { if (hubConnection != null) { await hubConnection.InvokeAsync("Edit", value); } });
        }

        protected override void SendEditRequestToServer(UserDto value)
        {
            Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(addUserUrl))
                {
                    var newElem = await HttpRequester.SendRequestAsync<MessageDto>(baseUrl + editUserUrl, HttpRequestType.POST, jwtToken, value);
                }
            });
        }
    }
}
