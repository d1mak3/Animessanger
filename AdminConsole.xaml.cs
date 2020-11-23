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

    // Обрабатываем нажатие enter в окошке
    private void mainconsole_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter) // Обрабатываем отправку запроса
      {
        // Удаление сообщений
        if (mainconsole.Text.IndexOf("DELETE ") != -1) // Если запрос DELETE
        {
          string[] separatedRequest = mainconsole.Text.Split(' '); // Разделяем команду и id 
          string fullRequest = mainconsole.Text;
          mainconsole.Text = String.Empty; // Убираем сообщение после того, как сохранили ввод

          if (separatedRequest.Length == 2 && fullRequest[0] == 'D') // Первая проверка на правильность введённых данных
          {
            try // Вторая проверка введённых данных
            {              
              int id = Convert.ToInt32(separatedRequest[1]);

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
              deleteResponse.Close();
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
        else if (mainconsole.Text.IndexOf("BAN ") != -1) // Если запрос BAN
        {
          string[] separatedRequest = mainconsole.Text.Split(' '); // Разделяем команду и id 
          string fullRequest = mainconsole.Text;
          mainconsole.Text = String.Empty; // Убираем сообщение после того, как сохранили ввод

          if (separatedRequest.Length == 2 && fullRequest[0] == 'B') // Первая проверка на правильность введённых данных
          {
            var banRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/login/" + $"{separatedRequest[1]}"); // В request[1] хранится строка с ником
            banRequest.Method = "DELETE";

            var banResponse = banRequest.GetResponse();
            using (var stream = banResponse.GetResponseStream())
            {
              using (var reader = new StreamReader(stream))
              {
                responseBlock.Text = reader.ReadToEnd();
              }
            }
            banResponse.Close();
          }
          else
          {            
            responseBlock.Text = "Bad request";
          }
        }

        // Анбан юзера по нику
        else if (mainconsole.Text.IndexOf("UNBAN ") != -1) // Если запрос DELETE
        {
          string[] separatedRequest = mainconsole.Text.Split(' '); // Разделяем команду и id 
          string fullRequest = mainconsole.Text;
          mainconsole.Text = String.Empty; // Убираем сообщение после того, как сохранили ввод

          if (separatedRequest.Length == 2 && fullRequest[0] == 'U') // Первая проверка на правильность введённых данных
          {
            var banRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/login/" + $"{separatedRequest[1]}"); // В request[1] хранится строка с ником    
            var banResponse = banRequest.GetResponse();
            using (var stream = banResponse.GetResponseStream())
            {
              using (var reader = new StreamReader(stream))
              {
                responseBlock.Text = reader.ReadToEnd();
              }
            }
            banResponse.Close();
          }         

          else
          {
            responseBlock.Text = "Bad request";
          }
        }

        // Добавляем или удаляем админа по нику
        else if (mainconsole.Text.IndexOf("ADMIN ") != -1)
        {
          string[] separatedRequest = mainconsole.Text.Split(' '); // Разделяем команду и id 
          string fullRequest = mainconsole.Text;
          mainconsole.Text = String.Empty; // Убираем сообщение после того, как сохранили ввод

          if (separatedRequest.Length == 2 && fullRequest[0] == 'A') // Первая проверка на правильность введённых данных
          {
            var adminRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/Admin/");    
            adminRequest.Method = "POST";
            adminRequest.ContentType = "application/json";

            JsonSerializer serializer = new JsonSerializer();
            StringWriter jsonwriter = new StringWriter();
            serializer.Serialize(jsonwriter, new UserData { login = "null", nickname = separatedRequest[1], password = "null" }); // В request[1] хранится строка с ником

            using (var writer = new StreamWriter(adminRequest.GetRequestStream()))
						{
              writer.Write(jsonwriter);
						}

            var adminResponse = adminRequest.GetResponse();            
            using (var stream = adminResponse.GetResponseStream())
            {
              using (var reader = new StreamReader(stream))
              {
                if (Convert.ToBoolean(reader.ReadToEnd()) == false)
                  responseBlock.Text = "Cannot Admin or Unadmin user (maybe user doesnt exist)";
                else
                  responseBlock.Text = "Successful";
              }
            }
            adminResponse.Close();
          }
        }

        else if (mainconsole.Text == "HELP") // При вводе HELP будем выводить все команды
        {
          responseBlock.Text = "DELETE ID\nBAN NICKNAME\nUNBAN NICKNAME";
        }
        else
        {
          mainconsole.Text = String.Empty;
          responseBlock.Text = "Bad request";
        }
      }
    }
  }
}
