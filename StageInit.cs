using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Zitulmyth.Data;

namespace Zitulmyth
{

	public enum BlockType
	{
		None = 0,
		InvisibleBlock = 1,
		GreenGround =2,
		WoodPlatform = 10,
		LadderTop=15,
		LadderMid=16,
		LadderBottom=17,
	}

	public class StageInit
	{

		public static void InitBlockData()
		{
			StageData.indicateStage = new BlockType[24, 32];
			StageData.imgBlock = new Image[24, 32];
		}

		//block set
		public static void StageBlockSet(Canvas canvas)
		{ 

			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					switch (StageData.indicateStage[i, j])
					{
						case BlockType.GreenGround:

							var _imgBlock = new Image
							{
								Source = ImageData.cbBlocks[0, 1],
								Width = 32,	Height = 32,
							};

							StageData.imgBlock[i, j] = _imgBlock;

							break;
					}


					if(StageData.indicateStage[i,j] != BlockType.None &&
						StageData.indicateStage[i,j] != BlockType.InvisibleBlock)
					{
						canvas.Children.Add(StageData.imgBlock[i, j]);
						Canvas.SetTop(StageData.imgBlock[i, j], i * 32);
						Canvas.SetLeft(StageData.imgBlock[i, j], j * 32);
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
					
					if (StageData.indicateStage[i, j] != BlockType.None)
					{
						canvas.Children.Remove(StageData.imgBlock[i, j]);

					}

				}
			}

		}

		public static void StageObjectsRemove(Canvas canvas)
		{
			for(int i = 0; i < StageData.npcPosition.Count; i++)
			{
				canvas.Children.Remove(StageData.imgNpc[i]);
			}

			for (int i = 0; i < StageData.interiorPosition.Count; i++)
			{
				canvas.Children.Remove(StageData.imgInterior[i]);
			}

			for (int i = 0; i < StageData.furniturePosition.Count; i++)
			{
				canvas.Children.Remove(StageData.imgFurniture[i]);
			}

			StageData.npcPosition.Clear();
			StageData.interiorPosition.Clear();
			StageData.furniturePosition.Clear();

			StageData.imgNpc.Clear();
			StageData.imgInterior.Clear();
			StageData.imgFurniture.Clear();

			StageData.refCbFurniture.Clear();
		}

		public static void InitPlayer(Canvas canvas)
		{
			MainWindow.playerNowHp = MainWindow.playerMaxHp;
			MainWindow.playerNowMana = MainWindow.playerMaxMana;

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
			Canvas.SetZIndex(ImageData.imgPlayer, MainWindow.playerImageZindex);


		}

		public static void InitPlayerStatus(Canvas caLife ,Canvas caMana)
		{
			for (int i = 0; i < MainWindow.playerMaxHp; i++)
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

			for (int i = 0; i < MainWindow.playerMaxMana; i++)
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
