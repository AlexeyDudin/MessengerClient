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
        protected HubConnectionSettings hubSettiongs;
        protected string jwtToken = "";
        protected string userLogin = "";

        public Repository(string baseUrl, string jwtToken = "", HubConnectionSettings hubSettings = null)
        {
            this.baseUrl = baseUrl;
            this.jwtToken = jwtToken;
            if (!string.IsNullOrEmpty(jwtToken))
            {
                userLogin = jwtToken.DecodeJWTPayload("userLogin");
            }
            //Values.CollectionChanged += Values_CollectionChanged;
            this.hubSettiongs = hubSettings;
            if (hubSettings != null && !string.IsNullOrEmpty(hubSettings.SubscribeUrl))
            {
                try
                {
                    hubConnection = new HubConnectionBuilder()
                        .WithUrl(baseUrl + hubSettings.SubscribeUrl)
                        .Build();
                    if (!string.IsNullOrEmpty(hubSettings.AddFuncName))
                    {
                        // регистрируем функцию Receive для получения данных
                        hubConnection.On<T>(hubSettings.AddFuncName, (value) =>
                        {
                            Values.Add(value);
                        });
                    }
                    if (!string.IsNullOrEmpty(hubSettings.EditFuncName))
                    {
                        hubConnection.On<T>(hubSettings.EditFuncName, (T value) =>
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
                    if (!string.IsNullOrEmpty(hubSettings.DeleteFuncName))
                    {
                        hubConnection.On<T>(hubSettings.DeleteFuncName, (T value) =>
                        {
                            IDomain<T> domainElem = value as IDomain<T>;
                            IDomain<T> foundedValue = null;
                            foreach (IDomain<T> elem in Values)
                            {
                                if (elem.UniqueId == domainElem.UniqueId)
                                {
                                    Values.Remove(foundedValue as T);
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

        public void Add(T value)
        {
            if (value != null)
            {
                Values.Add(value);
                SendAddRequestToHub(value);
                SendAddRequestToServer(value);
            }
        }

        public void Edit(T value)
        {
            if (value != null)
            {
                foreach (var elem in Values)
                {
                    if (((IDomain<T>)elem).UniqueId == ((IDomain<T>)value).UniqueId)
                    {
                        ((IDomain<T>)elem).ChangeValues(value);
                        break;
                    }
                }
                SendEditRequestToHub(value);
                SendEditRequestToServer(value);
            }
        }

        public void Delete(T value)
        {
            if (value != null)
            {
                foreach (var elem in Values)
                {
                    if (((IDomain<T>)elem).UniqueId == ((IDomain<T>)value).UniqueId)
                    {
                        Values.Remove(elem);
                        break;
                    }
                }
                SendDeleteRequestToHub(value);
                SendDeleteRequestToServer(value);
            }

        }

        protected abstract void SendAddRequestToServer(T value);
        protected abstract void SendAddRequestToHub(T value);
        protected abstract void SendEditRequestToServer(T value);
        protected abstract void SendEditRequestToHub(T value);
        protected abstract void SendDeleteRequestToServer(T value);
        protected abstract void SendDeleteRequestToHub(T value);

        //private void Values_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    Task.Run(async () =>
        //    {
        //        if (hubSettiongs != null)
        //        {
        //            if (e.Action == NotifyCollectionChangedAction.Add)
        //            {
        //                foreach (var elem in e.NewItems)
        //                {
        //                    var result = await HttpRequester.SendRequestAsync<T>(baseUrl + hubSettiongs.AddFuncName, HttpRequestType.POST, jwtToken, elem);
        //                }
        //            }
        //            else if (e.Action == NotifyCollectionChangedAction.Remove)
        //            {
        //                foreach (var elem in e.OldItems)
        //                {
        //                    var result = await HttpRequester.SendRequestAsync<T>(baseUrl + hubSettiongs.DeleteFuncName, HttpRequestType.DELETE, jwtToken, elem);
        //                }
        //            }
        //        }
        //    }).Wait();
        //}


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
