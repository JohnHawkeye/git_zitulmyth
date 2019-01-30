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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zitulmyth.Checking;
using Zitulmyth.Data;
using Zitulmyth.Enums;
using static Zitulmyth.StageEditorWindow;

namespace Zitulmyth
{
	//listview only
	public class EditorListViewClearCondition
	{
		public int id { get; set; }
		public string targetName { get; set; }
		public int targetKillNum { get; set; }
		public bool targetReach { get; set; }
		public Vector targetVector { get; set; }
		public int targetTalkFlag { get; set; }
		public int targetTime { get; set; }
	}
	public class EditorListViewObjectData
	{
		public int id { get; set; }
		public string objectName { get; set; }
		public Vector objectPosition { get; set; }
		public Vector objectSize { get; set; }
		public int objectZindex { get; set; }
		public bool objectToggleSwitch { get; set; }
		public TargetType objectTargetType { get; set; }
		public int objectTargetId { get; set; }
		public int objectTalkID { get; set; }
	}
	public class EditorListViewEnemyData
	{
		public int id { get; set; }
		public string enemyName { get; set; }
		public Vector enemyPosition { get; set; }
		public bool enemyDirection { get; set; }
		
	}
	public class EditorListViewItemData
	{
		public int id { get; set; }
		public string itemName { get; set; }
		public Vector itemPosition { get; set; }
	}

	//convert only
	public class EditorObjectDataListConvert
	{
		public string objectName;
		public Vector objectPosition;
		public Vector objectSize;
		public int objectZindex;
		public bool objectToggleSwitch;
		public TargetType objectTargetType;
		public int objectTargetId;
		public int objectTalkID;
	}

	public class EditorEnemyDataListConvert
	{
		public EnemyName enemyName;
		public Vector enemyPosition;
		public bool enemyDirection;	
	}

	public class EditorItemDataListConvert
	{
		public ItemName itemName;
		public Vector itemPosition;
		
	}

	//palette
	public class EditorBlockPalette
	{
		public Rectangle rectangle;
		public Image image;
		public Label label;
	}

	public class EditorObjectPalette
	{
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
		public static List<EditorListViewClearCondition> lstListViewClearConditions = new List<EditorListViewClearCondition>();
		public static List<EditorListViewObjectData> lstListViewObjectData = new List<EditorListViewObjectData>();
		public static List<EditorListViewEnemyData> lstListViewEnemyData = new List<EditorListViewEnemyData>();
		public static List<EditorListViewItemData> lstListViewItemData = new List<EditorListViewItemData>();


		public static List<EditorBlockPalette> lstEditorBlockPalette = new List<EditorBlockPalette>();
		public static List<EditorObjectPalette> lstEditorObjectPalette = new List<EditorObjectPalette>();
		public static List<EditorEnemyPalette> lstEditorEnemyPalette = new List<EditorEnemyPalette>();
		public static List<EditorItemPalette> lstEditorItemPalette = new List<EditorItemPalette>();

		public static List<EditorObjectDataListConvert> lstObjectDataConvert = new List<EditorObjectDataListConvert>();
		public static List<EditorEnemyDataListConvert> lstEnemyDataConvert = new List<EditorEnemyDataListConvert>();
		public static List<EditorItemDataListConvert> lstItemDataConvert = new List<EditorItemDataListConvert>();


		public static Image imgEditorPlayer = new Image();
		public static Image[] imgPaletteCursor = new Image[4];
		public static Image imgEditorPointerCursor = new Image();

		public static bool isPaletteSelected = false;

		public static Vector memoryPlayerStartPos;

		public static int blockPaletteSelected = 0;
		public static int objectPaletteSelected = 0;
		public static EnemyName enemyPaletteSelected = EnemyName.Zigitu01;
		public static ItemName itemPaletteSelected = ItemName.Apple;

		public static PaletteMode paletteMode = PaletteMode.None;


