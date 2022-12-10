using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
using NetDz2_pochta.Server;
using NetDz2_pochta.Models;

namespace NetDz2_pochta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerApl ser = new ServerApl();
        public MainWindow()
        {
            InitializeComponent();
            ser.create();
        }
        async private void Button_Click(object sender, RoutedEventArgs e)
        {                     
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("127.0.0.1", 8888);
            MessageBox.Show("Подключение установлено");
            NetworkStream stream = tcpClient.GetStream();
            //// отправляем сообщение для отправки
            var message = IndexBox.Text;            
            // кодируем его в массив байт
            var data = Encoding.UTF8.GetBytes(message);
            // отправляем массив байт на сервер 
            await stream.WriteAsync(data);
            //MessageBox.Show($"Данные отправлены на сервер");
 

            var responseData = new byte[512];
            // StringBuilder для склеивания полученных данных в одну строку
            var response = new StringBuilder();
            int bytes = 1;  // количество полученных байтов
                // получаем данные
                bytes = await stream.ReadAsync(responseData);
                // преобразуем в строку и добавляем ее в StringBuilder
                response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                ListBoxClient.Items.Add(response);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (ind.Text != "" || str.Text != "")
            {
                Models.Index at = new Models.Index() { index = ind.Text, Street = str.Text };
                db.Games.Add(at);
                db.SaveChanges();
                MessageBox.Show("Данные удачно добавленны");
            }
            else
            {
                MessageBox.Show("Введите данные");
            }
        }

    }
}
