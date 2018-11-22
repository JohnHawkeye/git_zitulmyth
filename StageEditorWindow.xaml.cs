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

			if (targetStageNum < 3)
			{
				targetStageNum++;
				txbStageNum.Text = targetStageNum.ToString();

				StageLoad();
				StageEditorDataSetting();

				EditorPlayerPaletteSetting();
			}
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
				MessageBox.Show("ステージ" + StageManager.stageNum.ToString() + "番のデータを保存しますか？", "JSONファイルの書き込み",
				MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<StageEditorData>));

				FileStream fs = new FileStream("json/stage/stage" + StageManager.stageNum.ToString() + ".json", FileMode.Create);
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
			StageEditorOperator.lstEditorClearConditions.Clear();

			for (int i = 0; i<stageEditorData.lstEditClearCondition.Count;i++)
			{
				StageEditorOperator.lstEditorClearConditions.Add(new EditorClearCondition {

					id= i,
					targetName = stageEditorData.lstEditClearCondition[i].conditionName.ToString(),

					targetKillNum = stageEditorData.lstEditClearCondition[i].targetNumKill,
					targetReach = stageEditorData.lstEditClearCondition[i].isReach,
					targetVector = stageEditorData.lstEditClearCondition[i].targetPoint,
					targetTalkFlag = stageEditorData.lstEditClearCondition[i].targetTalkFlag,
					targetTime = stageEditorData.lstEditClearCondition[i].targetTime});
			}

			lsvClearCondition.ItemsSource = StageEditorOperator.lstEditorClearConditions;
			lsvClearCondition.Items.Refresh();
			lsvClearCondition.DataContext = StageEditorOperator.lstEditorClearConditions;

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


			stageEditorData.enemyName = new EnemyName[SpawnEnemy.lstEnemyData.Count];
			stageEditorData.enemyPosition = new Vector[SpawnEnemy.lstEnemyData.Count];
			stageEditorData.enemyDirection = new bool[SpawnEnemy.lstEnemyData.Count];

			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{
				stageEditorData.enemyName[i] = SpawnEnemy.lstEnemyData[i].name;
				stageEditorData.enemyPosition[i] = SpawnEnemy.lstEnemyData[i].position;
				stageEditorData.enemyDirection[i] = SpawnEnemy.lstEnemyData[i].direction;
			}

			stageEditorData.itemName = new ItemName[Item.lstItemData.Count];
			stageEditorData.itemPosition = new Vector[Item.lstItemData.Count];

			for (int i = 0; i < Item.lstItemData.Count; i++)
			{
				stageEditorData.itemName[i] = Item.lstItemData[i].itemName;
				stageEditorData.itemPosition[i] = Item.lstItemData[i].position;
			}
			
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
	}
}