		public static void EditorImageDataSetting()
		{
			imgEditorPlayer.Width = 32; imgEditorPlayer.Height = 64;
			imgEditorPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteMoveR);
			MainWindow.mainCanvas.Children.Add(imgEditorPlayer);

			imgEditorPointerCursor.Width = imgEditorPointerCursor.Height = 32;
			imgEditorPointerCursor.Source = ImageData.cbEmpty;
			imgEditorPointerCursor.Opacity = 0.6;
			MainWindow.mainCanvas.Children.Add(imgEditorPointerCursor);
			Canvas.SetZIndex(imgEditorPointerCursor, ImageZindex.handCursor);

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

		public static void EditorImagesRemove()
		{
			MainWindow.mainCanvas.Children.Remove(imgEditorPlayer);
			MainWindow.mainCanvas.Children.Remove(imgEditorPointerCursor);

			ctlCanvasBlockPalette.Children.Clear();
			ctlCanvasEnemyPalette.Children.Clear();
			ctlCanvasObjectPalette.Children.Clear();
			ctlCanvasItemPalette.Children.Clear();
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
			int size = 32;
			int arrangedLength = 0;
			int col = 0;
			int marginLeft = 20,marginTop = 20;

			Vector setpos = new Vector(0, 0);

			for(int i = 0; i < StageData.lstDbBlock.Count; i++)
			{
				lstEditorBlockPalette.Add(new EditorBlockPalette { image = new Image(), rectangle = new Rectangle(), label = new Label() });
				lstEditorBlockPalette[i].image.Tag = i;

				lstEditorBlockPalette[i].image.Source =	ImageData.ImageSourceSelector(CategoryName.Block, StageData.lstDbBlock[i].sprite);
				lstEditorBlockPalette[i].label.Content = StageData.lstDbBlock[i].name;

				lstEditorBlockPalette[i].image.Width = size;
				lstEditorBlockPalette[i].image.Height = size;

				if(arrangedLength + col * marginLeft >= 400)
				{
					arrangedLength = 0;
					marginTop += 64;
					col = 0;
				}

				ctlCanvasBlockPalette.Children.Add(lstEditorBlockPalette[i].image);
				Canvas.SetLeft(lstEditorBlockPalette[i].image, marginLeft + setpos.X + col * size + arrangedLength);
				Canvas.SetTop(lstEditorBlockPalette[i].image, marginTop + setpos.Y);

				lstEditorBlockPalette[i].rectangle.Width = size;
				lstEditorBlockPalette[i].rectangle.Height = size;
				lstEditorBlockPalette[i].rectangle.StrokeThickness = 1;

				lstEditorBlockPalette[i].rectangle.Stroke = Brushes.Black;
				ctlCanvasBlockPalette.Children.Add(lstEditorBlockPalette[i].rectangle);
				Canvas.SetLeft(lstEditorBlockPalette[i].rectangle, marginLeft + setpos.X + col * size + arrangedLength);
				Canvas.SetTop(lstEditorBlockPalette[i].rectangle, marginTop + setpos.Y);

				lstEditorBlockPalette[i].label.HorizontalAlignment = HorizontalAlignment.Left;
				ctlCanvasBlockPalette.Children.Add(lstEditorBlockPalette[i].label);
				Canvas.SetLeft(lstEditorBlockPalette[i].label, marginLeft + setpos.X+ col * size + arrangedLength);
				Canvas.SetTop(lstEditorBlockPalette[i].label, marginTop + setpos.Y+size);

				lstEditorBlockPalette[i].image.MouseLeftButtonDown += new MouseButtonEventHandler(BlockPaletteClick);
				
				if(i == 0)
				{
					Canvas.SetLeft(imgPaletteCursor[0],Canvas.GetLeft(lstEditorBlockPalette[i].image));
					Canvas.SetTop(imgPaletteCursor[0], Canvas.GetTop(lstEditorBlockPalette[i].image));
				}

				arrangedLength += size;
				col++;

			}

		}


