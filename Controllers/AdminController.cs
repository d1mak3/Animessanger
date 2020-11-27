using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;

namespace ServerForMessanger.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : Controller
	{
    public static List<User> bannedUsers = new List<User>(); // Забаненные юзеры

    // DELETE api/Admin/nickname
    [HttpDelete("{nickname}")]
    public string BanUser(string nickname)
    {
      LoginController.Fill(LoginController.allUsers); // Пытаемся заполнить allUsers, если там ничего нет
      LoginController.Fill(bannedUsers); // Пытаемся заполнить bannedUsers, если там ничего нет

      foreach (User u in LoginController.allUsers)
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
            LoginController.allUsers.Remove(u);
            writejson.Write(JsonSerializer.Serialize(LoginController.allUsers));
          }
          return "Successful";
        }
      }

      return "There is no user with this nickname";
    }

    // GET api/Admin/nickname
    [HttpGet("{nickname}")]
    public string UnbanUser(string nickname)
    {
      LoginController.Fill(LoginController.allUsers); // Пытаемся заполнить allUsers, если там ничего нет
      LoginController.Fill(bannedUsers); // Пытаемся заполнить bannedUsers, если там ничего нет

      foreach (User u in bannedUsers)
      {
        if (u.nickName == nickname)
        {
          using (StreamWriter writejson = new StreamWriter("Users.txt"))
          {
            LoginController.allUsers.Add(u);
            writejson.Write(JsonSerializer.Serialize(LoginController.allUsers));
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

    // POST api/Admin/nickname
    [HttpPost]
    public bool CreateOrDeleteAdmin(User user)
		{
      LoginController.Fill(LoginController.allUsers); // Пытаемся заполнить allUsers, если там ничего нет
      LoginController.Fill(bannedUsers); // Пытаемся заполнить bannedUsers, если там ничего нет

      bool checkIfExists = false;
      
      foreach (User u in bannedUsers)
      {
        if (u.nickName == user.nickName)
          return false;
      }

      // Ищем юзера в списке (если нет, то возвращаем false
      foreach (User u in LoginController.allUsers) 
			{
        if (u.nickName == user.nickName)
				{
          checkIfExists = true;
          break;
        }      
			}
      if (checkIfExists == false)
			{
        return false;
			}

      
      // Проверяем, есть ли админ с таким ником
      using (var reader = new StreamReader("admins.txt"))
			{
        string list = reader.ReadToEnd();
        if (list.IndexOf($"%{user.nickName}%") == -1)
				{
          checkIfExists = false;
				}
        else
				{
          checkIfExists = true;
				}
			}

      // Если такого админа нет, то добавляем
      if (checkIfExists == false)
			{
        using (var writer = new StreamWriter("admins.txt", true))
        {
          writer.WriteLine("\n" + $"%{user.nickName}%");
        }

        MessageController.allMessages.AddMessage("Server", $"{user.nickName} is now admin. Congratulations!");

        return true;
      }
      // Иначе убираем
      // Переписываем всех админов, кроме того, которого нужно убрать. Затем переписываем получившийся список обратно в файл
      else 
			{
        List<string> admins = new List<string>();

        using (var reader = new StreamReader("admins.txt"))
				{
          string admin = String.Empty;

          while (admin != null)
					{
            admin = reader.ReadLine();
            if (admin != $"%{user.nickName}%")
						{
              admins.Add(admin);
						}
					}          
        }        
        
        System.IO.File.WriteAllLines("admins.txt", admins);

        MessageController.allMessages.AddMessage("Server", $"{user.nickName} has lost admin status!");

        return true;
      }
    }
  }
}
