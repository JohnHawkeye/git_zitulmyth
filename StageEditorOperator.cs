﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zitulmyth.Checking;
using Zitulmyth.Data;
using Zitulmyth.Enums;
using static Zitulmyth.StageEditorWindow;

namespace Zitulmyth
{

	public class EditorClearCondition
	{
		public int id { get; set; }
		public string targetName { get; set; }
		public int targetKillNum { get; set; }
		public bool targetReach { get; set; }
		public Vector targetVector { get; set; }
		public int targetTalkFlag { get; set; }
		public int targetTime { get; set; }
	}

	public class EditorBlockPalette
	{
		public BlockType type;
		public Rectangle rectangle;
		public Image image;
		public Label label;
	}

	public class EditorObjectPalette
	{
		public ObjectName type;
		public Rectangle rectangle;
		public Image image;
		public Label label;
	}

	public class EditorEnemyPalette
	{
		public EnemyName type;
		public Rectangle rectangle;
		public Image image;
		public Label label;
	}

	public class EditorItemPalette
	{
		public ItemName type;
		public Rectangle rectangle;
		public Image image;
		public Label label;
	}

	public class StageEditorOperator
    {
		public static List<EditorClearCondition> lstEditorClearConditions = new List<EditorClearCondition>();
		public static List<EditorBlockPalette> lstEditorBlockPalette = new List<EditorBlockPalette>();
		public static List<EditorObjectPalette> lstEditorObjectPalette = new List<EditorObjectPalette>();
		public static List<EditorEnemyPalette> lstEditorEnemyPalette = new List<EditorEnemyPalette>();
		public static List<EditorItemPalette> lstEditorItemPalette = new List<EditorItemPalette>();

		public static Image imgEditorPlayer = new Image();
		public static Image[] imgPaletteCursor = new Image[4];
		public static Image imgEditorPointerCursor = new Image();

		public static bool isPaletteSelected = false;

		public static Vector memoryPlayerStartPos;

		public static BlockType blockPaletteSelected = BlockType.None;
		public static ObjectName objectPaletteSelected = ObjectName.Empty_Collider;
		public static EnemyName enemyPaletteSelected = EnemyName.Zigitu01;
		public static ItemName itemPaletteSelected = ItemName.Apple;

		public static PaletteMode paletteMode = PaletteMode.None;


		public static void EditorImageDataSetting()
		{
			imgEditorPlayer.Width = 32; imgEditorPlayer.Height = 64;
			imgEditorPlayer.Source = ImageData.cbPlayer[0];
			MainWindow.mainCanvas.Children.Add(imgEditorPlayer);

			imgEditorPointerCursor.Width = imgEditorPointerCursor.Height = 32;
			imgEditorPointerCursor.Source = ImageData.cbEmpty;
			imgEditorPointerCursor.Opacity = 0.6;
			MainWindow.mainCanvas.Children.Add(imgEditorPointerCursor);

			for (int i = 0; i < imgPaletteCursor.Length; i++)
			{
				imgPaletteCursor[i] = new Image {Width = 32,Height = 32,Source = ImageData.cbPaletteCursor};				
			}

			ctlCanvasBlockPalette.Children.Add(imgPaletteCursor[0]);
			ctlCanvasObjectPalette.Children.Add(imgPaletteCursor[1]);
			ctlCanvasEnemyPalette.Children.Add(imgPaletteCursor[2]);
			ctlCanvasItemPalette.Children.Add(imgPaletteCursor[3]);
			Canvas.SetZIndex(imgPaletteCursor[0], ImageZindex.debugview);
			Canvas.SetZIndex(imgPaletteCursor[1], ImageZindex.debugview);
			Canvas.SetZIndex(imgPaletteCursor[2], ImageZindex.debugview);
			Canvas.SetZIndex(imgPaletteCursor[3], ImageZindex.debugview);

		}
		
