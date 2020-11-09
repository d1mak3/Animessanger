using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerForMessanger
{
  [Serializable]
  public class User // Класс, характеризующий юзера
  {
    public string login { get; set; }
    public string password { get; set; }
    public string nickName { get; set; }

    public User()
    {

    }

    public User(string _login, string _password, string _nickname) // Конструктор для заполнение данных юзера
    {
      login = _login;
      password = _password;
      nickName = _nickname;
    }    
  }
}
