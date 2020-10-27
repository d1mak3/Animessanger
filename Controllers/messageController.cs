using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

namespace ServerForMessanger.Controllers
{
  [EnableCors("CorsApi")] // Так можно ограничить запросы (точнее описано в файле startup.cs), но хз, влияет ли это на безопасность
  [Route("api/[controller]")] // Ссылка на контроллер
  [ApiController]
  public class messageController : Controller
  {
    private static MessagesHandler allMessages = new MessagesHandler();

    // GET api/messages
    [HttpGet]
    public string GetAllMessages() // Получаем все сообщения
    {
      var messagesFromJson = JsonSerializer.Serialize(allMessages.messages);
      return messagesFromJson.ToString();
    }

    // GET api/messages/0
    [HttpGet("{id}")] 
    public string GetMessageFromID(int id) // Получаем сообщение по айди
    {
      string message = "Message doesnt exist";
      if (id < allMessages.messages.Count && id >= 0)
        message = JsonSerializer.Serialize(allMessages.messages[id]);
      return message.ToString();
    }    

    // POST api/messages
    [HttpPost] 
    public void SendMessage(Message _newMessage) // Отправляем сообщение
    {   
      allMessages.AddMessage(_newMessage.userName, _newMessage.message);
    }
     
    
  }
}
