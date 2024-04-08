using Domain;
using Infrastructure;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Repository
{
    public abstract class Repository<T> : IDisposable, INotifyPropertyChanged where T : class
    {
        private ObservableCollection<T> values = new ObservableCollection<T>();
        protected readonly string baseUrl;
        protected HubConnection hubConnection;
        protected HubConnectionSettings hubSettiongs;
        protected string jwtToken = "";
        protected string userLogin = "";

        public Repository(Dispatcher dispatcher, string baseUrl, string jwtToken = "", HubConnectionSettings hubSettings = null)
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
                        .WithAutomaticReconnect()
                        .Build();
                    if (!string.IsNullOrEmpty(hubSettings.AddFuncName))
                    {
                        // регистрируем функцию Receive для получения данных
                        hubConnection.On<T>(hubSettings.AddFuncName, (value) =>
                        {
                            dispatcher.Invoke(() =>
                            {
                                Values.Add(value);
                            });
                        });
                    }
                    if (!string.IsNullOrEmpty(hubSettings.EditFuncName))
                    {
                        hubConnection.On<T>(hubSettings.EditFuncName, (T value) =>
                        {
                            dispatcher.Invoke(() =>
                            {
                                IDomain<T> domainElem = value as IDomain<T>;
                                IDomain<T> foundedValue = null;
                                foreach (IDomain<T> elem in Values)
                                {
                                    if (elem.UniqueId == domainElem.UniqueId)
                                    {
                                        elem.ChangeValues(value);
                                        break;
                                    }
                                }
                            });
                        });
                    }
                    if (!string.IsNullOrEmpty(hubSettings.DeleteFuncName))
                    {
                        hubConnection.On<T>(hubSettings.DeleteFuncName, (T value) =>
                        {
                            dispatcher.Invoke(() =>
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

        public void Dispose()
        {
            if (hubConnection != null)
            {
                hubConnection.StopAsync().Wait();
            }
        }
    }
}
