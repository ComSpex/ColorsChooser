// $Header: /WPF/ColorfulWindow/ColorfulWindow.xaml.cs 9     13/02/02 16:29 Yosuke $
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
using System.Windows.Shapes;
using ComSpex;
using ComSpexWpf;

namespace ComSpexApp {
	/// <summary>
	/// Interaction logic for ColorfulWindow.xaml
	/// </summary>
	public partial class ColorfulWindow:Window {
		public ColorfulWindow() {
			InitializeComponent();
			this.AllowsTransparency=false;
			this.WindowStyle=WindowStyle.ThreeDBorderWindow;
		}
		protected override void OnContentRendered(EventArgs e) {
			base.OnContentRendered(e);
			A.SelectedIndex=0;
			B.SelectedIndex=9;
		}
		void DoMouseDown(){
			if(A.SelectedItem!=null&&B.SelectedItem!=null) {
				int index=A.SelectedIndex;
				A.SelectedIndex=B.SelectedIndex;
				B.SelectedIndex=index;
				A.ScrollIntoView(A.SelectedItem);
				B.ScrollIntoView(B.SelectedItem);
				string text=textOfA.Text;
				textOfA.Text=textOfB.Text;
				textOfB.Text=text;
			}
		}
		void DoMouseWheel(MouseWheelEventArgs e){
			Text_Rotate(this.OfTextC,3.0,e);
			if(A.SelectedItem!=null&&B.SelectedItem!=null) {
				if(e.Delta<0) {
					if(++A.SelectedIndex>=A.Items.Count) {
						A.SelectedIndex=0;
					}
					if(++B.SelectedIndex>=B.Items.Count) {
						B.SelectedIndex=0;
					}
					if(++C.SelectedIndex>=C.Items.Count){
						C.SelectedIndex=0;
					}
				} else {
					if(--A.SelectedIndex<=0) {
						A.SelectedIndex=A.Items.Count-1;
					}
					if(--B.SelectedIndex<=0) {
						B.SelectedIndex=B.Items.Count-1;
					}
					if(--C.SelectedIndex<0) {
						C.SelectedIndex=C.Items.Count-1;
					}
				}
				A.ScrollIntoView(A.SelectedItem);
				B.ScrollIntoView(B.SelectedItem);
				C.ScrollIntoView(C.SelectedItem);
			}
		}
		private void Window_MouseDown(object sender,MouseButtonEventArgs e) {
			if(e.RightButton==MouseButtonState.Pressed){
				DoMouseDown();
				e.Handled=true;
			}else{
				e.Handled=false;
			}
		}
		private void Window_MouseWheel(object sender,MouseWheelEventArgs e) {
			DoMouseWheel(e);
		}
		void Text_Rotate(UIElement elem,double offsetAngle,MouseWheelEventArgs e) {
			double angle=((RotateTransform)elem.RenderTransform).Angle;
			angle%=360.0;
			if(e.Source!=null&&e.Delta<0) {
				angle+=offsetAngle;
			} else {
				angle-=offsetAngle;
			}
			elem.RenderTransform=new RotateTransform(angle);
		}
		private void A_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			ColorsListbox box=sender as ColorsListbox;
			ChangeGradation();
			CopyToClipboard(box.SelectedItem);
			if(box.SelectedItem!=null){
				textOfA.Text=box.SelectedItem.LegibleName;
			}
			e.Handled=true;
		}
		private void B_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			ColorsListbox box=sender as ColorsListbox;
			ChangeGradation();
			CopyToClipboard(box.SelectedItem);
			textOfB.Text=box.SelectedItem.LegibleName;
			e.Handled=true;
		}
		void ChangeGradation() {
			if(A.SelectedItem==null||B.SelectedItem==null) {
				return;
			}
			{
				LinearGradientBrush Lg=new LinearGradientBrush(ColorB,ColorA,90);
				grad.BorderBrush=Lg;
			}
			{
				LinearGradientBrush Lg=new LinearGradientBrush(ColorA,ColorB,90);
				grad.Background=Lg;
			}
		}
		Color ColorA {
			get {
				SolidColorBrush brush=(A.SelectedItem as NamedBrush).Brush as SolidColorBrush;
				return brush.Color;
			}
		}
		Color ColorB {
			get {
				SolidColorBrush brush=(B.SelectedItem as NamedBrush).Brush as SolidColorBrush;
				return brush.Color;
			}
		}
		void CopyToClipboard(NamedBrush nb) {
			if(nb!=null){
				SolidColorBrush scb=nb.Brush as SolidColorBrush;
				string colorName=nb.ClipboardName;
				string text=String.Format("{0}=#{4:x02}{1:x02}{2:x02}{3:x02}",
					colorName,
					scb.Color.R,scb.Color.G,scb.Color.B,scb.Color.A
				);
				Report("{0}",text);
				try {
					Clipboard.SetText(text);
				} catch { }
			}
		}
		private void C_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			try{
				ColorsListbox box=sender as ColorsListbox;
				textOfC.Stroke=
				textOfB.Fill=
				textOfA.Fill=box.SelectedValue;
				CopyToClipboard(box.SelectedItem);
				textOfC.Text=C.SelectedItem.LegibleName;
				textOfA.Text=A.SelectedItem.LegibleName;
				textOfB.Text=B.SelectedItem.LegibleName;
			}catch(Exception ex){
				Report(ex);
			}
			e.Handled=true;
		}
		string Report(string format,params object[] args) {
			string text=String.Format(format,args);
			System.Diagnostics.Debug.WriteLine(text,DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
			return text;
		}
		string Report(Exception ex) {
			return Report("{0}",ex);
		}
		private void Grid_MouseEnter(object sender,MouseEventArgs e) {
			Grid grid=sender as Grid;
			Border bor=grid.Children[0] as Border;
			bor.BorderBrush=Brushes.Red;
			bor.BorderThickness=new Thickness(0.5);
		}
		private void Grid_MouseLeave(object sender,MouseEventArgs e) {
			Grid grid=sender as Grid;
			Border bor=grid.Children[0] as Border;
			bor.BorderBrush=Brushes.Transparent;
		}
		private void Grid_MouseDown(object sender,MouseButtonEventArgs e) {
			Grid grid=sender as Grid;
			Border bor=grid.Children[0] as Border;
			SetOutlinedText(bor.Child as OutlinedText);
		}
		void SetOutlinedText(OutlinedText ot) {
			//Clipboard.SetText(ot.ToXAML());
			Clipboard.SetText(ot.ToString());
			MessageBox.Show(this,"Copied to Clipboard",this.Title,MessageBoxButton.OK,MessageBoxImage.Information);
		}
	}
}