		public static void EditorObjectPaletteSetting()
		{

			Vector setpos = new Vector(0, 0);
			Vector size = new Vector(32,32);
			int arrangedLength = 0;
			int col = 0 ,row = 0;
			int marginLeft = 20, marginTop = 20;

			for (int i = 0; i < StageData.lstDbObject.Count; i++)
			{
				lstEditorObjectPalette.Add(new EditorObjectPalette{
					image = new Image(),
					rectangle = new Rectangle(),
					label = new Label()
				});

				lstEditorObjectPalette[i].image.Tag = i;

				lstEditorObjectPalette[i].image.Source = ImageData.ImageSourceSelector(CategoryName.Object, StageData.lstDbObject[i].spriteA);
				lstEditorObjectPalette[i].label.Content = StageData.lstDbObject[i].name;
				size = StageData.lstDbObject[i].size;
				lstEditorObjectPalette[i].image.Width = size.X;
				lstEditorObjectPalette[i].image.Height = size.Y;

				if (arrangedLength + col * marginLeft >= 400)
				{
					arrangedLength = 0;
					marginTop += 64;
					col = 0;
					row++;
				}

				ctlCanvasObjectPalette.Children.Add(lstEditorObjectPalette[i].image);
				Canvas.SetLeft(lstEditorObjectPalette[i].image, marginLeft + setpos.X + col * 32 + arrangedLength);
				Canvas.SetTop(lstEditorObjectPalette[i].image, marginTop + setpos.Y + row * 32);

				lstEditorObjectPalette[i].rectangle.Width = size.X;
				lstEditorObjectPalette[i].rectangle.Height = size.Y;
				lstEditorObjectPalette[i].rectangle.StrokeThickness = 1;

				lstEditorObjectPalette[i].rectangle.Stroke = Brushes.Black;
				ctlCanvasObjectPalette.Children.Add(lstEditorObjectPalette[i].rectangle);
				Canvas.SetLeft(lstEditorObjectPalette[i].rectangle, marginLeft + setpos.X + col * 32 + arrangedLength);
				Canvas.SetTop(lstEditorObjectPalette[i].rectangle, marginTop + setpos.Y + row * 32);

				lstEditorObjectPalette[i].label.HorizontalAlignment = HorizontalAlignment.Left;
				ctlCanvasObjectPalette.Children.Add(lstEditorObjectPalette[i].label);
				Canvas.SetLeft(lstEditorObjectPalette[i].label, marginLeft + setpos.X + col * 32 + arrangedLength);
				Canvas.SetTop(lstEditorObjectPalette[i].label, marginTop + setpos.Y + row * 32 + size.Y);

				lstEditorObjectPalette[i].image.MouseLeftButtonDown += new MouseButtonEventHandler(ObjectPaletteClick);

				arrangedLength += (int)size.X;
				col++;

			}

		}

		public static void EditorEnemyPaletteSetting()
		{
			var enemyEnum = Enum.GetValues(typeof(EnemyName)).Cast<EnemyName>().ToList();

			Vector setpos = new Vector(20, 20);
			Vector size = new Vector(32,32);
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
						lstEditorEnemyPalette[i].image.Source =
							ImageData.ImageSourceSelector(CategoryName.Enemy, "BoarIdleL");
						lstEditorEnemyPalette[i].label.Content = "いのしし";
						size = new Vector(64, 32);

						break;
					case EnemyName.Zigitu01:
						lstEditorEnemyPalette[i].image.Source =
							ImageData.ImageSourceSelector(CategoryName.Enemy, "ZigituIdle");
						lstEditorEnemyPalette[i].label.Content = "ズィギツ";
						size = new Vector(32, 64);
						break;
				}

				
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
			Vector size = new Vector(32, 32);
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
						lstEditorItemPalette[i].image.Source = 
							ImageData.ImageSourceSelector(CategoryName.Item,"Apple");
						lstEditorItemPalette[i].label.Content = "りんご";
						size = new Vector(32, 32);
						break;
					case ItemName.BoarMeat:
						lstEditorItemPalette[i].image.Source =
							ImageData.ImageSourceSelector(CategoryName.Item, "BoarMeat");
						lstEditorItemPalette[i].label.Content = "いのししの肉";
						size = new Vector(32, 32);
						break;
					case ItemName.Coin:
						lstEditorItemPalette[i].image.Source =
							ImageData.ImageSourceSelector(CategoryName.Item, "Coin");
						lstEditorItemPalette[i].label.Content = "コイン";
						size = new Vector(32, 32);
						break;
					case ItemName.StarFruit:
						lstEditorItemPalette[i].image.Source =
							ImageData.ImageSourceSelector(CategoryName.Item, "StarFruit");
						lstEditorItemPalette[i].label.Content = "スターフルーツ";
						size = new Vector(32, 32);
						break;
					case ItemName.TreeBranch:
						lstEditorItemPalette[i].image.Source =
							ImageData.ImageSourceSelector(CategoryName.Item, "TreeBranch");
						lstEditorItemPalette[i].label.Content = "木の枝";
						size = new Vector(64, 32);
						break;
				}

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

