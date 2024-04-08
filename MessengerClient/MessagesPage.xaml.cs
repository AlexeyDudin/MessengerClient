using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MessengerClient
{
    /// <summary>
    /// Логика взаимодействия для MessagesPage.xaml
    /// </summary>
    public partial class MessagesPage : UserControl
    {
        public MessagesPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IMainWindow mainWindow = (IMainWindow)DataContext;
            MessageDto newMessage = new MessageDto()
            {
                From = mainWindow.MyLogin,
                Message = MessageTextBox.Text
            };
            if (mainWindow.SelectedUser != null)
            {
                newMessage.ToUser = mainWindow.SelectedUser.Login;
            }
            mainWindow.MessageRepo.Add(newMessage);
            MessageTextBox.Text = string.Empty;
        }

        private void ListingDataView_Filter(object sender, FilterEventArgs e)
        {
            IMainWindow mainWindow = (IMainWindow)DataContext;
            if (mainWindow == null || mainWindow.SelectedUser == null)
            {
                e.Accepted = false;
                return;
            }

            var item = e.Item as MessageDto;

            e.Accepted = item.ToUser == mainWindow.SelectedUser.Login || item.From == mainWindow.SelectedUser.Login;
        }

        public void UpdateCollection()
        {
            CollectionViewSource.GetDefaultView(ListingDataView.ItemsSource).Refresh();
        }
    }
}
