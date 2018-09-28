using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zitulmyth.Checking;

namespace Zitulmyth.Data
{
	public class ImageData
	{

		//images
		public static Image[] imgTitle = new Image[2];
		public static Image imgPlayer;


		public static List<Image> imgSubWeapon = new List<Image>();
		public static List<Image> imgLife = new List<Image>();
		public static List<Image> imgMana = new List<Image>();

		public static BitmapImage[] bmTitle = new BitmapImage[2];

		public static BitmapImage bmPlayer;
		public static BitmapImage bmEnemy;
		public static BitmapImage bmSubWeapon;
		public static BitmapImage bmLadder;
		public static BitmapImage bmPlat;
		public static BitmapImage bmPlayerStatus;
		public static BitmapImage bmEmpty;
		public static BitmapImage bmNpc;
		public static BitmapImage bmInterior;
		public static BitmapImage bmFurniture;

		public static CroppedBitmap[] cbTitle = new CroppedBitmap[2];
		public static CroppedBitmap[] cbPlayer = new CroppedBitmap[2];
		public static CroppedBitmap[] cbEnemy = new CroppedBitmap[2];
		public static CroppedBitmap[] cbNpc = new CroppedBitmap[2];
		public static CroppedBitmap cbEmpty = new CroppedBitmap();
		public static CroppedBitmap cbSubWeapon = new CroppedBitmap();
		public static CroppedBitmap[] cbLife = new CroppedBitmap[2];
		public static CroppedBitmap[] cbMana = new CroppedBitmap[2];
		public static CroppedBitmap[] cbInterior = new CroppedBitmap[2];
		public static CroppedBitmap[] cbFurniture = new CroppedBitmap[3];


		public static void ImageLoadFirst()
		{
			bmTitle[0] = new BitmapImage(new Uri("title.png", UriKind.Relative));
			bmTitle[1] = new BitmapImage(new Uri("title_str.png", UriKind.Relative));
			cbTitle[0] = new CroppedBitmap(bmTitle[0], new Int32Rect(0, 0, 1024, 768));
			cbTitle[1] = new CroppedBitmap(bmTitle[1], new Int32Rect(0, 0, 448, 32));

			bmEmpty = new BitmapImage(new Uri("empty.png", UriKind.Relative));
			bmPlayer = new BitmapImage(new Uri("vtulp01.png", UriKind.Relative));
			bmEnemy = new BitmapImage(new Uri("zigitu01.png", UriKind.Relative));
			bmSubWeapon = new BitmapImage(new Uri("feather_knife.png", UriKind.Relative));
			bmPlayerStatus = new BitmapImage(new Uri("status.png", UriKind.Relative));
			BalloonMessage.bmBalloon = new BitmapImage(new Uri("balloon.png", UriKind.Relative));
			
			cbPlayer[0] = new CroppedBitmap(bmPlayer, new Int32Rect(0, 0, 32, 64));
			cbPlayer[1] = new CroppedBitmap(bmPlayer, new Int32Rect(32, 0, 32, 64));

			cbEnemy[0] = new CroppedBitmap(bmEnemy, new Int32Rect(0, 0, 32, 64));
			cbEnemy[1] = new CroppedBitmap(bmEnemy, new Int32Rect(32, 0, 32, 64));

			cbEmpty = new CroppedBitmap(bmEmpty, new Int32Rect(0, 0, 32, 32));

			cbSubWeapon = new CroppedBitmap(bmSubWeapon, new Int32Rect(0, 0, 32, 32));

			cbLife[0] = new CroppedBitmap(bmPlayerStatus, new Int32Rect(0, 0, 32, 32));
			cbLife[1] = new CroppedBitmap(bmPlayerStatus, new Int32Rect(64, 0, 32, 32));

			cbMana[0] = new CroppedBitmap(bmPlayerStatus, new Int32Rect(0, 32, 32, 32));
			cbMana[1] = new CroppedBitmap(bmPlayerStatus, new Int32Rect(32, 32, 32, 32));

			BalloonMessage.cbBalloon[0] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(0, 0, 16, 16));
			BalloonMessage.cbBalloon[1] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(16, 0, 16, 16));
			BalloonMessage.cbBalloon[2] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(48, 0, 16, 16));
			BalloonMessage.cbBalloon[3] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(0, 16, 16, 16));
			BalloonMessage.cbBalloon[4] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(16, 16, 16, 16));
			BalloonMessage.cbBalloon[5] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(48, 16, 16, 16));
			BalloonMessage.cbBalloon[6] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(0, 48, 16, 16));
			BalloonMessage.cbBalloon[7] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(16, 48, 16, 16));
			BalloonMessage.cbBalloon[8] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(48, 48, 16, 16));
			BalloonMessage.cbBalloon[9] = new CroppedBitmap(BalloonMessage.bmBalloon, new Int32Rect(0, 64, 16, 16));

		}

		public static void ImageLoadAfterSecond()
		{
			switch(StageManager.stageNum)
			{
				case 1:

					bmFurniture = new BitmapImage(new Uri("furniture.png", UriKind.Relative));
					cbFurniture[0] = new CroppedBitmap(bmFurniture, new Int32Rect(0, 0, 32, 64));	//chair
					cbFurniture[1] = new CroppedBitmap(bmFurniture, new Int32Rect(32, 32, 64, 32));	//table
					cbFurniture[2] = new CroppedBitmap(bmFurniture, new Int32Rect(96, 32, 64, 32)); //huton

					bmInterior = new BitmapImage(new Uri("interior_opsa.png", UriKind.Relative));
					cbInterior[0] = new CroppedBitmap(bmInterior, new Int32Rect(0, 0, 704, 384));

					bmNpc = new BitmapImage(new Uri("chara_opsa.png", UriKind.Relative));

					cbNpc[0] = new CroppedBitmap(bmNpc, new Int32Rect(0, 0, 32, 64));
					cbNpc[1] = new CroppedBitmap(bmNpc, new Int32Rect(32, 0, 32, 64));

					break;
				case 2:
					break;
			}
		}
	}
}
