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
using System.Text.RegularExpressions;
using System.IO;
using static Zitulmyth.MainWindow;

namespace Zitulmyth
{

	public partial class CroppingImageDialog : Window
	{

		private Rectangle rctCursor;
		private Int32Rect cursorRange = new Int32Rect(0, 0, 0, 0);

		private Line lineH = new Line();
		private Line lineV = new Line();

		private BitmapImage bmiOriginalImage = new BitmapImage();
		private Image imgOriginalImage = new Image();
		private int maxWidth;
		private int maxHeight;

		public Vector clickPosition = new Vector();

		private bool mouseLeftClick;

		private void SnapLineDraw()
		{
			if (!(bool)ckbSnap.IsChecked)
			{
				lineH.Visibility = Visibility.Hidden;
				lineV.Visibility = Visibility.Hidden;
			}
			else
			{
				lineH.Visibility = Visibility.Visible;
				lineV.Visibility = Visibility.Visible;
			}
		}

		public CroppingImageDialog()
		{
			InitializeComponent();

			//snap line
			DoubleCollection dc = new DoubleCollection();
			dc.Add(2);
			dc.Add(2);

			lineH.StrokeThickness = 1;
			lineV.StrokeThickness = 1;
			lineH.Stroke = Brushes.Black;
			lineV.Stroke = Brushes.Black;
			lineH.StrokeDashArray = dc;
			lineV.StrokeDashArray = dc;

			cvsDesk.Children.Add(lineH);
			cvsDesk.Children.Add(lineV);
			//

			if (File.Exists("Assets/" + MainWindow.imageManager.tbkFileName.Text.ToString()))
			{
				bmiOriginalImage = new BitmapImage(
					new Uri("Assets/" + MainWindow.imageManager.tbkFileName.Text.ToString(), UriKind.Relative));
				maxWidth = bmiOriginalImage.PixelWidth;
				maxHeight = bmiOriginalImage.PixelHeight;

				imgOriginalImage.Source = bmiOriginalImage;
				imgOriginalImage.Width = maxWidth;
				imgOriginalImage.Height = maxHeight;
				cvsDesk.Width = maxWidth;
				cvsDesk.Height = maxHeight;

				cvsDesk.Children.Add(imgOriginalImage);
			}

			tbxStartX.Text = MainWindow.imageManager.croppingRange.X.ToString();
			tbxStartY.Text = MainWindow.imageManager.croppingRange.Y.ToString();
			tbxWidth.Text = MainWindow.imageManager.croppingRange.Width.ToString();
			tbxHeight.Text = MainWindow.imageManager.croppingRange.Height.ToString();
			CursorSetting(int.Parse(tbxStartX.Text), int.Parse(tbxStartY.Text),
					int.Parse(tbxWidth.Text), int.Parse(tbxHeight.Text));

		}

