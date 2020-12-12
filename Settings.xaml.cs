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
    }

    // Когда окошко загружается, записываем туда нужные данные
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      WidthBlock.Text = Convert.ToString(Owner.Width);
      HeightBlock.Text = Convert.ToString(Owner.Height);

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

    // Обработка нажатия кнопки Enter в текстбоксах
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
          MessageBox.Show("Wrong width (You may enter only positive numbers)");
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
          MessageBox.Show("Wrong height (You may enter only positive numbers)");
        }
      }
    }

    // Обработка нажатия кнопки OK
    private void Button_Click(object sender, RoutedEventArgs e)
    {
      string errors = String.Empty; // Записываем сюда ошибки
      try
      {
        Owner.Width = Convert.ToInt32(WidthBlock.Text);
      }
      catch
      {
        errors += "Wrong width ";          
      }
      try
      {
        Owner.Height = Convert.ToInt32(HeightBlock.Text);
      }
      catch
      {
        errors += "Wrong height ";        
      }

      if (errors != String.Empty)
			{
        MessageBox.Show(errors + "(You may enter only positive numbers)");
			}
      this.Close();
    }

    // Если окно закрыли, то открываем основную форму
    private void Window_Closed(object sender, EventArgs e)
    {
      Owner.Visibility = Visibility.Visible;
    }		
	}
}
