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

    private void Main_Loaded_1(object sender, RoutedEventArgs e) // Когда главное окошко загрузилось
    {
      // Создаём окошко с логином
      LoginWindow login = new LoginWindow();
      login.Owner = this;
      this.Visibility = Visibility.Hidden;
      login.Show();           
    }

    private void Main_SizeChanged(object sender, SizeChangedEventArgs e) // Меняем ввод и кнопку в зависимости от размеров окошка
    {
      TypeTextBox.Width = this.Width / 1.6;
      SendButton.Width = this.Width / 10;
    }    

    public async Task GetMessages() // Асинхронный (чтобы приложение не зависало) метод, который осуществляет приём сообщений
    {
      List<Message> get = new List<Message>(); // Список сообщений, которые мы будем выводить на экран      

      while (true) // Приём сообщений работает, пока работает окошко
      {
        Dispatcher.Invoke(() => // Диспетчер для того, чтобы дать возможность управлять MessagePanel потоку, который работает асинхронно
        {
          operation.GetMessages(out get); // Принимаем все сообщения       
          if (get.Count != 0) // Если сообщений > 0 
          {
            MessagePanel.Children.Clear(); // Очищаем панель, чтобы сообщения вставлялись один раз
            foreach(Message m in get) // Заносим все сообщения в панельку
              MessagePanel.Children.Add(new TextBlock { Text = $"{m.userName}\n{m.message}" });                        
          }
        });
        await Task.Delay(1);
      }      
    }

    private void SendButton_Click(object sender, RoutedEventArgs e) // Отправка сообщений
    {
      if (TypeTextBox.Text != String.Empty) // Если TextBox в который мы вводим текст имеет хоть что-то
      {
        Message newMessage = new Message("Server", TypeTextBox.Text); // Создаём новое сообщение с текстом из TextBox
        operation.SendMessage(newMessage); // Отправляем сообщение     
        TypeTextBox.Text = String.Empty; // Стираем сообщение из TextBox
      }
    }

    private async void MessagePanel_Loaded(object sender, RoutedEventArgs e) // Вызываем асинхронный приём сообщений при загрузке основного окошечка
    {
      await GetMessages();
    }
  }
}
