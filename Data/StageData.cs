using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Media;

namespace Zitulmyth.Data
{
	public class StageData
	{
		public static Vector startPlayerPosition;

		public static List<Vector> npcPosition = new List<Vector>();
		public static List<Vector> interiorPosition = new List<Vector>();
		public static List<Vector> furniturePosition = new List<Vector>();

		//StageBlockData
		public static BlockType[,] indicateStage = new BlockType[24, 32];
		public static Image[,] imgBlock = new Image[24, 32];

		public static List<CroppedBitmap> refCbFurniture = new List<CroppedBitmap>();

	}

	public class StageDataSetting
	{

		public static void SetData()
		{

			StageData.furniturePosition.Clear();
			StageData.refCbFurniture.Clear();
			StageData.npcPosition.Clear();

			switch (StageManager.stageNum)
			{
				case 0:

					StageData.startPlayerPosition = new Vector(671, 300);

					for (int i = 0; i < 32; i++)
					{
						StageData.indicateStage[23, i] = BlockType.GreenGround;
					}

					break;

				case 1:



					StageData.startPlayerPosition = new Vector(592, 480);

					for (int i = 0; i < 32; i++)
					{
						StageData.indicateStage[17, i] = BlockType.InvisibleBlock;
					}

					StageData.npcPosition.Add(new Vector(352, 480));		//opsa 352,480
					StageData.interiorPosition.Add(new Vector(160, 192));	//house 160,192
					StageData.furniturePosition.Add(new Vector(384,480));	//chair 384,480
					StageData.furniturePosition.Add(new Vector(448, 512));	//table 448,512
					StageData.furniturePosition.Add(new Vector(576, 512));  //huton 576,512
					StageData.refCbFurniture.Add(ImageData.cbFurniture[0]);
					StageData.refCbFurniture.Add(ImageData.cbFurniture[1]);
					StageData.refCbFurniture.Add(ImageData.cbFurniture[2]);

					break;
			}
			
		}


	}
}
