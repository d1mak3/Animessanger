using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using System.IO;


namespace ClientForMessenger
{
  /// <summary>
  /// Логика взаимодействия для Window1.xaml
  /// </summary>
  public partial class LoginWindow : Window
  {
    public LoginWindow()
    {
      InitializeComponent();      
    }

    private void Window_Closed(object sender, EventArgs e) // Закрываем клиент, если юзер закрыл окошко логина
    {      
      if (Owner.Visibility == Visibility.Hidden) 
        Application.Current.Shutdown();
    }

    public static bool CheckPass(string _login, string _password)
    {
      if (_login != String.Empty && _password != String.Empty) // Если поля заполнены
      {
        if (_login.IndexOf("%") != -1) // Ники со знаком % запрещены !!!
        {
          MessageBox.Show("Логин не должен содержать знак %");
          return false;
        }
        else
        {
          UserData user = new UserData("null", _login, _password); // Заполняем юзера для проверки
          string response = user.LoginCheck(); // Записываем ответ проверки

          if (response == "You are banned")
          {
            MessageBox.Show("You are banned");
            return false;
          }

          // Обрабатываем ответ
          if (response != "There is no account with this login" && response != "Wrong login or password") // Если пришёл ник, открываем основное окно
          {
            user.nickname = response; // Записываем ник, который пришел, в юзера

            // Добавляем юзера в config.json
            if (File.Exists("config.json")) // Если конфиг уже создан
            {
              try
              {
                string json = String.Empty;

                // Записываем новые данные в поля в config.json
                using (var readjson = new StreamReader("config.json"))
                {
                  json = readjson.ReadToEnd();
                }

                MainWindow.nickname = user.nickname;
                var jsonobject = JObject.Parse(json);                
                jsonobject["login"] = user.login;
                jsonobject["password"] = user.password;

                using (var writejson = new StreamWriter("config.json"))
                {
                  writejson.Write(jsonobject.ToString());
                }
              }
              // Если не удалось открыть config.json выходим из приложения
              catch
              {
                MessageBox.Show("config.json is already opened");
                Application.Current.Shutdown();
              }
            }
            else // Если config.json не создан
            {
              var jsonobject = new JObject();
              
              jsonobject.Add("login", user.login);
              jsonobject.Add("password", user.password);
              jsonobject.Add("width", 500);
              jsonobject.Add("height", 500);
              jsonobject.Add("isAdmin", false);
              jsonobject.Add("autologin", false);

              using (var writejson = new StreamWriter("config.json"))
              {
                writejson.Write(jsonobject.ToString());
              }
            }

            return true;
          }
          else // Если пришёл не ник, выводим то, что пришло
          {
            MessageBox.Show(response);
            return false;
          }
        }        
      }
      return false;
    }

    private void Button_Click(object sender, RoutedEventArgs e) // Если успешный логин, то открываем основное окно
    {
      bool response = CheckPass(loginTextBox.Text, passwordBox.Password);
      loginTextBox.Text = String.Empty; passwordBox.Password = String.Empty;
      if (response == true)
			{
        Owner.Visibility = Visibility.Visible;
        this.Close();
			}
    }

    private void Button_Click_1(object sender, RoutedEventArgs e) // Если нажали на Sign Up, то открываем окно регистрации
    {
      Reg openReg = new Reg();
      openReg.Owner = this;
      openReg.Show();
      this.Visibility = Visibility.Hidden;
    }    
            
    private void loginTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (loginTextBox.Text == "Login: ")
      {
        loginTextBox.Text = String.Empty;
        loginTextBox.FontWeight = FontWeights.Normal;
        loginTextBox.FontStyle = FontStyles.Normal;
      }
    }

    private void loginTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (loginTextBox.Text == String.Empty)
      {
        loginTextBox.Text = "Login: ";
        loginTextBox.FontWeight = FontWeights.ExtraLight;
        loginTextBox.FontStyle = FontStyles.Italic;
      }
    }    

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      passwordTextBox.Visibility = Visibility.Hidden;
      passwordBox.Visibility = Visibility.Visible;
      passwordBox.Focus();
    }    

    private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if(passwordBox.Password == String.Empty)
      {
        passwordBox.Visibility = Visibility.Hidden;
        passwordTextBox.Visibility = Visibility.Visible;
      }
    }

		private void loginTextBox_KeyUp(object sender, KeyEventArgs e)
		{
      if (e.Key == Key.Enter)
			{
        passwordTextBox.Focus();
      }
		}

		private void passwordBox_KeyUp(object sender, KeyEventArgs e)
		{
      if (e.Key == Key.Enter)
			{
        Button_Click(this, null);
			}
		}
	}
}
