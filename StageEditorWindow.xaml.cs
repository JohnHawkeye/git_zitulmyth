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
using System.Media;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using Zitulmyth.Enums;
using Zitulmyth.Data;
using static Zitulmyth.StageEditorOperator;


namespace Zitulmyth
{
	[DataContract]
	public class StageEditorData
	{
		public int ID;

		[DataMember]
		public Vector editPlayerStartPosition;

		[DataMember]
		public string scenemyName;
		[DataMember]
		public List<StageClearCondition> lstEditClearCondition = new List<StageClearCondition>();
		[DataMember]
		public bool editRespqwnEnemy;

		[DataMember]
		public int[] editIndicateStage = new int[768];

		[DataMember]
		public string[] objectName;
		[DataMember]
		public Vector[] objectPosition;
		[DataMember]
		public Vector[] objectSize;
		[DataMember]
		public int[] objectZindex;
		[DataMember]
		public bool[] objectToggleSwitch;
		[DataMember]
		public TargetType[] objectTargetType;
		[DataMember]
		public int[] objectTargetId;
		[DataMember]
		public int[] objectTalkID;

		[DataMember]
		public EnemyName[] enemyName;
		[DataMember]
		public Vector[] enemyPosition;
		[DataMember]
		public bool[] enemyDirection;

		[DataMember]
		public ItemName[] itemName;
		[DataMember]
		public Vector[] itemPosition;
	}

	public partial class StageEditorWindow : Window
	{

		public StageOrderWindow stageOrderWindow;

		public static StageEditorData stageEditorData = new StageEditorData();

		public static Grid ctlGridMain;
		public static TextBox ctlTextPlayerPosX;
		public static TextBox ctlTextPlayerPosY;
		public static Canvas ctlCanvasBlockPalette;
		public static Canvas ctlCanvasObjectPalette;
		public static Canvas ctlCanvasEnemyPalette;
		public static Canvas ctlCanvasItemPalette;
		public static ListView ctlListViewObject;
		public static ListView ctlListViewEnemy;
		public static ListView ctlListViewItem;

		private List<string> lstSceneryName;

		private string[] strClearConditionName;
		private string[] strTargetType;
		
		public StageEditorWindow()
		{
			InitializeComponent();

		}

		

		private void tbcEditSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			paletteMode = PaletteMode.None;
		}

		private void btnStageNumDecrease_Click(object sender, RoutedEventArgs e)
		{
			if (StageManager.stageNum > 0)
			{
				StageManager.stageNum--;
				StageLoad();

				StageEditorDataSetting();
				EditorPlayerPaletteSetting();
			}
		}

		private void btnStageNumIncrease_Click(object sender, RoutedEventArgs e)
		{

			if ( StageManager.stageNum < StageOrder.lstStageOrder.Count - 1)
			{
				StageManager.stageNum++;
				StageLoad();

				StageEditorDataSetting();
				EditorPlayerPaletteSetting();
			}

		}

