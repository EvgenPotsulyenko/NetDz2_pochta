using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace NetDz2_pochta.Server
{   
    internal class ServerApl
    {
        public ServerApl()
        {

        }
        async public void create()
        {          
            var tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();    // запускаем сервер               
            try
            {                       
                while (true)
                {
                    // получаем подключение в виде TcpClient
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                    NetworkStream stream = tcpClient.GetStream();
                    var responseData = new byte[512];
                    // StringBuilder для склеивания полученных данных в одну строку
                    var response = new StringBuilder();
                    int bytes;  // количество полученных байтов
                    do
                    {
                        // получаем данные
                        bytes = await stream.ReadAsync(responseData);
                        // преобразуем в строку и добавляем ее в StringBuilder
                        response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                        //return response;
                        //if(response.ToString() == "2122")
                        //{
                            string street = "" ;
                            ApplicationDbContext db = new ApplicationDbContext();
                            var ct = db.Games
                           .Where(c => c.index == response.ToString())
                           .ToList();
                            foreach (var c1 in ct)
                                street = c1.Street;
                            //string street =  "st.shepetova,st.pochtovaa,st.bratskaa"; 
                            byte[] msg = Encoding.UTF8.GetBytes( street);
                            await stream.WriteAsync(msg);

                        //}
                    }
                    while (bytes > 0); // пока данные есть в потоке 
                    {
                        // выводим данные
                    }
                    tcpClient.Close();
                }
                
            }
            finally
            {
                tcpListener.Stop(); // останавливаем сервер
            }
        }
    }
}
