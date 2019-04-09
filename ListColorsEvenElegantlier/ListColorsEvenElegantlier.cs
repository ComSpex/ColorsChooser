//----------------------------------------------------------
// ListColorsEvenElegantlier.cs (c) 2006 by Charles Petzold
//----------------------------------------------------------
using Petzold.ListNamedBrushes;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using ComSpex;
using NamedBrush = ComSpexWpf.NamedBrush;

namespace Petzold.ListColorsEvenElegantlier {
	public class ListColorsEvenElegantlier:Window {
		bool saveToFile;
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
			if(saveToFile){
				this.Cursor=Cursors.Wait;
				using(StreamWriter sw=new StreamWriter("ListColorEventElegantlier.xaml")){
					sw.WriteLine(XamlWriter.Save(this));
				}
				this.Cursor=Cursors.Arrow;
			}
			base.OnClosing(e);
		}
		[STAThread]
		public static void Main() {
			Application app=new Application();
			app.Run(new ListColorsEvenElegantlier());
		}
		ListBox A,B;
		Border grad;
		Grid box;
		TextBlock block,border;
		OutlinedText ot;
		public ListColorsEvenElegantlier() {
			Title="List Colors Even Elegantlier";
			MakeResources();
			Border world=new Border();
			world.BorderThickness=new Thickness(50);
			world.CornerRadius=new CornerRadius(70.710678118654752440084436210485);
			Grid grid=new Grid();
			world.Child=grid;
			{
				Grid area=new Grid();
				area.HorizontalAlignment=HorizontalAlignment.Right;
				area.VerticalAlignment=VerticalAlignment.Bottom;
				ListBox lstbox=A=ElegantColorChooser();
				A.SelectionChanged+=new SelectionChangedEventHandler(A_SelectionChanged);
				// Bind the SelectedValue to window Background.
				lstbox.SelectedValuePath="Brush";
				lstbox.SetBinding(ListBox.SelectedValueProperty,"Background");
				lstbox.DataContext=world;
				area.Children.Add(lstbox);
				grid.Children.Add(area);
			}
			{
				Grid area=new Grid();
				area.HorizontalAlignment=HorizontalAlignment.Left;
				area.VerticalAlignment=VerticalAlignment.Bottom;
				ListBox lstbox=B=ElegantColorChooser();
				B.SelectionChanged+=new SelectionChangedEventHandler(B_SelectionChanged);
				// Bind the SelectedValue to window Background.
				lstbox.SelectedValuePath="Brush";
				lstbox.SetBinding(ListBox.SelectedValueProperty,"BorderBrush");
				lstbox.DataContext=world;
				area.Children.Add(lstbox);
				grid.Children.Add(area);
			}
			{
				Grid area=new Grid();
				area.HorizontalAlignment=HorizontalAlignment.Center;
				area.VerticalAlignment=VerticalAlignment.Center;
				grad=new Border();
				grad.Name="grad";
				grad.BorderThickness=new Thickness(20);
				grad.CornerRadius=new CornerRadius(28.284271247461900976033774484194);
				//grad.Background=Brushes.Black;
				//grad.BorderBrush=Brushes.Teal;
				box=new Grid();
				box.Width=200;
				box.Height=200;
				grad.Child=box;
				area.Children.Add(grad);
				grid.Children.Add(area);
			}
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions[0].Height=new GridLength(300,GridUnitType.Star);
			grid.RowDefinitions[1].Height=new GridLength(50);
			grid.ShowGridLines=false;
		
			{
				block=new TextBlock();
				block.Text="This is a sample text.";
				block.TextAlignment=TextAlignment.Center;
				block.FontFamily=new FontFamily("Segou UI");
				block.FontSize=32;
				Binding myBinding=new Binding("SelectedValue");
				myBinding.Source=B;
				block.SetBinding(TextBlock.ForegroundProperty,myBinding);
				grid.Children.Add(block);
				Grid.SetRow(block,1);
			}
			{
				border=new TextBlock();
				border.Text="This is a sample text.";
				border.TextAlignment=TextAlignment.Center;
				border.FontFamily=new FontFamily("Segou UI");
				border.FontSize=32;
				Binding myBinding=new Binding("SelectedValue");
				myBinding.Source=A;
				border.SetBinding(TextBlock.ForegroundProperty,myBinding);
				border.RenderTransform=new TranslateTransform(0,60);
				grid.Children.Add(border);
				Grid.SetRow(border,1);
			}
			{
				ot=new OutlinedText();
				ot.MouseDown+=new MouseButtonEventHandler(ot_MouseDown);
				ot.Text="This is a sample text.";
				ot.FontFamily=new FontFamily("Maiandra GD");
				ot.FontSize=50;
				ot.FontWeight=FontWeights.Bold;
				ot.StrokeThickness=2;
				ot.RenderTransformOrigin=new Point(0.5,0.5);
				ot.HorizontalAlignment=HorizontalAlignment.Center;
				ot.VerticalAlignment=VerticalAlignment.Center;
				Binding bindFill=new Binding("SelectedValue");
				bindFill.Source=A;
				ot.SetBinding(OutlinedText.FillProperty,bindFill);
				Binding bindStor=new Binding("SelectedValue");
				bindStor.Source=B;
				ot.SetBinding(OutlinedText.StrokeProperty,bindStor);
				ot.RenderTransform=new RotateTransform(-90);
				grid.Children.Add(ot);
				Grid.SetRow(ot,0);
			}
			
			Content=world;
			this.MouseDown+=new MouseButtonEventHandler(ListColorsEvenElegantlier_MouseDown);
			this.MouseWheel+=new MouseWheelEventHandler(ListColorsEvenElegantlier_MouseWheel);
			this.ot.MouseWheel+=new MouseWheelEventHandler(ot_MouseWheel);
		}
		void ListColorsEvenElegantlier_MouseWheel(object sender,MouseWheelEventArgs e) {
			Report("{0}:{1}",e.Delta,e.MiddleButton);
			ot_Rotate(this.ot,3.0,e);
			if(A.SelectedItem!=null&&B.SelectedItem!=null){
				if(e.Delta<0){
					if(++A.SelectedIndex>=A.Items.Count){
						A.SelectedIndex=0;
					}
					if(++B.SelectedIndex>=B.Items.Count) {
						B.SelectedIndex=0;
					}
				} else {
					if(--A.SelectedIndex<=0) {
						A.SelectedIndex=A.Items.Count-1;
					}
					if(--B.SelectedIndex<=0){
						B.SelectedIndex=B.Items.Count-1;
					}
				}
				A.ScrollIntoView(A.SelectedItem);
				B.ScrollIntoView(B.SelectedItem);
			}
		}
		void ListColorsEvenElegantlier_MouseDown(object sender,MouseButtonEventArgs e) {
			if(A.SelectedItem!=null&&B.SelectedItem!=null){
				int index=A.SelectedIndex;
				A.SelectedIndex=B.SelectedIndex;
				B.SelectedIndex=index;
				A.ScrollIntoView(A.SelectedItem);
				B.ScrollIntoView(B.SelectedItem);
			}
		}
		void Report(string format,params object[] args){
			System.Diagnostics.Debug.WriteLine(String.Format(format,args),DateTime.Now.ToString("HH:mm:ss.fff"));
		}
		void ot_MouseWheel(object sender,MouseWheelEventArgs e) {
			ot_Rotate(sender as OutlinedText,3.0,e);
		}
		void ot_MouseDown(object sender,MouseButtonEventArgs e) {
			Report("{0}:{1}:{2}",e.LeftButton,e.MiddleButton,e.RightButton);
			ot_Rotate(sender as OutlinedText,30.0,e);
			e.Handled=true;
		}
		void ot_Rotate(OutlinedText ot,double offsetAngle,MouseWheelEventArgs e) {
			double angle=((RotateTransform)ot.RenderTransform).Angle;
			angle%=360.0;
			if(e.Source!=null&&e.Delta<0){
				angle+=offsetAngle;
			} else {
				angle-=offsetAngle;
			}
			ot.RenderTransform=new RotateTransform(angle);
		}
		void ot_Rotate(OutlinedText ot,double offsetAngle,MouseButtonEventArgs e) {
			double angle=((RotateTransform)ot.RenderTransform).Angle;
			angle%=360.0;
			if(e.Source!=null&&e.ChangedButton==MouseButton.Left) {
				angle+=offsetAngle;
			} else {
				angle-=offsetAngle;
			}
			if(e.ClickCount>0) {
				angle*=e.ClickCount;
			}
			ot.RenderTransform=new RotateTransform(angle);
		}
		
