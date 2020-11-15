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

namespace ClientForMessenger
{
  /// <summary>
  /// Логика взаимодействия для Reg.xaml
  /// </summary>
  public partial class Reg : Window
  {
    public Reg()
    {
      InitializeComponent();
    }

    // Убираем надписи Nick: Login: и Pass: в текстбоксах при фокусе
    private void NicknameBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (NicknameBox.Text == "Nick: ") 
      {
        NicknameBox.Text = String.Empty;
        NicknameBox.FontWeight = FontWeights.Normal;
        NicknameBox.FontStyle = FontStyles.Normal;
      }
    }

    private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (LoginBox.Text == "Login: ")
      {
        LoginBox.Text = String.Empty;
        LoginBox.FontWeight = FontWeights.Normal;
        LoginBox.FontStyle = FontStyles.Normal;
      }
    }

    private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (PasswordBox.Text == "Pass: ")
      {
        PasswordBox.Text = String.Empty;
        PasswordBox.FontWeight = FontWeights.Normal;
        PasswordBox.FontStyle = FontStyles.Normal;
      }
    }
    // Создаём надписи Nick: Login: и Pass: в текстбоксах при лостфокусе
    private void NicknameBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (NicknameBox.Text == String.Empty)
      {
        NicknameBox.Text = "Nick: ";
        NicknameBox.FontWeight = FontWeights.ExtraLight;
        NicknameBox.FontStyle = FontStyles.Italic;
      }
    }

    private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (LoginBox.Text == String.Empty)
      {
        LoginBox.Text = "Login: ";
        LoginBox.FontWeight = FontWeights.ExtraLight;
        LoginBox.FontStyle = FontStyles.Italic;
      }
    }

    private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (PasswordBox.Text == String.Empty)
      {
        PasswordBox.Text = "Pass: ";
        PasswordBox.FontWeight = FontWeights.ExtraLight;
        PasswordBox.FontStyle = FontStyles.Italic;
      }
    }

    // Обрабатываем кнопку регистрации
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if ((NicknameBox.Text != String.Empty || NicknameBox.Text != "Nick: ") && (LoginBox.Text != String.Empty || LoginBox.Text != "Login: ")
        && (PasswordBox.Text != String.Empty || PasswordBox.Text != "Pass: ")) // Проверяем, чтобы текстбоксы были заполнены
      {
        UserData newUser = new UserData(NicknameBox.Text, LoginBox.Text, PasswordBox.Text); // Создаём юзера
        string response = newUser.Registration(); // Регистрируем юзера

        // Проверяем ответ на запрос регистрации
        if (response == "Sucсessful") // Если успешно, то закрываем окошко регистрации и показываем окошко логина
        {
          MessageBox.Show(response);
          Owner.Visibility = Visibility.Visible;
          this.Close();
        }
        else // Если не удалось зарегистрироваться, то просто показываем ответ сервера
          MessageBox.Show(response);
      }
    }

    private void Registration_Closed(object sender, EventArgs e)
    {
      Owner.Visibility = Visibility.Visible;
    }
  }
}