		public static void EditorPlayerStartPosDecision()
		{

			imgEditorPlayer.Opacity = 1;
			stageEditorData.editPlayerStartPosition = MainWindow.mouseMainCanvasPosition;
			Canvas.SetLeft(imgEditorPlayer, stageEditorData.editPlayerStartPosition.X);
			Canvas.SetTop(imgEditorPlayer, stageEditorData.editPlayerStartPosition.Y);
			ctlTextPlayerPosX.Text = stageEditorData.editPlayerStartPosition.X.ToString();
			ctlTextPlayerPosY.Text = stageEditorData.editPlayerStartPosition.Y.ToString();

			paletteMode = PaletteMode.None;
			ctlGridMain.IsEnabled = true;
			MainWindow.stageEditor.Focus();
		}

		public static void EditorPlayerPaletteSetting()
		{
			
			Canvas.SetLeft(imgEditorPlayer, stageEditorData.editPlayerStartPosition.X);
			Canvas.SetTop(imgEditorPlayer, stageEditorData.editPlayerStartPosition.Y);
			Canvas.SetZIndex(imgEditorPlayer, ImageZindex.player);

		}

		public static void EditorBlockPaletteSetting()
		{
			var blockEnum = Enum.GetValues(typeof(BlockType)).Cast<BlockType>().ToList();
			int size = 32;
			Vector setpos = new Vector(20, 20);

			for(int i = 0; i < blockEnum.Count; i++)
			{
				lstEditorBlockPalette.Add(new EditorBlockPalette {
					type = blockEnum[i], image = new Image(), rectangle = new Rectangle(),label = new Label()});

				lstEditorBlockPalette[i].image.Name = lstEditorBlockPalette[i].type.ToString();

				switch (blockEnum[i])
				{
					case BlockType.GreenGround:
						lstEditorBlockPalette[i].image.Source = ImageData.cbBlocks[0, 1];
						lstEditorBlockPalette[i].label.Content = "草の地面";
						break;
					case BlockType.InvisibleBlock:
						lstEditorBlockPalette[i].image.Source = ImageData.cbEmpty;
						lstEditorBlockPalette[i].label.Content = "透明ブロック";
						break;
					case BlockType.InvisiblePlat:
						lstEditorBlockPalette[i].image.Source = ImageData.cbEmpty;
						lstEditorBlockPalette[i].label.Content = "透明プラット";
						break;
					case BlockType.LadderBottom:
						lstEditorBlockPalette[i].image.Source = ImageData.cbLadder[2];
						lstEditorBlockPalette[i].label.Content = "はしご下";
						break;
					case BlockType.LadderMid:
						lstEditorBlockPalette[i].image.Source = ImageData.cbLadder[1];
						lstEditorBlockPalette[i].label.Content = "はしご中";
						break;
					case BlockType.LadderTop:
						lstEditorBlockPalette[i].image.Source = ImageData.cbLadder[0];
						lstEditorBlockPalette[i].label.Content = "はしご上";
						break;
					case BlockType.None:
						lstEditorBlockPalette[i].image.Source = ImageData.cbEmpty;
						lstEditorBlockPalette[i].label.Content = "なし";
						break;
					case BlockType.WoodPlatform:
						lstEditorBlockPalette[i].image.Source = ImageData.cbPlatform[0];
						lstEditorBlockPalette[i].label.Content = "木のプラット";
						break;
				}

				lstEditorBlockPalette[i].image.Width = size;
				lstEditorBlockPalette[i].image.Height = size;

				ctlCanvasBlockPalette.Children.Add(lstEditorBlockPalette[i].image);
				Canvas.SetLeft(lstEditorBlockPalette[i].image, setpos.X+64*i);
				Canvas.SetTop(lstEditorBlockPalette[i].image, setpos.Y);

				lstEditorBlockPalette[i].rectangle.Width = size;
				lstEditorBlockPalette[i].rectangle.Height = size;
				lstEditorBlockPalette[i].rectangle.StrokeThickness = 1;

				lstEditorBlockPalette[i].rectangle.Stroke = Brushes.Black;
				ctlCanvasBlockPalette.Children.Add(lstEditorBlockPalette[i].rectangle);
				Canvas.SetLeft(lstEditorBlockPalette[i].rectangle, setpos.X + 64 * i);
				Canvas.SetTop(lstEditorBlockPalette[i].rectangle, setpos.Y);

				lstEditorBlockPalette[i].label.HorizontalAlignment = HorizontalAlignment.Left;
				ctlCanvasBlockPalette.Children.Add(lstEditorBlockPalette[i].label);
				Canvas.SetLeft(lstEditorBlockPalette[i].label, setpos.X + 64 * i);
				Canvas.SetTop(lstEditorBlockPalette[i].label, setpos.Y+size);

				lstEditorBlockPalette[i].image.MouseLeftButtonDown += new MouseButtonEventHandler(BlockPaletteClick);
				
				if(blockEnum[i] == BlockType.None)
				{
					Canvas.SetLeft(imgPaletteCursor[0],Canvas.GetLeft(lstEditorBlockPalette[i].image));
					Canvas.SetTop(imgPaletteCursor[0], Canvas.GetTop(lstEditorBlockPalette[i].image));
				}
			}

		}


