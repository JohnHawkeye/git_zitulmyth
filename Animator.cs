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
using Zitulmyth.Data;


namespace Zitulmyth
{

	public class Animator
	{

		private const int patternNum = 4;
		private static int keyFlameTime = 800;
		private static CroppedBitmap[] cbAnimCell = new CroppedBitmap[4];

		public static void AnimationObject()
		{
			for(int i = 0; i< ObjectChecker.lstObject.Count; i++)
			{

				if(ObjectChecker.lstObject[i].totalAnimTime < keyFlameTime)
				{

					ObjectChecker.lstObject[i].totalAnimTime += MainWindow.elapsedTime;

				}
				else
				{
					switch (ObjectChecker.lstObject[i].objName)
					{
						case ObjectName.Obj_CampFire:

							cbAnimCell[0] = ImageData.cbObject[3];
							cbAnimCell[1] = ImageData.cbObject[4];
							cbAnimCell[2] = ImageData.cbObject[5];
							cbAnimCell[3] = ImageData.cbObject[2];

							break;

						case ObjectName.Npc_Yeeda:

							cbAnimCell[0] =	cbAnimCell[1] = cbAnimCell[2] = cbAnimCell[3] =  ImageData.cbNpc[1];

							break;

						case ObjectName.Empty_Collider:

							cbAnimCell[0] = cbAnimCell[1] = cbAnimCell[2] = cbAnimCell[3] = ImageData.cbDebug[0];

							break;
					}

					switch (ObjectChecker.lstObject[i].keyFlame)
					{
						case 0:
							ObjectChecker.lstObject[i].imgObject.Source = cbAnimCell[0];
							break;
						case 1:
							ObjectChecker.lstObject[i].imgObject.Source = cbAnimCell[1];
							break;
						case 2:
							ObjectChecker.lstObject[i].imgObject.Source = cbAnimCell[2];
							break;
						case 3:
							ObjectChecker.lstObject[i].imgObject.Source = cbAnimCell[3];
							break;
					}

					if (ObjectChecker.lstObject[i].keyFlame != 3)
					{
						ObjectChecker.lstObject[i].keyFlame++;
					}
					else
					{
						ObjectChecker.lstObject[i].keyFlame = 0;
					}

					ObjectChecker.lstObject[i].totalAnimTime = 0;


				}

				
			}
		}
	}

}
