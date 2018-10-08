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


			for (int i = 0; i < StageData.objectPosition.Count; i++)
			{

				int width, height;

				width = ImageData.cbObject[i].SourceRect.Width;
				height = ImageData.cbObject[i].SourceRect.Height;

				var _imgObject = new Image
				{
					Source = ImageData.cbObject[i],
					Width = width,
					Height = height,
				};

				StageData.imgObject[i] = _imgObject;
				canvas.Children.Add(StageData.imgObject[i]);
				Canvas.SetLeft(StageData.imgObject[i], StageData.objectPosition[i].X);
				Canvas.SetTop(StageData.imgObject[i], StageData.objectPosition[i].Y);
				Canvas.SetZIndex(StageData.imgObject[i], furnitureZindex);
			}

			for (int i = 0; i < StageData.npcPosition.Count; i++)
			{

				int width, height;

				width = ImageData.cbNpc[i].SourceRect.Width;
				height = ImageData.cbNpc[i].SourceRect.Height;

				var _imgNpc = new Image
				{
					Source = ImageData.cbNpc[i],
					Width = width,
					Height = height,
				};

				StageData.imgNpc[i] = _imgNpc;
				canvas.Children.Add(StageData.imgNpc[i]);
				Canvas.SetLeft(StageData.imgNpc[i], StageData.npcPosition[i].X);
				Canvas.SetTop(StageData.imgNpc[i], StageData.npcPosition[i].Y);
				Canvas.SetZIndex(StageData.imgNpc[i], npcZindex);

			}

			Item.ItemGenerate(canvas);
		}
	}
}