		public static void EditorObjectPaletteSetting()
		{
			var objEnum = Enum.GetValues(typeof(ObjectName)).Cast<ObjectName>().ToList();

			Vector setpos = new Vector(20, 20);
			int colcount = 0;
			int rowcount = 0;

			for (int i = 0; i < objEnum.Count; i++)
			{
				lstEditorObjectPalette.Add(new EditorObjectPalette
				{
					type = objEnum[i],
					image = new Image(),
					rectangle = new Rectangle(),
					label = new Label()
				});

				lstEditorObjectPalette[i].image.Name = lstEditorObjectPalette[i].type.ToString();

				switch (objEnum[i])
				{
					case ObjectName.Empty_Collider:
						lstEditorObjectPalette[i].image.Source = ImageData.cbDebug[0];
						lstEditorObjectPalette[i].image.Stretch = Stretch.Fill;
						lstEditorObjectPalette[i].label.Content = "トリガーエリア";
						break;
					case ObjectName.Npc_Ilsona:
						lstEditorObjectPalette[i].image.Source = ImageData.cbNpc[4];
						lstEditorObjectPalette[i].label.Content = "イルソナ";
						break;
					case ObjectName.Npc_Opsa:
						lstEditorObjectPalette[i].image.Source = ImageData.cbNpc[0];
						lstEditorObjectPalette[i].label.Content = "オプサ";
						break;
					case ObjectName.Npc_Yeeda:
						lstEditorObjectPalette[i].image.Source = ImageData.cbNpc[2];
						lstEditorObjectPalette[i].label.Content = "イェーダ";
						break;
					case ObjectName.Obj_CampFire:
						lstEditorObjectPalette[i].image.Source = ImageData.cbObject[2];
						lstEditorObjectPalette[i].label.Content = "たきび";
						break;
					case ObjectName.Obj_Chair:
						lstEditorObjectPalette[i].image.Source = ImageData.cbObject[6];
						lstEditorObjectPalette[i].label.Content = "いす";
						break;
					case ObjectName.Obj_Huton:
						lstEditorObjectPalette[i].image.Source = ImageData.cbObject[8];
						lstEditorObjectPalette[i].label.Content = "ふとん";
						break;
					case ObjectName.Obj_Table:
						lstEditorObjectPalette[i].image.Source = ImageData.cbObject[7];
						lstEditorObjectPalette[i].label.Content = "テーブル";
						break;
				}

				Vector size = new Vector(64,64);
				if (setpos.X + size.X * colcount+64 >= ctlCanvasObjectPalette.Width)
				{
					colcount = 0;
					rowcount++;

				}

				lstEditorObjectPalette[i].image.Width = size.X;
				lstEditorObjectPalette[i].image.Height = size.Y;

				ctlCanvasObjectPalette.Children.Add(lstEditorObjectPalette[i].image);
				Canvas.SetLeft(lstEditorObjectPalette[i].image, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorObjectPalette[i].image, setpos.Y + rowcount * size.Y);

				lstEditorObjectPalette[i].rectangle.Width = size.X;
				lstEditorObjectPalette[i].rectangle.Height = size.Y;
				lstEditorObjectPalette[i].rectangle.StrokeThickness = 1;

				lstEditorObjectPalette[i].rectangle.Stroke = Brushes.Black;
				ctlCanvasObjectPalette.Children.Add(lstEditorObjectPalette[i].rectangle);
				Canvas.SetLeft(lstEditorObjectPalette[i].rectangle, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorObjectPalette[i].rectangle, setpos.Y + rowcount * size.Y);

				lstEditorObjectPalette[i].label.HorizontalAlignment = HorizontalAlignment.Left;
				ctlCanvasObjectPalette.Children.Add(lstEditorObjectPalette[i].label);
				Canvas.SetLeft(lstEditorObjectPalette[i].label, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorObjectPalette[i].label, setpos.Y+size.Y + rowcount * size.Y);

				lstEditorObjectPalette[i].image.MouseLeftButtonDown += new MouseButtonEventHandler(ObjectPaletteClick);

				colcount++;
			}

		}

