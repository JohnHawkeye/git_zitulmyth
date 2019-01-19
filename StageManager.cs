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

	public enum StageClearConditionName
	{
		NumEnemiesKilled,
		ReachTheTartgetPoint,
		TalkFlag,
		TimeElapsed,
	}

	public class StageOrderList
	{
		public int id;
		public string name;
		public string stageFileName;
		public string eventFileName;
	}

	public class StageClearCondition
	{
		public StageClearConditionName conditionName;
		public int targetNumKill;
		public bool isReach;
		public Vector targetPoint;
		public int targetTalkFlag;
		public int targetTime;
	}

	public class StageManager
	{

		public static List<StageClearCondition> lstClearCondition = new List<StageClearCondition>();

		public static int stageNum = 0;

		public  static int interiorZindex = 1;
		public static int furnitureZindex = 2;
		public static int npcZindex = 3;

		public static int numKillEnemy = 0;
		public static bool respawnEnemy;

		public static void StageObjectsSetting(Canvas canvas)
		{
	
			Canvas.SetLeft(ImageData.imgPlayer,StageData.startPlayerPosition.X);
			Canvas.SetTop(ImageData.imgPlayer, StageData.startPlayerPosition.Y);

			StageData.imgScenery = new Image { Source = ImageData.ImageSourceSelector(CategoryName.Scenery,StageData.sceneryImageName), Width = 1024, Height = 768, };

			canvas.Children.Add(StageData.imgScenery);
			Canvas.SetLeft(StageData.imgScenery, 0);
			Canvas.SetTop(StageData.imgScenery, 0);
			Canvas.SetZIndex(StageData.imgScenery, ImageZindex.scenery);


			for (int i = 0; i < ObjectChecker.lstObject.Count; i++)
			{
				var _imgObject = new Image
				{
					Source = ImageData.ImageSourceSelector(CategoryName.Object,ObjectChecker.lstObject[i].objName),
					Width = ObjectChecker.lstObject[i].size.X,
					Height = ObjectChecker.lstObject[i].size.Y,
					Stretch = Stretch.Fill,
				};

				ObjectChecker.lstObject[i].imgObject = _imgObject;
				canvas.Children.Add(ObjectChecker.lstObject[i].imgObject);
				Canvas.SetLeft(ObjectChecker.lstObject[i].imgObject, ObjectChecker.lstObject[i].position.X);
				Canvas.SetTop(ObjectChecker.lstObject[i].imgObject, ObjectChecker.lstObject[i].position.Y);
				Canvas.SetZIndex(ObjectChecker.lstObject[i].imgObject, ObjectChecker.lstObject[i].zindex);
			}

			for (int i = 0; i < Item.lstItemData.Count; i++)
			{
				canvas.Children.Add(Item.lstItemData[i].imgItem);
				Canvas.SetLeft(Item.lstItemData[i].imgItem, Item.lstItemData[i].position.X);
				Canvas.SetTop(Item.lstItemData[i].imgItem, Item.lstItemData[i].position.Y);
				Canvas.SetZIndex(Item.lstItemData[i].imgItem, ImageZindex.item);
			}

			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{

				canvas.Children.Add(SpawnEnemy.lstEnemyData[i].imgEnemy);
				Canvas.SetLeft(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.X);
				Canvas.SetTop(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.Y);
				Canvas.SetZIndex(SpawnEnemy.lstEnemyData[i].imgEnemy,ImageZindex.enemy);

			}
		}

		public static bool StageClearCheck()
		{

			int satisfyCount = 0;

			for(int i = 0; i < lstClearCondition.Count; i++)
			{
				switch (lstClearCondition[i].conditionName)
				{
					case StageClearConditionName.NumEnemiesKilled:

						if (numKillEnemy >= lstClearCondition[i].targetNumKill)
						{
							satisfyCount++;
						}

						break;
					case StageClearConditionName.ReachTheTartgetPoint:

						if (numKillEnemy >= lstClearCondition[i].targetNumKill)
						{
							satisfyCount++;
						}

						break;
					case StageClearConditionName.TalkFlag:

						if (TalkCommander.memoryTalkFlagID == lstClearCondition[i].targetTalkFlag)
						{
							satisfyCount++;
						}
						break;

					case StageClearConditionName.TimeElapsed:
						if (numKillEnemy >= lstClearCondition[i].targetNumKill)
						{
							satisfyCount++;
						}

						break;
				}

			}

			if (satisfyCount >= lstClearCondition.Count)
			{
				numKillEnemy = 0;
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
