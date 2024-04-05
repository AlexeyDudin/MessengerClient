using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private async Task Button_Click(object sender, RoutedEventArgs e)
        {
            MessageDto newMessage = new MessageDto()
            {
                From = 
            };
        }
    }
}
