using Domain.Dtos;
using Infrastructure;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Repository
{
    public class MessageRepository : Repository<MessageDto>
    {
        private static string fillMessagesUrl = "/api/messages";
        private static string addMessagesUrl = "/api/user/add";
        private static string deleteMessagesUrl = "/api/user/delete";
        public MessageRepository(string baseUrl, string jwtToken = "") : base(baseUrl, jwtToken, "/messages", "RecieveMessage")
        {
        }

        public override void FillRepository()
        {
            Task.Run(async () =>
            {
                var userLogin = jwtToken.DecodeJWTPayload("userLogin");
                Values = await HttpRequester.SendRequestAsync<ObservableCollection<MessageDto>>(baseUrl + fillMessagesUrl, HttpRequestType.GET, jwtToken, userLogin);
            });
        }

        public override async void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var elem in e.NewItems)
                {
                    var result = await HttpRequester.SendRequestAsync<MessageDto>(baseUrl + addMessagesUrl, HttpRequestType.POST, jwtToken, elem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var elem in e.OldItems)
                {
                    var result = await HttpRequester.SendRequestAsync<MessageDto>(baseUrl + deleteMessagesUrl, HttpRequestType.DELETE, jwtToken, elem);
                }
            }
        }
    }
}