		public static void EditorEnemyPaletteSetting()
		{
			var enemyEnum = Enum.GetValues(typeof(EnemyName)).Cast<EnemyName>().ToList();

			Vector setpos = new Vector(20, 20);
			int colcount = 0;
			int rowcount = 0;

			for (int i = 0; i < enemyEnum.Count; i++)
			{
				lstEditorEnemyPalette.Add(new EditorEnemyPalette
				{
					type = enemyEnum[i],
					image = new Image(),
					rectangle = new Rectangle(),
					label = new Label()
				});

				lstEditorEnemyPalette[i].image.Name = lstEditorEnemyPalette[i].type.ToString();

				switch (enemyEnum[i])
				{
					case EnemyName.Boar:
						lstEditorEnemyPalette[i].image.Source = ImageData.lstCBEnemy[1].lstCBIdle[0];
						lstEditorEnemyPalette[i].label.Content = "いのしし";
						break;
					case EnemyName.Zigitu01:
						lstEditorEnemyPalette[i].image.Source = ImageData.lstCBEnemy[0].lstCBIdle[0];
						lstEditorEnemyPalette[i].label.Content = "ズィギツ";
						break;
				}

				Vector size = new Vector(64, 64);
				if (setpos.X + size.X * colcount + 64 >= ctlCanvasEnemyPalette.Width)
				{
					colcount = 0;
					rowcount++;

				}

				lstEditorEnemyPalette[i].image.Width = size.X;
				lstEditorEnemyPalette[i].image.Height = size.Y;

				ctlCanvasEnemyPalette.Children.Add(lstEditorEnemyPalette[i].image);
				Canvas.SetLeft(lstEditorEnemyPalette[i].image, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorEnemyPalette[i].image, setpos.Y + rowcount * size.Y);

				lstEditorEnemyPalette[i].rectangle.Width = size.X;
				lstEditorEnemyPalette[i].rectangle.Height = size.Y;
				lstEditorEnemyPalette[i].rectangle.StrokeThickness = 1;

				lstEditorEnemyPalette[i].rectangle.Stroke = Brushes.Black;
				ctlCanvasEnemyPalette.Children.Add(lstEditorEnemyPalette[i].rectangle);
				Canvas.SetLeft(lstEditorEnemyPalette[i].rectangle, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorEnemyPalette[i].rectangle, setpos.Y + rowcount * size.Y);

				lstEditorEnemyPalette[i].label.HorizontalAlignment = HorizontalAlignment.Left;
				ctlCanvasEnemyPalette.Children.Add(lstEditorEnemyPalette[i].label);
				Canvas.SetLeft(lstEditorEnemyPalette[i].label, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorEnemyPalette[i].label, setpos.Y + size.Y + rowcount * size.Y);

				lstEditorEnemyPalette[i].image.MouseLeftButtonDown += new MouseButtonEventHandler(EnemyPaletteClick);

				colcount++;
			}

		}

