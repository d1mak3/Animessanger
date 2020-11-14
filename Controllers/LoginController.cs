using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;

namespace ServerForMessanger.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LoginController : Controller
  {
    List<User> onlineUsers = new List<User>();
    List<User> offlineUsers = new List<User>();
    public static List<User> allUsers = new List<User>();    

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
          offlineUsers = allUsers;
        }
        readAllUsers.Close();
      }

      return JsonSerializer.Serialize(allUsers).ToString(); // Возвращаем сериализованный список юзеров
    }    

    // POST api/login
    [HttpPost]
    public bool CheckPass(User _userForCheck) // Проверка пароля
    {
      if (allUsers.Count == 0) // Если в allUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Users.txt");
        string reader;
        while ((reader = readAllUsers.ReadLine()) != null)
        {
          allUsers.Add(JsonSerializer.Deserialize<User>(reader));
          offlineUsers = allUsers;
        }
        readAllUsers.Close();
      }

      // Ищем юзера с таким же логином
      User temp = new User();
      foreach(User u in allUsers)
      {
        if (u.login == _userForCheck.login)
        {
          temp = u;
          break;
        }
      }
      if (temp.password == null) // Если не нашли
        return false;

      // Проверяем пароль
      if (BCrypt.Net.BCrypt.Verify(_userForCheck.password, temp.password) == true)
      {
        bool checkForOnline = false;
        foreach (User u in onlineUsers)
        {
          if (u.nickName == _userForCheck.nickName)
            checkForOnline = true;
        }
        if (checkForOnline == false)
          onlineUsers.Add(_userForCheck);

        return true;
      }
      else
        return false;
    }

  }
}
