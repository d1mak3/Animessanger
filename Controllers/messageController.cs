using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using System.IO;


namespace ServerForMessanger.Controllers
{
  //[EnableCors("CorsApi")] // Так можно ограничить запросы (точнее описано в файле startup.cs), но хз, влияет ли это на безопасность
  [Route("api/[controller]")] // Ссылка на контроллер
  [ApiController]
  public class messageController : Controller
  {
    private static MessagesHandler allMessages = new MessagesHandler();

    // GET api/messages
    [HttpGet]
    public string GetAllMessages() // Получаем все сообщения
    {
      // Если в списке нет сообщений пробуем загрузить их из файла
      if (allMessages.messages.Count == 0)
      {
        StreamReader readMessageInFile = new StreamReader("History.txt");
        string reader;
        while ((reader = readMessageInFile.ReadLine()) != null)
        {
          allMessages.AddMessage(JsonSerializer.Deserialize<Message>(reader));
        }
        readMessageInFile.Close();
      }      

      var messagesToJson = JsonSerializer.Serialize(allMessages.messages);
      return messagesToJson.ToString();
    }

    // GET api/messages/0
    [HttpGet("{id}")] 
    public string GetMessageFromID(int id) // Получаем сообщение по айди
    {
      // Если в списке нет сообщений пробуем загрузить их из файла
      if (allMessages.messages.Count == 0)
      {
        StreamReader readMessageInFile = new StreamReader("History.txt");
        string reader;
        while ((reader = readMessageInFile.ReadLine()) != null)
        {
          allMessages.AddMessage(JsonSerializer.Deserialize<Message>(reader));
        }
        readMessageInFile.Close();
      }

      string message = "Message doesnt exist";
      if (id < allMessages.messages.Count && id >= 0)
        message = JsonSerializer.Serialize(allMessages.messages[id]);
      return message.ToString();
    }    

    // POST api/messages
    [HttpPost] 
    public void SendMessage(Message _newMessage) // Отправляем сообщение
    {
      StreamWriter writeMessageInFile = new StreamWriter("History.txt", true);
      writeMessageInFile.WriteLine(JsonSerializer.Serialize(_newMessage));
      writeMessageInFile.Close();

      allMessages.AddMessage(_newMessage.userName, _newMessage.message);
    }

    // DELETE api/messages
    [HttpDelete]
    public void ListClear()
    {
      StreamWriter writeMessageInFile = new StreamWriter("History.txt", false);
      writeMessageInFile.Close();

      allMessages.messages.Clear();
    }


    // DELETE api/messages/1
    [HttpDelete("{id}")]
    public void DelteElement(int id)
    {
      if (id < allMessages.messages.Count && id >= 0)
      {
        allMessages.messages.RemoveAt(id);

        StreamWriter writeMessageInFile = new StreamWriter("History.txt", false);
        foreach(Message m in allMessages.messages)
        {
          writeMessageInFile.WriteLine(JsonSerializer.Serialize(m));
        }
        writeMessageInFile.Close();
      }
    }
  }
}
