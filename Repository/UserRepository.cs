using Domain.Dtos;
using Infrastructure;
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

        public override void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
