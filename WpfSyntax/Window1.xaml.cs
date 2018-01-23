using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSyntax {
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1:Window {
		public Window1() {
			InitializeComponent();
		}
		private void color_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			ListBox list=sender as ListBox;
			if(list!=null&&sample!=null){
				Brush brush=((list.SelectedValue as ListBoxItem).Content as Rectangle).Stroke;
				sample.Foreground=brush;
			}
		}
	}
}
