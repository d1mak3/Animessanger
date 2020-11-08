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

namespace ClientForMessenger
{  
  public partial class MainWindow : Window
  {
    MessagesOperationHandler operation = new MessagesOperationHandler();


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

     
      ContactsPanel.Children.Add(new TextBlock { Text = "Дима" }); // Пример юзера
      MessagePanel.Children.Add(new TextBlock { Text = "Дима\nПривет" }); // Пример сообщения     
    }

    private void Main_SizeChanged(object sender, SizeChangedEventArgs e) // Меняем ввод и кнопку в зависимости от размеров окошка
    {
      TypeTextBox.Width = this.Width / 1.6;
      SendButton.Width = this.Width / 10;
    }

    /* public async void GetMessages(StackPanel _MessagePanel)
     {
       Message get = new Message();
       int currentID = 0;
       await Task.Run(() =>
       {
         while (true)
         {
           operation.GetMessage(out get, currentID++);
           if (get.message != "Message doesnt exist")
           {
             _MessagePanel.Children.Add(new TextBlock { Text = $"{get.userName}\n{get.message}" });
             ++currentID;
           }
         }        
       });
     }*/

    public async Task GetMessages()
    {
      List<Message> get = new List<Message>();
      int currentID = 0;

      while (true)
      {
        Dispatcher.Invoke(() =>
        {
          operation.GetMessages(out get);          
          if (get.Count != 0)
          {
            MessagePanel.Children.Clear();
            foreach(Message m in get)
              MessagePanel.Children.Add(new TextBlock { Text = $"{m.userName}\n{m.message}" });
            ++currentID;            
          }
        });
        await Task.Delay(5);
      }      
    }

    private void SendButton_Click(object sender, RoutedEventArgs e) // Отправка сообщений
    {
      if (TypeTextBox.Text != String.Empty)
      {
        Message newMessage = new Message("Server", TypeTextBox.Text);
        operation.SendMessage(newMessage);
        // MessagePanel.Children.Add(new TextBlock { Text = $"{newMessage.userName}" + "\n" + $"{newMessage.message}" });
        TypeTextBox.Text = String.Empty;
      }
    }

    private async void MessagePanel_Loaded(object sender, RoutedEventArgs e)
    {
      await GetMessages();
    }
  }
}
