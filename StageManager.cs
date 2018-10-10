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


			for (int i = 0; i < Object.lstObject.Count; i++)
			{

				int width, height;

				width = Object.lstObject[i].cbSource.SourceRect.Width;
				height = Object.lstObject[i].cbSource.SourceRect.Height;

				var _imgObject = new Image
				{
					Source = Object.lstObject[i].cbSource,
					Width = width,
					Height = height,
				};

				Object.lstObject[i].imgObject = _imgObject;
				canvas.Children.Add(Object.lstObject[i].imgObject);
				Canvas.SetLeft(Object.lstObject[i].imgObject, Object.lstObject[i].position.X);
				Canvas.SetTop(Object.lstObject[i].imgObject, Object.lstObject[i].position.Y);
				Canvas.SetZIndex(Object.lstObject[i].imgObject, Object.lstObject[i].zindex);
			}

			Item.ItemGenerate(canvas);
		}
	}
}
