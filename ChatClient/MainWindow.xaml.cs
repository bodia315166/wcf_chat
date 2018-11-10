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
using ChatClient.ServiceChat;

namespace ChatClient
{
    
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChatClient client; // обєкт типу нашого хоста 
        int ID;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender,RoutedEventArgs e)
        {
        }
        

        void ConnectUser()  //робота кнопки і заміна контенту кнопки
        {
            if (!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));//на момент загруження вікна у нас буде створюватися обєкт
                ID = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;      //коли ввели імя ми неможемо його вже змінювати...
                bConnDicon.Content = "Disconnect";
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);     
                client = null;
                tbUserName.IsEnabled = true; //коли відключені то можем поміняти імя
                bConnDicon.Content = "Connect";
                isConnected = false;
            }

        }
		
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();   
            }
            else
            {
                ConnectUser();
            }

        }

        public void MsgCallback(string msg) // через цей метод всі смс буде видно в текстовому лісті
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count-1]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser(); 
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client!=null) 
                {
                    client.SendMsg(tbMessage.Text, ID);
                    tbMessage.Text = string.Empty;    
                }               
            }
        }

        private void tbMessage_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lbChat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
