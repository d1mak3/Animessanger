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
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ClientForMessenger
{  
  public partial class MainWindow : Window
  {
    MessagesOperationHandler operation = new MessagesOperationHandler();
    bool IsFullWindowed = false;
    public static string nickname = "null";
    public MainWindow()
    {
      InitializeComponent();      
      TypeTextBox.Width = this.Width / 1.6;
      SendButton.Width = this.Width / 10;
      Settings.Width = this.Width / 7.92;
      Logout.Width = this.Width / 7.92;
      Admin.Width = this.Width / 7.92;      

      try // Если config.json нет, создаём его с пустыми (дефолтными) параметрами
      {
        // Достаём размеры формы из файла config.json (если они там есть)
        var jsonobject = new JObject();

        using (var readjson = new StreamReader("config.json"))
        {
          jsonobject = JObject.Parse(readjson.ReadToEnd()); // Парсим содержимое файла в jsonobject
        }

        if ((double)jsonobject["width"] != 0 && (double)jsonobject["height"] != 0)
        {
          this.Width = (double)jsonobject["width"];
          this.Height = (double)jsonobject["height"];
        }
      }
      catch
      {
        var jsonobject = new JObject();
       
        jsonobject.Add("login", "null");
        jsonobject.Add("password", "null");
        jsonobject.Add("width", 500);
        jsonobject.Add("height", 500);
        jsonobject.Add("isAdmin", false);
        jsonobject.Add("autologin", false);

        using (var writejson = new StreamWriter("config.json"))
        {
          writejson.Write(jsonobject.ToString());
        }
      }
    }    

    private void Main_Loaded_1(object sender, RoutedEventArgs e) // Когда главное окошко загрузилось
    {    
      var jsonobject = new JObject(); 

      using (var readjson = new StreamReader("config.json"))
      {
        jsonobject = JObject.Parse(readjson.ReadToEnd()); // Парсим содержимое файла в jsonobject
      }

      // Проверяем не стоит ли авто логин (и на всякий случай поля юзера)
      if ((bool)jsonobject["autologin"] == false)
      {
        this.Visibility = Visibility.Hidden;

        // Создаём окошко с логином  
        LoginWindow login = new LoginWindow();
        login.Show();
        login.Owner = this;
      }   
      // Проверяем подлинность данных в config.json
      else if ((bool)jsonobject["autologin"] == true)
			{
        bool response = LoginWindow.CheckPass((string)jsonobject["login"], (string)jsonobject["password"]);
        if (response == false)
				{
          using (var jsonwriter = new StreamWriter("config.json"))
					{
            jsonobject["autologin"] = false;
            jsonwriter.Write(jsonobject.ToString());
					}         
          // Создаём окошко с логином  
          LoginWindow login = new LoginWindow();
          login.Show();
          login.Owner = this;
          this.Visibility = Visibility.Hidden;
          
				}
			}
    }

    private void Main_SizeChanged(object sender, SizeChangedEventArgs e) // Меняем ввод и кнопку в зависимости от размеров окошка
    {
      TypeTextBox.Width = this.Width / 1.6;
      SendButton.Width = this.Width / 10;
      Settings.Width = this.Width / (7.92 * 2);
      Logout.Width = this.Width / 7.92;
      Admin.Width = this.Width / (7.92 * 2);
    }    

    public async Task GetMessages() // Асинхронный (чтобы приложение не зависало) метод, который осуществляет приём сообщений
    {   
      List<Message> get = new List<Message>(); // Список сообщений, которые мы будем выводить на экран     
            
      while (true) // Приём сообщений работает, пока работает окошко
      {
        // Достаём ник
        var jsonobject = new JObject();

        using (var readjson = new StreamReader("config.json"))
        {
          jsonobject = JObject.Parse(readjson.ReadToEnd()); // Парсим содержимое файла в jsonobject
        }
                        
        try
				{
          Dispatcher.Invoke(() => // Диспетчер для того, чтобы дать возможность управлять MessagePanel потоку, который работает асинхронно
          {
            operation.GetMessages(out get); // Принимаем все сообщения       

            MessagePanel.Children.Clear(); // Очищаем панель, чтобы сообщения вставлялись один раз
            foreach (Message m in get) // Заносим все сообщения в панельку
            {
              if (m.userName == nickname)
              {
                m.userName = "You";
                MessagePanel.Children.Add(new TextBlock { Text = $"{m.userName}\n{m.message}\n\t\t{m.time}", HorizontalAlignment = HorizontalAlignment.Right, FontSize = 15 });
              }
              else
              {
                MessagePanel.Children.Add(new TextBlock { Text = $"{m.userName}\n{m.message}\n\t\t{m.time}", HorizontalAlignment = HorizontalAlignment.Left, FontSize = 15 });
              }
            }
          });
        }
        catch
				{
          MessageBox.Show("Lost connection to the server");
          App.Current.Shutdown();
				}
        await Task.Delay(1);
      }      
    }

    private void SendButton_Click(object sender, RoutedEventArgs e) // Отправка сообщений
    {
      if (TypeTextBox.Text != String.Empty) // Если TextBox в который мы вводим текст имеет хоть что-то
      {
        // Достаём логин из файла config.json
        var jsonobject = new JObject();

        using (var readjson = new StreamReader("config.json"))
        {
          jsonobject = JObject.Parse(readjson.ReadToEnd()); // Парсим содержимое файла в jsonobject
        }        

        Message newMessage = new Message(nickname, TypeTextBox.Text); // Создаём новое сообщение с текстом из TextBox
        bool result = operation.SendMessage(newMessage); // Отправляем сообщение     
        TypeTextBox.Text = String.Empty; // Стираем сообщение из TextBox

        if (result == true && (bool)jsonobject["isAdmin"] == false) // Если юзер - админ, но в конфиге ещё не записано, то включаем режим админа
        {
          jsonobject["isAdmin"] = true;
          MessageBox.Show("Admin mode: ON");
          Admin.Visibility = Visibility.Visible;
          using (var jsonwriter = new StreamWriter("config.json"))
          {
            jsonwriter.Write(jsonobject.ToString());
          }
        }
        else if (result == false && newMessage.message == "/admin") // Если юзер - не админ
        {
          MessageBox.Show("You are not admin");
        }
        else if (result == true && (bool)jsonobject["isAdmin"] == true) // Если юзер - админ, и в конфиге это уже записано, то отключаем режим админа
        {
          MessageBox.Show("Admin mode: OFF");
          jsonobject["isAdmin"] = false;
          Admin.Visibility = Visibility.Hidden;
          using (var jsonwriter = new StreamWriter("config.json"))
          {
            jsonwriter.Write(jsonobject.ToString());
          }
        }
      }

      TypeTextBox.Focus();
    }

    private async void MessagePanel_Loaded(object sender, RoutedEventArgs e) // Вызываем асинхронный приём сообщений при загрузке основного окошечка
    {
      await GetMessages();
    }

    private void Main_Closed(object sender, EventArgs e) 
    {
      // Записываем размеры окна в config.json
      var jsonobject = new JObject();

      using (var readjson = new StreamReader("config.json"))
      {
        jsonobject = JObject.Parse(readjson.ReadToEnd()); // Парсим содержимое файла в jsonobject
      }

      jsonobject["width"] = this.Width;
      jsonobject["height"] = this.Height;

      using (var writejson = new StreamWriter("config.json"))
      {
        writejson.Write(jsonobject.ToString());
      }      
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
      Settings settingsWindow = new Settings();
      settingsWindow.Owner = this;
      settingsWindow.Show();
      this.IsEnabled = false;
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
      var jsonobject = new JObject();

      using (var readjson = new StreamReader("config.json"))
      {
        jsonobject = JObject.Parse(readjson.ReadToEnd()); // Парсим содержимое файла в jsonobject
      }

      this.Visibility = Visibility.Hidden;
      jsonobject["autologin"] = false;
      jsonobject["isAdmin"] = false;

      using (var writejson = new StreamWriter("config.json"))
      {
        writejson.Write(jsonobject.ToString());
      }

      LoginWindow login = new LoginWindow();
      login.Owner = this;
      login.Show();
    }

    private void Main_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (this.Visibility == Visibility.Hidden)
      {
        MessagePanel.IsEnabled = false;
      }
      else
      {
        var jsonobject = new JObject();

        using (var jsonreader = new StreamReader("config.json"))
        {
          jsonobject = JObject.Parse(jsonreader.ReadToEnd());
        }

        if ((bool)jsonobject["isAdmin"] == true)
        {
          Admin.Visibility = Visibility.Visible;
        }
        else
        {
          Admin.Visibility = Visibility.Hidden;
        }
      }
    }

    private void Admin_Click(object sender, RoutedEventArgs e)
    {
      AdminConsole console = new AdminConsole();
      console.Show();
    }

    // Обрабатываем полный экран
    private void Main_StateChanged(object sender, EventArgs e)
    {      
      if (this.WindowState == WindowState.Maximized)
      { 
        if (IsFullWindowed == false)
				{
          this.WindowState = WindowState.Normal;
          this.Width = SystemParameters.WorkArea.Width;
          this.Height = SystemParameters.WorkArea.Height;

          // Располагаем окошко по центру 
          this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
          this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;

          TypeTextBox.Width = this.Width / 1.6;
          SendButton.Width = this.Width / 10;
          Settings.Width = this.Width / (7.92 * 2);
          Logout.Width = this.Width / 7.92;
          Admin.Width = this.Width / (7.92 * 2);

          IsFullWindowed = true;
        }
        else
				{
          this.WindowState = WindowState.Normal;
          this.Width = 1000;
          this.Height = 500;

          // Располагаем окошко по центру 
          this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
          this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;

          TypeTextBox.Width = this.Width / 1.6;
          SendButton.Width = this.Width / 10;
          Settings.Width = this.Width / (7.92 * 2);
          Logout.Width = this.Width / 7.92;
          Admin.Width = this.Width / (7.92 * 2);

          IsFullWindowed = false;
        }
      }      
    }
  }
}
