﻿using System;
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
	public class StageManager
	{
		public static int stageNum = 0;

		public  static int interiorZindex = 1;
		public static int furnitureZindex = 2;
		public static int npcZindex = 3;

			
		public static void StageObjectsSetting(Canvas canvas)
		{
	
			Canvas.SetLeft(ImageData.imgPlayer,StageData.startPlayerPosition.X);
			Canvas.SetTop(ImageData.imgPlayer, StageData.startPlayerPosition.Y);

			StageData.imgScenery = new Image { Source = ImageData.cbScenery, Width = 1024, Height = 768, };
			canvas.Children.Add(StageData.imgScenery);
			Canvas.SetLeft(StageData.imgScenery, 0);
			Canvas.SetTop(StageData.imgScenery, 0);
			Canvas.SetZIndex(StageData.imgScenery, 1);


			for (int i = 0; i < ObjectChecker.lstObject.Count; i++)
			{

				var _imgObject = new Image
				{
					Source = ObjectChecker.lstObject[i].cbSource,
					Width = ObjectChecker.lstObject[i].width * 32,
					Height = ObjectChecker.lstObject[i].height * 32,
					Stretch = Stretch.Fill,
				};

				ObjectChecker.lstObject[i].imgObject = _imgObject;
				canvas.Children.Add(ObjectChecker.lstObject[i].imgObject);
				Canvas.SetLeft(ObjectChecker.lstObject[i].imgObject, ObjectChecker.lstObject[i].position.X);
				Canvas.SetTop(ObjectChecker.lstObject[i].imgObject, ObjectChecker.lstObject[i].position.Y);
				Canvas.SetZIndex(ObjectChecker.lstObject[i].imgObject, ObjectChecker.lstObject[i].zindex);
			}

			Item.ItemGenerate(canvas);

			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{

				canvas.Children.Add(SpawnEnemy.lstEnemyData[i].imgEnemy);
				Canvas.SetLeft(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.X);
				Canvas.SetTop(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.Y);
				Canvas.SetZIndex(SpawnEnemy.lstEnemyData[i].imgEnemy,6);

			}
		}
	}
}
