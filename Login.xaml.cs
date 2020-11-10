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
  /// Логика взаимодействия для Window1.xaml
  /// </summary>
  public partial class LoginWindow : Window
  {
    public LoginWindow()
    {
      InitializeComponent();      
    }

    private void Button_MouseEnter(object sender, MouseEventArgs e)
    {
      
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Owner.Visibility = Visibility.Visible;      
      this.Close();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      if (loginTextBox.Text == String.Empty || loginTextBox.Text == "Login:")
        Application.Current.Shutdown();
    }

    private void loginTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      loginTextBox.Text = String.Empty;
      loginTextBox.FontWeight = FontWeights.Normal;
      loginTextBox.FontStyle = FontStyles.Normal;
    }

    private void loginTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      loginTextBox.Text = "Login: ";
      loginTextBox.FontWeight = FontWeights.ExtraLight;
      loginTextBox.FontStyle = FontStyles.Italic;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
      passwordTextBox.Text = "Pass: ";
      passwordTextBox.FontWeight = FontWeights.ExtraLight;
      passwordTextBox.FontStyle = FontStyles.Italic;      
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
      passwordTextBox.Text = String.Empty;
      passwordTextBox.FontWeight = FontWeights.Normal;
      passwordTextBox.FontStyle = FontStyles.Normal;
    }
  }
}
