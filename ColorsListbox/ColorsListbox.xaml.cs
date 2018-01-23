// $Header: /WPF/ColorsListbox/ColorsListbox.xaml.cs 6     11/05/20 1:58 Yosuke $
using System;
using System.Collections.Generic;
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

namespace ComSpexWpf {
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class ColorsListbox:UserControl {
		public event SelectionChangedEventHandler SelectionChanged;
		public static readonly DependencyProperty SelectedValueProperty=DependencyProperty.Register(
			"SelectedValue",typeof(Brush),typeof(ColorsListbox)
		);
		public static readonly DependencyProperty SelectedValuePathProperty=DependencyProperty.Register(
			"SelectedValuePath",typeof(string),typeof(ColorsListbox)
		);
		static ColorsListbox() {
		}
		public ColorsListbox() {
			InitializeComponent();
		}
		public int SelectedIndex{
			get{return List.SelectedIndex;}
			set{List.SelectedIndex=Math.Max(-1,value);}
		}
		public NamedBrush SelectedItem{
			get{return List.SelectedItem as NamedBrush;}
			set{List.SelectedItem=value;}
		}
		public ItemCollection Items{
			get{return List.Items;}
			set{
				List.Items.Clear();
				foreach(object item in value){
					List.Items.Add(item);
				}
			}
		}
		public Brush SelectedValue{
			get{return (Brush)GetValue(SelectedValueProperty);}
			set{SetValue(SelectedValueProperty,value);}
		}
		public string SelectedValuePath{
			get{return (string)GetValue(SelectedValuePathProperty);}
			set{
				List.SelectedValuePath=value;
				SetValue(SelectedValuePathProperty,value);
			}
		}
		public void ScrollIntoView(NamedBrush namedBrush) {
			List.ScrollIntoView(namedBrush);
		}
		private void List_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			if(SelectedItem!=null){
				SelectedValue=SelectedItem.Brush;
			}
			if(SelectionChanged!=null){
				SelectionChanged(this,e);
			}
		}
	}
}
