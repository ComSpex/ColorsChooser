//-------------------------------------------
// NamedBrush.cs (c) 2006 by Charles Petzold
//-------------------------------------------
using System;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Text.RegularExpressions;

namespace ComSpexWpf {
	public class NamedBrush:Control {
		public static readonly DependencyProperty BrushNameProperty;
		public static readonly DependencyProperty BrushProperty;
		static NamedBrush[] nbrushes;
		string str;

		// Static constructor.
		static NamedBrush() {
			BrushNameProperty=DependencyProperty.Register("BrushName",typeof(string),typeof(NamedBrush));
			BrushProperty=DependencyProperty.Register("Brush",typeof(Brush),typeof(NamedBrush));
			
			PropertyInfo[] props=typeof(Brushes).GetProperties();
			nbrushes=new NamedBrush[props.Length];
			for(int i=0;i<props.Length;i++){
				nbrushes[i]=new NamedBrush(props[i].Name,(Brush)props[i].GetValue(null,null));
			}
		}

		public NamedBrush(){
		}
		public NamedBrush this[int index]{
			get{return All[index];}
		}
		
		// Private constructor.
		private NamedBrush(string str,Brush brush) {
			this.str=str;
			this.Brush=brush;
			this.BrushName=Text;
		}

		// Static read-only property.
		public static NamedBrush[] All {
			get { return nbrushes; }
		}

		// Read-only properties.
		public Brush Brush {
			get { return (Brush)GetValue(BrushProperty);}
			set { SetValue(BrushProperty,value);}
		}
		public string Text {
			get {
				if(String.IsNullOrEmpty(str)) {
					Match Ma=Regex.Match(BrushName,"[#][0-9A-Fa-f]{6}.(?<name>[A-Za-z ]+)");
					if(Ma.Success) {
						str=Ma.Groups["name"].Value;
					}
				}
				Color c=(Color)Brush.GetValue(SolidColorBrush.ColorProperty);
				string strSpaced=String.Format("#{0:X2}{1:X2}{2:X2}",c.R,c.G,c.B);
				strSpaced+=" "+str[0].ToString();
				for(int i=1;i<str.Length;i++){
					strSpaced+=(char.IsUpper(str[i])?" ":"")+str[i].ToString();
				}
				return strSpaced;
			}
		}
		public string BrushName{
			get{return (string)GetValue(BrushNameProperty);}
			set{SetValue(BrushNameProperty,value);}
		}
		// Override of ToString method.
		public override string ToString() {
			return str;
		}
		public string ClipboardName{
			get {
				string[] names=Text.Split(' ');
				string colorName=String.Join("",names,1,names.Length-1);
				return colorName;
			}
		}
		public string LegibleName{
			get{
				string[] names=Text.Split(new char[]{' '},StringSplitOptions.RemoveEmptyEntries);
				string colorName=String.Join(" ",names,1,names.Length-1);
				return colorName;
			}
		}
	}
}
