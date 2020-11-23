using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;

namespace ServerForMessanger.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RegistrationController : Controller
  {  
    // POST api/registration
    [HttpPost]
    public string Registration(User _newUser) // Регистрируем юзера
    {
      
      LoginController.Fill(LoginController.allUsers);
      LoginController.Fill(AdminController.bannedUsers); 

      // Проверяем использован ли ник или логин
      bool checkNameUsed = false;

      foreach (User u in AdminController.bannedUsers)
			{
        if (u.login == _newUser.login || u.nickName == _newUser.login)
          checkNameUsed = true;
			}
      
      foreach (User u in LoginController.allUsers)
      {
        if (_newUser.login == u.login || _newUser.nickName == u.nickName)
        {
          checkNameUsed = true;
          break;
        }
      }

      // Если ник и логин не заняты
      if (checkNameUsed == false)
      {
        // Хешируем пароль
        if (_newUser.password != null)
          _newUser.password = BCrypt.Net.BCrypt.HashPassword(_newUser.password);

        StreamWriter writeUserInFile = new StreamWriter("Users.txt");
        LoginController.allUsers.Add(_newUser);        
        writeUserInFile.WriteLine(JsonSerializer.Serialize(LoginController.allUsers));
        writeUserInFile.Close();  
        
        LoginController.onlineUsers.Add(_newUser, 0);
        messageController.allMessages.AddMessage("Server", $"{_newUser.nickName} just registered. Welcome!");
        return "Sucсessful";
      }
      return "nickname or login is already used";
    }

    // GET api/login
    [HttpGet]
    public string GetAllUsers() // Возвращаем всех юзеров
    {
      if (LoginController.allUsers.Count == 0) // Если в allUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Users.txt");
        string reader;
        while ((reader = readAllUsers.ReadLine()) != null)
        {
          LoginController.allUsers.Add(JsonSerializer.Deserialize<User>(reader));          
        }
        readAllUsers.Close();
      }

      return JsonSerializer.Serialize(LoginController.allUsers).ToString(); // Возвращаем сериализованный список юзеров
    }
  }
}
