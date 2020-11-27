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
    public static Dictionary<User, int> onlineUsers = new Dictionary<User, int>();
    public static List<User> allUsers = new List<User>();

    // Метод, с помощью которого мы записываем данные из файлов в оперативную память
    public static void Fill(List<User> _users)
    {
      if (_users.Count == 0 && _users == allUsers) // Если в allUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Users.txt");
        string reader = readAllUsers.ReadLine();

        allUsers = JsonSerializer.Deserialize<List<User>>(reader);

        readAllUsers.Close();

        // Записываем юзеров в список онлайна
        foreach (User u in allUsers)
        {
          onlineUsers.Add(u, 0);
        }
      }
      else if (_users.Count == 0 && _users == AdminController.bannedUsers) // Если в bannedUsers ничего нет, то пробуем добавить туда юзеров из файла
      {
        StreamReader readAllUsers = new StreamReader("Banlist.txt");
        string reader = readAllUsers.ReadLine();

        AdminController.bannedUsers = JsonSerializer.Deserialize<List<User>>(reader);

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
      Fill(AdminController.bannedUsers); // Пытаемся заполнить bannedUsers, если там ничего нет

      // Проверяем, есть ли юзер в банлисте
      foreach (User u in AdminController.bannedUsers)
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
      if (temp.nickName == null) // Если не нашли
        return "There is no account with this login";

      // Проверяем пароль
      if (BCrypt.Net.BCrypt.Verify(_userForCheck.password, temp.password) == true)
      {
        _userForCheck = temp;
        if (onlineUsers[_userForCheck] == 0)
        {
          MessageController.allMessages.AddMessage("Server", $"{_userForCheck.nickName} is now online!");
        }
        onlineUsers[_userForCheck]++;
        return _userForCheck.nickName;
      }
      return "Wrong login or password";
    }

    // DELETE api/login/nickname
    [HttpDelete("{nickname}")]
    public void GettingOffline(string nickname)
		{
      foreach(User u in allUsers)
			{
        if (u.nickName == nickname)
				{
          onlineUsers[u]--;          
          if (onlineUsers[u] == 0)
					{
            MessageController.allMessages.AddMessage("Server", $"{u.nickName} has left the chat");
					}
				}
			}      
		}
  }
}
