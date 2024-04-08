using Domain;
using Domain.Dtos;
using Infrastructure;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Repository
{
    public class MessageRepository : Repository<MessageDto>
    {
        private static string fillMessagesUrl = "/api/messages";
        private static string addMessagesUrl = string.Empty; //"/api/user/add";
        private static string deleteMessagesUrl = string.Empty; //"/api/user/delete";
        private static string editMessagesUrl = string.Empty; //"/api/"
        public MessageRepository(Dispatcher dispatcher, string baseUrl, string jwtToken = "") : base(dispatcher, baseUrl, jwtToken, new HubConnectionSettings("/messages", "RecieveMessage", "EditMessage", "DeleteMessage"))
        {
        }

        public override void FillRepository()
        {
            Task.Run(async () =>
            {
                Values = await HttpRequester.SendRequestAsync<ObservableCollection<MessageDto>>(baseUrl + fillMessagesUrl, HttpRequestType.GET, jwtToken, userLogin);
            });
        }

        protected override void SendAddRequestToHub(MessageDto value)
        {
            Task.Run(async () =>
            {
                if (hubConnection != null)
                {
                    await hubConnection.InvokeAsync("Add", value);
                }
            });
        }

        protected override void SendAddRequestToServer(MessageDto value)
        {
            //Task.Run(async () =>
            //{
            //    if (!string.IsNullOrEmpty(addMessagesUrl))
            //    {
            //        var newElem = await HttpRequester.SendRequestAsync<MessageDto>(baseUrl + addMessagesUrl, HttpRequestType.POST, jwtToken, value);
            //    }
            //});
        }

        protected override void SendDeleteRequestToHub(MessageDto value)
        {
            Task.Run(async () =>
            {
                if (hubConnection != null)
                {
                    await hubConnection.InvokeAsync("Delete", value);
                }
            });
        }

        protected override void SendDeleteRequestToServer(MessageDto value)
        {
            //throw new System.NotImplementedException();
        }

        protected override void SendEditRequestToHub(MessageDto value)
        {
            Task.Run(async () =>
            {
                if (hubConnection != null)
                {
                    await hubConnection.InvokeAsync("Edit", value);
                }
            });
        }

        protected override void SendEditRequestToServer(MessageDto value)
        {
            //throw new System.NotImplementedException();
        }
    }
}
