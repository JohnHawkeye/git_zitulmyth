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
		public static List<Vector> objectPosition = new List<Vector>();

		public static Image imgScenery = new Image();
		public static List<Image> imgObject = new List<Image>();
		public static List<Image> imgNpc = new List<Image>();

		//StageBlockData
		public static BlockType[,] indicateStage = new BlockType[24, 32];
		public static Image[,] imgBlock = new Image[24, 32];

		public static List<CroppedBitmap> refCbObject = new List<CroppedBitmap>();

	}

	public class StageDataSetting
	{

		public static void SetData()
		{

			switch (StageManager.stageNum)
			{
				case 0:

					StageData.startPlayerPosition = new Vector(300, 671);

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
					StageData.objectPosition.Add(new Vector(384,480));	//chair 384,480
					StageData.objectPosition.Add(new Vector(448, 512));	//table 448,512
					StageData.objectPosition.Add(new Vector(576, 512));  //huton 576,512
					StageData.refCbObject.Add(ImageData.cbObject[0]);
					StageData.refCbObject.Add(ImageData.cbObject[1]);
					StageData.refCbObject.Add(ImageData.cbObject[2]);

					StageData.imgNpc.Add(null);
					StageData.imgObject.Add(null);
					StageData.imgObject.Add(null);
					StageData.imgObject.Add(null);
					break;

				case 2:

					for (int i = 0; i < 32; i++){StageData.indicateStage[23, i] = BlockType.GreenGround;}

					StageData.indicateStage[18, 8] = StageData.indicateStage[18, 9] = StageData.indicateStage[18, 10] =
					StageData.indicateStage[18, 11] = StageData.indicateStage[18, 12] =
					BlockType.InvisibleBlock;

					StageData.indicateStage[21, 4] = StageData.indicateStage[21, 5] = StageData.indicateStage[21, 6] =
					BlockType.InvisiblePlat;

					StageData.indicateStage[17, 13] = StageData.indicateStage[17, 14] = StageData.indicateStage[17, 15] =
					BlockType.InvisiblePlat;

					StageData.indicateStage[15, 20] = StageData.indicateStage[15, 21] = StageData.indicateStage[15, 22] =
					BlockType.InvisiblePlat;

					StageData.indicateStage[15, 14] = StageData.indicateStage[15, 15] =	StageData.indicateStage[15, 16] =
					StageData.indicateStage[15, 17] =
					BlockType.WoodPlatform;

					StageData.indicateStage[15, 18] = BlockType.LadderTop;
					for(int i=0; i < 6; i++){StageData.indicateStage[16+i, 18] = BlockType.LadderMid;}
					StageData.indicateStage[22, 18] = BlockType.LadderBottom;


					StageData.startPlayerPosition = new Vector(64, 672);

					StageData.npcPosition.Add(new Vector(928, 672));        //opsa 352,480
					StageData.objectPosition.Add(new Vector(864, 672));
					StageData.refCbObject.Add(ImageData.cbObject[2]);

					StageData.imgNpc.Add(null);
					StageData.imgObject.Add(null);

					break;
			}
			
		}


	}
}
