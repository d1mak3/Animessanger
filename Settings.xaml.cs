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
  /// Логика взаимодействия для Settings.xaml
  /// </summary>
  public partial class Settings : Window
  {
    public Settings()
    {
      InitializeComponent();
      WidthBlock.Text = Convert.ToString(this.Width);
      HeightBlock.Text = Convert.ToString(this.Height);

      var jsonobject = new JObject();

      using (var jsonreader = new StreamReader("config.json"))
      {
        jsonobject = JObject.Parse(jsonreader.ReadToEnd());
      }

      if ((bool)jsonobject["autologin"] == true)
      {
        checkAutoLog.IsChecked = true;
      }
    }


    // Обработчики флажка
    private void checkAutoLog_Checked(object sender, RoutedEventArgs e)
    {
      var jsonobject = new JObject();

      using (var jsonreader = new StreamReader("config.json"))
      {
        jsonobject = JObject.Parse(jsonreader.ReadToEnd());
      }

      jsonobject["autologin"] = true;

      using (var jsonwriter = new StreamWriter("config.json"))
      {
        jsonwriter.Write(jsonobject.ToString());
      }
    }

    private void checkAutoLog_Unchecked(object sender, RoutedEventArgs e)
    {
      var jsonobject = new JObject();

      using (var jsonreader = new StreamReader("config.json"))
      {
        jsonobject = JObject.Parse(jsonreader.ReadToEnd());
      }

      jsonobject["autologin"] = false;

      using (var jsonwriter = new StreamWriter("config.json"))
      {
        jsonwriter.Write(jsonobject.ToString());
      }
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      WidthBlock.Text = Convert.ToString(this.Width);
      HeightBlock.Text = Convert.ToString(this.Height);
    }

    private void WidthBlock_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        try
        {
          Owner.Width = Convert.ToInt32(WidthBlock.Text);
        }
        catch
        {
          MessageBox.Show("Wrong width (You may enter only numbers");
        }
      }
    }

    private void HeightBlock_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        try
        {
          Owner.Height = Convert.ToInt32(HeightBlock.Text);
        }
        catch
        {
          MessageBox.Show("Wrong height (You may enter only numbers");
        }
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {      
      this.Close();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      Owner.IsEnabled = true;
    }
  }
}
