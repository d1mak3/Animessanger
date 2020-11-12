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

    private void NicknameBox_GotFocus(object sender, RoutedEventArgs e)
    {
      NicknameBox.Text = String.Empty;
      NicknameBox.FontWeight = FontWeights.Normal;
      NicknameBox.FontStyle = FontStyles.Normal;
    }

    private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
    {
      LoginBox.Text = String.Empty;
      LoginBox.FontWeight = FontWeights.Normal;
      LoginBox.FontStyle = FontStyles.Normal;
    }

    private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
    {
      PasswordBox.Text = String.Empty;
      PasswordBox.FontWeight = FontWeights.Normal;
      PasswordBox.FontStyle = FontStyles.Normal;
    }

    private void NicknameBox_LostFocus(object sender, RoutedEventArgs e)
    {
      NicknameBox.Text = "Nick: ";
      NicknameBox.FontWeight = FontWeights.ExtraLight;
      NicknameBox.FontStyle = FontStyles.Italic;
    }

    private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
    {
      LoginBox.Text = "Login: ";
      LoginBox.FontWeight = FontWeights.ExtraLight;
      LoginBox.FontStyle = FontStyles.Italic;
    }

    private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
    {
      PasswordBox.Text = "Pass: ";
      PasswordBox.FontWeight = FontWeights.ExtraLight;
      PasswordBox.FontStyle = FontStyles.Italic;
    }
  }
}
