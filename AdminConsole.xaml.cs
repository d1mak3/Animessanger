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
using System.Windows.Shapes;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace ClientForMessenger
{
  /// <summary>
  /// Логика взаимодействия для AdminConsole.xaml
  /// </summary>
  public partial class AdminConsole : Window
  {
    public AdminConsole()
    {
      InitializeComponent();
      mainconsole.Focus();
    }

    private void mainconsole_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) // Обрабатываем отправку запроса
      {
        // Удаление сообщений
        if (mainconsole.Text.IndexOf("DELETE") != -1) // Если запрос DELETE
        {
          string[] request = mainconsole.Text.Split(' '); // Разделяем команду и id 
          mainconsole.Text = String.Empty; // Убираем сообщение после того, как сохранили ввод

          if (request.Length == 2) // Первая проверка на правильность введённых данных
          {
            try // Вторая проверка введённых данных
            {
              int id = Convert.ToInt32(request[1]);

              var deleteRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/message/" + $"{id}");
              deleteRequest.Method = "DELETE";

              var deleteResponse = deleteRequest.GetResponse();
              using (var stream = deleteResponse.GetResponseStream())
              {
                using (var reader = new StreamReader(stream))
                {
                  responseBlock.Text = reader.ReadToEnd();
                }
              }
            }
            catch
            {
              responseBlock.Text = "Bad request";
            }
          }
          else
          {
            responseBlock.Text = "Bad request";
          }
        }

        // Бан юзера по нику
        else if (mainconsole.Text.IndexOf("BAN") != -1) // Если запрос DELETE
        {
          string[] request = mainconsole.Text.Split(' '); // Разделяем команду и id 
          mainconsole.Text = String.Empty; // Убираем сообщение после того, как сохранили ввод

          if (request.Length == 2) // Первая проверка на правильность введённых данных
          {
            var banRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/login/" + $"{request[1]}"); // В request[1] хранится строка с ником
            banRequest.Method = "DELETE";

            var banResponse = banRequest.GetResponse();
            using (var stream = banResponse.GetResponseStream())
            {
              using (var reader = new StreamReader(stream))
              {
                responseBlock.Text = reader.ReadToEnd();
              }
            }
          }
          else
          {
            responseBlock.Text = "Bad request";
          }
        }

        // Анбан юзера по нику
        else if (mainconsole.Text.IndexOf("BAN") != -1) // Если запрос DELETE
        {
          string[] request = mainconsole.Text.Split(' '); // Разделяем команду и id 
          mainconsole.Text = String.Empty; // Убираем сообщение после того, как сохранили ввод

          if (request.Length == 2) // Первая проверка на правильность введённых данных
          {
            var banRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/login/" + $"{request[1]}"); // В request[1] хранится строка с ником
            banRequest.Method = "DELETE";

            var banResponse = banRequest.GetResponse();
            using (var stream = banResponse.GetResponseStream())
            {
              using (var reader = new StreamReader(stream))
              {
                responseBlock.Text = reader.ReadToEnd();
              }
            }
          }
          else
          {
            responseBlock.Text = "Bad request";
          }
        }

        else if (mainconsole.Text == "HELP") // При вводе HELP будем выводить все команды
        {
          responseBlock.Text = "DELETE ID\nBAN NICKNAME\nUNBAN NICKNAME";
        }
      }
    }
  }
}
