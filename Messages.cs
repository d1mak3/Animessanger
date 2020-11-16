using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClientForMessenger
{
  [Serializable]
  public class Message // Класс, описывающий одно сообщение
  {
    public string userName { get; set; } // Ник отправителя
    public string message { get; set; } // Отправляемое сообщение
    public DateTime time { get; set; } // Время отправления сообщение
    public Message() // Конструктор для автоматического заполнения сообщения
    {
      userName = "Server";
      message = "Working";
      time = DateTime.UtcNow;
    }
    public Message(string _userName, string _message) // Конструктор для заполнения "в ручную"
    {
      userName = _userName;
      message = _message;
      time = DateTime.UtcNow;
    }
  }

  [Serializable]
  public class MessagesOperationHandler // Класс, обрабатывающий операции с сообщениями
  {
    public MessagesOperationHandler()
    {

    }

    public bool SendMessage(Message _message) // Отправляем сообщение заданное объектом
    {
      StringWriter Jsonwriter = new StringWriter(); // Сюда записываем результат сериализации

      // Сериализуем сообщение
      var serializer = new JsonSerializer();
      serializer.Serialize(Jsonwriter, _message);

      // Создаём POST запрос, с json'ом
      var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/message");
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "POST";

      // Записываем наше json сообщение в поток запроса
      using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
      {
        streamWriter.Write(Jsonwriter);
      }

      // Получаем ответ (и проверяем запрос на админку)
      var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
      using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
      {
        var result = streamReader.ReadToEnd();
        if (Convert.ToBoolean(result) == true)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    public void GetMessages(out List<Message> _messages) // Получаем сообщение по id                      
    {
      // Создаём запрос
      WebRequest _request = WebRequest.Create("http://localhost:5000/api/message/");
      WebResponse _response = _request.GetResponse();

      // Записываем овет в строку
      string serializedMessage;
      using (Stream stream = _response.GetResponseStream())
      {
        using (StreamReader reader = new StreamReader(stream))
        {
          serializedMessage = reader.ReadToEnd();
        }
      }      
      // Десериализуем (пытаемся) строку с ответом
      var Jsonserializer = new JsonSerializer();
      if (serializedMessage != "Message doesnt exist")
        _messages = (List<Message>)Jsonserializer.Deserialize(new StringReader(serializedMessage), typeof(List<Message>));
      else
      {
        List<Message> MDE = new List<Message>();
        _messages = MDE;
      }
      
      _response.Close(); // Закрываем поток ответа
    }
  }
}
