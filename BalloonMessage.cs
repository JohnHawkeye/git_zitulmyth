using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zitulmyth.Data;

namespace Zitulmyth
{
	public class BalloonMessage
	{

		public static BitmapImage bmBalloon;
		public static CroppedBitmap[] cbBalloon = new CroppedBitmap[10];
		public static Image[] imgBalloon = new Image[10];
		public static TextBlock txtBalloon = new TextBlock();
		public static StackPanel spnBalloon = new StackPanel();
		public static Canvas canBalloonTail = new Canvas();

		public static void GenerateBalloon(Canvas canvas)
		{
			spnBalloon.Width = 256;
			spnBalloon.Height = 128;
			spnBalloon.Visibility = Visibility.Hidden;
			spnBalloon.VerticalAlignment = VerticalAlignment.Top;
			spnBalloon.HorizontalAlignment = HorizontalAlignment.Left;
			
			canvas.Children.Add(spnBalloon);
			Canvas.SetTop(spnBalloon, 500);
			Canvas.SetLeft(spnBalloon,300);
			Canvas.SetZIndex(spnBalloon, 15);

			int imgWidth=16,imgHeight=16;
			int txtPosX = 0,txtPosY = 0;


			for (int i = 0; i < imgBalloon.Length; i++)
			{
				imgWidth = imgHeight = 17;
				if (i == 1) { imgWidth = 161; }
				if (i == 3) { imgHeight = 48; }
				if (i == 4) { imgWidth = 161; imgHeight = 48; }
				if (i == 5) { imgHeight = 48;}
				if (i == 7) { imgWidth = 161; }
				
					
				imgBalloon[i] = new Image
				{
					Source = cbBalloon[i],
					Width = imgWidth,
					Height = imgHeight,
					Stretch = Stretch.Fill,
				};

				Canvas canBalloon = new Canvas();

				canBalloon.HorizontalAlignment = HorizontalAlignment.Left;
				canBalloon.VerticalAlignment = VerticalAlignment.Top;

				spnBalloon.Children.Add(canBalloon);
				canBalloon.Children.Add(imgBalloon[i]);
	
				txtPosX = txtPosY = 0;
				if (i == 1) { txtPosX = 16; }
				if (i == 2) { txtPosX = 176; }
				if (i == 3) { txtPosY = 15; }
				if (i == 4) { txtPosX = 16; txtPosY = 15; }
				if (i == 5) { txtPosX = 176; txtPosY = 15; }
				if (i == 6) { txtPosY = 62; }
				if (i == 7) { txtPosX = 16; txtPosY = 62; }
				if (i == 8) { txtPosX = 176; txtPosY = 62; }
				if (i == 9) { txtPosX = 16;txtPosY = 74; canBalloonTail = canvas; }


				Canvas.SetLeft(imgBalloon[i],txtPosX);
				Canvas.SetTop(imgBalloon[i],txtPosY);

			}

			txtBalloon = new TextBlock
			{

				Width = 256,
				Height = 128,
				Foreground = Brushes.Black,
				FontFamily = new FontFamily("Yu Gothic UI"),
				FontSize = 16,
				TextWrapping = TextWrapping.Wrap,
				Margin = new Thickness(12, 6, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				
				Text = "これはテストです。一行10文字で三行まで。自動で改行されます。",

			};

			spnBalloon.Children.Add(txtBalloon);
			Canvas.SetTop(txtBalloon, 0);
			Canvas.SetLeft(txtBalloon, 0);

		}

		public static void OpenBalloon(int index,Canvas canvas,Vector blpos,Image target, String blstring,bool usepos)
		{
			double tempX = Canvas.GetLeft(target);
			double tempY = Canvas.GetTop(target);

			if (usepos)
			{
				tempX = blpos.X;	tempY = blpos.Y;

			}
			else
			{
	
			}

			Canvas.SetLeft(spnBalloon, tempX);
			Canvas.SetTop(spnBalloon, tempY-96);
			txtBalloon.Text = blstring;
			spnBalloon.Visibility = Visibility.Visible;
		}
		
	}
}

//textBlock.Background              = Brushes.AntiqueWhite;
//textBlock.Foreground              = Brushes.Navy;

//textBlock.FontFamily              = new FontFamily("Century Gothic");
//textBlock.FontSize                = 12;
//textBlock.FontStretch             = FontStretches.UltraExpanded;
//textBlock.FontStyle               = FontStyles.Italic;
//textBlock.FontWeight              = FontWeights.UltraBold;

//textBlock.LineHeight              = Double.NaN;
//textBlock.Padding                 = new Thickness(5, 10, 5, 10);
//textBlock.TextAlignment           = TextAlignment.Center;
//textBlock.TextWrapping            = TextWrapping.Wrap;

//textBlock.Typography.NumeralStyle = FontNumeralStyle.OldStyle;
//textBlock.Typography.SlashedZero  = true;