			blockPaletteSelected = (int)((Image)sender).Tag;
	
			Canvas.SetLeft(imgPaletteCursor[0],pos.X); Canvas.SetTop(imgPaletteCursor[0], pos.Y);

			imgEditorPointerCursor.Source = (sender as Image).Source;
			imgEditorPointerCursor.Width = imgEditorPointerCursor.Height = 32;

			paletteMode = PaletteMode.Block;
		
		}

		private static void ObjectPaletteClick(object sender, RoutedEventArgs e)
		{

			Vector pos = VisualTreeHelper.GetOffset((Image)sender);

			objectPaletteSelected = (int)((Image)sender).Tag;

			Canvas.SetLeft(imgPaletteCursor[1], pos.X); Canvas.SetTop(imgPaletteCursor[1], pos.Y);

			imgEditorPointerCursor.Source = (sender as Image).Source;
			imgEditorPointerCursor.Width = (sender as Image).Width;
			imgEditorPointerCursor.Height = (sender as Image).Height;
			imgEditorPointerCursor.Stretch = Stretch.Fill;

			paletteMode = PaletteMode.Object;
		}

		private static void EnemyPaletteClick(object sender, RoutedEventArgs e)
		{

			Vector pos = VisualTreeHelper.GetOffset((Image)sender);

			enemyPaletteSelected = (EnemyName)Enum.Parse(typeof(EnemyName), ((Image)sender).Name);

			Canvas.SetLeft(imgPaletteCursor[2], pos.X); Canvas.SetTop(imgPaletteCursor[2], pos.Y);

			imgEditorPointerCursor.Source = (sender as Image).Source;
			imgEditorPointerCursor.Width = (sender as Image).Width;
			imgEditorPointerCursor.Height = (sender as Image).Height;

			paletteMode = PaletteMode.Enemy;
		}

		private static void ItemPaletteClick(object sender, RoutedEventArgs e)
		{

			Vector pos = VisualTreeHelper.GetOffset((Image)sender);

			itemPaletteSelected = (ItemName)Enum.Parse(typeof(ItemName), ((Image)sender).Name);

			Canvas.SetLeft(imgPaletteCursor[3], pos.X); Canvas.SetTop(imgPaletteCursor[3], pos.Y);

			imgEditorPointerCursor.Source = (sender as Image).Source;
			imgEditorPointerCursor.Width = (sender as Image).Width;
			imgEditorPointerCursor.Height = (sender as Image).Height;

			paletteMode = PaletteMode.Item;
		}

		//Setup Method
		//blocks setup MainCanvas
		public static void EditSetupBlockOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y-1) * 32) - 1;
			int blockX = (int)blockpos.X -1;
			int blockY = (int)blockpos.Y -1;
			Image _image = new Image();
			

			if(stageEditorData.editIndicateStage[index] == 0)
			{
				stageEditorData.editIndicateStage[index] = blockPaletteSelected;

				string spritename = StageData.lstDbBlock[blockPaletteSelected].sprite;

				_image = new Image
				{
					Source = ImageData.ImageSourceSelector(CategoryName.Block, spritename),
					Width = 32,
					Height = 32,
				};

				StageData.imgBlock[blockY, blockX] = _image;
				StageData.indicateStage[blockY, blockX] = blockPaletteSelected;

				if (stageEditorData.editIndicateStage[index] != 0)
				{
					
					MainWindow.mainCanvas.Children.Add(StageData.imgBlock[blockY, blockX]);
					Canvas.SetTop(StageData.imgBlock[blockY, blockX], blockY * 32);
					Canvas.SetLeft(StageData.imgBlock[blockY, blockX], blockX * 32);
					Canvas.SetZIndex(StageData.imgBlock[blockY, blockX], ImageZindex.block);
				}
			}
			
		}


		public static void EditRemoveBlockOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y - 1) * 32) - 1;
			int blockX = (int)blockpos.X - 1;
			int blockY = (int)blockpos.Y - 1;
		

			if (stageEditorData.editIndicateStage[index] != 0)
			{
				stageEditorData.editIndicateStage[index] = 0;
				MainWindow.mainCanvas.Children.Remove(StageData.imgBlock[blockY, blockX]);
				StageData.imgBlock[blockY, blockX] = null;
			}

		}

		//Object setup MainCanvas
		public static void EditSetupObjectOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y - 1) * 32) - 1;
			int blockX = (int)blockpos.X - 1;
			int blockY = (int)blockpos.Y - 1;
			bool checkflag = false;
			Image _image = new Image();

			for(int i = 0; i < stageEditorData.objectPosition.Length; i++)
			{
				if(blockX * 32 == stageEditorData.objectPosition[i].X &&
					blockY * 32 == stageEditorData.objectPosition[i].Y )
				{
					checkflag = true;
					break;
				}
			}

			if (!checkflag)
			{

				SystemOperator.EditorObjectDataListConverter(0,true);

				int arraylength =  stageEditorData.objectName.Length - 1;

				stageEditorData.objectName[arraylength] = StageData.lstDbObject[objectPaletteSelected].name;
				stageEditorData.objectPosition[arraylength] = new Vector(blockX*32, blockY*32);

				stageEditorData.objectSize[arraylength] = StageData.lstDbObject[objectPaletteSelected].size;
				stageEditorData.objectZindex[arraylength] = ImageZindex.object_back;

				ObjectChecker.lstObject.Add(new ObjectData {
					objName = stageEditorData.objectName[arraylength],
					position = stageEditorData.objectPosition[arraylength],
					size = stageEditorData.objectSize[arraylength],
					zindex = stageEditorData.objectZindex[arraylength],
					toggleSwitch = false,
					targetType = TargetType.Player,
					targetId = 0,
					talkID = 0,
				});

				int lstobjeIndex = ObjectChecker.lstObject.Count - 1;

				var _imgObject = new Image
				{
					Source = ImageData.ImageSourceSelector(CategoryName.Object,StageData.lstDbObject[objectPaletteSelected].spriteA),
					Width = ObjectChecker.lstObject[lstobjeIndex].size.X,
					Height = ObjectChecker.lstObject[lstobjeIndex].size.Y,
					Stretch = Stretch.Fill,
				};

				ObjectChecker.lstObject[ObjectChecker.lstObject.Count - 1].imgObject = _imgObject;
				
				MainWindow.mainCanvas.Children.Add(ObjectChecker.lstObject[lstobjeIndex].imgObject);
				Canvas.SetTop(ObjectChecker.lstObject[lstobjeIndex].imgObject, blockY * 32);
				Canvas.SetLeft(ObjectChecker.lstObject[lstobjeIndex].imgObject, blockX * 32);
				Canvas.SetZIndex(ObjectChecker.lstObject[lstobjeIndex].imgObject, ImageZindex.object_back);

				ListViewObjectDataUpdate();

			}
		}

		public static void EditRemoveObjectOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y - 1) * 32) - 1;
			int blockX = (int)blockpos.X - 1;
			int blockY = (int)blockpos.Y - 1;
			
			for (int i = 0; i < stageEditorData.objectPosition.Length; i++)
			{
				if (blockX * 32 == stageEditorData.objectPosition[i].X &&
					blockY * 32 == stageEditorData.objectPosition[i].Y)
				{
					SystemOperator.EditorObjectDataListConverter(i, false);

					MainWindow.mainCanvas.Children.Remove(ObjectChecker.lstObject[i].imgObject);
					ObjectChecker.lstObject.RemoveAt(i);
					ListViewObjectDataUpdate();
					break;
				}
			}
		}

		public static void ListViewObjectDataUpdate()
		{
			lstListViewObjectData.Clear();
			for (int i = 0; i < stageEditorData.objectName.Length; i++)
			{
				lstListViewObjectData.Add(new EditorListViewObjectData
				{
					id = i,
					objectName = stageEditorData.objectName[i].ToString(),
					objectPosition = stageEditorData.objectPosition[i],
					objectSize = stageEditorData.objectSize[i],
					objectZindex = stageEditorData.objectZindex[i],
					objectToggleSwitch = stageEditorData.objectToggleSwitch[i],
					objectTargetType = stageEditorData.objectTargetType[i],
					objectTargetId = stageEditorData.objectTargetId[i],
					objectTalkID = stageEditorData.objectTalkID[i],

				});
			}

			ctlListViewObject.ItemsSource = lstListViewObjectData;
			ctlListViewObject.Items.Refresh();
			ctlListViewObject.DataContext = lstListViewObjectData;
		}

		//Enemy setup MainCanvas
		public static void EditSetupEnemyOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y - 1) * 32) - 1;
			int blockX = (int)blockpos.X - 1;
			int blockY = (int)blockpos.Y - 1;
			bool checkflag = false;
			Image _image = new Image();

			for (int i = 0; i < stageEditorData.enemyPosition.Length; i++)
			{
				if (blockX * 32 == stageEditorData.enemyPosition[i].X &&
					blockY * 32 == stageEditorData.enemyPosition[i].Y)
				{
					checkflag = true;
					break;
				}
			}

			if (!checkflag)
			{

				SystemOperator.EditorEnemyDataListConverter(0, true);

				int arraylength = stageEditorData.enemyName.Length - 1;

				stageEditorData.enemyName[arraylength] = enemyPaletteSelected;
				stageEditorData.enemyPosition[arraylength] = new Vector(blockX * 32, blockY * 32);
				stageEditorData.enemyDirection[arraylength] = false;

				SpawnEnemy.lstEnemyData.Add(SpawnEnemy.SetEnemyData(
						stageEditorData.enemyName[arraylength], stageEditorData.enemyPosition[arraylength],
						stageEditorData.enemyDirection[arraylength]));

				int lstEnemyIndex = SpawnEnemy.lstEnemyData.Count - 1;

				MainWindow.mainCanvas.Children.Add(SpawnEnemy.lstEnemyData[lstEnemyIndex].imgEnemy);
				Canvas.SetTop(SpawnEnemy.lstEnemyData[lstEnemyIndex].imgEnemy, blockY * 32);
				Canvas.SetLeft(SpawnEnemy.lstEnemyData[lstEnemyIndex].imgEnemy, blockX * 32);
				Canvas.SetZIndex(SpawnEnemy.lstEnemyData[lstEnemyIndex].imgEnemy, ImageZindex.enemy);

				ListViewEnemyDataUpdate();

			}
		}

		public static void EditRemoveEnemyOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y - 1) * 32) - 1;
			int blockX = (int)blockpos.X - 1;
			int blockY = (int)blockpos.Y - 1;

			for (int i = 0; i < stageEditorData.enemyPosition.Length; i++)
			{
				if (blockX * 32 == stageEditorData.enemyPosition[i].X &&
					blockY * 32 == stageEditorData.enemyPosition[i].Y)
				{
					SystemOperator.EditorEnemyDataListConverter(i, false);

					MainWindow.mainCanvas.Children.Remove(SpawnEnemy.lstEnemyData[i].imgEnemy);
					SpawnEnemy.lstEnemyData.RemoveAt(i);
					ListViewEnemyDataUpdate();
					break;
				}
			}
		}

		public static void ListViewEnemyDataUpdate()
		{
			lstListViewEnemyData.Clear();
			for (int i = 0; i < stageEditorData.enemyName.Length; i++)
			{
				lstListViewEnemyData.Add(new EditorListViewEnemyData
				{
					id = i,
					enemyName = stageEditorData.enemyName[i].ToString(),
					enemyPosition = stageEditorData.enemyPosition[i],
					enemyDirection = stageEditorData.enemyDirection[i],

				});
			}

			ctlListViewEnemy.ItemsSource = lstListViewEnemyData;
			ctlListViewEnemy.Items.Refresh();
			ctlListViewEnemy.DataContext = lstListViewEnemyData;
		}

		//Item setup MainCanvas
		public static void EditSetupItemOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y - 1) * 32) - 1;
			int blockX = (int)blockpos.X - 1;
			int blockY = (int)blockpos.Y - 1;
			bool checkflag = false;
			Image _image = new Image();

			for (int i = 0; i < stageEditorData.itemPosition.Length; i++)
			{
				if (blockX * 32 == stageEditorData.itemPosition[i].X &&
					blockY * 32 == stageEditorData.itemPosition[i].Y)
				{
					checkflag = true;
					break;
				}
			}

			if (!checkflag)
			{

				SystemOperator.EditorItemDataListConverter(0, true);

				int arraylength = stageEditorData.itemName.Length - 1;

				stageEditorData.itemName[arraylength] = itemPaletteSelected;
				stageEditorData.itemPosition[arraylength] = new Vector(blockX * 32, blockY * 32);

				Item.lstItemData.Add(Item.SetItemData(
						stageEditorData.itemName[arraylength], stageEditorData.itemPosition[arraylength]));

				int lstItemIndex = Item.lstItemData.Count - 1;

				MainWindow.mainCanvas.Children.Add(Item.lstItemData[lstItemIndex].imgItem);
				Canvas.SetTop(Item.lstItemData[lstItemIndex].imgItem, blockY * 32);
				Canvas.SetLeft(Item.lstItemData[lstItemIndex].imgItem, blockX * 32);
				Canvas.SetZIndex(Item.lstItemData[lstItemIndex].imgItem, ImageZindex.item);

				ListViewItemDataUpdate();

			}
		}

		public static void EditRemoveItemOnMainCanvas(Vector blockpos)
		{
			int index = (int)(blockpos.X + (blockpos.Y - 1) * 32) - 1;
			int blockX = (int)blockpos.X - 1;
			int blockY = (int)blockpos.Y - 1;

			for (int i = 0; i < stageEditorData.itemPosition.Length; i++)
			{
				if (blockX * 32 == stageEditorData.itemPosition[i].X &&
					blockY * 32 == stageEditorData.itemPosition[i].Y)
				{
					SystemOperator.EditorItemDataListConverter(i, false);

					MainWindow.mainCanvas.Children.Remove(Item.lstItemData[i].imgItem);
					Item.lstItemData.RemoveAt(i);
					ListViewItemDataUpdate();
					break;
				}
			}
		}

		public static void ListViewItemDataUpdate()
		{
			lstListViewItemData.Clear();
			for (int i = 0; i < stageEditorData.itemName.Length; i++)
			{
				lstListViewItemData.Add(new EditorListViewItemData
				{
					id = i,
					itemName = stageEditorData.itemName[i].ToString(),
					itemPosition = stageEditorData.itemPosition[i],

				});
			}

			ctlListViewItem.ItemsSource = lstListViewItemData;
			ctlListViewItem.Items.Refresh();
			ctlListViewItem.DataContext = lstListViewItemData;
		}
	}

}
