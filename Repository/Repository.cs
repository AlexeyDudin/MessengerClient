using Domain;
using Domain.Dtos;
using Infrastructure;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class Repository<T> : INotifyPropertyChanged where T : class
    {
        private ObservableCollection<T> values = new ObservableCollection<T>();
        protected readonly string baseUrl;
        protected HubConnection hubConnection;
        protected string jwtToken = "";

        public Repository(string baseUrl, string jwtToken = "", string subscribeUrl = "", string subscribeAddFuncName = "", string subscribeEditFuncName = "")
        {
            this.baseUrl = baseUrl;
            this.jwtToken = jwtToken;
            if (!string.IsNullOrEmpty(subscribeUrl))
            {
                try
                {
                    hubConnection = new HubConnectionBuilder()
                        .WithUrl(baseUrl + subscribeUrl)
                        .Build();
                    if (!string.IsNullOrEmpty(subscribeAddFuncName))
                    {
                        // регистрируем функцию Receive для получения данных
                        hubConnection.On<T>(subscribeAddFuncName, (value) =>
                        {
                            Values.Add(value);
                        });
                    }
                    if (!string.IsNullOrEmpty(subscribeEditFuncName))
                    {
                        hubConnection.On<T>(subscribeEditFuncName, (T value) =>
                        {
                            IDomain<T> domainElem = value as IDomain<T>;
                            IDomain<T> foundedValue = null;
                            foreach (IDomain<T> elem in Values)
                            {
                                if (elem.UniqueId == domainElem.UniqueId)
                                {
                                    foundedValue.ChangeValues(value);
                                    break;
                                }
                            }
                        });
                    }


                    Task.Run(async () => await hubConnection.StartAsync()).Wait();
                }
                catch (Exception ex)
                {
                }
            }
            FillRepository();
        }

        public abstract void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //    {
        //        if (e.NewItems.Count != 0)
        //        {
        //            var newItem = e.NewItems[0] as T;
        //            if (newItem != null) 
        //            { 
        //                connection.InvokeAsync("SendMessage", newItem);
        //            }
        //        }
        //    }
        //}

        public ObservableCollection<T> Values 
        {
            get => values;
            set
            {
                values = value;
                OnPropertyChanged(nameof(Values));
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public abstract void FillRepository();
        //{
        //    Task.Run(async () =>
        //    {
        //        Values = await HttpRequester.GetInfoAsync<ObservableCollection<T>>(fillConnectionString, HttpRequestType.GET, login, userLogin);
        //    }).Wait();
        //}
    }
}