		void ChangeGradation(){
			if(A.SelectedItem==null||B.SelectedItem==null){
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
		void CopyToClipboard(NamedBrush nb){
			SolidColorBrush scb=nb.Brush as SolidColorBrush;
			string[] names=nb.Text.Split(' ');
			string colorName=String.Join("",names,1,names.Length-1);
			string text=String.Format("{0}=#{4:x02}{1:x02}{2:x02}{3:x02}",colorName,scb.Color.R,scb.Color.G,scb.Color.B,scb.Color.A);
			Clipboard.SetText(text);
		}
		void B_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			ChangeGradation();
			CopyToClipboard(((ListBox)sender).SelectedItem as NamedBrush);
		}
		void A_SelectionChanged(object sender,SelectionChangedEventArgs e) {
			ChangeGradation();
			CopyToClipboard(((ListBox)sender).SelectedItem as NamedBrush);
			if(this.GetBindingExpression(BackgroundProperty)==null){
				Binding bindFill=new Binding("SelectedValue");
				bindFill.Source=A;
				this.SetBinding(Window.BackgroundProperty,bindFill);
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
		Color Compute(Color l,Color r){
			string sL=String.Format("{0:X2}{1:X2}{2:X2}",l.R,l.G,l.B);
			string sR=String.Format("{0:X2}{1:X2}{2:X2}",r.R,r.G,r.B);
			int lhs=int.Parse(sL,System.Globalization.NumberStyles.HexNumber);
			int rhs=int.Parse(sR,System.Globalization.NumberStyles.HexNumber);
			int ans=lhs-rhs;
			string sA=String.Format("{0:X6}",ans&0xFFFFFF);
			byte R=byte.Parse(sA.Substring(0,2),System.Globalization.NumberStyles.HexNumber);
			byte G=byte.Parse(sA.Substring(2,2),System.Globalization.NumberStyles.HexNumber);
			byte B=byte.Parse(sA.Substring(4,2),System.Globalization.NumberStyles.HexNumber);
			return Color.FromRgb(R,G,B);
		}
		LinearGradientBrush Gradate(Color co){
			List<Color> Rhs=new List<Color>();
			Rhs.Add(Color.FromRgb(0x22,0xff,0x23));
			Rhs.Add(Color.FromRgb(0x70,0x10,0x70));
			Rhs.Add(Color.FromRgb(0x00,0x00,0x00));
			Rhs.Add(Color.FromRgb(0xce,0x31,0xce));
			Rhs.Add(Color.FromRgb(0x00,0x11,0x00));
			List<double> Offs=new List<double>();
			Offs.Add(0.0);
			Offs.Add(0.5);
			Offs.Add(0.5);
			Offs.Add(0.5);
			Offs.Add(1.0);
			LinearGradientBrush Lg=new LinearGradientBrush();
			Lg.StartPoint=new Point(0,0);
			Lg.EndPoint=new Point(0,1);
			System.Diagnostics.Debug.WriteLine(new String('-',10));
			for(int i=0;i<Rhs.Count;++i) {
				Color rh=Rhs[i];
				Color result=Compute(co,rh);
				System.Diagnostics.Debug.WriteLine(String.Format("{0:X2}{1:X2}{2:X2}",result.R,result.G,result.B));
				Lg.GradientStops.Add(new GradientStop(result,Offs[i]));
			}
			return Lg;
		}
		void MakeResources(){
			{
				LinearGradientBrush Lg=new LinearGradientBrush();
				Lg.StartPoint=new Point(0,0);
				Lg.EndPoint=new Point(0,1);
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x77,0x99,0x77),0.0));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x10,0x88,0x10),0.3));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x00,0x99,0x00),0.3));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x2a,0xc5,0x2a),0.3));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x00,0x88,0x00),1.0));
				Resources.Add("greener",Lg);
			}
			{
				LinearGradientBrush Lg=new LinearGradientBrush();
				Lg.StartPoint=new Point(0,0);
				Lg.EndPoint=new Point(0,1);
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0xdd,0xff,0xdd),0.0));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x90,0xee,0x90),0.3));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x00,0xff,0x00),0.3));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x32,0xcd,0x32),0.3));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x00,0xee,0x00),1.0));
				Resources.Add("greeny",Lg);
			}
			{
				LinearGradientBrush Lg=new LinearGradientBrush();
				Lg.StartPoint=new Point(0,0);
				Lg.EndPoint=new Point(0,1);
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0xdd,0xff,0xdd),0.0));//-22ff23
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x90,0xee,0x90),0.5));//-701070
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x00,0xff,0x00),0.5));
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x32,0xcd,0x32),0.5));//-ce31ce
				Lg.GradientStops.Add(new GradientStop(Color.FromRgb(0x00,0xee,0x00),1.0));//-001100
				Resources.Add("vista",Lg);
			}
		}
		ListBox ElegantColorChooser() {
			// Create a DataTemplate for the items.
			DataTemplate template=new DataTemplate(typeof(NamedBrush));

			// Create a FrameworkElementFactory based on StackPanel.
			FrameworkElementFactory factoryStack=new FrameworkElementFactory(typeof(StackPanel));
			factoryStack.SetValue(StackPanel.OrientationProperty,Orientation.Horizontal);

			// Make that the root of the DataTemplate visual tree.
			template.VisualTree=factoryStack;

			// Create a FrameworkElementFactory based on Rectangle.
			FrameworkElementFactory factoryRectangle=new FrameworkElementFactory(typeof(Rectangle));
			factoryRectangle.SetValue(Rectangle.WidthProperty,16.0);
			factoryRectangle.SetValue(Rectangle.HeightProperty,16.0);
			factoryRectangle.SetValue(Rectangle.MarginProperty,new Thickness(2));
			factoryRectangle.SetValue(Rectangle.StrokeProperty,SystemColors.WindowTextBrush);
			factoryRectangle.SetBinding(Rectangle.FillProperty,new Binding("Brush"));
			// Add it to the StackPanel.
			factoryStack.AppendChild(factoryRectangle);

			// Create a FrameworkElementFactory based on TextBlock.
			FrameworkElementFactory factoryTextBlock=new FrameworkElementFactory(typeof(TextBlock));
			factoryTextBlock.SetValue(TextBlock.VerticalAlignmentProperty,VerticalAlignment.Center);
			factoryTextBlock.SetValue(TextBlock.TextProperty,new Binding("BrushName"));
			// Add it to the StackPanel.
			factoryStack.AppendChild(factoryTextBlock);

			// Create ListBox as content of window.
			ListBox lstbox=new ListBox();
			lstbox.FontFamily=new FontFamily("Courier New");
			lstbox.Width=250;
			lstbox.Height=300;

			// Set the ItemTemplate property to the template created above.
			lstbox.ItemTemplate=template;

			// Set the ItemsSource to the array of NamedBrush objects.
			lstbox.ItemsSource=NamedBrush.All;

			return lstbox;
		}
	}
}