		public void CreateNewStageFile(StageEditorData sed)
		{

			sed.editPlayerStartPosition = new Vector(0,0);
			sed.editRespqwnEnemy = false;
			
			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					sed.editIndicateStage[i * 32 + j] = 0;
				}
			}

			sed.objectName = new string[] { };
			sed.objectPosition = new Vector[] { };
			sed.objectSize = new Vector[] { };
			sed.objectZindex = new int[] { };
			sed.objectToggleSwitch = new bool[] { };
			sed.objectTargetType = new TargetType[] { };
			sed.objectTargetId = new int[] { };
			sed.objectTalkID = new int[] { };

			sed.enemyName = new EnemyName[] { };
			sed.enemyPosition = new Vector[] { };
			sed.enemyDirection = new bool[] { };
					   		
			sed.itemName = new ItemName[] { };
			sed.itemPosition = new Vector[] { };

		}

		public void StageLoad()
		{
			//remove
			StageInit.StageBlockRemove(MainWindow.mainCanvas);
			StageInit.StageObjectsRemove(MainWindow.mainCanvas);
			StageInit.StageEnemyRemove(MainWindow.mainCanvas);
			StageInit.StageItemRemove(MainWindow.mainCanvas);

			StageManager.lstClearCondition.Clear();
			
			StageInit.InitBlockData();
			
			//StageData loding of JSONfile.
			StageDataSetting.SetData();
			txkStageName.Text = StageOrder.lstStageOrder[StageManager.stageNum].name;

			StageInit.StageBlockSet(MainWindow.mainCanvas);
			StageManager.StageObjectsSetting(MainWindow.mainCanvas);
		}

		private void btnJsonWrite_Click(object sender, RoutedEventArgs e)
		{

			MessageBoxResult result =
				MessageBox.Show("ステージ [ " + StageOrder.lstStageOrder[StageManager.stageNum].name + " ] のデータを上書きします。\nよろしいですか？", "データの上書き",
				MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(StageEditorData));

				FileStream fs = new FileStream("Assets/json/stage/" + StageOrder.lstStageOrder[StageManager.stageNum].stageFileName, FileMode.Create);

				try
				{
					json.WriteObject(fs, stageEditorData);
				}
				finally
				{
					fs.Close();
				}
			}
		}

		private void StageEditorWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ImageData.SpriteReading();

			ctlGridMain = grdMain;
			ctlCanvasBlockPalette = cavBlockPalette;
			ctlCanvasObjectPalette = cavObjectPalette;
			ctlCanvasEnemyPalette = cavEnemyPalette;
			ctlCanvasItemPalette = cavItemPalette;
			ctlTextPlayerPosX = txbPlayerStartX;
			ctlTextPlayerPosY = txbPlayerStartY;

			ctlListViewObject = lsvObjectList;
			ctlListViewEnemy = lsvEnemyList;
			ctlListViewItem = lsvItemList;

			strClearConditionName = Enum.GetNames(typeof(StageClearConditionName));
			strTargetType = Enum.GetNames(typeof(TargetType));
			
			lstSceneryName = ImageData.PatternNameListCreating(CategoryName.Scenery);
			cmbSceneryName.ItemsSource = lstSceneryName;

			StageManager.stageNum = 0;
			StageLoad();
			StageEditorDataSetting();

			EditorImageDataSetting();

			EditorPlayerPaletteSetting();
			EditorBlockPaletteSetting();
			EditorObjectPaletteSetting();
			EditorEnemyPaletteSetting();
			EditorItemPaletteSetting();
			
		}

		public void StageEditorDataSetting()
		{

			stageEditorData.editPlayerStartPosition = StageData.startPlayerPosition;
			txbPlayerStartX.Text = stageEditorData.editPlayerStartPosition.X.ToString();
			txbPlayerStartY.Text = stageEditorData.editPlayerStartPosition.Y.ToString();

			stageEditorData.scenemyName = StageData.sceneryImageName;

			if(stageEditorData.scenemyName == null)
			{
				cmbSceneryName.SelectedIndex = 0;
			}
			else
			{
				cmbSceneryName.SelectedItem = StageData.sceneryImageName;
			}


			stageEditorData.lstEditClearCondition = StageManager.lstClearCondition;
			ListViewClearConditionUpdate();


			stageEditorData.editRespqwnEnemy = StageManager.respawnEnemy;
			ckbEnemyRespawn.IsChecked = (bool)stageEditorData.editRespqwnEnemy;

			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					stageEditorData.editIndicateStage[i*32+ j] = StageData.indicateStage[i, j];
				}
			}

			stageEditorData.objectName = new string[ObjectChecker.lstObject.Count];
			stageEditorData.objectPosition = new Vector[ObjectChecker.lstObject.Count];
			stageEditorData.objectSize = new Vector[ObjectChecker.lstObject.Count];
			stageEditorData.objectZindex = new int[ObjectChecker.lstObject.Count];
			stageEditorData.objectToggleSwitch = new bool[ObjectChecker.lstObject.Count];
			stageEditorData.objectTargetType = new TargetType[ObjectChecker.lstObject.Count];
			stageEditorData.objectTargetId = new int[ObjectChecker.lstObject.Count];
			stageEditorData.objectTalkID = new int[ObjectChecker.lstObject.Count];

			for (int i = 0; i < ObjectChecker.lstObject.Count; i++)
			{
				stageEditorData.objectName[i] = ObjectChecker.lstObject[i].objName;
				stageEditorData.objectPosition[i] = ObjectChecker.lstObject[i].position;
				stageEditorData.objectSize[i] = ObjectChecker.lstObject[i].size;
				stageEditorData.objectZindex[i] = ObjectChecker.lstObject[i].zindex;
				stageEditorData.objectToggleSwitch[i] = ObjectChecker.lstObject[i].toggleSwitch;
				stageEditorData.objectTargetType[i] = ObjectChecker.lstObject[i].targetType;
				stageEditorData.objectTargetId[i] = ObjectChecker.lstObject[i].targetId;
				stageEditorData.objectTalkID[i] = ObjectChecker.lstObject[i].talkID;
			}

			ListViewObjectDataUpdate();

			stageEditorData.enemyName = new EnemyName[SpawnEnemy.lstEnemyData.Count];
			stageEditorData.enemyPosition = new Vector[SpawnEnemy.lstEnemyData.Count];
			stageEditorData.enemyDirection = new bool[SpawnEnemy.lstEnemyData.Count];

			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{
				stageEditorData.enemyName[i] = SpawnEnemy.lstEnemyData[i].name;
				stageEditorData.enemyPosition[i] = SpawnEnemy.lstEnemyData[i].position;
				stageEditorData.enemyDirection[i] = SpawnEnemy.lstEnemyData[i].direction;
			}

			ListViewEnemyDataUpdate();

			stageEditorData.itemName = new ItemName[Item.lstItemData.Count];
			stageEditorData.itemPosition = new Vector[Item.lstItemData.Count];

			for (int i = 0; i < Item.lstItemData.Count; i++)
			{
				stageEditorData.itemName[i] = Item.lstItemData[i].itemName;
				stageEditorData.itemPosition[i] = Item.lstItemData[i].position;
			}

			ListViewItemDataUpdate();
		}

		private void ListViewClearConditionUpdate()
		{
			lstListViewClearConditions.Clear();

			for (int i = 0; i < stageEditorData.lstEditClearCondition.Count; i++)
			{
				lstListViewClearConditions.Add(new EditorListViewClearCondition
				{

					id = i,
					targetName = stageEditorData.lstEditClearCondition[i].conditionName.ToString(),
					targetKillNum = stageEditorData.lstEditClearCondition[i].targetNumKill,
					targetReach = stageEditorData.lstEditClearCondition[i].isReach,
					targetVector = stageEditorData.lstEditClearCondition[i].targetPoint,
					targetTalkFlag = stageEditorData.lstEditClearCondition[i].targetTalkFlag,
					targetTime = stageEditorData.lstEditClearCondition[i].targetTime

				});
			}

			lsvClearCondition.ItemsSource = lstListViewClearConditions;
			lsvClearCondition.Items.Refresh();
			lsvClearCondition.DataContext = lstListViewClearConditions;
		}

		private void txbPlayerStartX_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void txbPlayerStartY_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void btnStartPositionClickSet_Click(object sender, RoutedEventArgs e)
		{
			memoryPlayerStartPos = new Vector(int.Parse(ctlTextPlayerPosX.Text), int.Parse(ctlTextPlayerPosY.Text));
			paletteMode = PaletteMode.Player;
			grdMain.IsEnabled = false;
			imgEditorPlayer.Opacity = 0.5;
		}

		private void cavBlockPalette_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			for(int i= 0; i < lstEditorBlockPalette.Count; i++)
			{
				if(i == 0)
				{
					Canvas.SetLeft(imgPaletteCursor[0], Canvas.GetLeft(lstEditorBlockPalette[i].image));
					Canvas.SetTop(imgPaletteCursor[0], Canvas.GetTop(lstEditorBlockPalette[i].image));
					break;
				}
			}

			blockPaletteSelected = 0;

		}

		private void ckbEnemyRespawn_Checked(object sender, RoutedEventArgs e)
		{
			stageEditorData.editRespqwnEnemy = true;
		}

		private void ckbEnemyRespawn_Unchecked(object sender, RoutedEventArgs e)
		{
			stageEditorData.editRespqwnEnemy = false;
		}

		private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void textBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void textBox3_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void textBox4_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void btnSettingAdd_Click(object sender, RoutedEventArgs e)
		{
			stageEditorData.lstEditClearCondition.Add(new StageClearCondition { });
			ListViewClearConditionUpdate();
		}

		private void btnSettingRemove_Click(object sender, RoutedEventArgs e)
		{
			if (lsvClearCondition.SelectedIndex >= 0)
			{
				stageEditorData.lstEditClearCondition.RemoveAt(lsvClearCondition.SelectedIndex);
				ListViewClearConditionUpdate();
			}
			
		}

		private void lsvClearCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int row = lsvClearCondition.SelectedIndex;

			if (row >= 0)
			{
				cmbClearConditionName.ItemsSource = strClearConditionName;
				cmbClearConditionName.SelectedItem = stageEditorData.lstEditClearCondition[row].conditionName.ToString();
				txbKillNum.Text = stageEditorData.lstEditClearCondition[row].targetNumKill.ToString();
				txbTargetPosX.Text = stageEditorData.lstEditClearCondition[row].targetPoint.X.ToString();
				txbTargetPosY.Text = stageEditorData.lstEditClearCondition[row].targetPoint.Y.ToString();
				txbTalkFlagId.Text = stageEditorData.lstEditClearCondition[row].targetTalkFlag.ToString();
				txbTime.Text = stageEditorData.lstEditClearCondition[row].targetTime.ToString();
			}

		}

		private void btnSettingUpdate_Click(object sender, RoutedEventArgs e)
		{
			int row = lsvClearCondition.SelectedIndex;

			if(row >= 0)
			{
				stageEditorData.lstEditClearCondition[row].conditionName = 
					(StageClearConditionName)Enum.Parse(typeof(StageClearConditionName),cmbClearConditionName.SelectedItem.ToString());

				stageEditorData.lstEditClearCondition[row].targetNumKill = int.Parse(txbKillNum.Text);
				stageEditorData.lstEditClearCondition[row].targetPoint = new Vector(int.Parse(txbTargetPosX.Text),int.Parse(txbTargetPosY.Text));
				stageEditorData.lstEditClearCondition[row].targetTalkFlag = int.Parse(txbTalkFlagId.Text);
				stageEditorData.lstEditClearCondition[row].targetTime = int.Parse(txbTime.Text);

				ListViewClearConditionUpdate();

			}
		}

		private void lsvObjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int row = lsvObjectList.SelectedIndex;

			if (row >= 0)
			{
				grbObjectOption.IsEnabled = true;

				if(stageEditorData.objectZindex[row] == 5)
				{
					rdbZindexBack.IsChecked = true;
				}
				else
				{
					rdbZindexFront.IsChecked = true;	//11
				}

				cmbObjectTargetType.ItemsSource = strTargetType;
				cmbObjectTargetType.SelectedItem = stageEditorData.objectTargetType[row].ToString();

				txbObjectTalkID.Text = stageEditorData.objectTalkID[row].ToString();

			}
			else
			{
				grbObjectOption.IsEnabled = false;
			}
		}

		private void txbObjectTalkID_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void btnObjectUpdate_Click(object sender, RoutedEventArgs e)
		{
			int row = lsvObjectList.SelectedIndex;

			if (row >= 0)
			{
				if (rdbZindexBack.IsChecked == true)
				{
					stageEditorData.objectZindex[row] = 5;
				}
				else
				{
					stageEditorData.objectZindex[row] = 11;
				}

				stageEditorData.objectTargetType[row] =
					(TargetType)Enum.Parse(typeof(TargetType), cmbObjectTargetType.SelectedItem.ToString());

				stageEditorData.objectTalkID[row] = int.Parse(txbObjectTalkID.Text);

				ListViewObjectDataUpdate();

			}
		}

		private void btnEventWindowOpen_Click(object sender, RoutedEventArgs e)
		{

			paletteMode = PaletteMode.None;

			MainWindow.stageEditor.IsEnabled = false;
			MainWindow.isOpenEventEditorWindow = true;

			MainWindow.eventEditor = new EventEditorWindow();
			MainWindow.eventEditor.ShowDialog();

		}

		private void cmbSceneryName_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
		
			stageEditorData.scenemyName = cmbSceneryName.SelectedItem.ToString();
		}

		private void btnStageOrder_Click(object sender, RoutedEventArgs e)
		{
			stageOrderWindow = new StageOrderWindow();
			stageOrderWindow.ShowDialog();
		}

		private void StageEditorWindow1_Closed(object sender, EventArgs e)
		{
			EditorImagesRemove();
			MainWindow.isOpenStageEditorWindow = false;
			MainWindow.ctlDatabaseButton.IsEnabled = true;
			MainWindow.ctlImageButton.IsEnabled = true;
			MainWindow.ctlMaterialButton.IsEnabled = true;
		}
	}
}
