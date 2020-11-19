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
    static List<User> onlineUsers = new List<User>();
    static List<User> offlineUsers = new List<User>();
    static List<User> bannedUsers = new List<User>();
    public static List<User> allUsers = new List<User>();

    private static void Fill(List<User> _users) // Метод, с помощью которого мы записываем данные из файлов в оперативную память
    {
      if (_users.Count == 0 && _users == allUsers) // Если в allUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Users.txt");
        string reader = readAllUsers.ReadLine();        

        allUsers = JsonSerializer.Deserialize<List<User>>(reader);
        offlineUsers = allUsers;

        readAllUsers.Close();
      }
      else if (_users.Count == 0 && _users == bannedUsers) // Если в bannedUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Banlist.txt");
        string reader = readAllUsers.ReadLine();

        bannedUsers = JsonSerializer.Deserialize<List<User>>(reader);

        readAllUsers.Close();
      }
    }

    // GET api/login
    [HttpGet]
    public string GetAllUsers() // Возвращаем всех юзеров
    {
      Fill(allUsers); // Пытаемся заполнить allUsers, если там ничего нет

      return JsonSerializer.Serialize(allUsers).ToString(); // Возвращаем сериализованный список юзеров
    }

    // POST api/login
    [HttpPost]
    public string CheckPass(User _userForCheck) // Проверка пароля
    {
      Fill(allUsers); // Пытаемся заполнить allUsers, если там ничего нет
      Fill(bannedUsers); // Пытаемся заполнить bannedUsers, если там ничего нет

      // Проверяем, есть ли юзер в банлисте
      foreach(User u in bannedUsers)
      {
        if (u.login == _userForCheck.login)
        {
          return "You are banned";
        }
      }

      // Ищем юзера с таким же логином
      User temp = new User();
      foreach (User u in allUsers)
      {
        if (u.login == _userForCheck.login)
        {
          _userForCheck.nickName = u.nickName;
          temp = u;
          break;
        }
      }
      if (temp.password == null) // Если не нашли
        return "There is no account with this login";

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

        return _userForCheck.nickName;
      }
      else
        return "Wrong login or password";
    }

    // DELETE api/login/nickname
    [HttpDelete("{nickname}")]
    public string BanUser(string nickname)
    {
      Fill(allUsers); // Пытаемся заполнить allUsers, если там ничего нет
      Fill(bannedUsers); // Пытаемся заполнить bannedUsers, если там ничего нет

      foreach (User u in allUsers)
      {
        if (u.nickName == nickname)
        {
          using (StreamWriter writejson = new StreamWriter("Banlist.txt"))
          {
            bannedUsers.Add(u);
            writejson.Write(JsonSerializer.Serialize(bannedUsers));
          }
          using (StreamWriter writejson = new StreamWriter("Users.txt"))
          {
            allUsers.Remove(u);
            writejson.Write(JsonSerializer.Serialize(allUsers));
          }
          return "Successful";
        }
      }

      return "There is no user with this nickname";
    }

    // POST api/login/nickname
    [HttpPost("{nickname}")]
    public string UnbanUser(string nickname)
    {
      Fill(allUsers); // Пытаемся заполнить allUsers, если там ничего нет
      Fill(bannedUsers); // Пытаемся заполнить bannedUsers, если там ничего нет

      foreach (User u in bannedUsers)
      {
        if (u.nickName == nickname)
        {
          using (StreamWriter writejson = new StreamWriter("Users.txt"))
          {
            allUsers.Add(u);
            writejson.Write(JsonSerializer.Serialize(allUsers));
          }
          using (StreamWriter writejson = new StreamWriter("Banlist.txt"))
          {
            bannedUsers.Remove(u);
            writejson.Write(JsonSerializer.Serialize(bannedUsers));
          }
          return "Successful";
        }        
      }
      return "There are no users with this nick in banlist";
    }
  }
}
