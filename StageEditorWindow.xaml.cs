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
		public List<StageClearCondition> lstEditClearCondition = new List<StageClearCondition>();

		[DataMember]
		public bool editRespqwnEnemy;

		[DataMember]
		public BlockType[] editIndicateStage = new BlockType[768];

		[DataMember]
		public ObjectName[] objectName;
		[DataMember]
		public Vector[] objectPosition;
		[DataMember]
		public int[] objectWidth;
		[DataMember]
		public int[] objectHeight;
		[DataMember]
		public int[] objectZindex;
		[DataMember]
		public bool[] objectTriggerAction;
		[DataMember]
		public ObjectName[] objectTriggerTarget;
		[DataMember]
		public bool[] objectTriggerType;
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

		private string[] strClearConditionName;
		private string[] strObjectName;

		public StageEditorWindow()
		{
			InitializeComponent();
		}

		private void StageEditorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!MainWindow.closeMainWindow)
			{
				e.Cancel = true;
				this.Visibility = Visibility.Hidden;
			}
			else
			{
				e.Cancel = false;
			}
		}

		private void tbcEditSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			paletteMode = PaletteMode.None;
		}

		private void btnStageNumDecrease_Click(object sender, RoutedEventArgs e)
		{
			int targetStageNum = int.Parse(txbStageNum.Text);

			if (targetStageNum > 0)
			{
				targetStageNum--;
				txbStageNum.Text = targetStageNum.ToString();

				StageLoad();
				StageEditorDataSetting();

				EditorPlayerPaletteSetting();
			}
		}

		private void btnStageNumIncrease_Click(object sender, RoutedEventArgs e)
		{
			int targetStageNum = int.Parse(txbStageNum.Text);

			string fileName = "Assets/json/stage/stage" + (targetStageNum + 1).ToString() + ".json";

			if (File.Exists(fileName))
			{
				targetStageNum++;
				txbStageNum.Text = targetStageNum.ToString();

				StageLoad();
				StageEditorDataSetting();

				EditorPlayerPaletteSetting();
			}
			else
			{
				MessageBoxResult result =
				MessageBox.Show("ステージ" + (targetStageNum + 1).ToString() + "番のファイルがありません。\n新規ステージを作成しますか？", "ファイルの確認",
				MessageBoxButton.YesNo, MessageBoxImage.Information);

				if (result == MessageBoxResult.Yes)
				{
					StageEditorData createNewStageFile = new StageEditorData();

					DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<StageEditorData>));

					FileStream fs = new FileStream("Assets/json/stage/stage" + (targetStageNum + 1).ToString() + ".json", FileMode.Create);

					try
					{
						CreateNewStageFile(createNewStageFile);

						json.WriteObject(fs, createNewStageFile);

						
					}
					finally
					{
						fs.Close();
					}

					if (File.Exists(fileName))
					{
						targetStageNum++;
						txbStageNum.Text = targetStageNum.ToString();

						StageLoad();
						StageEditorDataSetting();

						EditorPlayerPaletteSetting();
					}
				}
			}

		}

		private void CreateNewStageFile(StageEditorData sed)
		{

			sed.editPlayerStartPosition = new Vector(0,0);
			sed.editRespqwnEnemy = false;
			
			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					sed.editIndicateStage[i * 32 + j] = BlockType.None;
				}
			}

			sed.objectName = new ObjectName[] { };
			sed.objectPosition = new Vector[] { };
			sed.objectWidth = new int[] { };
			sed.objectHeight = new int[] { };
			sed.objectZindex = new int[] { };
			sed.objectTriggerAction = new bool[] { };
			sed.objectTriggerTarget = new ObjectName[] { };
			sed.objectTriggerType = new bool[] { };
			sed.objectTalkID = new int[] { };

			sed.enemyName = new EnemyName[] { };
			sed.enemyPosition = new Vector[] { };
			sed.enemyDirection = new bool[] { };
					   		
			sed.itemName = new ItemName[] { };
			sed.itemPosition = new Vector[] { };

		}

		private void StageLoad()
		{
			//remove
			StageInit.StageBlockRemove(MainWindow.mainCanvas);
			StageInit.StageObjectsRemove(MainWindow.mainCanvas);
			StageInit.StageEnemyRemove(MainWindow.mainCanvas);
			StageInit.StageItemRemove(MainWindow.mainCanvas);

			StageManager.lstClearCondition.Clear();

			//init

			StageManager.stageNum = int.Parse(txbStageNum.Text);

			ImageData.ImageLoadAfterSecond();

			StageInit.InitBlockData();
			StageDataSetting.SetData();

			StageInit.StageBlockSet(MainWindow.mainCanvas);
			StageManager.StageObjectsSetting(MainWindow.mainCanvas);
		}

		private void btnJsonWrite_Click(object sender, RoutedEventArgs e)
		{

			MessageBoxResult result =
				MessageBox.Show("ステージ" + StageManager.stageNum.ToString() + "番のデータを上書きします。\nよろしいですか？", "データの上書き",
				MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<StageEditorData>));

				FileStream fs = new FileStream("Assets/json/stage/stage" + StageManager.stageNum.ToString() + ".json", FileMode.Create);

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
			strObjectName = Enum.GetNames(typeof(ObjectName));

			ImageData.ImageLoadEditorMode();


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

		private void StageEditorDataSetting()
		{

			stageEditorData.editPlayerStartPosition = StageData.startPlayerPosition;
			txbPlayerStartX.Text = stageEditorData.editPlayerStartPosition.X.ToString();
			txbPlayerStartY.Text = stageEditorData.editPlayerStartPosition.Y.ToString();

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

			stageEditorData.objectName = new ObjectName[ObjectChecker.lstObject.Count];
			stageEditorData.objectPosition = new Vector[ObjectChecker.lstObject.Count];
			stageEditorData.objectWidth = new int[ObjectChecker.lstObject.Count];
			stageEditorData.objectHeight = new int[ObjectChecker.lstObject.Count];
			stageEditorData.objectZindex = new int[ObjectChecker.lstObject.Count];
			stageEditorData.objectTriggerAction = new bool[ObjectChecker.lstObject.Count];
			stageEditorData.objectTriggerTarget = new ObjectName[ObjectChecker.lstObject.Count];
			stageEditorData.objectTriggerType = new bool[ObjectChecker.lstObject.Count];
			stageEditorData.objectTalkID = new int[ObjectChecker.lstObject.Count];

			for (int i = 0; i < ObjectChecker.lstObject.Count; i++)
			{
				stageEditorData.objectName[i] = ObjectChecker.lstObject[i].objName;
				stageEditorData.objectPosition[i] = ObjectChecker.lstObject[i].position;
				stageEditorData.objectWidth[i] = ObjectChecker.lstObject[i].width;
				stageEditorData.objectHeight[i] = ObjectChecker.lstObject[i].height;
				stageEditorData.objectZindex[i] = ObjectChecker.lstObject[i].zindex;
				stageEditorData.objectTriggerAction[i] = ObjectChecker.lstObject[i].triggerAction;
				stageEditorData.objectTriggerTarget[i] = ObjectChecker.lstObject[i].triggerTarget;
				stageEditorData.objectTriggerType[i] = ObjectChecker.lstObject[i].triggerType;
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
				if(lstEditorBlockPalette[i].type == BlockType.None)
				{;

					Canvas.SetLeft(imgPaletteCursor[0], Canvas.GetLeft(lstEditorBlockPalette[i].image));
					Canvas.SetTop(imgPaletteCursor[0], Canvas.GetTop(lstEditorBlockPalette[i].image));
					break;
				}
			}

			blockPaletteSelected = BlockType.None;

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
				if(stageEditorData.objectZindex[row] == 5)
				{
					rdbZindexBack.IsChecked = true;
				}
				else
				{
					rdbZindexFront.IsChecked = true;	//11
				}

				cmbObjectTargetName.ItemsSource = strObjectName;
				cmbObjectTargetName.SelectedItem = stageEditorData.objectTriggerTarget[row].ToString();

				ckbAction.IsChecked = (bool)stageEditorData.objectTriggerAction[row];
				ckbTriggerType.IsChecked = (bool)stageEditorData.objectTriggerType[row];
				txbObjectTalkID.Text = stageEditorData.objectTalkID[row].ToString();
				
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

				stageEditorData.objectTriggerTarget[row] =
					(ObjectName)Enum.Parse(typeof(ObjectName), cmbObjectTargetName.SelectedItem.ToString());

				stageEditorData.objectTriggerAction[row] = (bool)ckbAction.IsChecked;
				stageEditorData.objectTriggerType[row] = (bool)ckbTriggerType.IsChecked;
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
			MainWindow.eventEditor.Show();
			MainWindow.eventEditor.Focus();

		}
	}
}
