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

							var _imgBlock = new Image { Source = ImageData.cbBlocks[0, 1], Width = 32, Height = 32,};
							StageData.imgBlock[i, j] = _imgBlock;
							break;

						case BlockType.WoodPlatform:

							var _imgWoodPlat = new Image{Source = ImageData.cbPlatform[1],Width = 32,Height = 32,};
							StageData.imgBlock[i, j] = _imgWoodPlat;
							break;

						case BlockType.LadderTop:

							var _imgLadderTop = new Image{Source = ImageData.cbLadder[0],Width = 32,Height = 32,};
							StageData.imgBlock[i, j] = _imgLadderTop;
							break;

						case BlockType.LadderMid:

							var _imgLadderMid = new Image{Source = ImageData.cbLadder[1],Width = 32,Height = 32,};
							StageData.imgBlock[i, j] = _imgLadderMid;
							break;

						case BlockType.LadderBottom:

							var _imgLadderBottom = new Image { Source = ImageData.cbLadder[2], Width = 32, Height = 32, };
							StageData.imgBlock[i, j] = _imgLadderBottom;
							break;
					}


					if(StageData.indicateStage[i,j] != BlockType.None &&
						StageData.indicateStage[i,j] != BlockType.InvisibleBlock &&
						StageData.indicateStage[i,j] != BlockType.InvisiblePlat)
					{
						canvas.Children.Add(StageData.imgBlock[i, j]);
						Canvas.SetTop(StageData.imgBlock[i, j], i * 32);
						Canvas.SetLeft(StageData.imgBlock[i, j], j * 32);
						Canvas.SetZIndex(StageData.imgBlock[i, j],2);
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

			canvas.Children.Remove(StageData.imgScenery);


			for (int i = 0; i < StageData.objectPosition.Count; i++)
			{
				canvas.Children.Remove(StageData.imgObject[i]);
			}

			StageData.npcPosition.Clear();
			StageData.objectPosition.Clear();

			StageData.imgNpc.Clear();
			StageData.imgObject.Clear();

			StageData.refCbObject.Clear();
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
