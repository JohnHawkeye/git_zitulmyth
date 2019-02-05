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

					for (int i = 0; i < ImageData.spriteBlock.Count; i++)
					{
						lstSpriteName.Add(ImageData.spriteBlock[i].patternSource.patternName);
					}

					lsbSprite.ItemsSource = lstSpriteName;
					lsbSprite.SelectedIndex = 0;

					break;

				case CategoryName.Enemy:

					for (int i = 0; i < ImageData.spriteEnemy.Count; i++)
					{
						lstSpriteName.Add(ImageData.spriteEnemy[i].patternSource.patternName);
					}

					lsbSprite.ItemsSource = lstSpriteName;
					lsbSprite.SelectedIndex = 0;

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

					for (int i = 0; i < ImageData.spriteItem.Count; i++)
					{
						lstSpriteName.Add(ImageData.spriteItem[i].patternSource.patternName);
					}

					lsbSprite.ItemsSource = lstSpriteName;
					lsbSprite.SelectedIndex = 0;
					break;
			}

			PreviewImage();

		}

		private void PreviewImage()
		{
			if (lsbSprite.SelectedIndex >= 0)
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

						imgPreview.Source = ImageData.spriteEnemy[index].patternSource.croppedBitmap[0];

						break;

					case CategoryName.Object:
						imgPreview.Source = ImageData.spriteObject[index].patternSource.croppedBitmap[0];
						break;

					case CategoryName.Item:

						imgPreview.Source = ImageData.spriteItem[index].patternSource.croppedBitmap[0];

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

						EnemySpriteAddressSelector(Convert.ToInt32(MainWindow.databaseWindow.ctlRefSpriteEnemy.Tag));
						
						MainWindow.databaseWindow.ctlRefSpriteEnemy.Source = ImageData.spriteEnemy[index].patternSource.croppedBitmap[0];
						MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].size =
								new Vector(ImageData.spriteEnemy[index].patternSource.croppedBitmap[0].PixelWidth,
											ImageData.spriteEnemy[index].patternSource.croppedBitmap[0].PixelHeight);

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

						MainWindow.databaseWindow.lstViewDbItem[MainWindow.databaseWindow.selectedIndex].sprite =
							lsbSprite.SelectedItem.ToString();
						MainWindow.databaseWindow.imgItem.Source = ImageData.spriteItem[index].patternSource.croppedBitmap[0];

						MainWindow.databaseWindow.lstViewDbItem[MainWindow.databaseWindow.selectedIndex].size =
								new Vector(ImageData.spriteItem[index].patternSource.croppedBitmap[0].PixelWidth,
											ImageData.spriteItem[index].patternSource.croppedBitmap[0].PixelHeight);
						break;
				}


				this.Close();
			}

		}

		private void EnemySpriteAddressSelector (int index)
		{
			switch (index)
			{
				case 0:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteIdleL =
							lsbSprite.SelectedItem.ToString();
					break;
				case 1:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteIdleR =
							lsbSprite.SelectedItem.ToString();
					break;
				case 2:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteSpawnL =
							lsbSprite.SelectedItem.ToString();
					break;
				case 3:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteSpawnR =
							lsbSprite.SelectedItem.ToString();
					break;
				case 4:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteMoveL =
							lsbSprite.SelectedItem.ToString();
					break;
				case 5:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteMoveR =
							lsbSprite.SelectedItem.ToString();
					break;
				case 6:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteAttackL =
						 lsbSprite.SelectedItem.ToString();
					break;
				case 7:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteAttackR =
							lsbSprite.SelectedItem.ToString();
					break;
				case 8:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteDamageL =
						 lsbSprite.SelectedItem.ToString();
					break;
				case 9:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteDamageR =
						 lsbSprite.SelectedItem.ToString();
					break;
				case 10:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteDeathL =
						lsbSprite.SelectedItem.ToString();
					break;
				case 11:
					MainWindow.databaseWindow.lstViewDbEnemy[MainWindow.databaseWindow.selectedIndex].spriteDeathR =
						lsbSprite.SelectedItem.ToString();
					break;
			}
		}
	}
}