		public static void EditorItemPaletteSetting()
		{
			var itemEnum = Enum.GetValues(typeof(ItemName)).Cast<ItemName>().ToList();

			Vector setpos = new Vector(20, 20);
			int colcount = 0;
			int rowcount = 0;

			for (int i = 0; i < itemEnum.Count; i++)
			{
				lstEditorItemPalette.Add(new EditorItemPalette
				{
					type = itemEnum[i],
					image = new Image(),
					rectangle = new Rectangle(),
					label = new Label()
				});

				lstEditorItemPalette[i].image.Name = lstEditorItemPalette[i].type.ToString();

				switch (itemEnum[i])
				{
					case ItemName.Apple:
						lstEditorItemPalette[i].image.Source = ImageData.lstCBItem[1].lstCBIdle[0];
						lstEditorItemPalette[i].label.Content = "りんご";
						break;
					case ItemName.BoarMeat:
						lstEditorItemPalette[i].image.Source = ImageData.lstCBItem[4].lstCBIdle[0];
						lstEditorItemPalette[i].label.Content = "いのししの肉";
						break;
					case ItemName.Coin:
						lstEditorItemPalette[i].image.Source = ImageData.lstCBItem[0].lstCBIdle[0];
						lstEditorItemPalette[i].label.Content = "コイン";
						break;
					case ItemName.StarFruit:
						lstEditorItemPalette[i].image.Source = ImageData.lstCBItem[2].lstCBIdle[0];
						lstEditorItemPalette[i].label.Content = "スターフルーツ";
						break;
					case ItemName.TreeBranch:
						lstEditorItemPalette[i].image.Source = ImageData.lstCBItem[3].lstCBIdle[0];
						lstEditorItemPalette[i].label.Content = "木の枝";
						break;
				}

				Vector size = new Vector(64, 64);
				if (setpos.X + size.X * colcount + 64 >= ctlCanvasEnemyPalette.Width)
				{
					colcount = 0;
					rowcount++;

				}

				lstEditorItemPalette[i].image.Width = size.X;
				lstEditorItemPalette[i].image.Height = size.Y;

				ctlCanvasItemPalette.Children.Add(lstEditorItemPalette[i].image);
				Canvas.SetLeft(lstEditorItemPalette[i].image, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorItemPalette[i].image, setpos.Y + rowcount * size.Y);

				lstEditorItemPalette[i].rectangle.Width = size.X;
				lstEditorItemPalette[i].rectangle.Height = size.Y;
				lstEditorItemPalette[i].rectangle.StrokeThickness = 1;

				lstEditorItemPalette[i].rectangle.Stroke = Brushes.Black;
				ctlCanvasItemPalette.Children.Add(lstEditorItemPalette[i].rectangle);
				Canvas.SetLeft(lstEditorItemPalette[i].rectangle, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorItemPalette[i].rectangle, setpos.Y + rowcount * size.Y);

				lstEditorItemPalette[i].label.HorizontalAlignment = HorizontalAlignment.Left;
				ctlCanvasItemPalette.Children.Add(lstEditorItemPalette[i].label);
				Canvas.SetLeft(lstEditorItemPalette[i].label, setpos.X + colcount * size.X);
				Canvas.SetTop(lstEditorItemPalette[i].label, setpos.Y + size.Y + rowcount * size.Y);

				lstEditorItemPalette[i].image.MouseLeftButtonDown += new MouseButtonEventHandler(ItemPaletteClick);

				colcount++;
			}

		}

