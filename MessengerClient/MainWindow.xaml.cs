using Domain.Dtos;
using Infrastructure;
using MahApps.Metro.Controls;
using Repository;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged, IMainWindow
    {
        private const string loginUrl = "/api/user/get";
        private string endPoint = "";
        private Repository<MessageDto> messageRepo;
        private Repository<UserDto> userRepo;
        private UserDto selectedUser;
        private string myLogin;
        private string jwtToken = "";
        public MainWindow()
        {
            InitializeComponent();
            endPoint = GetEndPoint();
        }

        public Repository<MessageDto> MessageRepo
        { 
            get => messageRepo;
            set
            {
                messageRepo = value;
                OnPropertyChanged(nameof(MessageRepo));
            }
        }

        public Repository<UserDto> UserRepo
        { 
            get => userRepo;
            set
            {
                userRepo = value;
                OnPropertyChanged(nameof(UserRepo));
            }
        }

        public UserDto SelectedUser 
        { 
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            } 
        }
        public string MyLogin 
        { 
            get => myLogin;
            set
            {
                myLogin = value;
                OnPropertyChanged(nameof(MyLogin));
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string GetEndPoint()
        {
            return Properties.Resources.ipAddress + ":" + Properties.Resources.port;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AuthUser();
            LoadUsers();
            LoadMessages();
        }
        private void AuthUser()
        {
            Task.Run(async () =>
            {
                jwtToken = await HttpRequester.SendRequestAsync<string>(endPoint + loginUrl, HttpRequestType.GET, null, "Admin", "1");
                MyLogin = jwtToken.DecodeJWTPayload("userLogin");
            }).Wait();
        }
        private void LoadUsers()
        {
            Task.Run(() =>
            {
                try
                {
                    UserRepo = new UserRepository(endPoint, jwtToken);
                }
                catch (Exception ex)
                {
                    
                }
            }).Wait();
        }

        private void LoadMessages()
        {
            Task.Run(() =>
            {
                try
                {
                    MessageRepo = new MessageRepository(endPoint, jwtToken);
                }
                catch (Exception ex)
                {

                }
            }).Wait();
        }
    }
}
