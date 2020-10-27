using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForMessanger
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
  public class MessagesHandler // Класс, обрабатывающий все сообщения
  {
    public List<Message> messages = new List<Message>(); // Список сообщений

    public MessagesHandler()
    {
      messages.Clear();
      Message defaultMessage = new Message();
      messages.Add(defaultMessage);
    }

    public MessagesHandler(List<Message> _messages)
    {
      messages = _messages;
    }

    public void AddMessage(Message _newMessage) // Добавление сообщения в список с помощью объекта
    {
      _newMessage.time = DateTime.UtcNow;
      messages.Add(_newMessage);
    }

    public void AddMessage(string _userName, string _message) // Добавление сообщения в список с помощью логина и сообщения
    {
      Message newMessage = new Message(_userName, _message);
      newMessage.time = DateTime.UtcNow;
      messages.Add(newMessage);
    }
  }
}
