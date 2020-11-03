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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientForMessenger
{  
  public partial class MainWindow : Window
  {  
    public MainWindow()
    {
      InitializeComponent();  
    }    

    private void Main_Loaded_1(object sender, RoutedEventArgs e)
    {
      // Создаём окошко с логином
      LoginWindow login = new LoginWindow();
      login.Owner = this;
      this.Visibility = Visibility.Hidden;
      login.Show();

      ContactsPanel.Children.Add(new TextBlock { Text = "Даня" });
      MessagePanel.Children.Add(new TextBlock { Text = "Привет" });
         
    }    
  }
}