		private void tbxStartX_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void tbxStartY_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void tbxWidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void tbxHeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void cvsDesk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!mouseLeftClick)
			{
				Point point = e.GetPosition(cvsDesk);

				int remainderX = (int)Math.Floor(point.X) % 16;
				int remainderY = (int)Math.Floor(point.Y) % 16;
				int calPosX, calPosY;

				if (rctCursor == null)
				{
					rctCursor = new Rectangle();

					rctCursor.StrokeThickness = 1;
					rctCursor.Stroke = Brushes.Blue;
					rctCursor.Fill = Brushes.LightBlue;
					rctCursor.Opacity = 0.6;
					rctCursor.Width = rctCursor.Height = 0;
					rctCursor.VerticalAlignment = VerticalAlignment.Top;
					rctCursor.HorizontalAlignment = HorizontalAlignment.Left;

					cvsDesk.Children.Add(rctCursor);
			
				}

				if ((bool)ckbSnap.IsChecked)
				{
					if (remainderX < 8)
					{
						calPosX = (int)Math.Floor(point.X) - remainderX;
					}
					else
					{
						calPosX = (int)Math.Floor(point.X) + (16 - remainderX);
					}

					if (remainderY < 8)
					{
						calPosY = (int)Math.Floor(point.Y) - remainderY;
					}
					else
					{
						calPosY = (int)Math.Floor(point.Y) + (16 - remainderY);
					}
				}
				else
				{
					calPosX = (int)Math.Floor(point.X);
					calPosY = (int)Math.Floor(point.Y);
				}

				clickPosition = new Vector(calPosX, calPosY);
				rctCursor.Width = rctCursor.Height = 0;

				Canvas.SetLeft(rctCursor, calPosX);
				Canvas.SetTop(rctCursor, calPosY);
				Canvas.SetZIndex(rctCursor, 10);

				tbxStartX.Text = calPosX.ToString();
				tbxStartY.Text = calPosY.ToString();

				tbxWidth.Text = rctCursor.Width.ToString();
				tbxHeight.Text = rctCursor.Height.ToString();

				mouseLeftClick = true;
			}
			
		}

		private void cvsDesk_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			mouseLeftClick = false;
		}

		private void cvsDesk_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = e.GetPosition(cvsDesk);

			int remainderX = (int)Math.Floor(point.X) % 16;
			int remainderY = (int)Math.Floor(point.Y) % 16;
			int snapPosX,snapPosY;


			if (remainderX < 8)
			{
				snapPosX = (int)Math.Floor(point.X)- remainderX;
			}
			else
			{
				snapPosX = (int)Math.Floor(point.X) + (16-remainderX);
			}
			
			if(remainderY < 8)
			{
				snapPosY = (int)Math.Floor(point.Y) - remainderY;
			}
			else
			{
				snapPosY = (int)Math.Floor(point.Y) + (16 - remainderY);
			}

			lineH.X1 = snapPosX - 128;
			lineH.Y1 = snapPosY;
			lineH.X2 = snapPosX +128;
			lineH.Y2 = snapPosY;

			lineV.X1 = snapPosX;
			lineV.Y1 = snapPosY - 128;
			lineV.X2 = snapPosX;
			lineV.Y2 = snapPosY + 128;
			
			int width, height;

			if (mouseLeftClick)
			{
				if (point.X <= clickPosition.X)
				{
					width = 1;
				}
				else
				{
					width = (int)Math.Floor(point.X - clickPosition.X);
				}
					
				if (point.Y <= clickPosition.Y)
				{
					height = 1;
				}
				else
				{
					height = (int)Math.Floor(point.Y - clickPosition.Y);
				}

				if ((bool)ckbSnap.IsChecked)
				{
					if (point.X > clickPosition.X)
					{
						if (remainderX < 8)
						{
							width = (int)Math.Floor(point.X - clickPosition.X) - remainderX;
						}
						else
						{
							width = (int)Math.Floor(point.X - clickPosition.X) + (16 - remainderX);
						}
					}

					if (point.Y > clickPosition.Y)
					{
						if (remainderY < 8)
						{
							height = (int)Math.Floor(point.Y - clickPosition.Y) - remainderY;
						}
						else
						{
							height = (int)Math.Floor(point.Y - clickPosition.Y) + (16 - remainderY);
						}
					}

				}

				rctCursor.Width = width;
				rctCursor.Height = height;

				tbxWidth.Text = rctCursor.Width.ToString();
				tbxHeight.Text = rctCursor.Height.ToString();
			}


		}

		private void tbxStartX_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Return)
			{
				if (int.Parse(tbxStartX.Text) < 0)
				{
					tbxStartX.Text = "0";
				}
				if (int.Parse(tbxStartX.Text) > maxWidth)
				{
					tbxStartX.Text = maxWidth.ToString();
				}

				CursorSetting(int.Parse(tbxStartX.Text), int.Parse(tbxStartY.Text),
					int.Parse(tbxWidth.Text), int.Parse(tbxHeight.Text));
			}
		}

		private void tbxStartY_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (int.Parse(tbxStartY.Text) < 0)
				{
					tbxStartY.Text = "0";
				}
				if (int.Parse(tbxStartY.Text) > maxHeight)
				{
					tbxStartY.Text = maxHeight.ToString();
				}

				CursorSetting(int.Parse(tbxStartX.Text), int.Parse(tbxStartY.Text),
					int.Parse(tbxWidth.Text), int.Parse(tbxHeight.Text));
			}
		}

		private void tbxWidth_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (int.Parse(tbxWidth.Text) < 0)
				{
					tbxWidth.Text = "0";
				}

				if (int.Parse(tbxStartX.Text) + int.Parse(tbxWidth.Text)>maxWidth)
				{
					tbxWidth.Text = (int.Parse(tbxWidth.Text)-(int.Parse(tbxStartX.Text) + int.Parse(tbxWidth.Text) - maxWidth)).ToString();
				}

				CursorSetting(int.Parse(tbxStartX.Text), int.Parse(tbxStartY.Text),
					int.Parse(tbxWidth.Text), int.Parse(tbxHeight.Text));
			}
		}

		private void tbxHeight_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (int.Parse(tbxHeight.Text) < 0)
				{
					tbxHeight.Text = "0";
				}

				if (int.Parse(tbxStartY.Text) + int.Parse(tbxHeight.Text) > maxHeight)
				{
					tbxHeight.Text = (int.Parse(tbxHeight.Text) - (int.Parse(tbxStartY.Text) + int.Parse(tbxHeight.Text) - maxHeight)).ToString();
				}

				CursorSetting(int.Parse(tbxStartX.Text), int.Parse(tbxStartY.Text),
					int.Parse(tbxWidth.Text), int.Parse(tbxHeight.Text));
			}
		}

		private void CursorSetting(int x,int y ,int w, int h)
		{

			if(rctCursor == null)
			{
				rctCursor = new Rectangle();

				rctCursor.StrokeThickness = 1;
				rctCursor.Stroke = Brushes.Blue;
				rctCursor.Fill = Brushes.LightBlue;
				rctCursor.Opacity = 0.6;
				rctCursor.Width = w;
				rctCursor.Height = h;
				rctCursor.VerticalAlignment = VerticalAlignment.Top;
				rctCursor.HorizontalAlignment = HorizontalAlignment.Left;
				cvsDesk.Children.Add(rctCursor);
				Canvas.SetLeft(rctCursor, x);
				Canvas.SetTop(rctCursor, y);
			}
			else
			{
				Canvas.SetLeft(rctCursor, x);
				Canvas.SetTop(rctCursor, y);
				rctCursor.Width = w;
				rctCursor.Height = h;
			}
			
			
		}

		private void ckbSnap_Checked(object sender, RoutedEventArgs e)
		{
			
		}

		private void ckbSnap_Click(object sender, RoutedEventArgs e)
		{
			SnapLineDraw();
		}

		private void cvsDesk_MouseLeave(object sender, MouseEventArgs e)
		{
			mouseLeftClick = false;
		}

		private void btnCropping_Click(object sender, RoutedEventArgs e)
		{
			imageManager.croppingRange.X = int.Parse(tbxStartX.Text);
			imageManager.croppingRange.Y = int.Parse(tbxStartY.Text);
			imageManager.croppingRange.Width = int.Parse(tbxWidth.Text);
			imageManager.croppingRange.Height = int.Parse(tbxHeight.Text);

			if(!imageManager.isNewPattern)
			{
				imageManager.ChildSelector()[imageManager.patternListIndex].cropRange[imageManager.croppingIndex] =
					imageManager.croppingRange;
			}
			else
			{
				imageManager.ChildSelector()[imageManager.patternListIndex].cropRange.Add(imageManager.croppingRange);
			}

			imageManager.PatternListLoading(imageManager.ChildSelector(), imageManager.parentName);

			imageManager.PatternDataWrite();

			this.Close();
		}
	}
}
