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

			for(int i=0;i<StageData.interiorPosition.Count;i++)
			{

				int width, height;

				width = ImageData.cbInterior[i].SourceRect.Width;
				height = ImageData.cbInterior[i].SourceRect.Height;

				var _imgInterior = new Image
				{
					Source = ImageData.cbInterior[i],
					Width = width,
					Height = height,
				};

				StageData.imgInterior[i] = _imgInterior;
				canvas.Children.Add(StageData.imgInterior[i]);
				Canvas.SetLeft(StageData.imgInterior[i], StageData.interiorPosition[i].X);
				Canvas.SetTop(StageData.imgInterior[i], StageData.interiorPosition[i].Y);
				Canvas.SetZIndex(StageData.imgInterior[i],interiorZindex);
			}

			for (int i = 0; i < StageData.furniturePosition.Count; i++)
			{

				int width, height;

				width = ImageData.cbFurniture[i].SourceRect.Width;
				height = ImageData.cbFurniture[i].SourceRect.Height;

				var _imgFurniture = new Image
				{
					Source = ImageData.cbFurniture[i],
					Width = width,
					Height = height,
				};

				StageData.imgFurniture[i] = _imgFurniture;
				canvas.Children.Add(StageData.imgFurniture[i]);
				Canvas.SetLeft(StageData.imgFurniture[i], StageData.furniturePosition[i].X);
				Canvas.SetTop(StageData.imgFurniture[i], StageData.furniturePosition[i].Y);
				Canvas.SetZIndex(StageData.imgFurniture[i], furnitureZindex);
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
		}
	}
}
