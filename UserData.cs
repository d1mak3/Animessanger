using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace ClientForMessenger
{
  [Serializable]
  class UserData
  {
    public string nickname { set; get; }
    public string login { set; get; }
    public string password { set; get; }
    public UserData()
    {

    }

    public UserData(string _nick, string _login, string _pass)
    {
      nickname = _nick;
      login = _login;
      password = _pass;
    }

    public string LoginCheck()
    {
      // Создаём POST запрос с json'ом
      var logRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/login");
      logRequest.ContentType = "application/json";
      logRequest.Method = "POST";

      // Записываем данные юзера в json
      JsonSerializer serializer = new JsonSerializer();
      StringWriter writejson = new StringWriter();
      serializer.Serialize(writejson, this);

      // Отправляем POST запрос с данными для проверки
      using (var writer = new StreamWriter(logRequest.GetRequestStream()))
      {
        writer.Write(writejson);
      }

      // Получаем ответ
      string result;
      using (var httpResponse = (HttpWebResponse)logRequest.GetResponse())
      {
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
          result = streamReader.ReadToEnd();
        }
      }

      return result;
    }

    public string Registration()
    {
      // Создаём POST запрос с json'ом
      var regRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/registration");
      regRequest.ContentType = "application/json";
      regRequest.Method = "POST";

      // Записываем данные юзера в json
      JsonSerializer serializer = new JsonSerializer();
      StringWriter jsonwriter = new StringWriter();
      serializer.Serialize(jsonwriter, this);

      // Отправляем POST запрос с данными для регистрации
      using (var streamWriter = new StreamWriter(regRequest.GetRequestStream()))
      {
        streamWriter.Write(jsonwriter);
      }

      // Получаем ответ
      string result;      
      var httpResponse = (HttpWebResponse)regRequest.GetResponse();
      using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
      {
        result = streamReader.ReadToEnd();
      }

      return result;
    }
  }
}
