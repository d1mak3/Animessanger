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
	public partial class Emoji : Window
	{
		private List<string> allEmoji = new List<string> { "☺", "⚀", "☻", "" };

		public Emoji()
		{
			InitializeComponent();
		}

		private void EmojiWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this.Visibility == Visibility.Visible)
			{

			}
		}
	}
}
