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
    List<User> allUsers = new List<User>();

    // POST api/registration
    [HttpPost]
    public string Registration(User _newUser) // Регистрируем юзера
    {
      if (allUsers.Count == 0) // Если в allUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Users.txt");
        string reader;
        while ((reader = readAllUsers.ReadLine()) != null)
        {
          allUsers.Add(JsonSerializer.Deserialize<User>(reader));          
        }
        readAllUsers.Close();
      }


      // Проверяем использован ли ник или логин
      bool checkNameUsed = false;
      foreach (User u in allUsers)
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

        StreamWriter writeUserInFile = new StreamWriter("Users.txt", true);
        writeUserInFile.WriteLine(JsonSerializer.Serialize(_newUser));
        writeUserInFile.Close();
        LoginController.allUsers.Add(_newUser);
        return "Sucсessful";
      }
      return "nickname or login is already used";
    }

    // GET api/login
    [HttpGet]
    public string GetAllUsers() // Возвращаем всех юзеров
    {
      if (allUsers.Count == 0) // Если в allUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Users.txt");
        string reader;
        while ((reader = readAllUsers.ReadLine()) != null)
        {
          allUsers.Add(JsonSerializer.Deserialize<User>(reader));          
        }
        readAllUsers.Close();
      }

      return JsonSerializer.Serialize(allUsers).ToString(); // Возвращаем сериализованный список юзеров
    }
  }
}
