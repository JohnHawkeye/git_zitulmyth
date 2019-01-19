using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Zitulmyth.Data;

namespace Zitulmyth
{
	public class StageInit
	{

		public static void InitBlockData()
		{
			StageData.indicateStage = new int[24, 32];
			StageData.imgBlock = new Image[24, 32];
		}

		//block set
		public static void StageBlockSet(Canvas canvas)
		{

			Image _image = new Image();

			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					int spriteindex = StageData.indicateStage[i, j];

					string spritename = StageData.lstDbBlock[spriteindex].sprite;

					_image = new Image
					{
						Source = ImageData.ImageSourceSelector(CategoryName.Block, spritename),
						Width = 32,
						Height = 32,
					};

					StageData.imgBlock[i, j] = _image;

					if (StageData.indicateStage[i,j] != 0)
					{
						canvas.Children.Add(StageData.imgBlock[i, j]);
						Canvas.SetTop(StageData.imgBlock[i, j], i * 32);
						Canvas.SetLeft(StageData.imgBlock[i, j], j * 32);
						Canvas.SetZIndex(StageData.imgBlock[i, j], ImageZindex.block);
					}
						
				}
			}
		}

		public static void StageBlockRemove(Canvas canvas)
		{
			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					
					if (StageData.indicateStage[i, j] != 0)
					{
						canvas.Children.Remove(StageData.imgBlock[i, j]);

					}

				}
			}

		}

		public static void StageObjectsRemove(Canvas canvas)
		{
			for(int i = 0; i < ObjectChecker.lstObject.Count; i++)
			{
				canvas.Children.Remove(ObjectChecker.lstObject[i].imgObject);
			}

			canvas.Children.Remove(StageData.imgScenery);

			ObjectChecker.lstObject.Clear();
			

		}


		public static void StageItemRemove(Canvas canvas)
		{
			for (int i = 0; i < Item.lstItemData.Count; i++)
			{
				canvas.Children.Remove(Item.lstItemData[i].imgItem);
			}

			Item.lstItemData.Clear();
		}

		public static void StageEnemyRemove(Canvas canvas)
		{
			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{
				canvas.Children.Remove(SpawnEnemy.lstEnemyData[i].imgEnemy);
			}
			
			SpawnEnemy.lstEnemyData.Clear();


		}

		public static void InitPlayer(Canvas canvas)
		{
			PlayerStatus.playerNowHp = PlayerStatus.playerMaxHp;
			PlayerStatus.playerNowMana = PlayerStatus.playerMaxMana;

			if (ImageData.imgPlayer != null)
			{
				canvas.Children.Remove(ImageData.imgPlayer);
				ImageData.imgPlayer = null;
			}

			var _imgPlayer = new Image()
			{
				Source = ImageData.bmEmpty,

				Width = 32,
				Height = 64,
			
			};
			
			ImageData.imgPlayer = _imgPlayer;
			canvas.Children.Add(_imgPlayer);

			Canvas.SetLeft(ImageData.imgPlayer, 300);
			Canvas.SetTop(ImageData.imgPlayer, 671);
			Canvas.SetZIndex(ImageData.imgPlayer, ImageZindex.player);


		}

		public static void InitPlayerStatus(Canvas caLife ,Canvas caMana)
		{
			for (int i = 0; i < PlayerStatus.playerMaxHp; i++)
			{
				var _imgLife = new Image()
				{
					Source = ImageData.cbLife[0],
					Width = 32,
					Height = 32,
				};

				ImageData.imgLife.Add(_imgLife);
				caLife.Children.Add(ImageData.imgLife[i]);
				Canvas.SetLeft(ImageData.imgLife[i], i * 32);
				Canvas.SetTop(ImageData.imgLife[i], 0);

			}

			for (int i = 0; i < PlayerStatus.playerMaxMana; i++)
			{
				var _imgMana = new Image()
				{
					Source = ImageData.cbMana[0],
					Width = 32,
					Height = 32,
				};

				ImageData.imgMana.Add(_imgMana);
				caMana.Children.Add(ImageData.imgMana[i]);
				Canvas.SetLeft(ImageData.imgMana[i], i * 32);
				Canvas.SetTop(ImageData.imgMana[i], 0);

			}
		}
	}
}
