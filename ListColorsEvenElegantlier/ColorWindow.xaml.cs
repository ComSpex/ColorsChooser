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

namespace ListColorsEvenElegantlier {
	/// <summary>
	/// Interaction logic for ColorWindow.xaml
	/// </summary>
	public partial class ColorWindow:Window {
		public ColorWindow() {
			InitializeComponent();
		}
		void ot_Rotate(OutlinedText ot,double offsetAngle,MouseWheelEventArgs e) {
			double angle=((RotateTransform)ot.RenderTransform).Angle;
			angle%=360.0;
			if(e.Source!=null&&e.Delta<0) {
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
		private void ListBox_MouseWheel(object sender,MouseWheelEventArgs e) {
			ot_Rotate(this.ot,3.0,e);
			if(A.SelectedItem!=null&&B.SelectedItem!=null) {
				if(e.Delta<0) {
					if(++A.SelectedIndex>=A.Items.Count) {
						A.SelectedIndex=0;
					}
					if(++B.SelectedIndex>=B.Items.Count) {
						B.SelectedIndex=0;
					}
				} else {
					if(--A.SelectedIndex<=0) {
						A.SelectedIndex=A.Items.Count-1;
					}
					if(--B.SelectedIndex<=0) {
						B.SelectedIndex=B.Items.Count-1;
					}
				}
				A.ScrollIntoView(A.SelectedItem);
				B.ScrollIntoView(B.SelectedItem);
			}
		}
		private void ListBox_MouseDown(object sender,MouseButtonEventArgs e) {
			if(A.SelectedItem!=null&&B.SelectedItem!=null) {
				int index=A.SelectedIndex;
				A.SelectedIndex=B.SelectedIndex;
				B.SelectedIndex=index;
				A.ScrollIntoView(A.SelectedItem);
				B.ScrollIntoView(B.SelectedItem);
			}
		}
	}
}
