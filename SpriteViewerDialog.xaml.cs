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
using Zitulmyth.Data;

namespace Zitulmyth
{
	/// <summary>
	/// SpriteViewerDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class SpriteViewerDialog : Window
	{

		public List<string> lstSpriteName = new List<string>();

		public SpriteViewerDialog()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			switch (MainWindow.databaseWindow.spriteCategory)
			{
				case CategoryName.Player:

					for (int i = 0; i < ImageData.spritePlayer.Count; i++)
					{
						lstSpriteName.Add(ImageData.spritePlayer[i].patternSource.patternName);
					}

					lsbSprite.ItemsSource = lstSpriteName;
					lsbSprite.SelectedIndex = 0;

					break;

				case CategoryName.Block:

					for(int i = 0; i < ImageData.spriteBlock.Count;i++)
					{
						lstSpriteName.Add(ImageData.spriteBlock[i].patternSource.patternName);
					}

					lsbSprite.ItemsSource = lstSpriteName;
					lsbSprite.SelectedIndex = 0;

					break;

				case CategoryName.Enemy:
					break;

				case CategoryName.Object:
					for (int i = 0; i < ImageData.spriteObject.Count; i++)
					{
						lstSpriteName.Add(ImageData.spriteObject[i].patternSource.patternName);
					}

					lsbSprite.ItemsSource = lstSpriteName;
					lsbSprite.SelectedIndex = 0;
					break;

				case CategoryName.Item:
					break;
			}

			PreviewImage();

		}

		private void PreviewImage()
		{
			if(lsbSprite.SelectedIndex >= 0)
			{
				int index = lsbSprite.SelectedIndex;

				switch (MainWindow.databaseWindow.spriteCategory)
				{
					case CategoryName.Player:

						imgPreview.Source = ImageData.spritePlayer[index].patternSource.croppedBitmap[0];

						break;

					case CategoryName.Block:

						imgPreview.Source = ImageData.spriteBlock[index].patternSource.croppedBitmap[0];
						
						break;

					case CategoryName.Enemy:
						break;

					case CategoryName.Object:
						imgPreview.Source = ImageData.spriteObject[index].patternSource.croppedBitmap[0];
						break;

					case CategoryName.Item:
						break;
				}
			}
		}

		private void lsbSprite_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PreviewImage();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{

			if (lsbSprite.SelectedIndex >= 0)
			{
				int index = lsbSprite.SelectedIndex;

				switch (MainWindow.databaseWindow.spriteCategory)
				{
					case CategoryName.Player:

						MainWindow.databaseWindow.ctlRefSpritePlayer.Tag = lsbSprite.SelectedItem.ToString();
						MainWindow.databaseWindow.ctlRefSpritePlayer.Source = ImageData.spritePlayer[index].patternSource.croppedBitmap[0];

						break;

					case CategoryName.Block:

						MainWindow.databaseWindow.lstViewDbBlock[MainWindow.databaseWindow.selectedIndex].sprite = 
							lsbSprite.SelectedItem.ToString();
						MainWindow.databaseWindow.imgBlockCB.Source = ImageData.spriteBlock[index].patternSource.croppedBitmap[0];

						MainWindow.databaseWindow.lstViewDbBlock[MainWindow.databaseWindow.selectedIndex].name =
							lsbSprite.SelectedItem.ToString();
						MainWindow.databaseWindow.txbBlockName.Text = lsbSprite.SelectedItem.ToString();

						MainWindow.databaseWindow.ListBoxBlockUpdate();
						MainWindow.databaseWindow.lsbBlock.SelectedIndex = MainWindow.databaseWindow.selectedIndex;

						break;

					case CategoryName.Enemy:
						break;

					case CategoryName.Object:

						if (!MainWindow.databaseWindow.choiceSpriteObject)
						{
							MainWindow.databaseWindow.lstViewDbObject[MainWindow.databaseWindow.selectedIndex].spriteA =
								lsbSprite.SelectedItem.ToString();
							MainWindow.databaseWindow.imgObjectA.Source = ImageData.spriteObject[index].patternSource.croppedBitmap[0];

							MainWindow.databaseWindow.lstViewDbObject[MainWindow.databaseWindow.selectedIndex].size =
								new Vector(ImageData.spriteObject[index].patternSource.croppedBitmap[0].PixelWidth,
											ImageData.spriteObject[index].patternSource.croppedBitmap[0].PixelHeight);
						}
						else
						{
							MainWindow.databaseWindow.lstViewDbObject[MainWindow.databaseWindow.selectedIndex].spriteB =
								lsbSprite.SelectedItem.ToString();
							MainWindow.databaseWindow.imgObjectB.Source = ImageData.spriteObject[index].patternSource.croppedBitmap[0];
						}
						

						break;

					case CategoryName.Item:
						break;
				}


				this.Close();
			}

		}
	}
}
