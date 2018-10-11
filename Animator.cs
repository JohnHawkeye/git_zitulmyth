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
			for(int i = 0; i< Object.lstObject.Count; i++)
			{

				if(Object.lstObject[i].totalAnimTime < keyFlameTime)
				{

					Object.lstObject[i].totalAnimTime += MainWindow.elapsedTime;

				}
				else
				{
					switch (Object.lstObject[i].objName)
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

							cbAnimCell[0] = cbAnimCell[1] = cbAnimCell[2] = cbAnimCell[3] = ImageData.cbDebug;

							break;
					}

					switch (Object.lstObject[i].keyFlame)
					{
						case 0:
							Object.lstObject[i].imgObject.Source = cbAnimCell[0];
							break;
						case 1:
							Object.lstObject[i].imgObject.Source = cbAnimCell[1];
							break;
						case 2:
							Object.lstObject[i].imgObject.Source = cbAnimCell[2];
							break;
						case 3:
							Object.lstObject[i].imgObject.Source = cbAnimCell[3];
							break;
					}

					if (Object.lstObject[i].keyFlame != 3)
					{
						Object.lstObject[i].keyFlame++;
					}
					else
					{
						Object.lstObject[i].keyFlame = 0;
					}
					
					Object.lstObject[i].totalAnimTime = 0;


				}

				
			}
		}
	}

}
