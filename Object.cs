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

	public enum ObjectName
	{
		Empty_Collider,
		Npc_Opsa,
		Npc_Yeeda,
		Obj_Chair,
		Obj_Table,
		Obj_Huton,
		Obj_CampFire,
	}

	public class ObjectData
	{
		public ObjectName objName;
		public Image imgObject;
		public CroppedBitmap cbSource;
		public Vector position;
		public int width;		//blocknum
		public int height;
		public int zindex;
		public int expirationTime;
		public int totalTime;
		public int keyFlame;
		public int totalAnimTime;
		public ObjectName triggerTarget;
		public bool triggerAction;
	}

	 public class Object
	{
		public static List<ObjectData> lstObject = new List<ObjectData>();

		private static Vector triggerTargetPosition;

		public static void CollisionPtoActionCollider()
		{
			for (int i = 0; i < lstObject.Count; i++)
			{

				Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
				Vector size1 = new Vector(PlayerStatus.playerSize.X, PlayerStatus.playerSize.Y);

				Vector p2 = new Vector(Canvas.GetLeft(lstObject[i].imgObject), Canvas.GetTop(lstObject[i].imgObject));
				Vector size2 = new Vector(lstObject[i].width * 32, lstObject[i].height * 32);

				if (CollisionCheck.Collision(p1, p2, size1, size2) && lstObject[i].triggerAction)
				{

					GetTriggerTargetPosition(i);

					Canvas.SetLeft(ImageData.imgPopCanTalk, triggerTargetPosition.X-16);
					Canvas.SetTop(ImageData.imgPopCanTalk, triggerTargetPosition.Y-32);
					ImageData.imgPopCanTalk.Visibility = Visibility.Visible;

					break;
				}
				else
				{
					ImageData.imgPopCanTalk.Visibility = Visibility.Hidden;
				}

			}
		}

		private static void GetTriggerTargetPosition(int index)
		{
			for(int i = 0; i < lstObject.Count; i++)
			{
				if(lstObject[index].triggerTarget == lstObject[i].objName)
				{

					triggerTargetPosition = lstObject[i].position;

					break;
				}
			}
		}


	}

	public class ObjectBehavior
	{

	}
}