		private static void BlockPaletteClick(object sender,RoutedEventArgs e)
		{
		
			Vector pos = VisualTreeHelper.GetOffset((Image)sender);

			blockPaletteSelected = (BlockType)Enum.Parse(typeof(BlockType),((Image)sender).Name);
	
			Canvas.SetLeft(imgPaletteCursor[0],pos.X); Canvas.SetTop(imgPaletteCursor[0], pos.Y);

			imgEditorPointerCursor.Source = (sender as Image).Source;
			

			paletteMode = PaletteMode.Block;
		
		}

		private static void ObjectPaletteClick(object sender, RoutedEventArgs e)
		{

			Vector pos = VisualTreeHelper.GetOffset((Image)sender);

			objectPaletteSelected = (ObjectName)Enum.Parse(typeof(ObjectName), ((Image)sender).Name);

			Canvas.SetLeft(imgPaletteCursor[1], pos.X); Canvas.SetTop(imgPaletteCursor[1], pos.Y);

		}

		private static void EnemyPaletteClick(object sender, RoutedEventArgs e)
		{

			Vector pos = VisualTreeHelper.GetOffset((Image)sender);

			enemyPaletteSelected = (EnemyName)Enum.Parse(typeof(EnemyName), ((Image)sender).Name);

			Canvas.SetLeft(imgPaletteCursor[2], pos.X); Canvas.SetTop(imgPaletteCursor[2], pos.Y);

		}

		private static void ItemPaletteClick(object sender, RoutedEventArgs e)
		{

			Vector pos = VisualTreeHelper.GetOffset((Image)sender);

			itemPaletteSelected = (ItemName)Enum.Parse(typeof(ItemName), ((Image)sender).Name);

			Canvas.SetLeft(imgPaletteCursor[3], pos.X); Canvas.SetTop(imgPaletteCursor[3], pos.Y);

		}

		public static void BlockImageLeftClick(object sender,RoutedEventArgs e)
		{

		}

		public static void BlockImageRightClick(object sender, RoutedEventArgs e)
		{

		}

		public static void EditSetupBlockOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y-1) * 32) - 1;
			int blockX = (int)blockpos.X -1;
			int blockY = (int)blockpos.Y -1;
			Image _image = new Image();

			

			if(stageEditorData.editIndicateStage[index] == BlockType.None)
			{
				stageEditorData.editIndicateStage[index] = blockPaletteSelected;

				switch (stageEditorData.editIndicateStage[index])
				{
					case BlockType.GreenGround:
						_image = new Image { Source = ImageData.cbBlocks[0, 1], Width = 32, Height = 32, };
						break;

					case BlockType.WoodPlatform:
						_image = new Image { Source = ImageData.cbPlatform[1], Width = 32, Height = 32, };
						break;

					case BlockType.LadderTop:
						_image = new Image { Source = ImageData.cbLadder[0], Width = 32, Height = 32, };
						break;

					case BlockType.LadderMid:
						_image = new Image { Source = ImageData.cbLadder[1], Width = 32, Height = 32, };
						break;

					case BlockType.LadderBottom:
						_image = new Image { Source = ImageData.cbLadder[2], Width = 32, Height = 32, };
						break;

					case BlockType.InvisibleBlock:
						_image = new Image { Source = ImageData.cbInvisibleBlock, Width = 32, Height = 32, };
						break;

					case BlockType.InvisiblePlat:
						_image = new Image { Source = ImageData.cbInvisiblePlat, Width = 32, Height = 32, };
						break;
				}

				StageData.imgBlock[blockY, blockX] = _image;

				if (stageEditorData.editIndicateStage[index] != BlockType.None)
				{
					StageData.imgBlock[blockY, blockX].MouseLeftButtonDown += new MouseButtonEventHandler(BlockImageLeftClick);

					MainWindow.mainCanvas.Children.Add(StageData.imgBlock[blockY, blockX]);
					Canvas.SetTop(StageData.imgBlock[blockY, blockX], blockY * 32);
					Canvas.SetLeft(StageData.imgBlock[blockY, blockX], blockX * 32);
					Canvas.SetZIndex(StageData.imgBlock[blockY, blockX], ImageZindex.block);
				}
			}
		
			
		}
	}

}
